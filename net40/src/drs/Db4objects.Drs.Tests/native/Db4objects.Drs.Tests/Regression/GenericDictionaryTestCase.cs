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
using System.Collections;
using Db4objects.Drs.Inside;
using Db4oUnit;

namespace Db4objects.Drs.Tests.Regression
{
    class GenericDictionaryTestCase : GenericCollectionTestCaseBase
    {
        public class Container
        {
            public string _name;
            public IDictionary<string, int> _items;

            public Container(string name, IDictionary<string, int> items)
            {
                _name = name;
                _items = items;
            }
        }

        public override object CreateItem()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < 10; i++)
            {
                dictionary.Add("Key " + i, i);
            }
            Container container = new Container("Dictionary Item", dictionary);
            return container;
        }

        public override void EnsureContent(ITestableReplicationProviderInside provider)
        {
            Container replicated = (Container)QueryItem(provider, typeof(Container));
            Container expected = (Container)CreateItem();
            Assert.AreNotSame(expected, replicated);
            Assert.AreEqual(expected._name, replicated._name);
            Iterator4Assert.AreEqual(expected._items.GetEnumerator(), replicated._items.GetEnumerator());
        }
    }
}
