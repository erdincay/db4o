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
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class DeactivateTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new DeactivateTestCase.Item("foo", new DeactivateTestCase.Item("bar", 
				null)));
		}

		public virtual void Test()
		{
			IQuery query = NewQuery();
			query.Descend("_name").Constrain("foo");
			IObjectSet results = query.Execute();
			Assert.AreEqual(1, results.Count);
			DeactivateTestCase.Item item1 = (DeactivateTestCase.Item)results.Next();
			DeactivateTestCase.Item item2 = item1._child;
			Assert.IsTrue(Db().IsActive(item1));
			Assert.IsTrue(Db().IsActive(item2));
			Db().Deactivate(item1);
			Assert.IsFalse(Db().IsActive(item1));
			Assert.IsTrue(Db().IsActive(item2));
		}

		public static void Main(string[] args)
		{
			new DeactivateTestCase().RunAll();
		}

		public class Item
		{
			public DeactivateTestCase.Item _child;

			public string _name;

			public Item(string name, DeactivateTestCase.Item child)
			{
				_name = name;
				_child = child;
			}
		}
	}
}
