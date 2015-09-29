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
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ServerPortUsedTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		private static readonly string DatabaseFile = "PortUsed.db";

		public static void Main(string[] args)
		{
			new ServerPortUsedTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oTearDownBeforeClean()
		{
			File4.Delete(DatabaseFile);
		}

		public virtual void Test()
		{
			int port = ClientServerFixture().ServerPort();
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_28(port));
		}

		private sealed class _ICodeBlock_28 : ICodeBlock
		{
			public _ICodeBlock_28(int port)
			{
				this.port = port;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oClientServer.OpenServer(ServerPortUsedTestCase.DatabaseFile, port);
			}

			private readonly int port;
		}
	}
}
#endif // !SILVERLIGHT
