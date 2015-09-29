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
	public class DeactivateDeletedObjectOnRollbackStrategyTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			config.Add(new TransparentPersistenceSupport(new _IRollbackStrategy_21()));
		}

		private sealed class _IRollbackStrategy_21 : IRollbackStrategy
		{
			public _IRollbackStrategy_21()
			{
			}

			public void Rollback(IObjectContainer container, object obj)
			{
				container.Ext().Deactivate(obj);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new Item("foo.tbd"));
		}

		public virtual void Test()
		{
			Item tbd = InsertAndRetrieve();
			tbd.SetName("foo.deleted");
			Db().Delete(tbd);
			Db().Rollback();
			Assert.AreEqual("foo.tbd", tbd.GetName());
		}

		private Item InsertAndRetrieve()
		{
			IQuery query = NewQuery(typeof(Item));
			query.Descend("name").Constrain("foo.tbd");
			IObjectSet set = query.Execute();
			Assert.AreEqual(1, set.Count);
			return (Item)set.Next();
		}

		public static void Main(string[] args)
		{
			new DeactivateDeletedObjectOnRollbackStrategyTestCase().RunAll();
		}
	}
}
