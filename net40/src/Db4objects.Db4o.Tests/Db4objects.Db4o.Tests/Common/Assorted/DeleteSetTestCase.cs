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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class DeleteSetTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new DeleteSetTestCase().RunAll();
		}

		public class Item
		{
			public Item()
			{
			}

			public Item(int v)
			{
				value = v;
			}

			public int value;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new DeleteSetTestCase.Item(1));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeleteStore()
		{
			object item = ((DeleteSetTestCase.Item)RetrieveOnlyInstance(typeof(DeleteSetTestCase.Item
				)));
			Db().Delete(item);
			Db().Store(item);
			Db().Commit();
			AssertOccurrences(typeof(DeleteSetTestCase.Item), 1);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeleteStoreStore()
		{
			DeleteSetTestCase.Item item = (DeleteSetTestCase.Item)((DeleteSetTestCase.Item)RetrieveOnlyInstance
				(typeof(DeleteSetTestCase.Item)));
			Db().Delete(item);
			item.value = 2;
			Db().Store(item);
			item.value = 3;
			Db().Store(item);
			Db().Commit();
			AssertOccurrences(typeof(DeleteSetTestCase.Item), 1);
			item = (DeleteSetTestCase.Item)((DeleteSetTestCase.Item)RetrieveOnlyInstance(typeof(
				DeleteSetTestCase.Item)));
			Assert.AreEqual(3, item.value);
		}
	}
}
