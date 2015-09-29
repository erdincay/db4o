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
using System.Text;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class EnumHandlerUpdateTestCase : HandlerUpdateTestCaseBase
    {
        enum EnumAsInteger
        {
            First = 1,
            Second,
            Third,
        }

        class Item
        {
            public EnumAsInteger _asInteger;

            public Item(EnumAsInteger asInteger)
            {
                _asInteger = asInteger;
            }

            public override bool Equals(object obj)
            {
                Item rhs = (Item)obj;
                if (rhs == null) return false;

                if (rhs.GetType() != GetType()) return false;

                return _asInteger == rhs._asInteger;
            }

            public override string ToString()
            {
                return "Item _asInteger:" + _asInteger ;
            }
        }

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {

        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            Item item = (Item)values[0];
            Assert.AreEqual(EnumAsInteger.Second, item._asInteger);
        }

        protected override object CreateArrays()
        {
            return null;
        }

        protected override object[] CreateValues()
        {
            Item[] values = new Item[1];
            Item item = new Item(EnumAsInteger.Second);
            values[0] = item;
            return values;
        }

        protected override string TypeName()
        {
            return "enum";
        }

    }
}
