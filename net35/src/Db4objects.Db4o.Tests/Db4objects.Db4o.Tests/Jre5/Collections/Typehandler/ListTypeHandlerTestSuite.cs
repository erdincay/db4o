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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class ListTypeHandlerTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			ListTypeHandlerTestElementsSpec[] elementSpecs = new ListTypeHandlerTestElementsSpec
				[] { ListTypeHandlerTestVariables.StringElementsSpec, ListTypeHandlerTestVariables
				.IntElementsSpec, ListTypeHandlerTestVariables.ObjectElementsSpec };
			return new IFixtureProvider[] { new Db4oFixtureProvider(), ListTypeHandlerTestVariables
				.ListFixtureProvider, new SimpleFixtureProvider(ListTypeHandlerTestVariables.ElementsSpec
				, (object[])elementSpecs), ListTypeHandlerTestVariables.TypehandlerFixtureProvider
				 };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(ListTypeHandlerTestSuite.ListTypeHandlerTestUnit) };
		}

		public class ListTypeHandlerTestUnit : CollectionTypeHandlerUnitTest
		{
			protected override AbstractItemFactory ItemFactory()
			{
				return (AbstractItemFactory)ListTypeHandlerTestVariables.ListImplementation.Value;
			}

			protected override ITypeHandler4 TypeHandler()
			{
				return (ITypeHandler4)ListTypeHandlerTestVariables.ListTypehander.Value;
			}

			protected override ListTypeHandlerTestElementsSpec ElementsSpec()
			{
				return (ListTypeHandlerTestElementsSpec)ListTypeHandlerTestVariables.ElementsSpec
					.Value;
			}

			protected override void FillItem(object item)
			{
				FillListItem(item);
			}

			protected override void AssertContent(object item)
			{
				AssertListContent(item);
			}

			protected override void AssertPlainContent(object item)
			{
				AssertPlainListContent((IList)item);
			}

			protected override void AssertCompareItems(object element, bool successful)
			{
				IQuery q = NewQuery();
				object item = ItemFactory().NewItem();
				IList list = ListFromItem(item);
				list.Add(element);
				q.Constrain(item);
				AssertQueryResult(q, successful);
			}

			public virtual void TestActivation()
			{
				object item = RetrieveItemInstance();
				IList list = ListFromItem(item);
				Assert.AreEqual(ExpectedElementCount(), list.Count);
				object element = list[0];
				if (Db().IsActive(element))
				{
					Db().Deactivate(item, int.MaxValue);
					Assert.IsFalse(Db().IsActive(element));
					Db().Activate(item, int.MaxValue);
					Assert.IsTrue(Db().IsActive(element));
				}
			}
		}
	}
}
