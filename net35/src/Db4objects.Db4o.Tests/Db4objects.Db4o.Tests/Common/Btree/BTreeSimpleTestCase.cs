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

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class BTreeSimpleTestCase : AbstractDb4oTestCase, IOptOutDefragSolo, IOptOutMultiSession
	{
		protected const int BtreeNodeSize = 4;

		internal int[] _keys = new int[] { 3, 234, 55, 87, 2, 1, 101, 59, 70, 300, 288 };

		internal int[] _values;

		internal int[] _sortedKeys = new int[] { 1, 2, 3, 55, 59, 70, 87, 101, 234, 288, 
			300 };

		internal int[] _sortedValues;

		internal int[] _keysOnRemoval = new int[] { 1, 2, 55, 59, 70, 87, 234, 288, 300 };

		internal int[] _valuesOnRemoval;

		internal int[] _one = new int[] { 1 };

		internal int[] _none = new int[] {  };

		public BTreeSimpleTestCase() : base()
		{
			_values = new int[_keys.Length];
			for (int i = 0; i < _keys.Length; i++)
			{
				_values[i] = _keys[i];
			}
			_sortedValues = new int[_sortedKeys.Length];
			for (int i = 0; i < _sortedKeys.Length; i++)
			{
				_sortedValues[i] = _sortedKeys[i];
			}
			_valuesOnRemoval = new int[_keysOnRemoval.Length];
			for (int i = 0; i < _keysOnRemoval.Length; i++)
			{
				_valuesOnRemoval[i] = _keysOnRemoval[i];
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestIntKeys()
		{
			BTree btree = BTreeAssert.CreateIntKeyBTree(Container(), 0, BtreeNodeSize);
			for (int i = 0; i < 5; i++)
			{
				btree = CycleIntKeys(btree);
			}
		}

		/// <exception cref="System.Exception"></exception>
		private BTree CycleIntKeys(BTree btree)
		{
			AddKeys(btree);
			ExpectKeys(btree, _sortedKeys);
			btree.Commit(Trans());
			ExpectKeys(btree, _sortedKeys);
			RemoveKeys(btree);
			ExpectKeys(btree, _keysOnRemoval);
			btree.Rollback(Trans());
			ExpectKeys(btree, _sortedKeys);
			int id = btree.GetID();
			Reopen();
			btree = BTreeAssert.CreateIntKeyBTree(Container(), id, BtreeNodeSize);
			ExpectKeys(btree, _sortedKeys);
			RemoveKeys(btree);
			ExpectKeys(btree, _keysOnRemoval);
			btree.Commit(Trans());
			ExpectKeys(btree, _keysOnRemoval);
			// remove all but 1
			for (int i = 1; i < _keysOnRemoval.Length; i++)
			{
				btree.Remove(Trans(), _keysOnRemoval[i]);
			}
			ExpectKeys(btree, _one);
			btree.Commit(Trans());
			ExpectKeys(btree, _one);
			btree.Remove(Trans(), 1);
			btree.Rollback(Trans());
			ExpectKeys(btree, _one);
			btree.Remove(Trans(), 1);
			btree.Commit(Trans());
			ExpectKeys(btree, _none);
			return btree;
		}

		private void RemoveKeys(BTree btree)
		{
			btree.Remove(Trans(), 3);
			btree.Remove(Trans(), 101);
		}

		private void AddKeys(BTree btree)
		{
			Transaction trans = Trans();
			for (int i = 0; i < _keys.Length; i++)
			{
				btree.Add(trans, _keys[i]);
			}
		}

		private void ExpectKeys(BTree btree, int[] keys)
		{
			BTreeAssert.AssertKeys(Trans(), btree, keys);
		}
	}
}
