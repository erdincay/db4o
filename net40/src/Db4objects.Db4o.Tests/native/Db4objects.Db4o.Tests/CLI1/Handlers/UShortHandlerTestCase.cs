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
using System;
using Db4objects.Db4o.Config;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    public class UShortHandlerTestCase : TypeHandlerTestCaseBase
    {
		protected override void Configure(IConfiguration config)
		{
			config.ExceptionsOnNotStorable(false);
		}

        public virtual void TestReadWrite()
        {
            MockWriteContext writeContext = new MockWriteContext(Db());
            ushort expected = 0x1122;
            UShortHandler().Write(writeContext, expected);
            MockReadContext readContext = new MockReadContext(writeContext);
            ushort ushortValue = (ushort)UShortHandler().Read(readContext);
            Assert.AreEqual(expected, ushortValue);
        }

        public virtual void TestStoreObject()
        {
            ULongHandlerTestCase.Item storedItem = new ULongHandlerTestCase.Item(0x1122, 0x8877);
            DoTestStoreObject(storedItem);
        }

        private Db4o.Internal.Handlers.UShortHandler UShortHandler()
        {
            return new Db4o.Internal.Handlers.UShortHandler();
        }

        public class Item
        {
            public ushort _ushort;

            public UInt16 _ushortWrapper;

            public Item(ushort u, UInt16 wrapper)
            {
                _ushort = u;
                _ushortWrapper = wrapper;
            }

            public override bool Equals(object obj)
            {
                if (obj == this)
                {
                    return true;
                }
                if (!(obj is Item))
                {
                    return false;
                }
                Item other = (Item)obj;
                return (other._ushort == _ushort) && _ushortWrapper.Equals(other._ushortWrapper
                    );
            }

            public override string ToString()
            {
                return "[" + _ushort + "," + _ushortWrapper + "]";
            }
        }

    }
}