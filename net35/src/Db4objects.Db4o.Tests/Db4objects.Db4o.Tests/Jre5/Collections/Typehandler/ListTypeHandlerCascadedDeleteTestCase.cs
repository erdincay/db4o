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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	/// <exclude></exclude>
	public class ListTypeHandlerCascadedDeleteTestCase : AbstractDb4oTestCase
	{
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			new ListTypeHandlerCascadedDeleteTestCase().RunSolo();
		}

		public class Item
		{
			public object _untypedList;

			public ArrayList _typedList;
		}

		public class Element
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(ListTypeHandlerCascadedDeleteTestCase.Item)).CascadeOnDelete
				(true);
			config.ObjectClass(typeof(ArrayList)).CascadeOnDelete(true);
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(ArrayList))
				, new CollectionTypeHandler());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			ListTypeHandlerCascadedDeleteTestCase.Item item = new ListTypeHandlerCascadedDeleteTestCase.Item
				();
			item._untypedList = new ArrayList();
			((IList)item._untypedList).Add(new ListTypeHandlerCascadedDeleteTestCase.Element(
				));
			item._typedList = new ArrayList();
			item._typedList.Add(new ListTypeHandlerCascadedDeleteTestCase.Element());
			Store(item);
		}

		public virtual void TestCascadedDelete()
		{
			ListTypeHandlerCascadedDeleteTestCase.Item item = (ListTypeHandlerCascadedDeleteTestCase.Item
				)RetrieveOnlyInstance(typeof(ListTypeHandlerCascadedDeleteTestCase.Item));
			Db4oAssert.PersistedCount(2, typeof(ListTypeHandlerCascadedDeleteTestCase.Element
				));
			Db().Delete(item);
			Db().Purge();
			Db().Commit();
			Db4oAssert.PersistedCount(0, typeof(ListTypeHandlerCascadedDeleteTestCase.Item));
			Db4oAssert.PersistedCount(0, typeof(ArrayList));
			Db4oAssert.PersistedCount(0, typeof(ListTypeHandlerCascadedDeleteTestCase.Element
				));
		}

		public virtual void TestArrayListCount()
		{
			Db4oAssert.PersistedCount(2, typeof(ArrayList));
		}
	}
}
