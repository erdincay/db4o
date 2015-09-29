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
using System.Collections.Generic;
using System.Linq;

namespace Db4objects.Db4o.Linq.Tests
{
	public class StringMethodTestCase : AbstractDb4oLinqTestCase
	{
		public class Person
		{
			public string Name;

			public override bool Equals(object obj)
			{
				Person p = obj as Person;
				if (p == null) return false;

				return p.Name == Name;
			}

			public override int GetHashCode()
			{
				return Name.GetHashCode();
			}
		}

		private static Person[] _people = new[] 
											{
												new Person { Name = "BiroBiro" },
												new Person { Name = "Luna" }, 
												new Person { Name = "Loustic" },
												new Person { Name = "Loupiot" },
												new Person { Name = "LeMiro" },
												new Person { Name = "Tounage" }
											};

		protected IEnumerable<Person> People()
		{
			return _people;
		}

		protected override void Store()
		{
			Store(new Person { Name = "BiroBiro" });
			Store(new Person { Name = "Luna" });
			Store(new Person { Name = "Loustic" });
			Store(new Person { Name = "Loupiot" });
			Store(new Person { Name = "LeMiro" });
			Store(new Person { Name = "Tounage" });
		}

		public void TestStartsWith()
		{
			AssertQuery("(Person(Name startswith 'Lo'))",
				delegate
				{
					var los = from Person p in Db()
								where p.Name.StartsWith("Lo")
								select p;

					AssertSet(new[]
						{
							new Person { Name = "Loustic" },
							new Person { Name = "Loupiot" }
						}, los);
				});
		}

		public void TestEndsWith()
		{
			AssertQuery("(Person(Name endswith 'iro'))",
				delegate
				{
					var los = from Person p in Db()
							  where p.Name.EndsWith("iro")
							  select p;

					AssertSet(new[]
						{
							new Person { Name = "BiroBiro" },
							new Person { Name = "LeMiro" }
						}, los);
				});
		}

		public void TestContains()
		{
			AssertQuery("(Person(Name contains 'una'))",
				delegate
				{
					var los = from Person p in Db()
							  where p.Name.Contains("una")
							  select p;

					AssertSet(new[]
						{
							new Person { Name = "Luna" },
							new Person { Name = "Tounage" }
						}, los);
				});
		}

		public void TestComplexContains()
		{
			AssertQuery("(Person((Name contains 'Duna') or (Name contains 'iro')))",
				delegate
				{
					var los = from Person p in Db()
							  where p.Name.Contains("Duna") ||
									p.Name.Contains("iro")
							  select p;

					AssertSet(	from Person p1 in People()
						        where	p1.Name.Contains("Duna") ||
						              	p1.Name.Contains("iro")
						        select p1,
								
								los);
				});
		}
	}
}
