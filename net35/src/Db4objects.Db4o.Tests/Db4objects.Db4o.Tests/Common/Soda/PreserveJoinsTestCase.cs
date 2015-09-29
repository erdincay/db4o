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
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class PreserveJoinsTestCase : AbstractDb4oTestCase
	{
		public class Parent
		{
			public Parent(PreserveJoinsTestCase.Child child, string value)
			{
				this.child = child;
				this.value = value;
			}

			public PreserveJoinsTestCase.Child child;

			public string value;
		}

		public class Child
		{
			public Child(string name)
			{
				this.name = name;
			}

			public string name;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new PreserveJoinsTestCase.Parent(new PreserveJoinsTestCase.Child("bar"), "parent"
				));
		}

		public virtual void Test()
		{
			IQuery barQuery = Db().Query();
			barQuery.Constrain(typeof(PreserveJoinsTestCase.Child));
			barQuery.Descend("name").Constrain("bar");
			object barObj = barQuery.Execute().Next();
			IQuery query = Db().Query();
			query.Constrain(typeof(PreserveJoinsTestCase.Parent));
			IConstraint c1 = query.Descend("value").Constrain("dontexist");
			IConstraint c2 = query.Descend("child").Constrain(barObj);
			IConstraint c1_and_c2 = c1.And(c2);
			IConstraint cParent = query.Descend("value").Constrain("parent");
			c1_and_c2.Or(cParent);
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
