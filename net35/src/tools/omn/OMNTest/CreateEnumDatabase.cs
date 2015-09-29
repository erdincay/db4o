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
using System.IO;
using Db4objects.Db4o;


namespace OMNTest
{
    class CreateEnumDatabase
    {
        static String FILE = "enum.db4o";

        public void Run()
        {
            File.Delete(FILE);
            IObjectContainer objectContainer = Db4oFactory.OpenFile(FILE);
            Store(objectContainer);
            objectContainer.Close();
        }

        private void Store(IObjectContainer objectContainer)
        {
            Item item = new Item();
            item._name = "First";
            item._enumAsInteger = EnumAsInteger.First;
            objectContainer.Store(item);

            item = new Item();
            item._name = "Second";
            item._enumAsInteger = EnumAsInteger.Second;
            objectContainer.Store(item);
        }

        public class Item
        {
            public String _name;

            public EnumAsInteger _enumAsInteger;


        }

        public enum EnumAsInteger
        {
            First,
            Second,
        }

    }


}
