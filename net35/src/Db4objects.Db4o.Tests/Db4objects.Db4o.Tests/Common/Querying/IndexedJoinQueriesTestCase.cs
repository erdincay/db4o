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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class IndexedJoinQueriesTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public int _id;

			public string _name;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < 10; i++)
			{
				IndexedJoinQueriesTestCase.Item item = new IndexedJoinQueriesTestCase.Item();
				item._id = i;
				item._name = i < 5 ? "A" : "B";
				Store(item);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			IObjectClass objectClass = config.ObjectClass(typeof(IndexedJoinQueriesTestCase.Item
				));
			objectClass.ObjectField("_id").Indexed(true);
			objectClass.ObjectField("_name").Indexed(true);
		}

		public virtual void TestSimpleAndExpectOne()
		{
			IQuery q = NewItemQuery();
			IConstraint c1 = q.Descend("_id").Constrain(3);
			IConstraint c2 = q.Descend("_name").Constrain("A");
			c1.And(c2);
			AssertResultSize(q, 1);
		}

		public virtual void TestSimpleAndExpectNone()
		{
			IQuery q = NewItemQuery();
			IConstraint c1 = q.Descend("_id").Constrain(3);
			IConstraint c2 = q.Descend("_name").Constrain("B");
			c1.And(c2);
			AssertResultSize(q, 0);
		}

		public virtual void TestSimpleOrExpectTwo()
		{
			IQuery q = NewItemQuery();
			IConstraint c1 = q.Descend("_id").Constrain(3);
			IConstraint c2 = q.Descend("_id").Constrain(4);
			c1.Or(c2);
			AssertResultSize(q, 2);
		}

		public virtual void TestSimpleOrExpectOne()
		{
			IQuery q = NewItemQuery();
			IConstraint c1 = q.Descend("_id").Constrain(3);
			IConstraint c2 = q.Descend("_id").Constrain(11);
			c1.Or(c2);
			AssertResultSize(q, 1);
		}

		public virtual void TestSimpleOrExpectNone()
		{
			IQuery q = NewItemQuery();
			IConstraint c1 = q.Descend("_id").Constrain(11);
			IConstraint c2 = q.Descend("_id").Constrain(13);
			c1.Or(c2);
			AssertResultSize(q, 0);
		}

		public virtual void TestThreeOrsExpectTen()
		{
			IQuery q = NewItemQuery();
			IConstraint c1 = q.Descend("_name").Constrain("A");
			IConstraint c2 = q.Descend("_name").Constrain("B");
			IConstraint c3 = q.Descend("_name").Constrain("C");
			c1.Or(c2).Or(c3);
			AssertResultSize(q, 10);
		}

		public virtual void TestAndOr()
		{
			IQuery q = NewItemQuery();
			IConstraint c1 = q.Descend("_id").Constrain(1);
			IConstraint c2 = q.Descend("_id").Constrain(2);
			IConstraint c3 = q.Descend("_name").Constrain("A");
			c1.Or(c2).And(c3);
			AssertResultSize(q, 2);
		}

		public virtual void TestOrAnd()
		{
			IQuery q = NewItemQuery();
			IConstraint c1 = q.Descend("_id").Constrain(1);
			IConstraint c2 = q.Descend("_name").Constrain("A");
			IConstraint c3 = q.Descend("_name").Constrain("B");
			c1.And(c2).Or(c3);
			AssertResultSize(q, 6);
		}

		private void AssertResultSize(IQuery q, int count)
		{
			Assert.AreEqual(count, q.Execute().Count);
		}

		private IQuery NewItemQuery()
		{
			return NewQuery(typeof(IndexedJoinQueriesTestCase.Item));
		}
	}
}
