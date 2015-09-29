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
using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class BTreeRollbackTestCase : BTreeTestCaseBase
	{
		public static void Main(string[] args)
		{
			new BTreeRollbackTestCase().RunSolo();
		}

		private static readonly int[] CommittedValues = new int[] { 6, 8, 15, 45, 43, 9, 
			23, 25, 7, 3, 2 };

		private static readonly int[] RolledBackValues = new int[] { 16, 18, 115, 19, 17, 
			13, 12 };

		public virtual void Test()
		{
			Add(CommittedValues);
			CommitBTree();
			for (int i = 0; i < 5; i++)
			{
				Add(RolledBackValues);
				RollbackBTree();
			}
			BTreeAssert.AssertKeys(Trans(), _btree, CommittedValues);
		}

		private void CommitBTree()
		{
			_btree.Commit(Trans());
			Trans().Commit();
		}

		private void RollbackBTree()
		{
			_btree.Rollback(Trans());
			Trans().Rollback();
		}
	}
}
