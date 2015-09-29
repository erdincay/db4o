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
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	/// <exclude></exclude>
	public class IntMatcherTestCase : ITestCase
	{
		public virtual void Test()
		{
			Assert.IsTrue(IntMatcher.Zero.Match(0));
			Assert.IsFalse(IntMatcher.Zero.Match(-1));
			Assert.IsFalse(IntMatcher.Zero.Match(1));
			Assert.IsFalse(IntMatcher.Zero.Match(int.MinValue));
			Assert.IsFalse(IntMatcher.Zero.Match(int.MaxValue));
			Assert.IsFalse(IntMatcher.Positive.Match(0));
			Assert.IsFalse(IntMatcher.Positive.Match(-1));
			Assert.IsTrue(IntMatcher.Positive.Match(1));
			Assert.IsFalse(IntMatcher.Positive.Match(int.MinValue));
			Assert.IsTrue(IntMatcher.Positive.Match(int.MaxValue));
			Assert.IsFalse(IntMatcher.Negative.Match(0));
			Assert.IsTrue(IntMatcher.Negative.Match(-1));
			Assert.IsFalse(IntMatcher.Negative.Match(1));
			Assert.IsTrue(IntMatcher.Negative.Match(int.MinValue));
			Assert.IsFalse(IntMatcher.Negative.Match(int.MaxValue));
		}
	}
}
