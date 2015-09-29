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

namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsCascadeDeleteToStructs : AbstractDb4oTestCase
    {
        public CDSStruct myStruct;

        protected override void Store()
        {
            myStruct = new CDSStruct(3, "hi");
            Store(this);
        }

        public void TestOne()
        {
            EnsureOccurrences(1, myStruct);
            myStruct.foo = 44;
            myStruct.bar = "cool";
            Db().Store(this);
            EnsureOccurrences(1, myStruct);

            Db().Delete(this);
            Db().Commit();
            EnsureOccurrences(0, myStruct);
        }

        private void EnsureOccurrences(int expected, object template)
        {
			Assert.AreEqual(expected, Db().QueryByExample(template).Count);
        }
    }

    public struct CDSStruct
    {
        public int foo;
        public string bar;

        public CDSStruct(int foo, string bar)
        {
            this.foo = foo;
            this.bar = bar;
        }
    }
}
