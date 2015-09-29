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
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class IntHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] args)
		{
			new IntHandlerTestCase().RunSolo();
		}

		public class Item
		{
			public int _int;

			public int _intWrapper;

			public Item(int i, int wrapper)
			{
				_int = i;
				_intWrapper = wrapper;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is IntHandlerTestCase.Item))
				{
					return false;
				}
				IntHandlerTestCase.Item other = (IntHandlerTestCase.Item)obj;
				return (other._int == this._int) && this._intWrapper.Equals(other._intWrapper);
			}

			public override string ToString()
			{
				return "[" + _int + "," + _intWrapper + "]";
			}
		}

		private IntHandler IntHandler()
		{
			return new IntHandler();
		}

		public virtual void TestReadWrite()
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			int expected = 100;
			IntHandler().Write(writeContext, expected);
			MockReadContext readContext = new MockReadContext(writeContext);
			int intValue = (int)IntHandler().Read(readContext);
			Assert.AreEqual(expected, intValue);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreObject()
		{
			IntHandlerTestCase.Item storedItem = new IntHandlerTestCase.Item(100, 200);
			DoTestStoreObject(storedItem);
		}
	}
}
