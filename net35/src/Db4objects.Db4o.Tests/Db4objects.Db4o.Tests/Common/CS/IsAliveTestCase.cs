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
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class IsAliveTestCase : TestWithTempFile
	{
		private static readonly string Username = "db4o";

		private static readonly string Password = "db4o";

		public virtual void TestIsAlive()
		{
			IObjectServer server = OpenServer();
			int port = server.Ext().Port();
			ClientObjectContainer client = OpenClient(port);
			Assert.IsTrue(client.IsAlive());
			client.Close();
			server.Close();
		}

		public virtual void TestIsNotAlive()
		{
			IObjectServer server = OpenServer();
			int port = server.Ext().Port();
			ClientObjectContainer client = OpenClient(port);
			server.Close();
			Assert.IsFalse(client.IsAlive());
			client.Close();
		}

		private IObjectServer OpenServer()
		{
			IObjectServer server = Db4oClientServer.OpenServer(Db4oClientServer.NewServerConfiguration
				(), TempFile(), -1);
			server.GrantAccess(Username, Password);
			return server;
		}

		private ClientObjectContainer OpenClient(int port)
		{
			ClientObjectContainer client = (ClientObjectContainer)Db4oClientServer.OpenClient
				(Db4oClientServer.NewClientConfiguration(), "localhost", port, Username, Password
				);
			return client;
		}
	}
}
#endif // !SILVERLIGHT
