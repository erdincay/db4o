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
using Db4objects.Db4o.Tests.Common.Sampledata;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsCollections : AbstractDb4oTestCase
    {
#if !SILVERLIGHT
        ArrayList arrayList;
        Hashtable hashTable;
        Queue queue;
        Stack stack;

        public CsCollections()
        {
        }

        override protected void Store()
        {   
            CsCollections csc = new CsCollections();
            csc.Fill();
            Store(csc);
        }

        public void Test()
        {
            CsCollections csc = (CsCollections) RetrieveOnlyInstance(typeof(CsCollections));
            csc.Check();
        }

        private void Fill()
        {
            arrayList = new ArrayList();
            Fill(arrayList);

            hashTable = new Hashtable();
            Fill(hashTable);

            queue = new Queue();
            queue.Enqueue(1);
            queue.Enqueue("hi");
            queue.Enqueue(new AtomData("foo"));

            stack = new Stack();
            stack.Push(1);
            stack.Push("hi");
            stack.Push(new AtomData("foo"));
        }

        private void Check()
        {
            Check(arrayList);
            Check(hashTable);

            Assert.IsTrue(queue.Dequeue().Equals(1));
            Assert.IsTrue(queue.Dequeue().Equals("hi"));
            Assert.IsTrue(queue.Dequeue().Equals(new AtomData("foo")));

            Assert.IsTrue(stack.Pop().Equals(new AtomData("foo")));
            Assert.IsTrue(stack.Pop().Equals("hi"));
            Assert.IsTrue(stack.Pop().Equals(1));
        }

        private void Fill(IList list)
        {
            list.Add(1);
            list.Add(null);
            list.Add(new AtomData("foo"));
            list.Add("foo");
        }


        private void Check(IList list)
        {
            Assert.IsTrue(list[0].Equals(1));
            Assert.IsTrue(list[1] == null);
            Assert.IsTrue(list[2].Equals(new AtomData("foo")));
            Assert.IsTrue(list[3].Equals("foo"));
        }

        private void Fill(IDictionary dict)
        {
            dict[1] = 1;
            dict["hey"] = "ho";
            dict[new AtomData("foo")] = new AtomData("bar");
            dict[4] = "Yoman";
        }

        private void Check(IDictionary dict)
        {
            Assert.IsTrue(dict[1].Equals(1));
            Assert.IsTrue(dict["hey"].Equals("ho"));
            Assert.AreEqual(new AtomData("bar"), dict[new AtomData("foo")]);
            Assert.AreEqual("Yoman", dict[4]);
        }
#endif
    }
}
