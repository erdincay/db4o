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
	public class DescendIndexQueryTestCase : AbstractDb4oTestCase
	{
		public class Parent
		{
			public DescendIndexQueryTestCase.Child _child;

			public string _name;
		}

		public class Child
		{
			public DescendIndexQueryTestCase.Child _child;

			public int _id;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			IObjectClass parentObjectClass = config.ObjectClass(typeof(DescendIndexQueryTestCase.Parent
				));
			parentObjectClass.ObjectField("_child").Indexed(true);
			parentObjectClass.ObjectField("_name").Indexed(true);
			IObjectClass childObjectClass = config.ObjectClass(typeof(DescendIndexQueryTestCase.Child
				));
			childObjectClass.ObjectField("_child").Indexed(true);
			childObjectClass.ObjectField("_id").Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			StoreParent("one", 0);
			StoreParent("two", 0);
			StoreParent("two", 10);
			StoreParent("three", 0);
			StoreParent("three", 10);
			StoreParent("three", 100);
		}

		private void StoreParent(string name, int addToId)
		{
			DescendIndexQueryTestCase.Parent parent = new DescendIndexQueryTestCase.Parent();
			parent._name = name;
			DescendIndexQueryTestCase.Child previousChild = null;
			for (int i = 4; i >= 0; i--)
			{
				DescendIndexQueryTestCase.Child currentChild = new DescendIndexQueryTestCase.Child
					();
				currentChild._id = i + addToId;
				currentChild._child = previousChild;
				previousChild = currentChild;
			}
			parent._child = previousChild;
			Store(parent);
		}

		public virtual void TestDescendParentName()
		{
			IQuery q = NewQuery(typeof(DescendIndexQueryTestCase.Parent));
			q.Descend("_name").Constrain("two");
			q.Descend("_child").Descend("_id").Constrain(0);
			AssertResultSize(q, 1);
		}

		public virtual void TestDescendParentNameSubQuery()
		{
			IQuery q = NewQuery(typeof(DescendIndexQueryTestCase.Parent));
			q.Descend("_name").Constrain("two");
			IQuery qChild = q.Descend("_child");
			qChild.Descend("_id").Constrain(0);
			AssertResultSize(qChild, 1);
		}

		public virtual void TestDescendChildId()
		{
			IQuery q = NewQuery(typeof(DescendIndexQueryTestCase.Parent));
			q.Descend("_child").Descend("_id").Constrain(0);
			AssertResultSize(q, 3);
		}

		public virtual void TestDescendChildIdSubQuery()
		{
			IQuery q = NewQuery(typeof(DescendIndexQueryTestCase.Parent));
			IQuery qChild = q.Descend("_child");
			qChild.Descend("_id").Constrain(0);
			AssertResultSize(qChild, 3);
		}

		public virtual void TestImplicitAndChildId()
		{
			IQuery q = NewQuery(typeof(DescendIndexQueryTestCase.Parent));
			IQuery qChild = q.Descend("_child");
			IQuery qId = qChild.Descend("_id");
			qId.Constrain(0);
			qId.Constrain(10);
			qId.Constrain(100);
			AssertResultSize(q, 0);
		}

		public virtual void TestOrChildId()
		{
			IQuery q = NewQuery(typeof(DescendIndexQueryTestCase.Parent));
			IQuery qChild = q.Descend("_child");
			IQuery qId = qChild.Descend("_id");
			IConstraint c1 = qId.Constrain(0);
			IConstraint c2 = qId.Constrain(10);
			IConstraint c3 = qId.Constrain(100);
			c1.Or(c2).Or(c3);
			AssertResultSize(q, 6);
		}

		private void AssertResultSize(IQuery q, int count)
		{
			Assert.AreEqual(count, q.Execute().Count);
		}
	}
}
