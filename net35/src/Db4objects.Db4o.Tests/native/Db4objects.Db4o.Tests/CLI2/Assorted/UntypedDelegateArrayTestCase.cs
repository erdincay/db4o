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
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	public class UntypedDelegateArrayTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public Action<string> typed;
			public Action<string> untyped;
			public Action<string>[] typedArray;
			public object[] untypedArray;

			public Item(Action<string> action)
			{
				typed = action;
				untyped = action;
				typedArray = new Action<string>[1] {action};
				untypedArray = new object[] {action};
			}
		}

		protected override void Configure(Config.IConfiguration config)
		{
			config.ExceptionsOnNotStorable(true);
			config.CallConstructors(true);
		}

		protected override void Store()
		{
			Store(new Item(StringAction));
		}

		public void Test()
		{
			Item item = RetrieveOnlyInstance<Item>();
			Assert.IsNull(item.typed);
			Assert.IsNull(item.untyped);
			ArrayAssert.AreEqual(new object[1], item.untypedArray);
			ArrayAssert.AreEqual(new Action<string>[1], item.typedArray);
		}

		private static void StringAction(string s)
		{
			throw new NotImplementedException();
		}
	}
}
