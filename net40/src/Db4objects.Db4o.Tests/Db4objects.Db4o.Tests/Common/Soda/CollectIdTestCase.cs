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
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	/// <exclude></exclude>
	public class CollectIdTestCase : AbstractDb4oTestCase
	{
		public class ListHolder
		{
			public IList _list;
		}

		public class Parent
		{
			public CollectIdTestCase.Child _child;
		}

		public class Child
		{
			public string _name;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			CollectIdTestCase.ListHolder holder = new CollectIdTestCase.ListHolder();
			holder._list = new ArrayList();
			CollectIdTestCase.Parent parent = new CollectIdTestCase.Parent();
			holder._list.Add(parent);
			parent._child = new CollectIdTestCase.Child();
			parent._child._name = "child";
			Store(holder);
		}

		public virtual void Test()
		{
			IQuery query = NewQuery(typeof(CollectIdTestCase.ListHolder));
			IQuery qList = query.Descend("_list");
			// qList.execute();
			IQuery qChild = qList.Descend("_child");
			qChild.Execute();
		}
		// Query qName = qList.descend("_name");
	}
}
