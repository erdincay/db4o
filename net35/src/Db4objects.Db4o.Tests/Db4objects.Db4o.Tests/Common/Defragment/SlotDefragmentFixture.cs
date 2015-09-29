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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Defragment;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public class SlotDefragmentFixture
	{
		public static readonly string PrimitiveFieldname = "_id";

		public static readonly string WrapperFieldname = "_wrapper";

		public static readonly string TypedobjectFieldname = "_next";

		public class Data
		{
			public int _id;

			public int _wrapper;

			public SlotDefragmentFixture.Data _next;

			public Data(int id, SlotDefragmentFixture.Data next)
			{
				_id = id;
				_wrapper = id;
				_next = next;
			}

			public Data()
			{
			}
		}

		public const int Value = 42;

		public static DefragmentConfig DefragConfig(string sourceFile, IEmbeddedConfiguration
			 db4oConfig, bool forceBackupDelete)
		{
			DefragmentConfig defragConfig = new DefragmentConfig(sourceFile, DefragmentTestCaseBase
				.BackupFileNameFor(sourceFile));
			defragConfig.ForceBackupDelete(forceBackupDelete);
			defragConfig.Db4oConfig(Db4oConfig(db4oConfig));
			return defragConfig;
		}

		private static IEmbeddedConfiguration Db4oConfig(IEmbeddedConfiguration db4oConfig
			)
		{
			db4oConfig.Common.ReflectWith(Platform4.ReflectorForType(typeof(SlotDefragmentFixture.Data
				)));
			return db4oConfig;
		}

		public static void CreateFile(string fileName, IEmbeddedConfiguration config)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(config, fileName);
			SlotDefragmentFixture.Data data = null;
			for (int value = Value - 1; value <= Value + 1; value++)
			{
				data = new SlotDefragmentFixture.Data(value, data);
				db.Store(data);
			}
			db.Close();
		}

		public static void ForceIndex(string databaseFileName, IEmbeddedConfiguration config
			)
		{
			config.Common.ObjectClass(typeof(SlotDefragmentFixture.Data)).ObjectField(PrimitiveFieldname
				).Indexed(true);
			config.Common.ObjectClass(typeof(SlotDefragmentFixture.Data)).ObjectField(WrapperFieldname
				).Indexed(true);
			config.Common.ObjectClass(typeof(SlotDefragmentFixture.Data)).ObjectField(TypedobjectFieldname
				).Indexed(true);
			IObjectContainer db = Db4oEmbedded.OpenFile(config, databaseFileName);
			Assert.IsTrue(db.Ext().StoredClass(typeof(SlotDefragmentFixture.Data)).StoredField
				(PrimitiveFieldname, typeof(int)).HasIndex());
			Assert.IsTrue(db.Ext().StoredClass(typeof(SlotDefragmentFixture.Data)).StoredField
				(WrapperFieldname, typeof(int)).HasIndex());
			Assert.IsTrue(db.Ext().StoredClass(typeof(SlotDefragmentFixture.Data)).StoredField
				(TypedobjectFieldname, typeof(SlotDefragmentFixture.Data)).HasIndex());
			db.Close();
		}

		/// <exception cref="System.IO.IOException"></exception>
		public static void AssertIndex(string fieldName, string databaseFileName, IClosure4
			 configProvider)
		{
			ForceIndex(databaseFileName, ((IEmbeddedConfiguration)configProvider.Run()));
			DefragmentConfig defragConfig = new DefragmentConfig(databaseFileName, DefragmentTestCaseBase
				.BackupFileNameFor(databaseFileName));
			defragConfig.Db4oConfig(((IEmbeddedConfiguration)configProvider.Run()));
			Db4objects.Db4o.Defragment.Defragment.Defrag(defragConfig);
			IObjectContainer db = Db4oEmbedded.OpenFile(((IEmbeddedConfiguration)configProvider
				.Run()), databaseFileName);
			IQuery query = db.Query();
			query.Constrain(typeof(SlotDefragmentFixture.Data));
			query.Descend(fieldName).Constrain(Value);
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			db.Close();
		}

		public static void AssertDataClassKnown(string databaseFileName, IEmbeddedConfiguration
			 config, bool expected)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(config, databaseFileName);
			try
			{
				IStoredClass storedClass = db.Ext().StoredClass(typeof(SlotDefragmentFixture.Data
					));
				if (expected)
				{
					Assert.IsNotNull(storedClass);
				}
				else
				{
					Assert.IsNull(storedClass);
				}
			}
			finally
			{
				db.Close();
			}
		}
	}
}
