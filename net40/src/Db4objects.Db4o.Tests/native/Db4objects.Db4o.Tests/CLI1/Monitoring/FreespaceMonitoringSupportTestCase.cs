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
#if !CF && !SILVERLIGHT

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Monitoring;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
    public class FreespaceMonitoringSupportTestCase : PerformanceCounterTestCaseBase
    {
        public class Item{
		
	    }
	

        protected override void  Configure(Db4objects.Db4o.Config.IConfiguration config)
        {
 	         config.Add(new FreespaceMonitoringSupport());
        }

	
	    public void Test(){
            // ensure client is fully connected to the server already
            Db().Commit();
		    AssertMonitoredFreespaceIsCorrect();
		    Item item = new Item();
		    Store(item);
		    Db().Commit();
		    AssertMonitoredFreespaceIsCorrect();
		    Db().Delete(item);
		    Db().Commit();
		    AssertMonitoredFreespaceIsCorrect();
	    }

	    private void AssertMonitoredFreespaceIsCorrect() {
		    IFreespaceManager freespaceManager = FileSession().FreespaceManager();
	        FreespaceCountingVisitor visitor = new FreespaceCountingVisitor();
	        freespaceManager.Traverse(visitor);
	        int freespace = visitor.TotalFreespace;
            int slotCount = visitor.SlotCount;
	        int averageSlotSize = slotCount == 0 ? 0 : freespace/slotCount;
            Assert.AreEqual(freespace, TotalFreespace());
	        Assert.AreEqual(slotCount, SlotCount());
            Assert.AreEqual(averageSlotSize, AverageSlotSize());
        }

        public class FreespaceCountingVisitor : IVisitor4
        {
            private int _totalFreespace;
            private int _slotCount;

            public int TotalFreespace
            {
                get { return _totalFreespace; }
            }

            public int SlotCount
            {
                get { return _slotCount; }
            }

            public void Visit(object obj)
            {
                Slot slot = obj as Slot;
                _totalFreespace += slot.Length();
                _slotCount++;
            }
            
        }
	
	    private int TotalFreespace() {
            return (int)PerformanceCounterSpec.TotalFreespace.PerformanceCounter(FileSession()).RawValue;
	    }
	
	    private int SlotCount() {
	        return (int) PerformanceCounterSpec.FreespaceSlotCount.PerformanceCounter(FileSession()).RawValue;
	    }

        private int AverageSlotSize()
        {
            return (int)PerformanceCounterSpec.FreespaceAverageSlotSize.PerformanceCounter(FileSession()).RawValue;
        }

    }
}
#endif
