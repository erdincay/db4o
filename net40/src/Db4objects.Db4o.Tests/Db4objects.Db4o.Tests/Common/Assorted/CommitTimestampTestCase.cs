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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class CommitTimestampTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateCommitTimestamps(true);
		}

		public class Item
		{
		}

		public virtual void TestUpdateAndQuery()
		{
			CommitTimestampTestCase.Item item1 = new CommitTimestampTestCase.Item();
			Store(item1);
			CommitTimestampTestCase.Item item2 = new CommitTimestampTestCase.Item();
			Store(item2);
			Commit();
			long initialCommitTimestamp1 = Db().GetObjectInfo(item1).GetCommitTimestamp();
			long initialCommitTimestamp2 = Db().GetObjectInfo(item2).GetCommitTimestamp();
			Assert.AreEqual(initialCommitTimestamp1, initialCommitTimestamp2);
			Store(item2);
			Commit();
			long secondCommitTimestamp1 = Db().GetObjectInfo(item1).GetCommitTimestamp();
			long secondCommitTimestamp2 = Db().GetObjectInfo(item2).GetCommitTimestamp();
			Assert.AreEqual(initialCommitTimestamp1, secondCommitTimestamp1);
			Assert.AreNotEqual(initialCommitTimestamp2, secondCommitTimestamp2);
			AssertQueryForTimestamp(item1, initialCommitTimestamp1);
			AssertQueryForTimestamp(item2, secondCommitTimestamp2);
		}

		private void AssertQueryForTimestamp(CommitTimestampTestCase.Item expected, long 
			timestamp)
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(CommitTimestampTestCase.Item));
			query.Descend(VirtualField.CommitTimestamp).Constrain(timestamp);
			IObjectSet objectSet = query.Execute();
			Assert.AreEqual(1, objectSet.Count);
			CommitTimestampTestCase.Item actual = (CommitTimestampTestCase.Item)objectSet.Next
				();
			Assert.AreSame(expected, actual);
		}
	}
}
