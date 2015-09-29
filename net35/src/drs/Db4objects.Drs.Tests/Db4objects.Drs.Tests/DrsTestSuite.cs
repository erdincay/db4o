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
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Regression;

namespace Db4objects.Drs.Tests
{
	public abstract class DrsTestSuite : ReflectionTestSuite
	{
		public const bool RunOneSingleTest = false;

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(DateReplicationTestCase), typeof(Db4objects.Drs.Tests.Foundation.AllTests
				), typeof(TheSimplest), typeof(ReplicationEventTest), typeof(ReplicationProviderTest
				), typeof(ReplicationAfterDeletionTest), typeof(SimpleArrayTest), typeof(SimpleParentChild
				), typeof(ByteArrayTest), typeof(ComplexListTestCase), typeof(ListTest), typeof(
				R0to4Runner), typeof(ReplicationFeaturesMain), typeof(CollectionHandlerImplTest)
				, typeof(BidirectionalReplicationTestCase), typeof(TimestampTestCase), typeof(MapTest
				), typeof(ArrayReplicationTest), typeof(SingleTypeCollectionReplicationTest), typeof(
				MixedTypesCollectionReplicationTest), typeof(TransparentActivationTestCase), typeof(
				DRS42Test), typeof(SameHashCodeTestCase) };
		}

		// Simple
		// Collection
		// Complex
		// General
		//regression
		protected virtual Type[] Concat(Type[] x, Type[] y)
		{
			Collection4 c = new Collection4(x);
			c.AddAll(y);
			return (Type[])c.ToArray(new Type[c.Size()]);
		}
	}
}
