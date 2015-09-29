/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2011  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using System;
using System.Threading;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Messaging;

namespace Db4odoc.Tutorial.F1.Chapter6
{
    /// <summary>
    /// starts a db4o server with the settings from ServerInfo.
    /// This is a typical setup for a long running server.
    /// The Server may be stopped from a remote location by running
    /// StopServer. The StartServer instance is used as a MessageRecipient
    /// and reacts to receiving an instance of a StopServer object.
    /// Note that all user classes need to be present on the server side
    /// and that all possible Db4oFactory.Configure() calls to alter the db4o
    /// configuration need to be executed on the client and on the server.
    /// </summary>
    public class StartServer : ServerInfo, IMessageRecipient
    {
        /// <summary>
        /// setting the value to true denotes that the server should be closed
        /// </summary>
        private bool stop = false;

        /// <summary>
        /// starts a db4o server using the configuration from
        /// ServerInfo.
        /// </summary>
        public static void Main(string[] arguments)
        {
            new StartServer().RunServer();
        }

        /// <summary>
        /// opens the IObjectServer, and waits forever until Close() is called
        /// or a StopServer message is being received.
        /// </summary>
        public void RunServer()
        {
            lock(this)
            {
                IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
                // Using the messaging functionality to redirect all
                // messages to this.processMessage
                config.Networking.MessageRecipient = this;
                IObjectServer db4oServer = Db4oClientServer.OpenServer(config, FILE, PORT);
                db4oServer.GrantAccess(USER, PASS);
                
                try
                {
                    if (! stop)
                    {
                        // wait forever until Close will change stop variable
                        Monitor.Wait(this);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                db4oServer.Close();
            }
        }

        /// <summary>
        /// messaging callback
        /// see com.db4o.messaging.MessageRecipient#ProcessMessage()
        /// </summary>
        public void ProcessMessage(IMessageContext con, object message)
        {
            if (message is StopServer)
            {
                Close();
            }
        }

        /// <summary>
        /// closes this server.
        /// </summary>
        public void Close()
        {
            lock(this)
            {
                stop = true;
                Monitor.PulseAll(this);
            }
        }
    }
}
