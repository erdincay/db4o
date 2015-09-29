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
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class EnumTestCase : AbstractDb4oTestCase
    {
        public enum MyEnum { A, B, C, D, F, INCOMPLETE }; 

        public class Item 
		{ 
            public MyEnum _enum; 
        } 

       protected override void Store()
       {
           Item item = new Item();
           item._enum = MyEnum.C;
           Store(item);
       }

       public void TestRetrieve()
       {
           Item item = (Item)RetrieveOnlyInstance(typeof(Item));
           Assert.AreEqual(MyEnum.C, item._enum);
       }

       public void TestPeekPersisted()
       {
           Item item = (Item) RetrieveOnlyInstance(typeof (Item));
           Item peeked = (Item) Db().PeekPersisted(item, int.MaxValue, true);
           Assert.AreEqual(item._enum, peeked._enum);
       }

    } 

}
