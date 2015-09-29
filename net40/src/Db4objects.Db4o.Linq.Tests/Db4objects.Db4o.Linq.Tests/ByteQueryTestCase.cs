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
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Db4oUnit;

namespace Db4objects.Db4o.Linq.Tests
{
    public class ByteQueryTestCase : AbstractDb4oLinqTestCase
    {
        public class Item
        {
            public byte b;
            public string name;
            public string pass;
        }

        protected override void Store()
        {
            Store(new Item { b = 0, name = "foo", pass = "foo" });
            Store(new Item { b = 1, name = "bar", pass = "bar" });
        }

        public void TestQuery()
        {
            IEnumerable<Item> result = from Item item in Db()
                                       where item.b == (byte)0 && item.name == "foo" && item.pass == "foo"
                                       select item;
            Assert.AreEqual(1, result.Count());
        }
    }
}
