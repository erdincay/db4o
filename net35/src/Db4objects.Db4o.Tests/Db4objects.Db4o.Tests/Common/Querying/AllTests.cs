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
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Querying.AllTests().RunAll();
		}

		//runSoloAndClientServer();
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(CascadedDeleteUpdate), typeof(CascadeDeleteArray), typeof(
				CascadeDeleteDeleted), typeof(CascadeDeleteFalse), typeof(CascadeOnActivate), typeof(
				CascadeOnDeleteTestCase), typeof(CascadeOnDeleteHierarchyTestCase), typeof(CascadeOnUpdateTestCase
				), typeof(CascadeToArray), typeof(ConjunctiveQbETestCase), typeof(DeepMultifieldSortingTestCase
				), typeof(DescendIndexQueryTestCase), typeof(IdentityQueryForNotStoredTestCase), 
				typeof(IdListQueryResultTestCase), typeof(IndexedJoinQueriesTestCase), typeof(IndexOnParentFieldTestCase
				), typeof(IndexedQueriesTestCase), typeof(InvalidFieldNameConstraintTestCase), typeof(
				LazyQueryResultTestCase), typeof(MultiFieldIndexQueryTestCase), typeof(NoClassIndexQueryTestSuite
				), typeof(NullConstraintQueryTestCase), typeof(ObjectSetTestCase), typeof(OrderedQueryTestCase
				), typeof(QueryByExampleTestCase), typeof(QueryingForAllObjectsTestCase), typeof(
				QueryingVersionFieldTestCase), typeof(SameChildOnDifferentParentQueryTestCase), 
				typeof(SortingOnUnknownClassTestCase) };
		}
	}
}
