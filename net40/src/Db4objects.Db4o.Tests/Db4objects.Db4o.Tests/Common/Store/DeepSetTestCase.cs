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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Store;

namespace Db4objects.Db4o.Tests.Common.Store
{
	public class DeepSetTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new DeepSetTestCase().RunSolo();
		}

		public class Item
		{
			public DeepSetTestCase.Item child;

			public string name;
		}

		private DeepSetTestCase.Item _item;

		protected override void Store()
		{
			_item = new DeepSetTestCase.Item();
			_item.name = "1";
			_item.child = new DeepSetTestCase.Item();
			_item.child.name = "2";
			_item.child.child = new DeepSetTestCase.Item();
			_item.child.child.name = "3";
			Store(_item);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IExtObjectContainer oc = Db();
			_item.name = "1";
			DeepSetTestCase.Item item = (DeepSetTestCase.Item)oc.QueryByExample(_item).Next();
			item.name = "11";
			item.child.name = "12";
			oc.Store(item, 2);
			oc.Deactivate(item, int.MaxValue);
			item.name = "11";
			item = (DeepSetTestCase.Item)oc.QueryByExample(item).Next();
			Assert.AreEqual("12", item.child.name);
			Assert.AreEqual("3", item.child.child.name);
		}
	}
}
