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
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Classindex;

namespace Db4objects.Db4o.Tests.Common.Classindex
{
	public class ClassIndexOffTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		internal static string Name = "1";

		public class Holder
		{
			public ClassIndexOffTestCase.Item _item;

			public ClassIndexOffTestCase.Item _nullItem;

			public Holder(ClassIndexOffTestCase.Item item)
			{
				_item = item;
			}
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		public static void Main(string[] args)
		{
			new ClassIndexOffTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			config.ObjectClass(typeof(ClassIndexOffTestCase.Item)).Indexed(false);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			ClassIndexOffTestCase.Item item = new ClassIndexOffTestCase.Item(Name);
			Store(new ClassIndexOffTestCase.Holder(item));
		}

		public virtual void TestNoItemInIndex()
		{
			IStoredClass storedClass = Db().StoredClass(typeof(ClassIndexOffTestCase.Item));
			Assert.IsFalse(storedClass.HasClassIndex());
			AssertNoItemFoundByQuery();
			Db().Commit();
			AssertNoItemFoundByQuery();
		}

		private void AssertNoItemFoundByQuery()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(ClassIndexOffTestCase.Item));
			Assert.AreEqual(0, q.Execute().Count);
		}

		public virtual void TestRetrievalThroughHolder()
		{
			AssertData();
		}

		private void AssertData()
		{
			ClassIndexOffTestCase.Holder holder = (ClassIndexOffTestCase.Holder)((ClassIndexOffTestCase.Holder
				)RetrieveOnlyInstance(typeof(ClassIndexOffTestCase.Holder)));
			Assert.IsNotNull(holder._item);
			Assert.AreEqual(Name, holder._item._name);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDefragment()
		{
			Defragment();
			AssertData();
		}
	}
}
