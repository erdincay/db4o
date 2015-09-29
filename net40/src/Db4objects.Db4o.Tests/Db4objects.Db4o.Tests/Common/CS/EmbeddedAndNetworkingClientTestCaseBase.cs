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
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public abstract class EmbeddedAndNetworkingClientTestCaseBase : ITestLifeCycle
	{
		private static readonly string Username = "db4o";

		private static readonly string Password = "db4o";

		private IExtObjectServer _server;

		private IExtObjectContainer _networkingClient;

		private ObjectContainerSession _embeddedClient;

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			IServerConfiguration serverConfiguration = Db4oClientServer.NewServerConfiguration
				();
			serverConfiguration.File.Storage = new MemoryStorage();
			_server = Db4oClientServer.OpenServer(serverConfiguration, string.Empty, Db4oClientServer
				.ArbitraryPort).Ext();
			_server.GrantAccess(Username, Password);
			_networkingClient = Db4oClientServer.OpenClient("localhost", _server.Port(), Username
				, Password).Ext();
			this._embeddedClient = ((ObjectContainerSession)_server.OpenClient().Ext());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			EmbeddedClient().Close();
			NetworkingClient().Close();
			_server.Close();
		}

		protected virtual IExtObjectContainer NetworkingClient()
		{
			return _networkingClient;
		}

		protected virtual ObjectContainerSession EmbeddedClient()
		{
			return _embeddedClient;
		}

		protected virtual IExtObjectContainer ServerObjectContainer()
		{
			return _server.ObjectContainer().Ext();
		}
	}
}
#endif // !SILVERLIGHT
