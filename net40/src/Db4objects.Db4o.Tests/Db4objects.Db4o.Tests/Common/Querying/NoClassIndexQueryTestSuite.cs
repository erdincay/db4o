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
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class NoClassIndexQueryTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		public class NoClassIndexQueryTestUnit : AbstractDb4oTestCase
		{
			public class Item
			{
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Configure(IConfiguration config)
			{
				config.ObjectClass(typeof(NoClassIndexQueryTestSuite.NoClassIndexQueryTestUnit.Item
					)).Indexed(false);
				config.Queries().EvaluationMode(((NoClassIndexQueryTestSuite.LabeledQueryMode)queryMode
					.Value).Mode());
			}

			/// <exception cref="System.Exception"></exception>
			protected override void Store()
			{
				Store(new NoClassIndexQueryTestSuite.NoClassIndexQueryTestUnit.Item());
			}

			public virtual void Test()
			{
				IObjectSet query = Db().Query(typeof(NoClassIndexQueryTestSuite.NoClassIndexQueryTestUnit.Item
					));
				Assert.AreEqual(0, query.Count);
			}
		}

		private static readonly FixtureVariable queryMode = FixtureVariable.NewInstance("queryMode"
			);

		public class LabeledQueryMode : ILabeled
		{
			private readonly QueryEvaluationMode _mode;

			public LabeledQueryMode(QueryEvaluationMode mode)
			{
				_mode = mode;
			}

			public virtual QueryEvaluationMode Mode()
			{
				return _mode;
			}

			public virtual string Label()
			{
				return _mode.ToString();
			}
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new Db4oFixtureProvider(), new SimpleFixtureProvider
				(queryMode, new NoClassIndexQueryTestSuite.LabeledQueryMode[] { new NoClassIndexQueryTestSuite.LabeledQueryMode
				(QueryEvaluationMode.Immediate), new NoClassIndexQueryTestSuite.LabeledQueryMode
				(QueryEvaluationMode.Snapshot), new NoClassIndexQueryTestSuite.LabeledQueryMode(
				QueryEvaluationMode.Lazy) }) };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(NoClassIndexQueryTestSuite.NoClassIndexQueryTestUnit) };
		}
	}
}
