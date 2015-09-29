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
#if !SILVERLIGHT
using System;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;

namespace Db4objects.Db4o.Tests.CLI1
{
    /// <summary>
    /// A facade to an ObjectContainer executing in a different AppDomain.
    /// </summary>
    public class MarshalByRefDatabase : MarshalByRefObject, IDisposable
    {
        protected IObjectServer _server;
        protected IObjectContainer _container;

        public void Open(string fname, bool clientServer)
        {
            if (clientServer)
            {
                _server = Db4oClientServer.OpenServer(fname, 0);
                _container = _server.OpenClient();
            }
            else
            {
                _container = Db4oFactory.OpenFile(fname);
            }
        }

        public void Dispose()
        {
            if (null != _container)
            {
                _container.Close();
                _container = null;
            }
            if (null != _server)
            {
                _server.Close();
                _server = null;
            }
            // MAGIC: give some time for the db4o background threads to exit
            System.Threading.Thread.Sleep(1000);
        }
    }
}
#endif