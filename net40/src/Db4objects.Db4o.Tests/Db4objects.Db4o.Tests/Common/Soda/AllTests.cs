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
using Db4objects.Db4o.Tests.Common.Soda;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Simple;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Typedhierarchy;
using Db4objects.Db4o.Tests.Common.Soda.Classes.Untypedhierarchy;
using Db4objects.Db4o.Tests.Common.Soda.Joins.Typed;
using Db4objects.Db4o.Tests.Common.Soda.Joins.Untyped;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;
using Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class AllTests : Db4oTestSuite
	{
		protected override Type[] TestCases()
		{
			return new Type[] { typeof(STOrderingTestCase), typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.AllTests
				), typeof(AndJoinOptimizationTestCase), typeof(ByteCoercionTestCase), typeof(CollectionIndexedJoinTestCase
				), typeof(InterfaceFieldConstraintTestCase), typeof(NullIdentityConstraintTestCase
				), typeof(OrderByParentFieldTestCase), typeof(OrderByWithComparableTestCase), typeof(
				OrderByWithNullValuesTestCase), typeof(OrderedOrConstraintTestCase), typeof(OrderFollowedByConstraintTestCase
				), typeof(PreserveJoinsTestCase), typeof(QueryUnknownClassTestCase), typeof(SODAClassTypeDescend
				), typeof(SortingNotAvailableField), typeof(SortMultipleTestCase), typeof(STBooleanTestCase
				), typeof(STBooleanWUTestCase), typeof(STByteTestCase), typeof(STByteWUTestCase)
				, typeof(STCharTestCase), typeof(STCharWUTestCase), typeof(STDoubleTestCase), typeof(
				STDoubleWUTestCase), typeof(STETH1TestCase), typeof(STFloatTestCase), typeof(STFloatWUTestCase
				), typeof(STIntegerTestCase), typeof(STIntegerWUTestCase), typeof(STLongTestCase
				), typeof(STLongWUTestCase), typeof(STOrTTestCase), typeof(STOrUTestCase), typeof(
				STOStringTestCase), typeof(STOIntegerTestCase), typeof(STOIntegerWTTestCase), typeof(
				STRTH1TestCase), typeof(STSDFT1TestCase), typeof(STShortTestCase), typeof(STShortWUTestCase
				), typeof(STStringUTestCase), typeof(STRUH1TestCase), typeof(STTH1TestCase), typeof(
				STUH1TestCase), typeof(TopLevelOrderExceptionTestCase), typeof(UntypedEvaluationTestCase
				), typeof(JointEqualsIdentityTestCase) };
		}

		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Soda.AllTests().RunSolo();
		}
	}
}
