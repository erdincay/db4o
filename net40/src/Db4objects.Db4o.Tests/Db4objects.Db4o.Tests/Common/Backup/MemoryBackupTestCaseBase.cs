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
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Backup;

namespace Db4objects.Db4o.Tests.Common.Backup
{
	public abstract class MemoryBackupTestCaseBase : TestWithTempFile
	{
		public class Item
		{
			public int _id;

			public Item(int id)
			{
				_id = id;
			}
		}

		private static readonly string DbPath = "database";

		private const int NumItems = 10;

		/// <exception cref="System.Exception"></exception>
		public virtual void TestMemoryBackup()
		{
			LocalObjectContainer origDb = (LocalObjectContainer)Db4oEmbedded.OpenFile(Config(
				OrigStorage()), DbPath);
			Store(origDb);
			Backup(origDb, TempFile());
			origDb.Close();
			IObjectContainer backupDb = Db4oEmbedded.OpenFile(Config(BackupStorage()), TempFile
				());
			IObjectSet result = backupDb.Query(typeof(MemoryBackupTestCaseBase.Item));
			Assert.AreEqual(NumItems, result.Count);
			backupDb.Close();
			BackupStorage().Delete(TempFile());
		}

		protected abstract void Backup(LocalObjectContainer origDb, string backupPath);

		protected abstract IStorage BackupStorage();

		protected abstract IStorage OrigStorage();

		private void Store(LocalObjectContainer origDb)
		{
			for (int itemId = 0; itemId < NumItems; itemId++)
			{
				origDb.Store(new MemoryBackupTestCaseBase.Item(itemId));
			}
			origDb.Commit();
		}

		private IEmbeddedConfiguration Config(IStorage storage)
		{
			IEmbeddedConfiguration origConfig = Db4oEmbedded.NewConfiguration();
			origConfig.File.Storage = storage;
			return origConfig;
		}
	}
}
