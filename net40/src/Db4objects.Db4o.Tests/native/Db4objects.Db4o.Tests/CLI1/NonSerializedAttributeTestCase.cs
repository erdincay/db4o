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

namespace Db4objects.Db4o.Tests.CLI1
{
    public class NonSerializedAttributeTestCase : AbstractDb4oTestCase
    {
		public struct Pair
		{
			public Pair(string name, int value)
			{
				Value = value;
				Name = name;
			}

			public override string ToString()
			{
				return string.Format("Pair({0}, {1}", Name, Value);
			}

			public int Value;
			public string Name;
		}

        public class Item
        {
            [NonSerialized]
            public int NonSerializedValue;

            [Transient]
            public int TransientValue;

        	[Transient]
			public Pair Pair;

            public int Value;
            
            public Item()
            {   
            }
            
            public Item(int value)
            {
                Value = value;
                NonSerializedValue = value;
                TransientValue = value;
				Pair = new Pair("p1", value);
            }
        }
        
        public class DerivedItem : Item
        {
            public DerivedItem()
            {   
            }
            
            public DerivedItem(int value) : base(value)
            {
            }
        }

        protected override void Store()
        {
            Store(new Item(42));
            Store(new DerivedItem(42));
        }
        
        public void Test()
        {
            IObjectSet found = NewQuery(typeof(Item)).Execute();
            Assert.AreEqual(2, found.Count);
            foreach (Item item in found)
            {
                Assert.AreEqual(0, item.NonSerializedValue);
                Assert.AreEqual(0, item.TransientValue);
				Assert.AreEqual(new Pair(), item.Pair);
                Assert.AreEqual(42, item.Value);
            }
        }
    }
}
