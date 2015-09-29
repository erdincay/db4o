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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Foundation;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class TreeKeyIteratorTestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(TreeKeyIteratorTestCase)).Run();
		}

		private static int[] Values = new int[] { 1, 3, 5, 7, 9, 10, 11, 13, 24, 76 };

		public virtual void TestIterate()
		{
			for (int i = 1; i <= Values.Length; i++)
			{
				AssertIterateValues(Values, i);
			}
		}

		public virtual void TestMoveNextAfterCompletion()
		{
			IEnumerator i = new TreeKeyIterator(CreateTree(Values));
			while (i.MoveNext())
			{
			}
			Assert.IsFalse(i.MoveNext());
		}

		private void AssertIterateValues(int[] values, int count)
		{
			int[] testValues = new int[count];
			System.Array.Copy(values, 0, testValues, 0, count);
			AssertIterateValues(testValues);
		}

		private void AssertIterateValues(int[] values)
		{
			ExpectingVisitor expectingVisitor = new ExpectingVisitor(IntArrays4.ToObjectArray
				(values), true, false);
			IEnumerator i = new TreeKeyIterator(CreateTree(values));
			while (i.MoveNext())
			{
				expectingVisitor.Visit(i.Current);
			}
			expectingVisitor.AssertExpectations();
		}

		private Tree CreateTree(int[] values)
		{
			Tree tree = new TreeInt(values[0]);
			for (int i = 1; i < values.Length; i++)
			{
				tree = tree.Add(new TreeInt(values[i]));
			}
			return tree;
		}

		public virtual void TestEmpty()
		{
			IEnumerator i = new TreeKeyIterator(null);
			Assert.IsFalse(i.MoveNext());
		}
	}
}
