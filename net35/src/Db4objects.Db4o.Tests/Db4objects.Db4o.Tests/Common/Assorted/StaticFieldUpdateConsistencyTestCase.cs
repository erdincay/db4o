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
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class StaticFieldUpdateConsistencyTestCase : AbstractDb4oTestCase
	{
		public class SimpleEnum
		{
			public string _name;

			public SimpleEnum(string name)
			{
				_name = name;
			}

			public static StaticFieldUpdateConsistencyTestCase.SimpleEnum A = new StaticFieldUpdateConsistencyTestCase.SimpleEnum
				("A");

			public static StaticFieldUpdateConsistencyTestCase.SimpleEnum B = new StaticFieldUpdateConsistencyTestCase.SimpleEnum
				("B");
		}

		public class Item
		{
			public StaticFieldUpdateConsistencyTestCase.SimpleEnum _value;

			public Item(StaticFieldUpdateConsistencyTestCase.SimpleEnum value)
			{
				_value = value;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.UpdateDepth(5);
			config.ObjectClass(typeof(StaticFieldUpdateConsistencyTestCase.SimpleEnum)).PersistStaticFieldValues
				();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new StaticFieldUpdateConsistencyTestCase.Item(StaticFieldUpdateConsistencyTestCase.SimpleEnum
				.A));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			UpdateAll();
			ConsistencyReport consistencyReport = new ConsistencyChecker(FileSession()).CheckSlotConsistency
				();
			Assert.IsTrue(consistencyReport.Consistent(), consistencyReport.ToString());
		}

		private void UpdateAll()
		{
			IObjectSet result = NewQuery(typeof(StaticFieldUpdateConsistencyTestCase.Item)).Execute
				();
			while (result.HasNext())
			{
				StaticFieldUpdateConsistencyTestCase.Item item = ((StaticFieldUpdateConsistencyTestCase.Item
					)result.Next());
				item._value = (item._value == StaticFieldUpdateConsistencyTestCase.SimpleEnum.A) ? 
					StaticFieldUpdateConsistencyTestCase.SimpleEnum.B : StaticFieldUpdateConsistencyTestCase.SimpleEnum
					.A;
				Store(item);
			}
			Commit();
		}
	}
}
