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
using Db4objects.Db4o.Ext;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class SimplestPossibleHandlerUpdateTestCase : LenientHandlerUpdateTestCaseBase
    {
        private readonly Item[] DATA = new Item[]
                {
                    new Item("foo", new TestStruct("one", 1), null),
                    new Item("bar", new TestStruct("two", 2), new TestStruct("three", 3)),
                    new Item("baz", new TestStruct("for", 4), new TestStruct("five", 5)),
                };

        protected override string TypeName()
        {
            return "simplest";
        }

        protected override object[] CreateValues()
        {
            return DATA;
        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            Item[] actual = (Item[]) values;
            Assert.AreEqual(DATA.Length, values.Length);
            for(int i = 0; i < DATA.Length; i++)
            {
                AssertItem(DATA[i], actual[i]);
            }
        }

        private void AssertItem(Item expected, Item actual)
        {
            Assert.AreEqual(expected.name, actual.name);
            AssertTestStruct(expected.value, actual.value);
            AssertNullableTestStruct(expected.boxedValue, actual.boxedValue);
            AssertNullableTestStruct(expected.nullabeValue, actual.nullabeValue);
        }

        private static void AssertNullableTestStruct(object expected, object actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
            }
            else
            {
                AssertTestStruct(
                    (TestStruct) expected, 
                    (TestStruct) actual);
            }
        }

        private static void AssertTestStruct(TestStruct expected, TestStruct actual)
        {
            Assert.AreEqual(expected.name, actual.name);
            Assert.AreEqual(expected.value, actual.value);
        }

        protected override object CreateArrays()
        {
            return null;
        }

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
        }

        private struct TestStruct
        {
            public int value;
            public string name;

            public TestStruct(string name_, int value_)
            {
                name = name_;
                value = value_;
            }
        }

        private class Item
        {
            public readonly string name;
            public readonly TestStruct value;
            public readonly TestStruct? nullabeValue;
            public readonly object boxedValue;

            public Item(string name_, TestStruct value_, TestStruct? nullableValue_)
            {
                name = name_;
                value = value_;
                nullabeValue = nullableValue_;
                boxedValue = value_;
            }
        }

        public class ItemArrays
        {
            //public NestedStruct[] _nestedStructArray;

            //public object[] _untypedArray;

            //public object _arrayInObject;

            //public NestedStruct?[] _nullableNestedStructArray;
        }

        protected override bool DefragmentInReadWriteMode()
        {
            return true;
        }

    }
}
