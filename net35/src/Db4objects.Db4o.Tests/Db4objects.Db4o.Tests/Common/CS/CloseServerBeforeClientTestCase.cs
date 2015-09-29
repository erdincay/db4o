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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class CloseServerBeforeClientTestCase : TestWithTempFile
	{
		public static void Main(string[] arguments)
		{
			for (int i = 0; i < 1000; i++)
			{
				new ConsoleTestRunner(typeof(CloseServerBeforeClientTestCase)).Run();
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IObjectServer server = Db4oClientServer.OpenServer(TempFile(), Db4oClientServer.ArbitraryPort
				);
			server.GrantAccess(string.Empty, string.Empty);
			IObjectContainer client = Db4oClientServer.OpenClient("localhost", ((ObjectServerImpl
				)server).Port(), string.Empty, string.Empty);
			IObjectContainer client2 = Db4oClientServer.OpenClient("localhost", ((ObjectServerImpl
				)server).Port(), string.Empty, string.Empty);
			client.Commit();
			client2.Commit();
			try
			{
				server.Close();
			}
			finally
			{
				try
				{
					client.Close();
					client2.Close();
				}
				catch (Db4oException)
				{
				}
			}
		}
		// database may have been closed
	}
}
#endif // !SILVERLIGHT
