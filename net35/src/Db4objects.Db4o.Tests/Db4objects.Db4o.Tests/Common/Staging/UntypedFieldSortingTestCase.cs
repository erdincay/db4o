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
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	/// <summary>#COR-1790</summary>
	public class UntypedFieldSortingTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public Item(object id)
			{
				_id = id;
			}

			public object _id;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new UntypedFieldSortingTestCase.Item(2));
			Store(new UntypedFieldSortingTestCase.Item(3));
			Store(new UntypedFieldSortingTestCase.Item(1));
		}

		public virtual void Test()
		{
			IQuery query = Db().Query();
			query.Constrain(typeof(UntypedFieldSortingTestCase.Item));
			query.Descend("_id").OrderAscending();
			IObjectSet objectSet = query.Execute();
			int lastId = 0;
			while (objectSet.HasNext())
			{
				UntypedFieldSortingTestCase.Item item = ((UntypedFieldSortingTestCase.Item)objectSet
					.Next());
				int currentId = ((int)item._id);
				Assert.IsGreater(lastId, currentId);
				currentId = lastId;
			}
		}
	}
}
