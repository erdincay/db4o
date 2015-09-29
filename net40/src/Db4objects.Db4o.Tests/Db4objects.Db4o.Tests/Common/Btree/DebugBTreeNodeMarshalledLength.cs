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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Tests.Common.Btree;

namespace Db4objects.Db4o.Tests.Common.Btree
{
	public class DebugBTreeNodeMarshalledLength : AbstractDb4oTestCase
	{
		public class Item
		{
			public int _int;

			public string _string;
		}

		public static void Main(string[] args)
		{
			new DebugBTreeNodeMarshalledLength().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			config.ObjectClass(typeof(DebugBTreeNodeMarshalledLength.Item)).ObjectField("_int"
				).Indexed(true);
			config.ObjectClass(typeof(DebugBTreeNodeMarshalledLength.Item)).ObjectField("_string"
				).Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < 50000; i++)
			{
				Store(new DebugBTreeNodeMarshalledLength.Item());
			}
		}

		public virtual void Test()
		{
			BTree btree = Btree().DebugLoadFully(SystemTrans());
			Store(new DebugBTreeNodeMarshalledLength.Item());
			btree.Write(SystemTrans());
		}

		private BTree Btree()
		{
			IClassIndexStrategy index = ClassMetadataFor(typeof(DebugBTreeNodeMarshalledLength.Item
				)).Index();
			return ((BTreeClassIndexStrategy)index).Btree();
		}
	}
}
