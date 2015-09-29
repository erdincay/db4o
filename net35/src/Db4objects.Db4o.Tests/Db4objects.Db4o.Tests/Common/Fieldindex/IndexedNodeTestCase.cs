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
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Fieldindex;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class IndexedNodeTestCase : FieldIndexProcessorTestCaseBase
	{
		public static void Main(string[] args)
		{
			new IndexedNodeTestCase().RunSolo();
		}

		protected override void Store()
		{
			StoreItems(new int[] { 3, 4, 7, 9 });
			StoreComplexItems(new int[] { 3, 4, 7, 9 }, new int[] { 2, 2, 8, 8 });
		}

		public virtual void TestTwoLevelDescendOr()
		{
			IQuery query = CreateComplexItemQuery();
			IConstraint c1 = query.Descend("child").Descend("foo").Constrain(4).Smaller();
			IConstraint c2 = query.Descend("child").Descend("foo").Constrain(4).Greater();
			c1.Or(c2);
			AssertSingleOrNode(query);
		}

		public virtual void TestMultipleOrs()
		{
			IQuery query = CreateComplexItemQuery();
			IConstraint c1 = query.Descend("foo").Constrain(4).Smaller();
			for (int i = 0; i < 5; i++)
			{
				IConstraint c2 = query.Descend("foo").Constrain(4).Greater();
				c1 = c1.Or(c2);
			}
			AssertSingleOrNode(query);
		}

		public virtual void TestDoubleDescendingOnIndexedNodes()
		{
			IQuery query = CreateComplexItemQuery();
			query.Descend("child").Descend("foo").Constrain(3);
			query.Descend("bar").Constrain(2);
			IIndexedNode index = SelectBestIndex(query);
			AssertComplexItemIndex("foo", index);
			Assert.IsFalse(index.IsResolved());
			IIndexedNode result = index.Resolve();
			Assert.IsNotNull(result);
			AssertComplexItemIndex("child", result);
			Assert.IsTrue(result.IsResolved());
			Assert.IsNull(result.Resolve());
			AssertComplexItems(new int[] { 4 }, result.ToTreeInt());
		}

		public virtual void TestTripleDescendingOnQuery()
		{
			IQuery query = CreateComplexItemQuery();
			query.Descend("child").Descend("child").Descend("foo").Constrain(3);
			IIndexedNode index = SelectBestIndex(query);
			AssertComplexItemIndex("foo", index);
			Assert.IsFalse(index.IsResolved());
			IIndexedNode result = index.Resolve();
			Assert.IsNotNull(result);
			AssertComplexItemIndex("child", result);
			Assert.IsFalse(result.IsResolved());
			result = result.Resolve();
			Assert.IsNotNull(result);
			AssertComplexItemIndex("child", result);
			AssertComplexItems(new int[] { 7 }, result.ToTreeInt());
		}

		private void AssertComplexItems(int[] expectedFoos, TreeInt found)
		{
			Assert.IsNotNull(found);
			AssertTreeInt(MapToObjectIds(CreateComplexItemQuery(), expectedFoos), found);
		}

		private void AssertSingleOrNode(IQuery query)
		{
			IEnumerator nodes = CreateProcessor(query).CollectIndexedNodes();
			Assert.IsTrue(nodes.MoveNext());
			OrIndexedLeaf node = (OrIndexedLeaf)nodes.Current;
			Assert.IsNotNull(node);
			Assert.IsFalse(nodes.MoveNext());
		}
	}
}
