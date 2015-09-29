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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    class IndexedDateTimeUpdateTestCase : HandlerUpdateTestCaseBase
    {

        public class Item
        {
            public DateTime _dateTime;

            public Item(DateTime dateTime)
            {
                _dateTime = dateTime;
            }
        }

        protected override void AssertQueries(IExtObjectContainer objectContainer)
        {
            AssertDateTimeQuery(objectContainer, DateTime.MinValue);
            AssertDateTimeQuery(objectContainer, DateTime.MaxValue);
        }

        private void AssertDateTimeQuery(IExtObjectContainer objectContainer, DateTime value)
        {
            IQuery query = objectContainer.Query();
            query.Constrain(typeof (Item));
            query.Descend("_dateTime").Constrain(value);
            Assert.AreEqual(1, query.Execute().Count);
        }


        protected override string TypeName()
        {
            return "indexedDate";
        }

        protected override object[] CreateValues()
        {
            object[] items = new object[]{
             new Item(DateTime.MinValue),
             new Item(DateTime.MaxValue),
            };
            return items;
        }

        protected override object CreateArrays()
        {
            return null;
        }

        protected override void ConfigureForStore(IConfiguration config)
        {
            config.ObjectClass(typeof(Item)).ObjectField("_dateTime").Indexed(true);
        }


        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {

        }

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            
        }
    }
}
