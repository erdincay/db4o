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
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Tests.Common.CS;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Foundation.Net.SSL
{
	public class SslIntegrationTestCase : ClientServerTestCaseBase, ICustomClientServerConfiguration, IOptOutAllButNetworkingCS
	{
		private class Item
		{
			public Item(string topSecret)
			{
				_topSecret = topSecret;
			}

			public string TopSecret
			{
				get { return _topSecret; }
			}

			private readonly string _topSecret;
		}

		public void ConfigureServer(IConfiguration config)
		{
			IServerConfiguration serverConfig = Db4oClientServerLegacyConfigurationBridge.AsServerConfiguration(config);
			serverConfig.AddConfigurationItem(new ServerSslSupport(ServerCertificate()));
		}

		private X509Certificate2 ServerCertificate()
		{
			if (_serverCertificate == null)
			{
				byte[] bytes = Certificates.CreateSelfSignCertificate("CN=ssl-test.db4o.anywere, OU=db4o", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), "db4o-ssl-test");
				_serverCertificate = new X509Certificate2(bytes, "db4o-ssl-test");
			}

			return _serverCertificate;
		}

		public void ConfigureClient(IConfiguration config)
		{
			IClientConfiguration clientConfig = Db4oClientServerLegacyConfigurationBridge.AsClientConfiguration(config);
			clientConfig.AddConfigurationItem(new ClientSslSupport(ValidateServerCertificate));
		}

		private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
		{
			return certificate.Subject.Contains("CN=ssl-test.db4o.anywere");
		}

		protected override void  Store()
		{
			Store(new Item("foo"));
			Store(new Item("bar"));
		}

		public void Test()
		{
			AssertQuery("foo");
			AssertQuery("bar");
		}

		private void AssertQuery(string value)
		{
			IList<Item> items = Db().Query<Item>(delegate(Item candidate1) { return candidate1.TopSecret == value; });
			Assert.AreEqual(1, items.Count);
			Assert.AreEqual(value, items[0].TopSecret);
		}

		private X509Certificate2 _serverCertificate;
	}
}

#endif