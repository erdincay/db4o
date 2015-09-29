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
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class BTreeSearchTestCase : AbstractDb4oTestCase, IOptOutDefragSolo, IOptOutMultiSession
	{
		protected const int BtreeNodeSize = 4;

		public static void Main(string[] arguments)
		{
			new BTreeSearchTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			CycleIntKeys(new int[] { 3, 5, 7, 10, 11, 12, 14, 15, 17, 20, 21, 25 });
		}

		/// <exception cref="System.Exception"></exception>
		private void CycleIntKeys(int[] values)
		{
			BTree btree = BTreeAssert.CreateIntKeyBTree(Container(), 0, BtreeNodeSize);
			for (int i = 0; i < 5; i++)
			{
				btree = CycleIntKeys(btree, values);
			}
		}

		/// <exception cref="System.Exception"></exception>
		private BTree CycleIntKeys(BTree btree, int[] values)
		{
			for (int i = 0; i < values.Length; i++)
			{
				btree.Add(Trans(), values[i]);
			}
			ExpectKeysSearch(Trans(), btree, values);
			btree.Commit(Trans());
			int id = btree.GetID();
			Container().Commit(Trans());
			Reopen();
			btree = BTreeAssert.CreateIntKeyBTree(Container(), id, BtreeNodeSize);
			ExpectKeysSearch(Trans(), btree, values);
			for (int i = 0; i < values.Length; i++)
			{
				btree.Remove(Trans(), values[i]);
			}
			BTreeAssert.AssertEmpty(Trans(), btree);
			btree.Commit(Trans());
			BTreeAssert.AssertEmpty(Trans(), btree);
			return btree;
		}

		private void ExpectKeysSearch(Transaction trans, BTree btree, int[] keys)
		{
			int lastValue = int.MinValue;
			for (int i = 0; i < keys.Length; i++)
			{
				if (keys[i] != lastValue)
				{
					ExpectingVisitor expectingVisitor = ExpectingVisitor.CreateExpectingVisitor(keys[
						i], IntArrays4.Occurences(keys, keys[i]));
					IBTreeRange range = btree.SearchRange(trans, keys[i]);
					BTreeAssert.TraverseKeys(range, expectingVisitor);
					expectingVisitor.AssertExpectations();
					lastValue = keys[i];
				}
			}
		}
	}
}
