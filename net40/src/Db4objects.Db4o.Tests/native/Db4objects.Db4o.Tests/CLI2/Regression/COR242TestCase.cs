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
namespace Db4objects.Db4o.Tests.CLI2.Regression
{
#if !MONO
    using System.Collections.Generic;
    using Db4oUnit;
    using Db4oUnit.Extensions;

    public class COR242TestCase : AbstractDb4oTestCase
    {
        public class Item
        {
            public IList<string> items;

            public Item(IList<string> items_)
            {
                items = items_;
            }
        }

        protected override void Store()
        {
            Store(new Item(new string[] {"foo", "bar"}));
        }

        public void _Test()
        {
            Item item = (Item) RetrieveOnlyInstance(typeof(Item));
            Assert.IsNotNull(item.items);
            Assert.IsInstanceOf(typeof(string[]), item.items);
            ArrayAssert.AreEqual(new string[] {"foo", "bar"}, (string[])item.items);
        }
    }
#endif
}
