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
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Soda
{
	public class STValueTypesOrderByTestCase : AbstractDb4oTestCase
	{
		const int ThingsCount = 10;

		private readonly List<object> _items = new List<object>();
		protected override void Store()
		{
			for (int i = 0; i < ThingsCount; i++)
			{
				object item = ItemVariable().New(i);
				_items.Add(item);
				Store(item);
			}
		}

		public void TestOrderByDescendingValueType()
		{
			AssertOrderBy(
				CompareDescending,
				delegate(IQuery target) { target.OrderDescending(); });
		}

		public void TestOrderByAscendingValueType()
		{
			AssertOrderBy(
				CompareAscending,
				delegate(IQuery target) { target.OrderAscending(); }
			);
		}

		private void AssertOrderBy(Comparison<object> comparison, Action<IQuery> setOrderBy)
		{
			_items.Sort(comparison);
			IQuery query = NewQuery(ItemVariable().Type());
			setOrderBy(query.Descend("_value"));

			Iterator4Assert.AreEqual(
				_items.ToArray(),
				query.Execute().GetEnumerator());
		}

		private static int CompareDescending(object lhs, object rhs)
		{
			return ItemVariable().Compare(rhs, lhs);
		}

		private static int CompareAscending(object lhs, object rhs)
		{
			return ItemVariable().Compare(lhs, rhs);
		}

		private static IValueTypeFixture ItemVariable()
		{
			return (IValueTypeFixture)STValueTypeOrderByTestSuite.VALUE_TYPE_TYPE_VARIABLE.Value;
		}
	}
}
