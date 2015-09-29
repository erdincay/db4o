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
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class Db4oClientServerDrsFixture : Db4oDrsFixture
	{
		private static readonly string Host = "localhost";

		private static readonly string Username = "db4o";

		private static readonly string Password = Username;

		private IObjectServer _server;

		private int _port;

		public Db4oClientServerDrsFixture(string name, int port) : base(name)
		{
			_port = port;
		}

		public override void Close()
		{
			base.Close();
			_server.Close();
		}

		public override void Open()
		{
			Config().MessageLevel(-1);
			_server = Db4oClientServer.OpenServer(Db4oClientServerLegacyConfigurationBridge.AsServerConfiguration
				(CloneConfiguration()), testFile.GetPath(), _port);
			_server.GrantAccess(Username, Password);
			_db = Db4oClientServer.OpenClient(Db4oClientServerLegacyConfigurationBridge.AsClientConfiguration
				(CloneConfiguration()), Host, _port, Username, Password).Ext();
			_provider = Db4oProviderFactory.NewInstance(_db, _name);
		}

		private IConfiguration CloneConfiguration()
		{
			return (IConfiguration)((IDeepClone)Config()).DeepClone(Config());
		}
	}
}
