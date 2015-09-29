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
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class CascadeOnDeleteTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public string item;
		}

		public class Holder
		{
			public CascadeOnDeleteTestCase.Item[] items;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestNoAccidentalDeletes()
		{
			AssertNoAccidentalDeletes(true, true);
			AssertNoAccidentalDeletes(true, false);
			AssertNoAccidentalDeletes(false, true);
			AssertNoAccidentalDeletes(false, false);
		}

		/// <exception cref="System.Exception"></exception>
		private void AssertNoAccidentalDeletes(bool cascadeOnUpdate, bool cascadeOnDelete
			)
		{
			DeleteAll(typeof(CascadeOnDeleteTestCase.Holder));
			DeleteAll(typeof(CascadeOnDeleteTestCase.Item));
			IObjectClass oc = Fixture().Config().ObjectClass(typeof(CascadeOnDeleteTestCase.Holder
				));
			oc.CascadeOnDelete(cascadeOnDelete);
			oc.CascadeOnUpdate(cascadeOnUpdate);
			Reopen();
			CascadeOnDeleteTestCase.Item item = new CascadeOnDeleteTestCase.Item();
			CascadeOnDeleteTestCase.Holder holder = new CascadeOnDeleteTestCase.Holder();
			holder.items = new CascadeOnDeleteTestCase.Item[] { item };
			Db().Store(holder);
			Db().Commit();
			holder.items[0].item = "abrakadabra";
			Db().Store(holder);
			if (!cascadeOnDelete && !cascadeOnUpdate)
			{
				// the only case, where we don't cascade
				Db().Store(holder.items[0]);
			}
			Assert.AreEqual(1, CountOccurences(typeof(CascadeOnDeleteTestCase.Item)));
			Db().Commit();
			Assert.AreEqual(1, CountOccurences(typeof(CascadeOnDeleteTestCase.Item)));
		}
	}
}
