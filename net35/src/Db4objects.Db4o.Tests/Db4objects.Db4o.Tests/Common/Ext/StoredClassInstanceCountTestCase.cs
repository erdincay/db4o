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
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Ext;

namespace Db4objects.Db4o.Tests.Common.Ext
{
	public class StoredClassInstanceCountTestCase : AbstractDb4oTestCase
	{
		public class ItemA
		{
		}

		public class ItemB
		{
		}

		private const int CountA = 5;

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int idx = 0; idx < CountA; idx++)
			{
				Store(new StoredClassInstanceCountTestCase.ItemA());
			}
			Store(new StoredClassInstanceCountTestCase.ItemB());
		}

		public virtual void TestInstanceCount()
		{
			AssertInstanceCount(typeof(StoredClassInstanceCountTestCase.ItemA), CountA);
			AssertInstanceCount(typeof(StoredClassInstanceCountTestCase.ItemB), 1);
			Store(new StoredClassInstanceCountTestCase.ItemA());
			DeleteAll(typeof(StoredClassInstanceCountTestCase.ItemB));
			AssertInstanceCount(typeof(StoredClassInstanceCountTestCase.ItemA), CountA + 1);
			AssertInstanceCount(typeof(StoredClassInstanceCountTestCase.ItemB), 0);
		}

		public virtual void TestTransactionalInstanceCount()
		{
			if (!IsMultiSession())
			{
				return;
			}
			IExtObjectContainer otherClient = OpenNewSession();
			Store(new StoredClassInstanceCountTestCase.ItemA());
			DeleteAll(typeof(StoredClassInstanceCountTestCase.ItemB));
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemA), CountA 
				+ 1);
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemB), 0);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemA), 
				CountA);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemB), 
				1);
			Db().Commit();
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemA), CountA 
				+ 1);
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemB), 0);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemA), 
				CountA + 1);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemB), 
				0);
			otherClient.Commit();
			otherClient.Store(new StoredClassInstanceCountTestCase.ItemB());
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemB), 0);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemB), 
				1);
			otherClient.Commit();
			AssertInstanceCount(Db(), typeof(StoredClassInstanceCountTestCase.ItemB), 1);
			AssertInstanceCount(otherClient, typeof(StoredClassInstanceCountTestCase.ItemB), 
				1);
			otherClient.Close();
		}

		private void AssertInstanceCount(Type clazz, int expectedCount)
		{
			AssertInstanceCount(Db(), clazz, expectedCount);
		}

		private void AssertInstanceCount(IExtObjectContainer container, Type clazz, int expectedCount
			)
		{
			IStoredClass storedClazz = container.Ext().StoredClass(clazz);
			Assert.AreEqual(expectedCount, storedClazz.InstanceCount());
		}

		public static void Main(string[] args)
		{
			new StoredClassInstanceCountTestCase().RunAll();
		}
	}
}
