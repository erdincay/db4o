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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ClientDisconnectTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] arguments)
		{
			new ClientDisconnectTestCase().RunConcurrency();
			new ClientDisconnectTestCase().RunConcurrency();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void _concDelete(IExtObjectContainer oc, int seq)
		{
			ClientObjectContainer client = (ClientObjectContainer)oc;
			try
			{
				if (seq % 2 == 0)
				{
					// ok to get something
					client.QueryByExample(null);
				}
				else
				{
					client.Socket().Close();
					Assert.IsFalse(oc.IsClosed());
					Assert.Expect(typeof(Db4oException), new _ICodeBlock_27(client));
				}
			}
			finally
			{
				oc.Close();
				Assert.IsTrue(oc.IsClosed());
			}
		}

		private sealed class _ICodeBlock_27 : ICodeBlock
		{
			public _ICodeBlock_27(ClientObjectContainer client)
			{
				this.client = client;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				client.QueryByExample(null);
			}

			private readonly ClientObjectContainer client;
		}
	}
}
#endif // !SILVERLIGHT
