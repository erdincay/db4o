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
using System.Collections;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    class DictionaryBaseTestCase: AbstractDb4oTestCase
    {
#if !CF && !SILVERLIGHT
        private static string[] DATA = new string[] {"one", "two", "three"};

        public class Item
        {
            public String _name;

            public CustomDictionary _dictionary;
        }

        public class CustomDictionary : DictionaryBase
        {
            public void Add(object key, object value)
            {
                Dictionary.Add(key, value);
            }

            public object this[object key]
            {
                get
                {
                    return Dictionary[key];
                }
            }
        }

        protected override void Store()
        {
            foreach (String str in DATA)
            {
                Item item = new Item();
                item._name = str;
                item._dictionary = new CustomDictionary();
                item._dictionary.Add(str, str);
                Store(item);
            }
        }

        public void TestRetrieve()
        {
            IQuery q = Db().Query();
            q.Constrain(typeof(Item));
            IObjectSet result = q.Execute();
            Assert.AreEqual(DATA.Length, result.Count);
            foreach(Item item in result)
            {
                object key = item._name;
                object value = item._dictionary[key];
                Assert.AreEqual(item._name, value);
            }
        }

        public void TestQuery()
        {
            foreach(String str in DATA)
            {
                IQuery q = Db().Query();
                q.Constrain(typeof(Item));
                q.Descend("_dictionary").Constrain(str);
                IObjectSet result = q.Execute();
                Assert.AreEqual(1, result.Count);
                Item item = (Item) result[0];
                Assert.AreEqual(str, item._name);
                Assert.AreEqual(str, item._dictionary[str]);
            }
        }
#endif

    }
    
}
