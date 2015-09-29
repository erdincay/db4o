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
	public class ClientTransactionHandleTestCase : ITestLifeCycle
	{
		public virtual void TestHandles()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.Storage = new MemoryStorage();
			LocalObjectContainer db = (LocalObjectContainer)Db4oFactory.OpenFile(config, ClientTransactionTestUtil
				.MainfileName);
			ClientTransactionPool pool = new ClientTransactionPool(db);
			try
			{
				ClientTransactionHandle handleA = new ClientTransactionHandle(pool);
				Assert.AreEqual(db, handleA.Transaction().Container());
				ClientTransactionHandle handleB = new ClientTransactionHandle(pool);
				Assert.AreNotEqual(handleA.Transaction(), handleB.Transaction());
				Assert.AreEqual(db, handleB.Transaction().Container());
				Assert.AreEqual(2, pool.OpenTransactionCount());
				Assert.AreEqual(1, pool.OpenFileCount());
				handleA.AcquireTransactionForFile(ClientTransactionTestUtil.FilenameA);
				Assert.AreEqual(3, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				Assert.AreNotEqual(db, handleA.Transaction().Container());
				handleB.AcquireTransactionForFile(ClientTransactionTestUtil.FilenameA);
				Assert.AreEqual(4, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				Assert.AreNotEqual(handleA.Transaction(), handleB.Transaction());
				Assert.AreEqual(handleA.Transaction().Container(), handleB.Transaction().Container
					());
				handleA.ReleaseTransaction(ShutdownMode.Normal);
				Assert.AreEqual(db, handleA.Transaction().Container());
				Assert.AreNotEqual(db, handleB.Transaction().Container());
				Assert.AreEqual(3, pool.OpenTransactionCount());
				Assert.AreEqual(2, pool.OpenFileCount());
				handleB.ReleaseTransaction(ShutdownMode.Normal);
				Assert.AreEqual(db, handleB.Transaction().Container());
				Assert.AreEqual(2, pool.OpenTransactionCount());
				Assert.AreEqual(1, pool.OpenFileCount());
				handleB.Close(ShutdownMode.Normal);
				Assert.AreEqual(1, pool.OpenTransactionCount());
				handleA.Close(ShutdownMode.Normal);
				Assert.AreEqual(0, pool.OpenTransactionCount());
			}
			finally
			{
				pool.Close();
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
