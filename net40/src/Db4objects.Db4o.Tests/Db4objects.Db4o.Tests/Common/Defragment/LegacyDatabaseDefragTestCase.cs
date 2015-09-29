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
using System.IO;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Defragment;
using Db4objects.Db4o.Tests.Common.Migration;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	/// <summary>test case for COR-785</summary>
	public class LegacyDatabaseDefragTestCase : ITestCase
	{
		private const int ItemCount = 50;

		public sealed class Item
		{
			public int value;

			public Item()
			{
			}

			public Item(int value)
			{
				this.value = value;
			}
		}

		// FIXME: solve the workspacePath issue and uncomment this
		/// <exception cref="System.Exception"></exception>
		public virtual void _test()
		{
			string dbFile = GetTempFile();
			CreateLegacyDatabase(dbFile);
			Defrag(dbFile);
			AssertContents(dbFile);
		}

		private void AssertContents(string dbFile)
		{
			IObjectContainer container = Db4oFactory.OpenFile(dbFile);
			try
			{
				IObjectSet found = QueryItems(container);
				for (int i = 1; i < ItemCount; i += 2)
				{
					Assert.IsTrue(found.HasNext());
					Assert.AreEqual(i, ((LegacyDatabaseDefragTestCase.Item)found.Next()).value);
				}
			}
			finally
			{
				container.Close();
			}
		}

		private IObjectSet QueryItems(IObjectContainer container)
		{
			IQuery q = container.Query();
			q.Constrain(typeof(LegacyDatabaseDefragTestCase.Item));
			q.Descend("value").OrderAscending();
			IObjectSet found = q.Execute();
			return found;
		}

		public virtual void CreateDatabase(string fname)
		{
			IObjectContainer container = Db4oFactory.OpenFile(fname);
			try
			{
				FragmentDatabase(container);
			}
			finally
			{
				container.Close();
			}
		}

		private void FragmentDatabase(IObjectContainer container)
		{
			LegacyDatabaseDefragTestCase.Item[] items = CreateItems();
			for (int i = 0; i < items.Length; ++i)
			{
				container.Store(items[i]);
			}
			for (int i = 0; i < items.Length; i += 2)
			{
				container.Delete(items[i]);
			}
		}

		private LegacyDatabaseDefragTestCase.Item[] CreateItems()
		{
			LegacyDatabaseDefragTestCase.Item[] items = new LegacyDatabaseDefragTestCase.Item
				[LegacyDatabaseDefragTestCase.ItemCount];
			for (int i = 0; i < items.Length; ++i)
			{
				items[i] = new LegacyDatabaseDefragTestCase.Item(i);
			}
			return items;
		}

		/// <exception cref="System.IO.IOException"></exception>
		private string GetTempFile()
		{
			return Path.GetTempFileName();
		}

		/// <exception cref="System.IO.IOException"></exception>
		private void Defrag(string dbFile)
		{
			DefragmentConfig config = new DefragmentConfig(dbFile);
			config.UpgradeFile(dbFile + ".upgraded");
			Db4objects.Db4o.Defragment.Defragment.Defrag(config);
		}

		/// <exception cref="System.Exception"></exception>
		private void CreateLegacyDatabase(string dbFile)
		{
			Db4oLibrary library = Librarian().ForVersion("6.1");
			library.environment.InvokeInstanceMethod(GetType(), "createDatabase", new object[
				] { dbFile });
		}

		private Db4oLibrarian Librarian()
		{
			return new Db4oLibrarian(new Db4oLibraryEnvironmentProvider(PathProvider.TestCasePath
				()));
		}
	}
}
