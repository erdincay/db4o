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
using Db4objects.Db4o.Foundation;
using Db4objects.Drs.Tests.Foundation;

namespace Db4objects.Drs.Tests.Foundation
{
	public class Set4Testcase : ITestCase
	{
		public virtual void TestSingleElementIteration()
		{
			Set4 set = NewSet("first");
			Assert.AreEqual("first", Iterators.Next(set.GetEnumerator()));
		}

		public virtual void TestContainsAll()
		{
			Set4 set = NewSet("42");
			set.Add("foo");
			Assert.IsTrue(set.ContainsAll(NewSet("42")));
			Assert.IsTrue(set.ContainsAll(NewSet("foo")));
			Assert.IsTrue(set.ContainsAll(set));
			Set4 other = new Set4(set);
			other.Add("bar");
			Assert.IsFalse(set.ContainsAll(other));
		}

		private Set4 NewSet(string firstElement)
		{
			Set4 set = new Set4();
			set.Add(firstElement);
			return set;
		}
	}
}
