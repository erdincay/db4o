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
using Db4objects.Db4o;
using Db4objects.Drs;
using Db4objects.Drs.Foundation;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class TimestampTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			ITestableReplicationProviderInside providerA = A().Provider();
			ITestableReplicationProviderInside providerB = B().Provider();
			providerA.StoreNew(new Pilot());
			providerA.Commit();
			IReplicationSession replication = Replication.Begin(providerA, providerB, null, _fixtures
				.reflector);
			TimeStamps initialTimeStampsB = AssertTimeStampsForFirstReplication(providerB);
			IObjectSet modifiedObjects = providerA.ObjectsChangedSinceLastReplication();
			while (modifiedObjects.HasNext())
			{
				replication.Replicate(modifiedObjects.Next());
			}
			replication.Commit();
			Pilot replicatedPilot = (Pilot)providerB.GetStoredObjects(typeof(Pilot)).Next();
			long version = providerB.ObjectVersion(replicatedPilot);
			Assert.AreEqual(initialTimeStampsB.Commit(), version);
			replication = Replication.Begin(providerA, providerB, null, _fixtures.reflector);
			TimeStamps timestampsBAfterReplication = AssertTimeStampsForSecondReplication(initialTimeStampsB
				, providerB);
			replication.Commit();
			Pilot pilotStoredAfterReplication = new Pilot();
			providerB.StoreNew(pilotStoredAfterReplication);
			providerB.Commit();
			providerB.WaitForPreviousCommits();
			version = providerB.ObjectVersion(pilotStoredAfterReplication);
			Assert.IsGreater(timestampsBAfterReplication.Commit(), version);
		}

		private TimeStamps AssertTimeStampsForFirstReplication(ITestableReplicationProviderInside
			 provider)
		{
			TimeStamps timeStamps = provider.TimeStamps();
			Assert.IsNotNull(timeStamps);
			Assert.AreEqual(0, timeStamps.From());
			Assert.IsGreater(0, timeStamps.To());
			Assert.AreEqual(timeStamps.To() + 1, timeStamps.Commit());
			return timeStamps;
		}

		private TimeStamps AssertTimeStampsForSecondReplication(TimeStamps initialTimeStamps
			, ITestableReplicationProviderInside provider)
		{
			TimeStamps timeStamps = provider.TimeStamps();
			Assert.IsNotNull(timeStamps);
			Assert.AreEqual(initialTimeStamps.Commit(), timeStamps.From());
			Assert.IsGreater(timeStamps.From(), timeStamps.To());
			Assert.AreEqual(timeStamps.To() + 1, timeStamps.Commit());
			return timeStamps;
		}
	}
}
