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
using Db4objects.Db4o.Tests.Common.Soda.Ordered;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class OrderByWithNullValuesTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public int _id;

			public string _name;

			public Item(int id, string name)
			{
				_id = id;
				_name = name;
			}

			public virtual string Name()
			{
				return _name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new OrderByWithNullValuesTestCase.Item(1, "a"));
			Store(new OrderByWithNullValuesTestCase.Item(2, null));
			Store(new OrderByWithNullValuesTestCase.Item(3, "b"));
			Store(new OrderByWithNullValuesTestCase.Item(4, null));
		}

		public virtual void TestOrderByWithNullValues()
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(OrderByWithNullValuesTestCase.Item));
			query.Descend("_name").OrderAscending();
			IObjectSet result = query.Execute();
			Assert.AreEqual(4, result.Count);
			Assert.IsNull(((OrderByWithNullValuesTestCase.Item)result.Next()).Name());
			Assert.IsNull(((OrderByWithNullValuesTestCase.Item)result.Next()).Name());
			Assert.AreEqual("a", ((OrderByWithNullValuesTestCase.Item)result.Next()).Name());
			Assert.AreEqual("b", ((OrderByWithNullValuesTestCase.Item)result.Next()).Name());
		}
	}
}
