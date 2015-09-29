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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Ordered;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	/// <exclude></exclude>
	public class OrderByParentFieldTestCase : AbstractDb4oTestCase
	{
		public class Parent
		{
			public string _name;

			public Parent(string name)
			{
				_name = name;
			}
		}

		public class Child : OrderByParentFieldTestCase.Parent
		{
			public int _age;

			public Child(string name, int age) : base(name)
			{
				_age = age;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new OrderByParentFieldTestCase.Child("One", 1));
			Store(new OrderByParentFieldTestCase.Child("Two", 2));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IQuery query = NewQuery(typeof(OrderByParentFieldTestCase.Child));
			query.Descend("_name").OrderAscending();
			query.Execute();
		}
	}
}
