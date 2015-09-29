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
#if !SILVERLIGHT
using System;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class OwnCommitCallbackFlaggedEmbeddedTestSuite : FixtureBasedTestSuite
	{
		public class Item
		{
			public int _id;

			public Item(int id)
			{
				// COR-1822
				_id = id;
			}
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new SimpleFixtureProvider(OwnCommittedCallbacksFixture
				.Factory, new OwnCommittedCallbacksFixture.IContainerFactory[] { new OwnCommittedCallbacksFixture.EmbeddedCSContainerFactory
				(), new OwnCommittedCallbacksFixture.EmbeddedSessionContainerFactory() }), new SimpleFixtureProvider
				(OwnCommittedCallbacksFixture.Action, new OwnCommittedCallbacksFixture.CommitAction
				[] { new OwnCommittedCallbacksFixture.ClientACommitAction(), new OwnCommittedCallbacksFixture.ClientBCommitAction
				() }) };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(OwnCommittedCallbacksFixture.OwnCommitCallbackFlaggedTestUnit
				) };
		}
	}
}
#endif // !SILVERLIGHT
