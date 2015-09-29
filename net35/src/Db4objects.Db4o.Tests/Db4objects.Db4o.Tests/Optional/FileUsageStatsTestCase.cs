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
using System.Collections;
using System.IO;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Optional;

namespace Db4objects.Db4o.Tests.Optional
{
	public class FileUsageStatsTestCase : Db4oTestWithTempFile
	{
		public class Child
		{
		}

		public class Item
		{
			public int _id;

			public string _name;

			public int[] _arr;

			public IList _list;

			public Item(int id, string name, IList list)
			{
				_id = id;
				_name = name;
				_arr = new int[] { id };
				_list = list;
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestFileStats()
		{
			CreateDatabase(new ArrayList());
			AssertFileStats();
			Defrag();
			AssertFileStats();
		}

		private void AssertFileStats()
		{
			FileUsageStats stats = FileUsageStatsCollector.RunStats(TempFile(), true, NewConfiguration
				());
			Assert.AreEqual(stats.FileSize(), stats.TotalUsage(), stats.ToString());
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void Defrag()
		{
			string backupPath = Path.GetTempFileName();
			DefragmentConfig config = new DefragmentConfig(TempFile(), backupPath);
			config.ForceBackupDelete(true);
			Db4objects.Db4o.Defragment.Defragment.Defrag(config);
			Delete(backupPath);
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void CreateDatabase(IList gaps)
		{
			Delete(TempFile());
			IEmbeddedConfiguration config = NewConfiguration();
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(config, TempFile());
			IList list = new ArrayList();
			list.Add(new FileUsageStatsTestCase.Child());
			FileUsageStatsTestCase.Item item = new FileUsageStatsTestCase.Item(0, "#0", list);
			db.Store(item);
			db.Commit();
			db.Close();
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void Delete(string file)
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.File.Storage.Delete(file);
		}

		protected override IEmbeddedConfiguration NewConfiguration()
		{
			IEmbeddedConfiguration config = base.NewConfiguration();
			config.Common.ObjectClass(typeof(FileUsageStatsTestCase.Item)).ObjectField("_id")
				.Indexed(true);
			config.Common.ObjectClass(typeof(FileUsageStatsTestCase.Item)).ObjectField("_name"
				).Indexed(true);
			config.File.GenerateUUIDs = ConfigScope.Globally;
			config.File.GenerateCommitTimestamps = true;
			return config;
		}
	}
}
#endif // !SILVERLIGHT
