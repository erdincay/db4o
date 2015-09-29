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
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class StructsTestCase : AbstractDb4oTestCase, IOptOutSilverlight
	{
		public static string GUID = "6a0d8033-444e-4b44-b0df-bf33dfe050f9";

		public class Item
		{
			public SimpleStruct simpleStruct;
			public RecursiveStruct recursiveStruct;
			public Guid guid;
		}

		public struct SimpleStruct
		{
			public int foo;
			public string bar;
		}

		public struct RecursiveStruct
		{
			public Item child;
		}

		protected override void  Store()
	    {
			Item item = new Item();
			item.simpleStruct.foo = 100;
			item.simpleStruct.bar = "first";

			RecursiveStruct r = new RecursiveStruct();
			r.child = new Item();
			r.child.simpleStruct.foo = 22;
			r.child.simpleStruct.bar = "second";

			item.recursiveStruct = r;

			item.guid = new Guid(GUID);
		    
		    Store(item);
		}

		public void TestQueryOnStructField()
		{
			Item item = QuerySingleItemByStructFoo(100);

			Assert.AreEqual(GUID, item.guid.ToString());
            Assert.AreEqual(100, item.simpleStruct.foo);
            Assert.AreEqual("first", item.simpleStruct.bar);
            Assert.AreEqual(22, item.recursiveStruct.child.simpleStruct.foo);
            Assert.AreEqual("second", item.recursiveStruct.child.simpleStruct.bar);

			Assert.AreSame(item.recursiveStruct.child, QuerySingleItemByStructFoo(22));
		}

		public void TestUpdate()
		{
            Assert.AreEqual(2, Db().Ext().StoredClass(typeof(SimpleStruct)).InstanceCount());
            Assert.AreEqual(2, Db().Ext().StoredClass(typeof(RecursiveStruct)).InstanceCount());
            Item item = QuerySingleItemByStructFoo(22);

            Db().Store(item, Int32.MaxValue);
            Db().Commit();
            Assert.AreEqual(2, Db().Ext().StoredClass(typeof(SimpleStruct)).InstanceCount());
            Assert.AreEqual(2, Db().Ext().StoredClass(typeof(RecursiveStruct)).InstanceCount());
        }

		// TODO:
//		public void TestDeactivationToLevel1()
//		{
//			Item item = QuerySingleItemByStructFoo(100);
//			Item childBeforeDeactivation = item.recursiveStruct.child;
//		}

		private Item QuerySingleItemByStructFoo(int foo)
		{
			IObjectSet objectSet = QueryItemBySimpleStructFoo(foo);
			Assert.AreEqual(1, objectSet.Count);
			return (Item)objectSet.Next();
		}

		private IObjectSet QueryItemBySimpleStructFoo(int foo)
		{
			IQuery q = NewQuery(typeof(Item));
			q.Descend("simpleStruct")
				.Descend("foo")
				.Constrain(foo);
			return q.Execute();
		}
	}
}
