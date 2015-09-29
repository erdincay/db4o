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

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
    public class NullableDateTimeTestCase : AbstractDb4oTestCase
    {
        public class Item
        {
            public DateTime? _typedDateTime;

            public object _untypedDateTime;

            public DateTime?[] _typedArray;

            public object _untypedArray;

			public Item()
			{
			}

            public Item(DateTime? value)
            {
				_typedDateTime = value;
                _untypedDateTime = value;
                _typedArray = new[] {value};
                _untypedArray = new[] {value};
            }
        }

        protected override void Store()
        {
        	Store(new Item(null));
        }

		protected override void Configure(Config.IConfiguration config)
		{
			config.ObjectClass(typeof(Item)).ObjectField("_typedDateTime").Indexed(true);
		}

        public void TestRetrievedIsNull()
        {
            Item item = RetrieveOnlyItem();
            AssertItemDateTime(item, null);
        }

        private Item RetrieveOnlyItem()
        {
            return (Item) RetrieveOnlyInstance(typeof (Item));
        }

        public void TestUpdate()
        {
            DateTime updatedDateTime = new DateTime(2009, 2, 18);
            Item item = RetrieveOnlyItem();
            UpdateDateTime(item, updatedDateTime);
            StoreCommitRefresh(item);
            AssertItemDateTime(item, updatedDateTime);
            UpdateDateTime(item, null);
            StoreCommitRefresh(item);
            AssertItemDateTime(item, null);
        }

        private void UpdateDateTime(Item item, DateTime? updatedDateTime)
        {
            item._typedDateTime = updatedDateTime;
            item._untypedDateTime = updatedDateTime;
            item._typedArray[0] = updatedDateTime;
            ((DateTime?[]) item._untypedArray)[0] = updatedDateTime;
        }

        private void StoreCommitRefresh(Item item)
        {
            Store(item);
            Db().Commit();
            Db().Refresh(item, int.MaxValue);
        }

        private void AssertItemDateTime(Item item, DateTime? expectedDateTime)
        {
            Assert.AreEqual(expectedDateTime, item._typedDateTime);
            Assert.AreEqual(expectedDateTime, item._untypedDateTime);
            Assert.AreEqual(expectedDateTime, item._typedArray[0]);
            Assert.AreEqual(expectedDateTime, ((DateTime?[])item._untypedArray)[0]);
        }

    }
}
