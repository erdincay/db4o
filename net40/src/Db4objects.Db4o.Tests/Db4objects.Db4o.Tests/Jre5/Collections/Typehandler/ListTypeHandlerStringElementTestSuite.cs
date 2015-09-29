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
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class ListTypeHandlerStringElementTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public override IFixtureProvider[] FixtureProviders()
		{
			ListTypeHandlerTestElementsSpec[] elementSpecs = new ListTypeHandlerTestElementsSpec
				[] { ListTypeHandlerTestVariables.StringElementsSpec };
			return new IFixtureProvider[] { new Db4oFixtureProvider(), ListTypeHandlerTestVariables
				.ListFixtureProvider, new SimpleFixtureProvider(ListTypeHandlerTestVariables.ElementsSpec
				, (object[])elementSpecs), ListTypeHandlerTestVariables.TypehandlerFixtureProvider
				 };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(ListTypeHandlerStringElementTestSuite.ListTypeHandlerStringElementTestUnit
				) };
		}

		public class ListTypeHandlerStringElementTestUnit : ListTypeHandlerTestUnitBase
		{
			/// <exception cref="System.Exception"></exception>
			public virtual void TestSuccessfulEndsWithQuery()
			{
				IQuery q = NewQuery(ItemFactory().ItemClass());
				q.Descend(AbstractItemFactory.ListFieldName).Constrain(SuccessfulEndChar()).EndsWith
					(false);
				AssertQueryResult(q, true);
			}

			/// <exception cref="System.Exception"></exception>
			public virtual void TestFailingEndsWithQuery()
			{
				IQuery q = NewQuery(ItemFactory().ItemClass());
				q.Descend(AbstractItemFactory.ListFieldName).Constrain(FailingEndChar()).EndsWith
					(false);
				AssertQueryResult(q, false);
			}

			private string SuccessfulEndChar()
			{
				return EndChar().ToString();
			}

			private string FailingEndChar()
			{
				return (EndChar() + 1).ToString();
			}

			private char EndChar()
			{
				string str = (string)Elements()[0];
				return str[str.Length - 1];
			}
		}
	}
}
