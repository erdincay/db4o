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
using Db4objects.Db4o.Tests.Common.Updatedepth;

namespace Db4objects.Db4o.Tests.Common.Updatedepth
{
	public class UpdateDepthWithCascadingDeleteTestCase : AbstractDb4oTestCase
	{
		private const int ChildId = 2;

		private const int RootId = 1;

		public class Item
		{
			public UpdateDepthWithCascadingDeleteTestCase.Item _item;

			public int _id;

			public Item(int id, UpdateDepthWithCascadingDeleteTestCase.Item item)
			{
				_id = id;
				_item = item;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(UpdateDepthWithCascadingDeleteTestCase.Item)).CascadeOnDelete
				(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new UpdateDepthWithCascadingDeleteTestCase.Item(RootId, new UpdateDepthWithCascadingDeleteTestCase.Item
				(ChildId, null)));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestUpdateDepth()
		{
			UpdateDepthWithCascadingDeleteTestCase.Item item = QueryItemByID(RootId);
			int changedRootID = 42;
			item._id = changedRootID;
			item._item._id = 43;
			Store(item);
			Reopen();
			UpdateDepthWithCascadingDeleteTestCase.Item changed = QueryItemByID(changedRootID
				);
			Assert.AreEqual(ChildId, changed._item._id);
		}

		private UpdateDepthWithCascadingDeleteTestCase.Item QueryItemByID(int id)
		{
			IQuery query = NewQuery(typeof(UpdateDepthWithCascadingDeleteTestCase.Item));
			query.Descend("_id").Constrain(id);
			IObjectSet result = query.Execute();
			Assert.IsTrue(result.HasNext());
			UpdateDepthWithCascadingDeleteTestCase.Item item = ((UpdateDepthWithCascadingDeleteTestCase.Item
				)result.Next());
			return item;
		}
	}
}
