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
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	/// <exclude></exclude>
	public class ServerTimeoutTestCase : ClientServerTestCaseBase
	{
		public static void Main(string[] arguments)
		{
			new ServerTimeoutTestCase().RunNetworking();
		}

		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().TimeoutClientSocket(1);
			config.ClientServer().TimeoutServerSocket(1);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void _test()
		{
			ObjectServerImpl serverImpl = (ObjectServerImpl)ClientServerFixture().Server();
			IEnumerator iter = serverImpl.IterateDispatchers();
			iter.MoveNext();
			IServerMessageDispatcher serverDispatcher = (IServerMessageDispatcher)iter.Current;
			IClientMessageDispatcher clientDispatcher = ((ClientObjectContainer)Db()).MessageDispatcher
				();
			clientDispatcher.Close();
			Runtime4.Sleep(1000);
			Assert.IsFalse(serverDispatcher.IsMessageDispatcherAlive());
		}
	}
}
#endif // !SILVERLIGHT
