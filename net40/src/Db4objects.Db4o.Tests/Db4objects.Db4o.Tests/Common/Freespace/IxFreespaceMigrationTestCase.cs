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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Freespace;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	public class IxFreespaceMigrationTestCase : FormatMigrationTestCaseBase
	{
		protected override void ConfigureForStore(IConfiguration config)
		{
			config.Freespace().UseIndexSystem();
		}

		protected override void Store(IObjectContainerAdapter objectContainer)
		{
			IxFreespaceMigrationTestCase.Item nextItem = null;
			for (int i = 9; i >= 0; i--)
			{
				IxFreespaceMigrationTestCase.Item storedItem = new IxFreespaceMigrationTestCase.Item
					("item" + i, nextItem);
				objectContainer.Store(storedItem);
				nextItem = storedItem;
			}
			objectContainer.Commit();
			IxFreespaceMigrationTestCase.Item item = QueryForItem(objectContainer, 0);
			for (int i = 0; i < 5; i++)
			{
				objectContainer.Delete(item);
				item = item._next;
			}
			objectContainer.Commit();
		}

		private IxFreespaceMigrationTestCase.Item QueryForItem(IObjectContainerAdapter objectContainer
			, int n)
		{
			return QueryForItem(objectContainer.Query(), n);
		}

		private IxFreespaceMigrationTestCase.Item QueryForItem(IQuery q, int n)
		{
			q.Constrain(typeof(IxFreespaceMigrationTestCase.Item));
			q.Descend("_name").Constrain("item" + n);
			return (IxFreespaceMigrationTestCase.Item)q.Execute().Next();
		}

		protected override void AssertObjectsAreReadable(IExtObjectContainer objectContainer
			)
		{
			AssertItemCount(objectContainer, 5);
			IxFreespaceMigrationTestCase.Item item = QueryForItem(objectContainer.Query(), 5);
			for (int i = 5; i < 10; i++)
			{
				Assert.AreEqual("item" + i, item._name);
				item = item._next;
			}
		}

		private void AssertItemCount(IExtObjectContainer objectContainer, int i)
		{
			IQuery q = objectContainer.Query();
			q.Constrain(typeof(IxFreespaceMigrationTestCase.Item));
			Assert.AreEqual(i, q.Execute().Count);
		}

		public class Item
		{
			public string _name;

			public IxFreespaceMigrationTestCase.Item _next;

			public Item(string name)
			{
				_name = name;
			}

			public Item(string name, IxFreespaceMigrationTestCase.Item next_)
			{
				_name = name;
				_next = next_;
			}
		}

		protected override string FileNamePrefix()
		{
			return "migrate_freespace_ix_";
		}
	}
}
