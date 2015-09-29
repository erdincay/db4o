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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class CommitTimestampMigrationTestCase : FormatMigrationTestCaseBase
	{
		public class Item
		{
		}

		protected override void ConfigureForTest(IConfiguration config)
		{
			ConfigureForStore(config);
		}

		protected override void ConfigureForStore(IConfiguration config)
		{
			config.GenerateVersionNumbers(ConfigScope.Globally);
			// This needs to be in a different method for .NET because .NET
			// tries to resolve the complete method body for jitting and will
			// throw without calling the first method. 
			ConfigureForStore8_0AndNewer(config);
		}

		protected virtual void ConfigureForStore8_0AndNewer(IConfiguration config)
		{
			config.GenerateCommitTimestamps(true);
		}

		protected override void AssertObjectsAreReadable(IExtObjectContainer objectContainer
			)
		{
			if (Db4oMajorVersion() <= 6 || (Db4oMajorVersion() == 7 && Db4oMinorVersion() == 
				0))
			{
				return;
			}
			CommitTimestampMigrationTestCase.Item item = ((CommitTimestampMigrationTestCase.Item
				)objectContainer.Query(typeof(CommitTimestampMigrationTestCase.Item)).Next());
			IObjectInfo objectInfo = objectContainer.GetObjectInfo(item);
			long version = objectInfo.GetCommitTimestamp();
			Assert.IsGreater(0, version);
		}

		protected override string FileNamePrefix()
		{
			return "commitTimestamp";
		}

		protected override void Store(IObjectContainerAdapter objectContainer)
		{
			objectContainer.Store(new CommitTimestampMigrationTestCase.Item());
		}
	}
}
