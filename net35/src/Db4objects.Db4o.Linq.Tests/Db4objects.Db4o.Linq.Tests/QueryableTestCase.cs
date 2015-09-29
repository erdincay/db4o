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
using System.Linq;
using System.Linq.Expressions;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests
{
	public class QueryableTestCase : AbstractDb4oLinqTestCase
	{
#if !CF_3_5
		public class Person
		{
			public string Name;
			public int Age;

			public override int GetHashCode()
			{
				return Age ^ Name.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				var p = obj as Person;
				if (p == null) return false;

				return Name == p.Name && Age == p.Age;
			}
		}

		protected override void Store()
		{
			Store(new Person { Name = "jb", Age = 26 });
			Store(new Person { Name = "jb", Age = 28 });
			Store(new Person { Name = "ro", Age = 32 });
			Store(new Person { Name = "an", Age = 22 });
		}

		public void TestQueryableWhere()
		{
			AssertQuery ("(Person(Name == 'jb'))", () =>
			{
				var queryable = Db().AsQueryable<Person>().Where(p => p.Name == "jb");

				Assert.AreEqual("jb", queryable.First().Name);
			});
		}

		public void TestQueryableWhereOrderBy()
		{
			AssertQuery ("(Person(Age)(Name == 'jb'))(orderby Age asc)", () =>
			{
				var aggregate = Db().AsQueryable<Person>().Where(p => p.Name == "jb").OrderBy(p => p.Age).Aggregate(0, (i, p) => p.Age + i);

				Assert.AreEqual(26 + 28, aggregate);
			});
		}

		public void TestQueryableMixDb4oEnumerable()
		{
			AssertQuery ("(Person(Name == 'jb'))", () =>
			{
				bool b = Db().AsQueryable<Person>().Where(p => p.Name == "jb").Select(p => p.Age).OrderBy(i => (i % 2)).Where(i => i > 27).Any();

				Assert.IsTrue(b);
			});
		}
#endif
	}
}

