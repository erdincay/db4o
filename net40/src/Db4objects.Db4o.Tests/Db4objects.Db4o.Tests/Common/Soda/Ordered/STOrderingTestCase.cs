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
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	/// <summary>Tests for COR-1007</summary>
	public class STOrderingTestCase : SodaBaseTestCase, IOptOutMultiSession
	{
		public static void Main(string[] args)
		{
			new STOrderingTestCase().RunSolo();
		}

		public override object[] CreateData()
		{
			return new object[] { new OrderTestSubject("Alexandr", 30, 5), new OrderTestSubject
				("Cris", 30, 5), new OrderTestSubject("Boris", 30, 5), new OrderTestSubject("Helen"
				, 25, 5), new OrderTestSubject("Zeus", 25, 3), new OrderTestSubject("Alexsandra"
				, 25, 3), new OrderTestSubject("Liza", 25, 4), new OrderTestSubject("Bred", 25, 
				3), new OrderTestSubject("Liza", 25, 3), new OrderTestSubject("Gregory", 25, 4) };
		}

		// 0
		// 1
		// 2
		// 3
		// 4
		// 5
		// 6
		// 7
		// 8
		// 9
		public virtual void TestFirstAndSecondFieldsAreIrrelevant()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(OrderTestSubject));
			q.Descend("_seniority").OrderAscending();
			q.Descend("_age").OrderAscending();
			q.Descend("_name").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 7, 8, 4, 9, 6, 3, 0, 2, 1 });
		}

		public virtual void TestSecondAndThirdFieldsAreIrrelevant()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(OrderTestSubject));
			q.Descend("_age").OrderAscending();
			q.Descend("_name").OrderAscending();
			q.Descend("_seniority").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 7, 9, 3, 8, 6, 4, 0, 2, 1 });
		}

		public virtual void TestOrderByNameAndAgeAscending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(OrderTestSubject));
			q.Descend("_age").OrderAscending();
			q.Descend("_name").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 7, 9, 3, 6, 8, 4, 0, 2, 1 });
		}

		public virtual void TestAscendingOrderWithOutAge()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(OrderTestSubject));
			q.Descend("_seniority").OrderAscending();
			q.Descend("_name").OrderAscending();
			ExpectOrdered(q, new int[] { 5, 7, 8, 4, 9, 6, 0, 2, 1, 3 });
		}
	}
}
