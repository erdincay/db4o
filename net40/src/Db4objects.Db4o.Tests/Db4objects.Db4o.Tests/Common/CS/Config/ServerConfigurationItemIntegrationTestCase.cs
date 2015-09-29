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
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.CS.Config;

namespace Db4objects.Db4o.Tests.Common.CS.Config
{
	public class ServerConfigurationItemIntegrationTestCase : ITestCase
	{
		public virtual void Test()
		{
			IServerConfiguration config = Db4oClientServer.NewServerConfiguration();
			config.File.Storage = new MemoryStorage();
			ServerConfigurationItemIntegrationTestCase.DummyConfigurationItem item = new ServerConfigurationItemIntegrationTestCase.DummyConfigurationItem
				(this);
			config.AddConfigurationItem(item);
			IObjectServer server = Db4oClientServer.OpenServer(config, string.Empty, Db4oClientServer
				.ArbitraryPort);
			item.Verify(config, server);
			server.Close();
		}

		private sealed class DummyConfigurationItem : IServerConfigurationItem
		{
			private int _prepareCount = 0;

			private int _applyCount = 0;

			private IServerConfiguration _config;

			private IObjectServer _server;

			public void Prepare(IServerConfiguration configuration)
			{
				this._config = configuration;
				this._prepareCount++;
			}

			public void Apply(IObjectServer server)
			{
				this._server = server;
				this._applyCount++;
			}

			internal void Verify(IServerConfiguration config, IObjectServer server)
			{
				Assert.AreSame(config, this._config);
				Assert.AreSame(server, this._server);
				Assert.AreEqual(1, this._prepareCount);
				Assert.AreEqual(1, this._applyCount);
			}

			internal DummyConfigurationItem(ServerConfigurationItemIntegrationTestCase _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			private readonly ServerConfigurationItemIntegrationTestCase _enclosing;
		}
	}
}
#endif // !SILVERLIGHT
