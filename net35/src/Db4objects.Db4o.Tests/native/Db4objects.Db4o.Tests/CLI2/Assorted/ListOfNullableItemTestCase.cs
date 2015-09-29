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
using System.Collections.Generic;
using Db4oUnit.Extensions;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
    public class ListOfNullableItemTestCase : AbstractDb4oTestCase
    {
        public class Item
        {
            public IList<int?> nullableList;
            public Item(IList<int?> nullableList_)
            {
                nullableList = nullableList_;
            }

        }

        private static IList<int?> nullableIntList1()
        {
            return new List<int?>(new int?[] { 1, 2, 3 });
        }

        protected override void Store()
        {
            Item item = new Item(nullableIntList1());
            Store(item);
        }

        public void test() 
        {
            Item item = (Item)RetrieveOnlyInstance(typeof(Item));
            Assert.IsNotNull(item.nullableList);
            Iterator4Assert.AreEqual(nullableIntList1().GetEnumerator(), item.nullableList.GetEnumerator());
        }
    }
}
