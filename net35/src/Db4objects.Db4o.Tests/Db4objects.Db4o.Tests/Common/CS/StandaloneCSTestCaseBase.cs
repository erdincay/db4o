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
using System.IO;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation.IO;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public abstract class StandaloneCSTestCaseBase : ITestCase
	{
		private int _port;

		public sealed class Item
		{
			// TODO fix db4ounit call logic - this should actually be run in C/S mode
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			string fileName = DatabaseFile();
			File4.Delete(fileName);
			IObjectServer server = Db4oClientServer.OpenServer(CreateServerConfiguration(), fileName
				, -1);
			_port = server.Ext().Port();
			try
			{
				server.GrantAccess("db4o", "db4o");
				RunTest();
			}
			finally
			{
				server.Close();
				File4.Delete(fileName);
			}
		}

		private IServerConfiguration CreateServerConfiguration()
		{
			return Db4oClientServerLegacyConfigurationBridge.AsServerConfiguration(CreateConfiguration
				());
		}

		private IConfiguration CreateConfiguration()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			Configure(config);
			return config;
		}

		/// <exception cref="System.Exception"></exception>
		protected virtual void WithClient(IContainerBlock block)
		{
			ContainerServices.WithContainer(OpenClient(), block);
		}

		protected virtual ClientObjectContainer OpenClient()
		{
			return (ClientObjectContainer)Db4oClientServer.OpenClient(CreateClientConfiguration
				(), "localhost", _port, "db4o", "db4o");
		}

		private IClientConfiguration CreateClientConfiguration()
		{
			return Db4oClientServerLegacyConfigurationBridge.AsClientConfiguration(CreateConfiguration
				());
		}

		protected virtual int Port()
		{
			return _port;
		}

		/// <exception cref="System.Exception"></exception>
		protected abstract void RunTest();

		protected abstract void Configure(IConfiguration config);

		private string DatabaseFile()
		{
			return Path.Combine(Path.GetTempPath(), "cc.db4o");
		}
	}
}
#endif // !SILVERLIGHT
