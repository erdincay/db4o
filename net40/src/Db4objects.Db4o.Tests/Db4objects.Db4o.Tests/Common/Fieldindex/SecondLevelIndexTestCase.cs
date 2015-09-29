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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class SecondLevelIndexTestCase : AbstractDb4oTestCase, IDiagnosticListener
	{
		public static void Main(string[] arguments)
		{
			new SecondLevelIndexTestCase().RunSolo();
		}

		public class ItemPair
		{
			public SecondLevelIndexTestCase.Item item1;

			public SecondLevelIndexTestCase.Item item2;

			public ItemPair()
			{
			}

			public ItemPair(SecondLevelIndexTestCase.Item item_, SecondLevelIndexTestCase.Item
				 item2_)
			{
				item1 = item_;
				item2 = item2_;
			}
		}

		public class Item
		{
			public string name;

			public Item()
			{
			}

			public Item(string name_)
			{
				name = name_;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Diagnostic().AddListener(this);
			config.ObjectClass(typeof(SecondLevelIndexTestCase.Item)).ObjectField("name").Indexed
				(true);
			config.ObjectClass(typeof(SecondLevelIndexTestCase.ItemPair)).ObjectField("item1"
				).Indexed(true);
			config.ObjectClass(typeof(SecondLevelIndexTestCase.ItemPair)).ObjectField("item2"
				).Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oTearDownBeforeClean()
		{
			Fixture().ConfigureAtRuntime(new _IRuntimeConfigureAction_54());
		}

		private sealed class _IRuntimeConfigureAction_54 : IRuntimeConfigureAction
		{
			public _IRuntimeConfigureAction_54()
			{
			}

			public void Apply(IConfiguration config)
			{
				config.Diagnostic().RemoveAllListeners();
			}
		}

		public virtual void Test()
		{
			SecondLevelIndexTestCase.Item itemOne = new SecondLevelIndexTestCase.Item("one");
			SecondLevelIndexTestCase.Item itemTwo = new SecondLevelIndexTestCase.Item("two");
			Store(new SecondLevelIndexTestCase.ItemPair(itemOne, itemTwo));
			IQuery query = NewQuery(typeof(SecondLevelIndexTestCase.ItemPair));
			query.Descend("item2").Descend("name").Constrain("two");
			IObjectSet objectSet = query.Execute();
			Assert.AreEqual(((SecondLevelIndexTestCase.ItemPair)objectSet.Next()).item1, itemOne
				);
		}

		public virtual void OnDiagnostic(IDiagnostic d)
		{
			Assert.IsFalse(d is LoadedFromClassIndex);
		}
	}
}
