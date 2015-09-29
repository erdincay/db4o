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
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class QueryByExampleTestCase : AbstractDb4oTestCase
	{
		internal const int Count = 10;

		internal static QueryByExampleTestCase.LinkedList list = QueryByExampleTestCase.LinkedList
			.NewLongCircularList();

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		public static void Main(string[] args)
		{
			new QueryByExampleTestCase().RunAll();
		}

		protected override void Store()
		{
			Store(list);
		}

		public virtual void TestDefaultQueryModeIsIdentity()
		{
			QueryByExampleTestCase.Item itemOne = new QueryByExampleTestCase.Item("one");
			QueryByExampleTestCase.Item itemTwo = new QueryByExampleTestCase.Item("two");
			Store(itemOne);
			Store(itemTwo);
			// Change the name of the "sample"
			itemOne._name = "two";
			// Query by Identity
			IQuery q = Db().Query();
			q.Constrain(itemOne);
			IObjectSet objectSet = q.Execute();
			AssertItem(objectSet, itemOne);
		}

		public virtual void TestConstrainByExample()
		{
			QueryByExampleTestCase.Item itemOne = new QueryByExampleTestCase.Item("one");
			QueryByExampleTestCase.Item itemTwo = new QueryByExampleTestCase.Item("two");
			Store(itemOne);
			Store(itemTwo);
			// Change the name of the "sample"
			itemOne._name = "two";
			// Query by Example
			IQuery q = Db().Query();
			q.Constrain(itemOne).ByExample();
			IObjectSet objectSet = q.Execute();
			// Expect to get the other 
			AssertItem(objectSet, itemTwo);
		}

		private void AssertItem(IObjectSet objectSet, QueryByExampleTestCase.Item item)
		{
			Assert.AreEqual(1, objectSet.Count);
			QueryByExampleTestCase.Item retrievedItem = (QueryByExampleTestCase.Item)objectSet
				.Next();
			Assert.AreSame(item, retrievedItem);
		}

		public virtual void TestQueryByExample()
		{
			QueryByExampleTestCase.Item itemOne = new QueryByExampleTestCase.Item("one");
			QueryByExampleTestCase.Item itemTwo = new QueryByExampleTestCase.Item("two");
			Store(itemOne);
			Store(itemTwo);
			// Change the name of the "sample"
			itemOne._name = "two";
			// Query by Example
			IObjectSet objectSet = Db().QueryByExample(itemOne);
			AssertItem(objectSet, itemTwo);
		}

		public virtual void TestQueryByExampleNoneFound()
		{
			QueryByExampleTestCase.Item itemOne = new QueryByExampleTestCase.Item("one");
			QueryByExampleTestCase.Item itemTwo = new QueryByExampleTestCase.Item("two");
			Store(itemOne);
			Store(itemTwo);
			// Change the name of the "sample"
			itemOne._name = "three";
			IObjectSet objectSet = Db().QueryByExample(itemOne);
			Assert.AreEqual(0, objectSet.Count);
		}

		public virtual void TestByExample()
		{
			IQuery q = Db().Query();
			q.Constrain(list).ByExample();
			IObjectSet result = q.Execute();
			Assert.AreEqual(Count, result.Count);
		}

		public virtual void TestByIdentity()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(QueryByExampleTestCase.LinkedList));
			IObjectSet result = q.Execute();
			Assert.AreEqual(Count, result.Count);
			while (result.HasNext())
			{
				Db().Delete(result.Next());
			}
			q = Db().Query();
			q.Constrain(typeof(QueryByExampleTestCase.LinkedList));
			result = q.Execute();
			Assert.AreEqual(0, result.Count);
			QueryByExampleTestCase.LinkedList newList = QueryByExampleTestCase.LinkedList.NewLongCircularList
				();
			Db().Store(newList);
			q = Db().Query();
			q.Constrain(newList);
			result = q.Execute();
			Assert.AreEqual(1, result.Count);
		}

		public virtual void TestClassConstraint()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(QueryByExampleTestCase.LinkedList));
			IObjectSet result = q.Execute();
			Assert.AreEqual(Count, result.Count);
			q = Db().Query();
			q.Constrain(typeof(QueryByExampleTestCase.LinkedList)).ByExample();
			result = q.Execute();
			Assert.AreEqual(Count, result.Count);
		}

		public class LinkedList
		{
			public QueryByExampleTestCase.LinkedList _next;

			[System.NonSerialized]
			public int _depth;

			public static QueryByExampleTestCase.LinkedList NewLongCircularList()
			{
				QueryByExampleTestCase.LinkedList head = new QueryByExampleTestCase.LinkedList();
				QueryByExampleTestCase.LinkedList tail = head;
				for (int i = 1; i < Count; i++)
				{
					tail._next = new QueryByExampleTestCase.LinkedList();
					tail = tail._next;
					tail._depth = i;
				}
				tail._next = head;
				return head;
			}

			public override string ToString()
			{
				return "List[" + _depth + "]";
			}
		}
	}
}
