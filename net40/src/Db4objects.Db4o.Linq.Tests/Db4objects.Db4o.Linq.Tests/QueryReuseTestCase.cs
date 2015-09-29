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
namespace Db4objects.Db4o.Linq.Tests
{
	public class QueryReuseTestCase : AbstractDb4oLinqTestCase
	{
		public class Item
		{
			public int Id;

			public Item(int id)
			{
				Id = id;
			}
		}

		protected override void Store()
		{	
			Store(new Item(1));
		}

		public void TestQueryCanBeReused()
		{
			// Db().Cast<Item>().Select(item => item.Id)
			//   Db4oQuery() { q => q.Constrain(typeof(Item) }
			//      Db4oProjection() => { query = ...; projection = lambda }
			var query = from Item item in Db() select item.Id;

			AssertItemQuery(query);
			AssertItemQuery(query);
		}

		private void AssertItemQuery(IDb4oLinqQuery<int> query)
		{
			AssertQuery("(Item)", () =>
			{
				AssertSequence(new[] { 1 }, query);
			});
		}

		public void TestQueryCanBeComposed()
		{
		}
	}
}
