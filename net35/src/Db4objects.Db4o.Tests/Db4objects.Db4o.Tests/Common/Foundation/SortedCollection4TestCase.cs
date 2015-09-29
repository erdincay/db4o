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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	/// <exclude></exclude>
	public class SortedCollection4TestCase : ITestCase
	{
		private sealed class _IComparison4_15 : IComparison4
		{
			public _IComparison4_15()
			{
			}

			public int Compare(object x, object y)
			{
				return ((int)x) - ((int)y);
			}
		}

		internal static readonly IComparison4 IntegerComparison = new _IComparison4_15();

		public virtual void TestAddAllAndToArray()
		{
			object[] array = IntArrays4.ToObjectArray(new int[] { 6, 4, 1, 2, 7, 3 });
			SortedCollection4 collection = NewSortedCollection();
			Assert.AreEqual(0, collection.Size());
			collection.AddAll(new ArrayIterator4(array));
			AssertCollection(new int[] { 1, 2, 3, 4, 6, 7 }, collection);
		}

		public virtual void TestToArrayOnEmptyCollection()
		{
			object[] array = new object[0];
			Assert.AreSame(array, NewSortedCollection().ToArray(array));
		}

		public virtual void TestAddRemove()
		{
			SortedCollection4 collection = NewSortedCollection();
			collection.Add(3);
			collection.Add(1);
			collection.Add(5);
			AssertCollection(new int[] { 1, 3, 5 }, collection);
			collection.Remove(3);
			AssertCollection(new int[] { 1, 5 }, collection);
			collection.Remove(1);
			AssertCollection(new int[] { 5 }, collection);
		}

		private void AssertCollection(int[] expected, SortedCollection4 collection)
		{
			Assert.AreEqual(expected.Length, collection.Size());
			ArrayAssert.AreEqual(IntArrays4.ToObjectArray(expected), collection.ToArray(new object
				[collection.Size()]));
		}

		private SortedCollection4 NewSortedCollection()
		{
			return new SortedCollection4(IntegerComparison);
		}
	}
}
