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
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    public class UIntHandlerTestCase : TypeHandlerTestCaseBase
    {
		protected override void Configure(Config.IConfiguration config)
		{
			config.ExceptionsOnNotStorable(false);
		}

        public virtual void TestReadWrite()
        {
            MockWriteContext writeContext = new MockWriteContext(Db());
            uint expected = 0x11223344;
            UIntHandler().Write(writeContext, expected);
            MockReadContext readContext = new MockReadContext(writeContext);
            uint uintValue = (uint)UIntHandler().Read(readContext);
            Assert.AreEqual(expected, uintValue);
        }

        public virtual void TestStoreObject()
        {
            Item storedItem = new Item(0x11223344, 0x55667788);
            DoTestStoreObject(storedItem);
        }

        private Db4o.Internal.Handlers.UIntHandler UIntHandler()
        {
            return new Db4o.Internal.Handlers.UIntHandler();
        }

        public class Item
        {
            public uint _uint;

            public UInt32 _uintWrapper;

            public Item(uint u, UInt32 wrapper)
            {
                _uint = u;
                _uintWrapper = wrapper;
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
                return (other._uint == _uint) && _uintWrapper.Equals(other._uintWrapper);
            }

            public override string ToString()
            {
                return "[" + _uint + "," + _uintWrapper + "]";
            }
        }

    }
}
