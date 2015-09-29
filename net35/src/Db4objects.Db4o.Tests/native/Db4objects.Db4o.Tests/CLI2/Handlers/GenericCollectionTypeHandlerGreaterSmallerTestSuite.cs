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
using Db4objects.Db4o.Query;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
	public class GenericCollectionTypeHandlerGreaterSmallerTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			IFixtureProvider ElementsFixtureProvider = new SimpleFixtureProvider(
				GenericCollectionTypeHandlerTestVariables.ElementSpec,
				new object[]
				{
					GenericCollectionTypeHandlerTestVariables.StringElementSpec,
					GenericCollectionTypeHandlerTestVariables.IntElementSpec,
					GenericCollectionTypeHandlerTestVariables.NullableIntElementSpec,
				});

			return new IFixtureProvider[]
					{
			       		new Db4oFixtureProvider(),
			       		GenericCollectionTypeHandlerTestVariables.CollectionFixtureProvider,
			       		ElementsFixtureProvider
			       	};
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(GenericCollectionTypeHandlerGreaterSmallerTestUnit) };
		}

		public class GenericCollectionTypeHandlerGreaterSmallerTestUnit : GenericCollectionTypeHandlerTestUnitBase
		{
			public virtual void TestSuccessfulSmallerQuery()
			{
				IQuery q = NewQuery(_helper.ItemType);
				q.Descend(GenericCollectionTestFactory.FieldName).Constrain(_helper.LargeElement).Smaller();
				AssertQueryResult(q, true);
			}

			public virtual void TestFailingGreaterQuery()
			{
				IQuery q = NewQuery(_helper.ItemType);
				q.Descend(GenericCollectionTestFactory.FieldName).Constrain(_helper.LargeElement).Greater();
				AssertQueryResult(q, false);
			}
		}
	}
}
