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

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Monitoring;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class PerObjectContainerPerformanceCounterTestCase : QueryMonitoringSupportTestCaseBase, IOptOutAllButNetworkingCS, ICustomClientServerConfiguration
	{
		public void ConfigureServer(IConfiguration config)
		{
		}

		public void ConfigureClient(IConfiguration config)
		{
			config.Add(new NativeQueryMonitoringSupport());
		}

		public void Test()
		{
			using (IExtObjectContainer client1 = OpenNewSession())
			using (IExtObjectContainer client2 = OpenNewSession())
			{
				AssertNativeQueriesPerSecond(client1);
				AssertNativeQueriesPerSecond(client2);
			}
		}

		private void AssertNativeQueriesPerSecond(IObjectContainer client)
		{
			AssertCounter(
                PerformanceCounterSpec.NativeQueriesPerSec.PerformanceCounter(client),
				delegate { ExecuteOptimizedNQ(client); });
		}
	}
}
#endif