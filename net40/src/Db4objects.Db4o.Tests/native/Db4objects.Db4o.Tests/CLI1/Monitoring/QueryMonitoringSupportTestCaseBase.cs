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
using System.Diagnostics;
#if !CF && !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.Foundation;
#if CF_3_5 || NET_3_5
using System.Linq;
using Db4objects.Db4o.Linq;
#endif

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class QueryMonitoringSupportTestCaseBase : PerformanceCounterTestCaseBase
	{

#if CF_3_5 || NET_3_5
		protected void ExecuteOptimizedLinq()
		{
			var found = (from Item item in Db()
			             where item.id == 42
			             select item).ToArray();
		}

		protected void ExecuteUnoptimizedLinq()
		{
			var found = (from Item item in Db()
			             where item.GetType() == typeof(Item)
			             select item).ToArray();
		}
#endif
		protected void AssertCounter(PerformanceCounter performanceCounter, Action4 action)
		{
			using (PerformanceCounter counter = performanceCounter)
			{
				Assert.AreEqual(0, counter.RawValue);

				for (int i = 0; i < 3; ++i)
				{
					action();
					Assert.AreEqual(i + 1, counter.RawValue);
				}
			}
		}

		protected void ExecuteOptimizedNQ()
		{
			ExecuteOptimizedNQ(Db());
		}

		protected void ExecuteOptimizedNQ(IObjectContainer container)
		{
			Predicate<Item> match = delegate(Item item) { return item.id == 42; };
			container.Query(match);
		}

		protected void ExecuteUnoptimizedNQ()
		{
			Db().Query<Item>(delegate(Item item) { return item.GetType() == typeof (Item); });
		}

		public class Item
		{
			public int id;
		}
	}
}
#endif