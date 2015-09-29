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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.CS;
using Db4objects.Db4o.Tests.Common.Ext;

namespace Db4objects.Db4o.Tests.Common.Ext
{
	public class TransientFieldRefreshNoClassesOnServerTestCase : ClientServerTestCaseBase
		, ICustomClientServerConfiguration
	{
		private static readonly string OriginalName = "foo";

		private const int OriginalId = 42;

		public class ItemA
		{
			public int persistentId;

			[System.NonSerialized]
			public string transientName;

			public ItemA(int id, string name)
			{
				persistentId = id;
				transientName = name;
			}
		}

		public class ItemB
		{
			[System.NonSerialized]
			public string transientName;

			public ItemB(string name)
			{
				transientName = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureClient(IConfiguration config)
		{
			config.ObjectClass(typeof(TransientFieldRefreshNoClassesOnServerTestCase.ItemB)).
				StoreTransientFields(true);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureServer(IConfiguration config)
		{
			config.ReflectWith(new ExcludingReflector(new Type[] { typeof(TransientFieldRefreshNoClassesOnServerTestCase.ItemA
				), typeof(TransientFieldRefreshNoClassesOnServerTestCase.ItemB) }));
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new TransientFieldRefreshNoClassesOnServerTestCase.ItemA(OriginalId, OriginalName
				));
			Store(new TransientFieldRefreshNoClassesOnServerTestCase.ItemB(OriginalName));
		}

		public virtual void TestRespectsPersistTransientFieldsConfiguration()
		{
			TransientFieldRefreshNoClassesOnServerTestCase.ItemB itemB = ((TransientFieldRefreshNoClassesOnServerTestCase.ItemB
				)RetrieveOnlyInstance(typeof(TransientFieldRefreshNoClassesOnServerTestCase.ItemB
				)));
			Assert.AreEqual(OriginalName, itemB.transientName);
			itemB.transientName = OriginalName + "X";
			Db().Refresh(itemB, int.MaxValue);
			Assert.AreEqual(OriginalName, itemB.transientName);
		}

		public virtual void TestRespectsTransientModifier()
		{
			TransientFieldRefreshNoClassesOnServerTestCase.ItemA item = ((TransientFieldRefreshNoClassesOnServerTestCase.ItemA
				)RetrieveOnlyInstance(typeof(TransientFieldRefreshNoClassesOnServerTestCase.ItemA
				)));
			Assert.IsNull(item.transientName);
			string newName = "Do not touch me";
			item.transientName = newName;
			item.persistentId = OriginalId + 1;
			Db().Refresh(item, int.MaxValue);
			Assert.AreEqual(newName, item.transientName);
			Assert.AreEqual(OriginalId, item.persistentId);
		}
	}
}
#endif // !SILVERLIGHT
