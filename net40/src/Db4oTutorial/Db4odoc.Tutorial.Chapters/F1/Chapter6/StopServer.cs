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
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Messaging;

namespace Db4odoc.Tutorial.F1.Chapter6
{
    /// <summary>
    /// stops the db4o Server started with StartServer.
    /// This is done by opening a client connection
    /// to the server and by sending a StopServer object as
    /// a message. StartServer will react in it's
    /// processMessage method.
    /// </summary>
    public class StopServer : ServerInfo
    {
        /// <summary>
        /// stops a db4o Server started with StartServer.
        /// </summary>
        /// <exception cref="Exception" />
        public static void Main(string[] args)
        {
            IObjectContainer IObjectContainer = null;
            try
            {
                // connect to the server
                IObjectContainer = Db4oClientServer.OpenClient(Db4oClientServer.NewClientConfiguration(),
                    HOST, PORT, USER, PASS);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
            if (IObjectContainer != null)
            {
                // get the messageSender for the IObjectContainer
                IMessageSender messageSender = IObjectContainer.Ext()
                    .Configure().ClientServer().GetMessageSender();

                // send an instance of a StopServer object
                messageSender.Send(new StopServer());
                
                // close the IObjectContainer
                IObjectContainer.Close();
            }
        }
    }
}
