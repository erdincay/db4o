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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class CsCascadedDeleteReaddChildReferenceTestCase : Db4oClientServerTestCase
	{
		public class ItemParent
		{
			public CsCascadedDeleteReaddChildReferenceTestCase.Item child;
		}

		public class Item
		{
			public string name;

			public Item(string name_)
			{
				name = name_;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(CsCascadedDeleteReaddChildReferenceTestCase.Item)).ObjectField
				("name").Indexed(true);
			config.ObjectClass(typeof(CsCascadedDeleteReaddChildReferenceTestCase.ItemParent)
				).ObjectField("child").Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			CsCascadedDeleteReaddChildReferenceTestCase.ItemParent parent = new CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				();
			CsCascadedDeleteReaddChildReferenceTestCase.Item child = new CsCascadedDeleteReaddChildReferenceTestCase.Item
				("child");
			parent.child = child;
			Store(parent);
		}

		public virtual void TestDeleteReadd()
		{
			IExtObjectContainer client1 = Db();
			IExtObjectContainer client2 = OpenNewSession();
			CsCascadedDeleteReaddChildReferenceTestCase.ItemParent parent1 = ((CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)RetrieveOnlyInstance(client1, typeof(CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)));
			CsCascadedDeleteReaddChildReferenceTestCase.ItemParent parent2 = ((CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)RetrieveOnlyInstance(client2, typeof(CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)));
			client1.Delete(parent1);
			client1.Commit();
			client2.Ext().Store(parent2, int.MaxValue);
			client2.Commit();
			client2.Close();
			AssertInstanceCountAndFieldIndexes(client1);
		}

		public virtual void TestRepeatedStore()
		{
			IExtObjectContainer client1 = Db();
			IExtObjectContainer client2 = OpenNewSession();
			CsCascadedDeleteReaddChildReferenceTestCase.ItemParent parent1 = ((CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)RetrieveOnlyInstance(client1, typeof(CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)));
			CsCascadedDeleteReaddChildReferenceTestCase.ItemParent parent2 = ((CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)RetrieveOnlyInstance(client2, typeof(CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)));
			client1.Ext().Store(parent1, int.MaxValue);
			client1.Commit();
			client2.Ext().Store(parent2, int.MaxValue);
			client2.Commit();
			client2.Close();
			AssertInstanceCountAndFieldIndexes(client1);
		}

		private void AssertInstanceCountAndFieldIndexes(IExtObjectContainer client1)
		{
			CsCascadedDeleteReaddChildReferenceTestCase.ItemParent parent3 = ((CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)RetrieveOnlyInstance(client1, typeof(CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				)));
			RetrieveOnlyInstance(client1, typeof(CsCascadedDeleteReaddChildReferenceTestCase.Item
				));
			client1.Refresh(parent3, int.MaxValue);
			long parentIdAfterUpdate = client1.GetID(parent3);
			long childIdAfterUpdate = client1.GetID(parent3.child);
			new FieldIndexAssert(typeof(CsCascadedDeleteReaddChildReferenceTestCase.ItemParent
				), "child").AssertSingleEntry(FileSession(), parentIdAfterUpdate);
			new FieldIndexAssert(typeof(CsCascadedDeleteReaddChildReferenceTestCase.Item), "name"
				).AssertSingleEntry(FileSession(), childIdAfterUpdate);
		}

		public static void Main(string[] arguments)
		{
			new CsCascadedDeleteReaddChildReferenceTestCase().RunAll();
		}
	}
}
#endif // !SILVERLIGHT
