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
#if !SILVERLIGHT
using System;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class AllTests : Db4oTestSuite
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.CS.AllTests().RunAll();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(Db4objects.Db4o.Tests.Common.CS.Caching.AllTests), typeof(
				Db4objects.Db4o.Tests.Common.CS.Config.AllTests), typeof(Db4objects.Db4o.Tests.Common.CS.Objectexchange.AllTests
				), typeof(BatchActivationTestCase), typeof(CallConstructorsConfigTestCase), typeof(
				ClientDisconnectTestCase), typeof(ClientTimeOutTestCase), typeof(ClientTransactionHandleTestCase
				), typeof(ClientTransactionPoolTestCase), typeof(CloseServerBeforeClientTestCase
				), typeof(CsCascadedDeleteReaddChildReferenceTestCase), typeof(CsDeleteReaddTestCase
				), typeof(IsAliveConcurrencyTestCase), typeof(IsAliveTestCase), typeof(NoTestConstructorsQEStringCmpTestCase
				), typeof(ObjectServerTestCase), typeof(PrefetchConfigurationTestCase), typeof(PrefetchIDCountTestCase
				), typeof(PrefetchObjectCountZeroTestCase), typeof(PrimitiveMessageTestCase), typeof(
				QueryConsistencyTestCase), typeof(ReferenceSystemIsolationTestCase), typeof(SendMessageToClientTestCase
				), typeof(ServerClosedTestCase), typeof(ServerObjectContainerIsolationTestCase), 
				typeof(ServerPortUsedTestCase), typeof(ServerQueryEventsTestCase), typeof(ServerRevokeAccessTestCase
				), typeof(ServerTimeoutTestCase), typeof(ServerToClientTestCase), typeof(ServerTransactionCountTestCase
				), typeof(SetSemaphoreTestCase) };
		}
	}
}
#endif // !SILVERLIGHT
