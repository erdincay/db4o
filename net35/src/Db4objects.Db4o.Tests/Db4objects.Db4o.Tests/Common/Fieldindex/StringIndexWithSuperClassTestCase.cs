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
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class StringIndexWithSuperClassTestCase : AbstractDb4oTestCase
	{
		private static readonly string FieldName = "_name";

		private static readonly string FieldValue = "test";

		public class ItemParent
		{
			public int _id;
		}

		public class Item : StringIndexWithSuperClassTestCase.ItemParent
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(StringIndexWithSuperClassTestCase.Item)).ObjectField(FieldName
				).Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new StringIndexWithSuperClassTestCase.Item(FieldValue));
			Store(new StringIndexWithSuperClassTestCase.Item(FieldValue + "X"));
		}

		public virtual void TestIndexAccess()
		{
			IQuery query = NewQuery(typeof(StringIndexWithSuperClassTestCase.Item));
			query.Descend(FieldName).Constrain(FieldValue);
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
