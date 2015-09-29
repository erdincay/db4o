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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Defragment;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public class CommitTimestampDefragmentTestCase : DefragmentTestCaseBase
	{
		public class Item
		{
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void TestKeepingBtrees()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.File.GenerateCommitTimestamps = true;
			long version = StoreItemAndGetCommitTimestamp(config);
			Assert.IsGreater(0, version);
			Defrag(TernaryBool.Unspecified);
			AssertVersionAfterDefrag(version, null);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void TestRemovingBtrees()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.File.GenerateCommitTimestamps = true;
			long version = StoreItemAndGetCommitTimestamp(config);
			Assert.IsGreater(0, version);
			Defrag(TernaryBool.No);
			IEmbeddedConfiguration afterDefragConfig = null;
			AssertVersionAfterDefrag(0, afterDefragConfig);
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void TestTurningOnGenerateCommitTimestampInDefrag()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			long version = StoreItemAndGetCommitTimestamp(config);
			Assert.AreEqual(0, version);
			Defrag(TernaryBool.Yes);
			IEmbeddedConfiguration afterDefragConfig = null;
			AssertVersionAfterDefrag(0, afterDefragConfig);
		}

		private void AssertVersionAfterDefrag(long version, IEmbeddedConfiguration afterDefragConfig
			)
		{
			IEmbeddedObjectContainer db = OpenContainer(afterDefragConfig);
			CommitTimestampDefragmentTestCase.Item retrievedItem = ((CommitTimestampDefragmentTestCase.Item
				)db.Query(typeof(CommitTimestampDefragmentTestCase.Item)).Next());
			long retrievedVersion = db.Ext().GetObjectInfo(retrievedItem).GetCommitTimestamp(
				);
			Assert.AreEqual(version, retrievedVersion);
			db.Close();
		}

		private long StoreItemAndGetCommitTimestamp(IEmbeddedConfiguration config)
		{
			IEmbeddedObjectContainer db = OpenContainer(config);
			CommitTimestampDefragmentTestCase.Item item = new CommitTimestampDefragmentTestCase.Item
				();
			db.Store(item);
			db.Commit();
			long commitTimestamp = db.Ext().GetObjectInfo(item).GetCommitTimestamp();
			db.Close();
			return commitTimestamp;
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void Defrag(TernaryBool generateCommitTimestamp)
		{
			DefragmentConfig config = new DefragmentConfig(SourceFile(), BackupFile());
			config.Db4oConfig(NewConfiguration());
			config.ForceBackupDelete(true);
			if (!generateCommitTimestamp.IsUnspecified())
			{
				config.Db4oConfig().GenerateCommitTimestamps(generateCommitTimestamp.DefiniteYes(
					));
			}
			Db4objects.Db4o.Defragment.Defragment.Defrag(config);
		}

		private IEmbeddedObjectContainer OpenContainer(IEmbeddedConfiguration config)
		{
			if (config == null)
			{
				config = NewConfiguration();
			}
			config.Common.ReflectWith(Platform4.ReflectorForType(typeof(CommitTimestampDefragmentTestCase.Item
				)));
			return config == null ? Db4oEmbedded.OpenFile(SourceFile()) : Db4oEmbedded.OpenFile
				(config, SourceFile());
		}
	}
}
