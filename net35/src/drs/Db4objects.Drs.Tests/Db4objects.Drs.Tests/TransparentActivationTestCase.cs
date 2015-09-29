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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;
using Sharpen;

namespace Db4objects.Drs.Tests
{
	public class TransparentActivationTestCase : DrsTestCase
	{
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			ActivatableItem item = new ActivatableItem("foo");
			A().Provider().StoreNew(item);
			A().Provider().Commit();
			if (A().Provider() is IDb4oReplicationProvider)
			{
				// TODO: We can't reopen Hibernate providers here if
				// they run on an in-memory database.
				// db4o should be reopened, otherwise Transparent Activation
				// is not tested.
				Reopen();
			}
			ReplicateAll(A().Provider(), B().Provider());
			Runtime.Gc();
			// Improve chances TA is really required
			IObjectSet items = B().Provider().GetStoredObjects(typeof(ActivatableItem));
			Assert.IsTrue(items.HasNext());
			ActivatableItem replicatedItem = (ActivatableItem)items.Next();
			Assert.AreEqual(item.Name(), replicatedItem.Name());
		}
	}
}
