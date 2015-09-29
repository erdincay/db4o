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
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	/// <exclude></exclude>
	public class Arrays4TestCase : ITestCase
	{
		public virtual void TestContainsInstanceOf()
		{
			object[] array = new object[] { "foo", 42 };
			Assert.IsTrue(Arrays4.ContainsInstanceOf(array, typeof(string)));
			Assert.IsTrue(Arrays4.ContainsInstanceOf(array, typeof(int)));
			Assert.IsTrue(Arrays4.ContainsInstanceOf(array, typeof(object)));
			Assert.IsFalse(Arrays4.ContainsInstanceOf(array, typeof(float)));
			Assert.IsFalse(Arrays4.ContainsInstanceOf(new object[0], typeof(object)));
			Assert.IsFalse(Arrays4.ContainsInstanceOf(new object[1], typeof(object)));
			Assert.IsFalse(Arrays4.ContainsInstanceOf(null, typeof(object)));
		}

		public virtual void TestCopyOfInt()
		{
			AssertCopyOf(new int[] {  });
			AssertCopyOf(new int[] { 42 });
			AssertCopyOf(new int[] { 42, 42 });
			AssertCopyOf(new int[] { 1, 2, 3 }, 2);
			AssertCopyOf(new int[] { 1, 2 }, 3);
		}

		private void AssertCopyOf(int[] array, int newLength)
		{
			AssertCopyOf(ExpectationFor(array, newLength), array, newLength);
		}

		private int[] ExpectationFor(int[] array, int newLength)
		{
			int[] expectation = new int[newLength];
			System.Array.Copy(array, 0, expectation, 0, Math.Min(array.Length, newLength));
			return expectation;
		}

		private void AssertCopyOf(int[] array)
		{
			AssertCopyOf(array, array, array.Length);
		}

		private void AssertCopyOf(int[] expected, int[] array, int newLength)
		{
			int[] copy = Arrays4.CopyOf(array, newLength);
			Assert.AreNotSame(array, copy);
			ArrayAssert.AreEqual(expected, copy);
		}
	}
}
