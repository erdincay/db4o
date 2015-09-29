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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Btree.AllTests().RunSolo();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(BTreeAddRemoveTestCase), typeof(BTreeAsSetTestCase), typeof(
				BTreeFreeTestCase), typeof(BTreeIteratorTestCase), typeof(BTreeNodeTestCase), typeof(
				BTreePointerTestCase), typeof(BTreeRangeTestCase), typeof(BTreeRollbackTestCase)
				, typeof(BTreeSearchTestCase), typeof(BTreeSimpleTestCase), typeof(BTreeStructureChangeListenerTestCase
				), typeof(SearcherLowestHighestTestCase), typeof(SearcherTestCase) };
		}
	}
}
