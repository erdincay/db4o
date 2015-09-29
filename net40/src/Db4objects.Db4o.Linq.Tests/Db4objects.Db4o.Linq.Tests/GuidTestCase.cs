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
using System.Linq;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Linq.Tests
{
	public class GuidTestCase : AbstractDb4oLinqTestCase
	{
		class Item
		{
			internal Item(Guid id, bool active)
			{
				this.active = active;
				this.id = id;
			}

			public Guid Id
			{
				get { return id; }
			}

			public bool Active
			{
				get { return active; }
			}

			public override bool Equals(object obj)
			{
				Item other = obj as Item;
				if (other == null) return false;

				if (other.GetType() != typeof(Item)) return false;

				return (id == other.id) && (active == other.Active);
			}

			public override int GetHashCode()
			{
				return id.GetHashCode() + active.GetHashCode();
			}

			public override string ToString()
			{
				return string.Format("Item(Id={0}, Active={1})", id, active);
			}

			private Guid id;
			private bool active;
		}


		private Item []Items = new[]
		                       	{
		                       		new Item(NewGuid(1), false),
									new Item(NewGuid(2), true),
									new Item(NewGuid(3), true),
		                       	};

		private static Guid NewGuid(byte i)
		{
			return new Guid(1, 2, 3, 4, 5, 6, 7, 8, 9, 0xA, i);
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(Item)).ObjectField("id").Indexed(true);
		}

		protected override void Store()
		{
			foreach (Item item in Items)
			{
				Store(item);
			}
		}

		public void Test()
		{
			Guid guid = Items[1].Id;
			AssertQuery(from Item item in Db() where item.Id == guid && item.Active select item,
						"(Item((id == 00000001-0002-0003-0405-060708090a02) and (active == True)))",
						from item in Items where item.Id == guid && item.Active select item);
		}
	}
}
