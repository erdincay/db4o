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
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ClientTransactionPoolTestCase : ITestLifeCycle
	{
		public virtual void TestPool()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.Storage = new MemoryStorage();
			LocalObjectContainer db = (LocalObjectContainer)Db4oFactory.OpenFile(config, ClientTransactionTestUtil
				.MainfileName);
			ClientTransactionPool pool = new ClientTransactionPool(db);
			try
			{
				Assert.AreEqual(0, pool.OpenTransactionCount());
				Assert.AreEqual(1, pool.OpenFileCount());
				Transaction trans1 = pool.Acquire(ClientTransactionTestUtil.MainfileName);
				Assert.AreEqual(db, trans1.Container());
				Assert.AreEqual(1, pool.OpenTransactionCount());
				Assert.AreEqual(1, pool.OpenFileCount());
				Transaction trans2 = pool.Acquire(ClientTransactionTestUtil.FilenameA);
				Assert.AreNotEqual(db, trans2.Container());
				Assert.AreEqual(2, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				Transaction trans3 = pool.Acquire(ClientTransactionTestUtil.FilenameA);
				Assert.AreEqual(trans2.Container(), trans3.Container());
				Assert.AreEqual(3, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				pool.Release(ShutdownMode.Normal, trans3, true);
				Assert.AreEqual(2, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				pool.Release(ShutdownMode.Normal, trans2, true);
				Assert.AreEqual(1, pool.OpenTransactionCount());
				Assert.AreEqual(1, pool.OpenFileCount());
			}
			finally
			{
				Assert.IsFalse(db.IsClosed());
				Assert.IsFalse(pool.IsClosed());
				pool.Close();
				Assert.IsTrue(db.IsClosed());
				Assert.IsTrue(pool.IsClosed());
				Assert.AreEqual(0, pool.OpenFileCount());
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			ClientTransactionTestUtil.DeleteFiles();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			ClientTransactionTestUtil.DeleteFiles();
		}
	}
}
#endif // !SILVERLIGHT
