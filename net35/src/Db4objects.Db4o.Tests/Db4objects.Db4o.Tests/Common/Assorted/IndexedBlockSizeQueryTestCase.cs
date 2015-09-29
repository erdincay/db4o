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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class IndexedBlockSizeQueryTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new IndexedBlockSizeQueryTestCase().RunNetworking();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.BlockSize(10);
			config.ObjectClass(typeof(IndexedBlockSizeQueryTestCase.Item)).ObjectField("_name"
				).Indexed(true);
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				// public Object _untypedMember;
				// _untypedMember = name;
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new IndexedBlockSizeQueryTestCase.Item("one"));
		}

		public virtual void Test()
		{
			IQuery q = NewQuery(typeof(IndexedBlockSizeQueryTestCase.Item));
			q.Descend("_name").Constrain("one");
			Assert.AreEqual(1, q.Execute().Count);
		}
	}
}
