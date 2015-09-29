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
	public class BooleanHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] arguments)
		{
			new BooleanHandlerTestCase().RunSolo();
		}

		public class Item
		{
			public bool _boolWrapper;

			public bool _bool;

			public Item(bool boolWrapper, bool @bool)
			{
				_boolWrapper = boolWrapper;
				_bool = @bool;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is BooleanHandlerTestCase.Item))
				{
					return false;
				}
				BooleanHandlerTestCase.Item other = (BooleanHandlerTestCase.Item)obj;
				return (other._bool == this._bool) && this._boolWrapper.Equals(other._boolWrapper
					);
			}

			public override string ToString()
			{
				return "[" + _bool + "," + _boolWrapper + "]";
			}
		}

		private BooleanHandler BooleanHandler()
		{
			return new BooleanHandler();
		}

		public virtual void TestReadWriteTrue()
		{
			DoTestReadWrite(true);
		}

		public virtual void TestReadWriteFalse()
		{
			DoTestReadWrite(false);
		}

		public virtual void DoTestReadWrite(bool b)
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			BooleanHandler().Write(writeContext, b);
			MockReadContext readContext = new MockReadContext(writeContext);
			bool res = (bool)BooleanHandler().Read(readContext);
			Assert.AreEqual(b, res);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreObject()
		{
			BooleanHandlerTestCase.Item storedItem = new BooleanHandlerTestCase.Item(false, true
				);
			DoTestStoreObject(storedItem);
		}
	}
}
