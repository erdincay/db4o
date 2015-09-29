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
using System.Collections;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Constraints;
using Db4objects.Db4o.Tests.Common.Constraints;

namespace Db4objects.Db4o.Tests.Common.Constraints
{
	public class UniqueFieldValueDoesNotThrowTestCase : AbstractDb4oTestCase, ICustomClientServerConfiguration
	{
		public class Item
		{
			public long id;

			public string name;

			public Item()
			{
			}

			public Item(int id, string name)
			{
				this.id = System.Convert.ToInt64(id);
				this.name = name;
			}

			public override int GetHashCode()
			{
				return id.GetHashCode();
			}
		}

		public class Holder
		{
			public Hashtable _items = new Hashtable();

			public virtual void Add(UniqueFieldValueDoesNotThrowTestCase.Item item)
			{
				_items[item] = item.id;
			}
		}

		public virtual void ConfigureClient(IConfiguration config)
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureServer(IConfiguration config)
		{
			Configure(config);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(UniqueFieldValueDoesNotThrowTestCase.Item)).ObjectField
				("id").Indexed(true);
			config.Add(new UniqueFieldValueConstraint(typeof(UniqueFieldValueDoesNotThrowTestCase.Item
				), "id"));
			config.ObjectClass(typeof(UniqueFieldValueDoesNotThrowTestCase.Holder)).CallConstructor
				(true);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Store(NewHolder(new string[] { "foo", "bar" }));
			Db().Commit();
		}

		private object NewHolder(string[] names)
		{
			UniqueFieldValueDoesNotThrowTestCase.Holder holder = new UniqueFieldValueDoesNotThrowTestCase.Holder
				();
			for (int i = 0; i < names.Length; i++)
			{
				holder.Add(new UniqueFieldValueDoesNotThrowTestCase.Item(i, names[i]));
			}
			return holder;
		}
	}
}
