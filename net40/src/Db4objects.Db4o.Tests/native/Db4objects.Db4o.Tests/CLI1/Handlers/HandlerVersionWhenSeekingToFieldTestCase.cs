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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	public class HandlerVersionWhenSeekingToFieldTestCase : FormatMigrationTestCaseBase
	{
		protected override void AssertObjectsAreReadable(IExtObjectContainer objectContainer)
		{
			var result = QuerySpecificObjects(objectContainer);
			
			Assert.AreEqual(1, result.Count);

			var item = (Item) result[0];
			Assert.AreEqual(NewItem(1971, 5, 1), item);
		}

		protected override void AssertObjectsAreUpdated(IExtObjectContainer objectContainer)
		{
			var result = QuerySpecificObjects(objectContainer);
			Assert.AreEqual(2, result.Count);
		}

		protected override void Update(IExtObjectContainer objectContainer)
		{
			objectContainer.Store(NewItem(2004, 2, 23));
			objectContainer.Commit();
		}

		private static IObjectSet QuerySpecificObjects(IExtObjectContainer objectContainer)
		{
			var query = objectContainer.Query();
			query.Constrain(typeof (Item));
			query.Descend("value").Constrain(42);

			return query.Execute();
		}

		protected override string FileNamePrefix()
		{
			return "SeekToField";
		}

		protected override void Store(IObjectContainerAdapter objectContainer)
		{
			objectContainer.Store(NewItem(1971, 5, 1));
		}

		private static Item NewItem(int year, int month, int day)
		{
			return new Item(new DateTime(year, month, day), 42);
		}
	}

	public class Item
	{
		private DateTime date;
		private int value;

		public Item(DateTime date, int value)
		{
			this.date = date;
			this.value = value;
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != typeof(Item)) return false;

			Item other = (Item) obj;
			return other.date == date && other.value == value;
		}
	}
}
