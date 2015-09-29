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
namespace Db4objects.Db4o.Tests.CLI1.Aliases
{
    class JavaFromNetWithSameNameTestCase : JavaFromNetAliasesTestCase
    {
        protected override System.Type GetAliasedDataType()
        {
            return typeof (com.db4o.test.aliases.Person2);
        }
    }
}


namespace com.db4o.test.aliases
{
    public class Person2 : Db4objects.Db4o.Tests.CLI1.Aliases.IPerson
    {
        private string _name;

        public Person2(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public override bool Equals(object obj)
        {
            Person2 other = obj as Person2;
            if (null == other) return false;
            return Db4objects.Db4o.Tests.CLI1.Aliases.CFHelper.AreEqual(_name, other._name);
        }
    }
}
