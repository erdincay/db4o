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
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class SimplestPossibleTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new SimplestPossibleTestCase().RunNetworking();
		}

		protected override void Store()
		{
			Db().Store(new SimplestPossibleTestCase.Item("one"));
		}

		public virtual void Test()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(SimplestPossibleTestCase.Item));
			q.Descend("name").Constrain("one");
			IObjectSet objectSet = q.Execute();
			SimplestPossibleTestCase.Item item = (SimplestPossibleTestCase.Item)objectSet.Next
				();
			Assert.IsNotNull(item);
			Assert.AreEqual("one", item.GetName());
		}

		public class Item
		{
			public string name;

			public Item()
			{
			}

			public Item(string name_)
			{
				this.name = name_;
			}

			public virtual string GetName()
			{
				return name;
			}
		}
	}
}
