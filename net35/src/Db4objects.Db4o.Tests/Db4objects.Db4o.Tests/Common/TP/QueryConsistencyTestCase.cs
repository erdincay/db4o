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
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TP;

namespace Db4objects.Db4o.Tests.Common.TP
{
	public class QueryConsistencyTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new QueryConsistencyTestCase().RunNetworking();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new Item("42"));
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentPersistenceSupport());
			config.OptimizeNativeQueries(false);
		}

		public virtual void TestUpdate()
		{
			Item found = SodaQueryForItem("42");
			Assert.AreEqual("42", found.GetName());
			Assert.AreSame(found, NativeQueryForItem("42"));
			found.SetName("21");
			Assert.AreSame(found, SodaQueryForItem("21"));
			Assert.AreSame(found, NativeQueryForItem("21"));
			Assert.IsNull(SodaQueryForItem("42"));
			Db().Commit();
			Assert.AreSame(found, NativeQueryForItem("21"));
		}

		private Item NativeQueryForItem(string name)
		{
			IObjectSet result = Db().Query(new QueryConsistencyTestCase.ItemByName(name));
			return ((Item)FirstOrNull(result));
		}

		[System.Serializable]
		public sealed class ItemByName : Predicate
		{
			public string name;

			public ItemByName(string name)
			{
				this.name = name;
			}

			public bool Match(Item candidate)
			{
				return candidate.GetName().Equals(name);
			}
		}

		private Item SodaQueryForItem(string id)
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(Item));
			q.Descend("name").Constrain(id);
			return ((Item)FirstOrNull(q.Execute()));
		}

		private object FirstOrNull(IObjectSet result)
		{
			return result.HasNext() ? result.Next() : null;
		}
	}
}
