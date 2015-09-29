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
	/// <summary>COR-1188</summary>
	public class OrderFollowedByConstraintTestCase : AbstractDb4oTestCase
	{
		public class Data
		{
			public int _id;

			public Data(int id)
			{
				_id = id;
			}
		}

		private static readonly int[] Ids = new int[] { 42, 47, 11, 1, 50, 2 };

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int idIdx = 0; idIdx < Ids.Length; idIdx++)
			{
				Store(new OrderFollowedByConstraintTestCase.Data(Ids[idIdx]));
			}
		}

		public virtual void TestOrderFollowedByConstraint()
		{
			IQuery query = NewQuery(typeof(OrderFollowedByConstraintTestCase.Data));
			query.Descend("_id").OrderAscending();
			query.Descend("_id").Constrain(0).Greater();
			AssertOrdered(query.Execute());
		}

		public virtual void TestLastOrderWins()
		{
			IQuery query = NewQuery(typeof(OrderFollowedByConstraintTestCase.Data));
			query.Descend("_id").OrderDescending();
			query.Descend("_id").OrderAscending();
			query.Descend("_id").Constrain(0).Greater();
			AssertOrdered(query.Execute());
		}

		private void AssertOrdered(IObjectSet result)
		{
			Assert.AreEqual(Ids.Length, result.Count);
			int previousId = 0;
			while (result.HasNext())
			{
				OrderFollowedByConstraintTestCase.Data data = (OrderFollowedByConstraintTestCase.Data
					)result.Next();
				Assert.IsTrue(previousId < data._id);
				previousId = data._id;
			}
		}

		public static void Main(string[] args)
		{
			new OrderFollowedByConstraintTestCase().RunSolo();
		}
	}
}
