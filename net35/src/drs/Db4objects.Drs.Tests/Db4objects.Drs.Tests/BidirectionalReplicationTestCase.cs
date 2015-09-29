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
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Drs;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class BidirectionalReplicationTestCase : DrsTestCase
	{
		public virtual void TestObjectsAreOnlyReplicatedOnce()
		{
			ITestableReplicationProviderInside providerA = A().Provider();
			ITestableReplicationProviderInside providerB = B().Provider();
			StoreNewPilotIn(providerA);
			int replicatedObjects = ReplicateBidirectional(providerA, providerB, null);
			Assert.AreEqual(1, replicatedObjects);
			ModifyPilotIn(providerA, "modifiedInA");
			replicatedObjects = ReplicateBidirectional(providerA, providerB, typeof(Pilot));
			Assert.AreEqual(1, replicatedObjects);
			ModifyPilotIn(providerB, "modifiedInB");
			replicatedObjects = ReplicateBidirectional(providerA, providerB, null);
			Assert.AreEqual(1, replicatedObjects);
			StoreNewPilotIn(providerA);
			StoreNewPilotIn(providerB);
			replicatedObjects = ReplicateBidirectional(providerA, providerB, typeof(Pilot));
			Assert.AreEqual(2, replicatedObjects);
		}

		private void ModifyPilotIn(ITestableReplicationProviderInside provider, string newName
			)
		{
			Pilot pilot;
			pilot = (Pilot)provider.GetStoredObjects(typeof(Pilot)).Next();
			pilot.SetName(newName);
			provider.Update(pilot);
			provider.Commit();
			provider.WaitForPreviousCommits();
		}

		private int ReplicateBidirectional(ITestableReplicationProviderInside providerA, 
			ITestableReplicationProviderInside providerB, Type clazz)
		{
			int replicatedObjects = 0;
			IReplicationSession replicationSession = Replication.Begin(providerA, providerB, 
				null, _fixtures.reflector);
			IObjectSet changedInA = clazz == null ? providerA.ObjectsChangedSinceLastReplication
				() : providerA.ObjectsChangedSinceLastReplication(clazz);
			foreach (object obj in changedInA)
			{
				replicatedObjects++;
				replicationSession.Replicate(obj);
			}
			IObjectSet changedInB = clazz == null ? providerB.ObjectsChangedSinceLastReplication
				() : providerB.ObjectsChangedSinceLastReplication(clazz);
			foreach (object obj in changedInB)
			{
				replicatedObjects++;
				replicationSession.Replicate(obj);
			}
			replicationSession.Commit();
			return replicatedObjects;
		}

		private void StoreNewPilotIn(ITestableReplicationProviderInside provider)
		{
			Pilot pilot = new Pilot();
			provider.StoreNew(pilot);
			provider.Commit();
			provider.WaitForPreviousCommits();
		}
	}
}
