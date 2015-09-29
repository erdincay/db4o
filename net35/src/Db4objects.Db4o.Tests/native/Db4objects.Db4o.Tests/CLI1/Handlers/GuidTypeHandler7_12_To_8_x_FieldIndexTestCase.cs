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
using System.IO;
using System.Linq;
using Db4oUnit;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Tests.Common.Migration;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	public class GuidTypeHandler7_12_To_8_x_FieldIndexTestCase : ITestLifeCycle
	{
#if !CF && !SILVERLIGHT
		class Item
		{
			public Guid id;
			public string name;
		}

		public void SetUp()
		{
			db4oFilePath = Path.GetTempFileName();
			if (environmentProvider == null)
			{
				environmentProvider = new Db4oLibraryEnvironmentProvider(PathProvider.TestCasePath());
			}
		}

		public void TearDown()
		{
			testsFinished++;
			if (testsFinished == testMethodCount)
			{
				environmentProvider.DisposeAll();
				testsFinished = 0;
			}

			if (db4oFilePath != null)
			{
				File.Delete(db4oFilePath);
			}
		}

		public void TestDefragWorksAfterReopening()
		{
			Environment().InvokeInstanceMethod(GetType(), "CreateDatabase", db4oFilePath, true);

			using(var db = Db4oEmbedded.OpenFile(db4oFilePath))
			{
			}

			var config = new DefragmentConfig(db4oFilePath);
			config.ForceBackupDelete(true);
			Defragment.Defragment.Defrag(config);
		}

		private Db4oLibraryEnvironment Environment()
		{
			if (environment == null)
			{
				var envPath = Path.Combine(Db4oLibrarian.LibraryPath(), "7.1\\" + typeof(Db4oEmbedded).Assembly.GetName().Name + ".dll");
				environment = environmentProvider.EnvironmentFor(envPath);
			}
			return environment;
		}

		public void TestStoreWorksAfterReopening()
		{
			db4oFilePath = Path.GetTempFileName();
			Environment().InvokeInstanceMethod(GetType(), "CreateDatabase", db4oFilePath, true);

			using (var db = Db4oEmbedded.OpenFile(db4oFilePath))
			{
			}

			using(var db = Db4oEmbedded.OpenFile(db4oFilePath))
			{
				db.Store(new Item { id = Guid.NewGuid(), name= "Foo " + Db4oVersion.Name});
			}
		}
		
		public void CreateDatabase(string path, bool indexed)
		{
			var config = Db4oFactory.NewConfiguration();
			config.ObjectClass(typeof (Item)).ObjectField("id").Indexed(indexed);

			using (var db = Db4oFactory.OpenFile(config, path))
			{
				db.Store(new Item { id = Guid.NewGuid(), name = "Foo" });
				db.Store(new Item { id = Guid.NewGuid(), name = "Bar" });
				db.Store(new Item { id = Guid.NewGuid(), name = "Baz" });
			}
		}


		static GuidTypeHandler7_12_To_8_x_FieldIndexTestCase()
		{
			testMethodCount = TestMethodCountFor(typeof(GuidTypeHandler7_12_To_8_x_FieldIndexTestCase));
		}

		private static int TestMethodCountFor(Type type)
		{
			return type.GetMethods().Count(method => method.Name.StartsWith("Test") && method.GetParameters().Length == 0);
		}

		private string db4oFilePath;
		private static Db4oLibraryEnvironmentProvider environmentProvider;
		private static Db4oLibraryEnvironment environment;
		private static int testMethodCount;
		private static int testsFinished;
#else
		public void SetUp()
		{
		}

		public void TearDown()
		{
		}
#endif
	}
}
