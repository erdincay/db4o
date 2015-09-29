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
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	/// <summary>Jira ticket: COR-373</summary>
	/// <exclude></exclude>
	public class StringIndexCorruptionTestCase : StringIndexTestCaseBase
	{
		public static void Main(string[] arguments)
		{
			new StringIndexCorruptionTestCase().RunSolo();
		}

		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			config.BTreeNodeSize(4);
		}

		public virtual void TestStressSet()
		{
			IExtObjectContainer container = Db();
			int itemCount = 300;
			for (int i = 0; i < itemCount; ++i)
			{
				StringIndexTestCaseBase.Item item = new StringIndexTestCaseBase.Item(ItemName(i));
				container.Store(item);
				container.Store(item);
				container.Commit();
				container.Store(item);
				container.Store(item);
				container.Commit();
			}
			for (int i = 0; i < itemCount; ++i)
			{
				string itemName = ItemName(i);
				StringIndexTestCaseBase.Item found = Query(itemName);
				Assert.IsNotNull(found, "'" + itemName + "' not found");
				Assert.AreEqual(itemName, found.name);
			}
		}

		private string ItemName(int i)
		{
			return "item " + i;
		}
	}
}
