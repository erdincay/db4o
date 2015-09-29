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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

#if !CF && !SILVERLIGHT

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
    public class ObjectLifecycleMonitoringSupportTestCase : PerformanceCounterTestCaseBase, ICustomClientServerConfiguration
    {

        public class Item
        {

            public Item _child;

            public Item(Item child)
            {
                _child = child;
            }

        }

        public void TestObjectsStored()
        {
            Assert.AreEqual(0, ObjectsStoredPerSec());
            Item item = new Item(new Item(null));
            Store(item);
            Assert.AreEqual(2, ObjectsStoredPerSec());
            Store(item);
            Assert.AreEqual(3, ObjectsStoredPerSec());
        }

        private long ObjectsStoredPerSec()
        {
            return PerformanceCounterSpec.ObjectsStoredPerSec.PerformanceCounter(Container()).RawValue;
        }

        public void TestObjectsDeleted()
        {
            Item item = new Item(new Item(null));
            Store(item);
            Db().Commit();
            Db().Delete(item);
            Db().Commit();
            Assert.AreEqual(2, ObjectsDeletedPerSec());
        }

        private long ObjectsDeletedPerSec()
        {
            return PerformanceCounterSpec.ObjectsDeletedPerSec.PerformanceCounter(FileSession()).RawValue;
        }

        public void TestObjectsActivated()
        {
            IObjectSet objectSet = StoredItems();
            foreach (object o in objectSet)
            {

            }
            Assert.AreEqual(2, ObjectsActivatedPerSec());
        }

        private long ObjectsActivatedPerSec()
        {
            return PerformanceCounterSpec.ObjectsActivatedPerSec.PerformanceCounter(Container()).RawValue;
        }

        public void TestObjectsDeactivated() 
        {
            foreach(object o in StoredItems())
            {
                Db().Deactivate(o);
            }
            Assert.AreEqual(2, ObjectsDeactivatedPerSec());
        }

        private long ObjectsDeactivatedPerSec()
        {
            return PerformanceCounterSpec.ObjectsDeactivatedPerSec.PerformanceCounter(Container()).RawValue;
        }

        private IObjectSet StoredItems()
        {
            Item item = new Item(new Item(null));
            Store(item);
            Reopen();
            IQuery query = NewQuery(typeof(Item));
            return query.Execute();
        }

        protected override void Configure(IConfiguration config)
        {
            base.Configure(config);
            config.Add(new ObjectLifecycleMonitoringSupport());
            config.ObjectClass(typeof(Item)).CascadeOnDelete(true);
        }

        public void ConfigureServer(IConfiguration config)
        {
            Configure(config);
        }

        public void ConfigureClient(IConfiguration config)
        {
            Configure(config);
        }
    }
}
#endif
