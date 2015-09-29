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
using System.Collections;
using Db4oUnit;
using Db4objects.Drs;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class TheSimplest : DrsTestCase
	{
		public virtual void Test()
		{
			StoreInA();
			Replicate();
			ModifyInB();
			Replicate2();
			ModifyInA();
			Replicate3();
		}

		private void StoreInA()
		{
			string name = "c1";
			SPCChild child = CreateChildObject(name);
			A().Provider().StoreNew(child);
			A().Provider().Commit();
			EnsureNames(A(), "c1");
		}

		private void Replicate()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureNames(A(), "c1");
			EnsureNames(B(), "c1");
		}

		private void ModifyInB()
		{
			SPCChild child = GetTheObject(B());
			child.SetName("c2");
			B().Provider().Update(child);
			B().Provider().Commit();
			EnsureNames(B(), "c2");
		}

		private void Replicate2()
		{
			ReplicateAll(B().Provider(), A().Provider());
			EnsureNames(A(), "c2");
			EnsureNames(B(), "c2");
		}

		private void ModifyInA()
		{
			SPCChild child = GetTheObject(A());
			child.SetName("c3");
			A().Provider().Update(child);
			A().Provider().Commit();
			EnsureNames(A(), "c3");
		}

		private void Replicate3()
		{
			ReplicateClass(A().Provider(), B().Provider(), typeof(SPCChild));
			EnsureNames(A(), "c3");
			EnsureNames(B(), "c3");
		}

		protected virtual SPCChild CreateChildObject(string name)
		{
			return new SPCChild(name);
		}

		private void EnsureNames(IDrsProviderFixture fixture, string childName)
		{
			EnsureOneInstance(fixture, typeof(SPCChild));
			SPCChild child = GetTheObject(fixture);
			Assert.AreEqual(childName, child.GetName());
		}

		private SPCChild GetTheObject(IDrsProviderFixture fixture)
		{
			return (SPCChild)GetOneInstance(fixture, typeof(SPCChild));
		}

		protected override void ReplicateClass(ITestableReplicationProviderInside providerA
			, ITestableReplicationProviderInside providerB, Type clazz)
		{
			IReplicationSession replication = Replication.Begin(providerA, providerB, _fixtures
				.reflector);
			IEnumerator allObjects = providerA.ObjectsChangedSinceLastReplication(clazz).GetEnumerator
				();
			while (allObjects.MoveNext())
			{
				object obj = allObjects.Current;
				//System.out.println("obj = " + obj);
				replication.Replicate(obj);
			}
			replication.Commit();
		}
	}
}
