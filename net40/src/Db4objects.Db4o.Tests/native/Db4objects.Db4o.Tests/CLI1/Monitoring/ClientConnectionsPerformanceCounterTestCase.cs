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
#if !CF && !SILVERLIGHT

using System;
using System.Threading;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Monitoring;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Tests.Common.Api;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class ClientConnectionsPerformanceCounterTestCase : TestWithTempFile
	{
		private const string UserName = "db4o";
		private const string Password = "db4o";

		public void TestConnectedClients() 
		{
			for(int i=0; i < 10; i++)
			{
				Assert.AreEqual(0, ConnectedClientCount(), "No client yet.");
				IExtObjectContainer client1 = OpenNewSession();
				Assert.AreEqual(1, ConnectedClientCount(), "client1:" + i);
				IExtObjectContainer client2 = OpenNewSession();
				Assert.AreEqual(2, ConnectedClientCount(), "client1 and client2: " + i);
				EnsureClose(client1);
				Assert.AreEqual(1, ConnectedClientCount(), "client2: " + i);
				EnsureClose(client2);
				Assert.AreEqual(0, ConnectedClientCount(), "" + i);
			}		
		}

		private void EnsureClose(IObjectContainer client) 
		{
			client.Close();
			_clientDisconnectedEvent.WaitOne();
		}

		private IExtObjectContainer OpenNewSession() 
		{
			return (IExtObjectContainer) Db4oClientServer.OpenClient("localhost", _server.Ext().Port(), UserName, Password);
		}

		private long ConnectedClientCount() 
		{
            return PerformanceCounterSpec.NetClientConnections.PerformanceCounter(_server.Ext().ObjectContainer()).RawValue;
		}

		public override void SetUp()
		{
			Db4oPerformanceCounterInstaller.ReInstall();

			IServerConfiguration serverConfiguration = Db4oClientServer.NewServerConfiguration();
			serverConfiguration.AddConfigurationItem(new ClientConnectionsMonitoringSupport());
			serverConfiguration.AddConfigurationItem(new ConnectionCloseEventSupport(ClientDisconnected));

			_server = Db4oClientServer.OpenServer(serverConfiguration, TempFile(), Db4oClientServer.ArbitraryPort);
			_server.GrantAccess(UserName, Password);
		}

		public override void TearDown()
		{
			_clientDisconnectedEvent.Close();
			_server.Close();

			base.TearDown();
		}
	
		void ClientDisconnected(object sender, StringEventArgs e)
		{
			_clientDisconnectedEvent.Set();
		}

		private IObjectServer _server;
		private readonly EventWaitHandle _clientDisconnectedEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
	}

	class ConnectionCloseEventSupport : IServerConfigurationItem 
	{
		private readonly EventHandler<StringEventArgs> _handler;
		
		public ConnectionCloseEventSupport(EventHandler<StringEventArgs> handler) {
			_handler = handler;
		}
		
		public void Prepare(IServerConfiguration configuration) 
		{
		}

		public void Apply(IObjectServer server)
		{
			((IObjectServerEvents)server).ClientDisconnected += _handler;
		}
	}
}

#endif