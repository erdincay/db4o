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
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class UntypedHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] args)
		{
			new UntypedHandlerTestCase().RunSolo();
		}

		public class Item
		{
			public object _member;

			public Item(object member)
			{
				_member = member;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is UntypedHandlerTestCase.Item))
				{
					return false;
				}
				UntypedHandlerTestCase.Item other = (UntypedHandlerTestCase.Item)obj;
				if (this._member.GetType().IsArray)
				{
					return ArraysEquals((object[])this._member, (object[])other._member);
				}
				return this._member.Equals(other._member);
			}

			private bool ArraysEquals(object[] arr1, object[] arr2)
			{
				if (arr1.Length != arr2.Length)
				{
					return false;
				}
				for (int i = 0; i < arr1.Length; i++)
				{
					if (!arr1[i].Equals(arr2[i]))
					{
						return false;
					}
				}
				return true;
			}

			public override int GetHashCode()
			{
				int hash = 7;
				hash = 31 * hash + (null == _member ? 0 : _member.GetHashCode());
				return hash;
			}

			public override string ToString()
			{
				return "[" + _member + "]";
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreIntItem()
		{
			DoTestStoreObject(new UntypedHandlerTestCase.Item(3355));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreStringItem()
		{
			DoTestStoreObject(new UntypedHandlerTestCase.Item("one"));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreArrayItem()
		{
			DoTestStoreObject(new UntypedHandlerTestCase.Item(new string[] { "one", "two", "three"
				 }));
		}
	}
}
