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
using Db4objects.Db4o.Foundation;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class Db4oDrsTestSuiteBuilder : ITestSuiteBuilder
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(new Db4oDrsTestSuiteBuilder()).Run();
		}

		public virtual IEnumerator GetEnumerator()
		{
			if (false)
			{
				return new DrsTestSuiteBuilder(new Db4oDrsFixture("db4o-a"), new Db4oDrsFixture("db4o-b"
					), typeof(Db4oDrsTestSuite)).GetEnumerator();
			}
			if (false)
			{
				return new DrsTestSuiteBuilder(new Db4oClientServerDrsFixture("db4o-cs-a", unchecked(
					(int)(0xdb40))), new Db4oClientServerDrsFixture("db4o-cs-b", 4455), typeof(Db4oDrsTestSuite
					)).GetEnumerator();
			}
			return Iterators.Concat(Iterators.Concat(new DrsTestSuiteBuilder(new Db4oDrsFixture
				("db4o-a"), new Db4oDrsFixture("db4o-b"), typeof(Db4oDrsTestSuite)).GetEnumerator
				(), new DrsTestSuiteBuilder(new Db4oClientServerDrsFixture("db4o-cs-a", unchecked(
				(int)(0xdb40))), new Db4oClientServerDrsFixture("db4o-cs-b", 4455), typeof(Db4oDrsTestSuite
				)).GetEnumerator()), Iterators.Concat(new DrsTestSuiteBuilder(new Db4oDrsFixture
				("db4o-a"), new Db4oClientServerDrsFixture("db4o-cs-b", 4455), typeof(Db4oDrsTestSuite
				)).GetEnumerator(), new DrsTestSuiteBuilder(new Db4oClientServerDrsFixture("db4o-cs-a"
				, 4455), new Db4oDrsFixture("db4o-b"), typeof(Db4oDrsTestSuite)).GetEnumerator()
				));
		}
	}
}
