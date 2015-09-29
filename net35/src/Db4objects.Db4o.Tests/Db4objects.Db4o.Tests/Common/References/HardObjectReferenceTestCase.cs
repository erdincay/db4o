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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.References;

namespace Db4objects.Db4o.Tests.Common.References
{
	public class HardObjectReferenceTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new HardObjectReferenceTestCase().RunSolo();
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (GetType() != obj.GetType())
				{
					return false;
				}
				return _name.Equals(((HardObjectReferenceTestCase.Item)obj)._name);
			}
		}

		public virtual void TestPeekPersisted()
		{
			HardObjectReferenceTestCase.Item item = new HardObjectReferenceTestCase.Item("one"
				);
			Store(item);
			int id = (int)Db().GetID(item);
			Assert.AreEqual(item, Peek(id)._object);
			Db().Delete(item);
			Db().Commit();
			Assert.IsNull(Peek(id));
		}

		private HardObjectReference Peek(int id)
		{
			return HardObjectReference.PeekPersisted(Trans(), id, 1);
		}
	}
}
