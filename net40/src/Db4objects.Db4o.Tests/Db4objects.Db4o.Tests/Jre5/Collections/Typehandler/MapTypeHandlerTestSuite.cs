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
using System.Collections;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class MapTypeHandlerTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new Db4oFixtureProvider(), MapTypeHandlerTestVariables
				.MapFixtureProvider, MapTypeHandlerTestVariables.MapKeysProvider, MapTypeHandlerTestVariables
				.MapValuesProvider, MapTypeHandlerTestVariables.TypehandlerFixtureProvider };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(MapTypeHandlerTestSuite.MapTypeHandlerUnitTestCase) };
		}

		public class MapTypeHandlerUnitTestCase : CollectionTypeHandlerUnitTest
		{
			protected override void FillItem(object item)
			{
				FillMapItem(item);
			}

			protected override void AssertContent(object item)
			{
				AssertMapContent(item);
			}

			protected override void AssertPlainContent(object item)
			{
				AssertPlainMapContent((IDictionary)item);
			}

			protected override AbstractItemFactory ItemFactory()
			{
				return (AbstractItemFactory)MapTypeHandlerTestVariables.MapImplementation.Value;
			}

			protected override ITypeHandler4 TypeHandler()
			{
				return (ITypeHandler4)MapTypeHandlerTestVariables.MapTypehander.Value;
			}

			protected override ListTypeHandlerTestElementsSpec ElementsSpec()
			{
				return (ListTypeHandlerTestElementsSpec)MapTypeHandlerTestVariables.MapKeysSpec.Value;
			}

			protected override void AssertCompareItems(object element, bool successful)
			{
				IQuery q = NewQuery();
				object item = ItemFactory().NewItem();
				IDictionary map = MapFromItem(item);
				map[element] = Values()[0];
				q.Constrain(item);
				AssertQueryResult(q, successful);
			}
		}
	}
}
