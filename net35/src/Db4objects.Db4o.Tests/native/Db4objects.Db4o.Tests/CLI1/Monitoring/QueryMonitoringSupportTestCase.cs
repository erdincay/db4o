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
using System.Diagnostics;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Monitoring;
using Db4objects.Db4o.Monitoring.Internal;
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	class QueryMonitoringSupportTestCase : QueryMonitoringSupportTestCaseBase, ICustomClientServerConfiguration
	{
		protected override void Configure(IConfiguration config)
		{
			config.Add(new QueryMonitoringSupport());
		}

		public void ConfigureServer(IConfiguration config)
		{
			Configure(config);
		}

		public void ConfigureClient(IConfiguration config)
		{
		}

        protected override void Db4oSetupAfterStore()
        {
            Container().ProduceClassMetadata(ReflectClass(typeof(Item)));
        }

		public void TestQueriesPerSecond()
		{
            using (PerformanceCounter counter = PerformanceCounterSpec.QueriesPerSec.PerformanceCounter(FileSession()))
            {
                Assert.AreEqual(0, counter.RawValue);

		        ExecuteGetAllQuery();
		        ExecuteClassOnlyQuery();
		        ExecuteOptimizedNQ();
		        ExecuteUnoptimizedNQ();

#if CF_3_5 || NET_3_5
		        ExecuteOptimizedLinq();
		        ExecuteUnoptimizedLinq();
		        Assert.AreEqual(6, counter.RawValue);
#else
				Assert.AreEqual(4, counter.RawValue);
#endif
            }
		}

		public void TestClassIndexScansPerSecond()
		{
			AssertCounter(
                PerformanceCounterSpec.ClassIndexScansPerSec.PerformanceCounter(FileSession()),
				ExecuteSodaClassIndexScan);
		}
		
		private void ExecuteClassOnlyQuery()
		{
			NewQuery(typeof(Item)).Execute();
		}

		private void ExecuteGetAllQuery()
		{
			NewQuery().Execute();
		}

		private void ExecuteSodaClassIndexScan()
		{
			IQuery query = NewQuery(typeof(Item));
			query.Descend("_id").Constrain(42);
			query.Execute();
		}
	}
}
#endif
