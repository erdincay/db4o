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
using System.Collections;
using Db4oUnit;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class MapTest : DrsTestCase
	{
		protected virtual void ActualTest()
		{
			StoreMapToProviderA();
			ReplicateAllToProviderBFirstTime();
			ModifyInProviderB();
			ReplicateAllStep2();
			AddElementInProviderA();
			ReplicateHolderStep3();
		}

		private void StoreMapToProviderA()
		{
			MapHolder mh = new MapHolder("h1");
			MapContent mc1 = new MapContent("c1");
			MapContent mc2 = new MapContent("c2");
			mh.Put("key1", mc1);
			mh.Put("key2", mc2);
			A().Provider().StoreNew(mh);
			A().Provider().Commit();
			EnsureContent(A(), new string[] { "h1" }, new string[] { "key1", "key2" }, new string
				[] { "c1", "c2" });
		}

		private void ReplicateAllToProviderBFirstTime()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureContent(A(), new string[] { "h1" }, new string[] { "key1", "key2" }, new string
				[] { "c1", "c2" });
			EnsureContent(B(), new string[] { "h1" }, new string[] { "key1", "key2" }, new string
				[] { "c1", "c2" });
		}

		private void ModifyInProviderB()
		{
			MapHolder mh = (MapHolder)GetOneInstance(B(), typeof(MapHolder));
			mh.SetName("h2");
			MapContent mc1 = (MapContent)mh.GetMap()["key1"];
			MapContent mc2 = (MapContent)mh.GetMap()["key2"];
			mc1.SetName("co1");
			mc2.SetName("co2");
			B().Provider().Update(mc1);
			B().Provider().Update(mc2);
			B().Provider().Update(mh.GetMap());
			B().Provider().Update(mh);
			B().Provider().Commit();
			EnsureContent(B(), new string[] { "h2" }, new string[] { "key1", "key2" }, new string
				[] { "co1", "co2" });
		}

		private void ReplicateAllStep2()
		{
			ReplicateAll(B().Provider(), A().Provider());
			EnsureContent(A(), new string[] { "h2" }, new string[] { "key1", "key2" }, new string
				[] { "co1", "co2" });
			EnsureContent(B(), new string[] { "h2" }, new string[] { "key1", "key2" }, new string
				[] { "co1", "co2" });
		}

		private void AddElementInProviderA()
		{
			MapHolder mh = (MapHolder)GetOneInstance(A(), typeof(MapHolder));
			mh.SetName("h3");
			MapContent mc3 = new MapContent("co3");
			A().Provider().StoreNew(mc3);
			mh.GetMap()["key3"] = mc3;
			A().Provider().Update(mh.GetMap());
			A().Provider().Update(mh);
			A().Provider().Commit();
			EnsureContent(A(), new string[] { "h3" }, new string[] { "key1", "key2", "key3" }
				, new string[] { "co1", "co2", "co3" });
		}

		private void ReplicateHolderStep3()
		{
			ReplicateClass(A().Provider(), B().Provider(), typeof(MapHolder));
			EnsureContent(A(), new string[] { "h3" }, new string[] { "key1", "key2", "key3" }
				, new string[] { "co1", "co2", "co3" });
			EnsureContent(B(), new string[] { "h3" }, new string[] { "key1", "key2", "key3" }
				, new string[] { "co1", "co2", "co3" });
		}

		private void EnsureContent(IDrsProviderFixture fixture, string[] holderNames, string
			[] keyNames, string[] valueNames)
		{
			int holderCount = holderNames.Length;
			EnsureInstanceCount(fixture, typeof(MapHolder), holderCount);
			// After dropping generating uuid for collection, it does not
			//  make sense to count collection because collection is never reused
			// ensureInstanceCount(provider, Map.class, holderCount);
			int i = 0;
			IEnumerator objectSet = fixture.Provider().GetStoredObjects(typeof(MapHolder)).GetEnumerator
				();
			while (objectSet.MoveNext())
			{
				MapHolder lh = (MapHolder)objectSet.Current;
				Assert.AreEqual(holderNames[i], lh.GetName());
				IDictionary Map = lh.GetMap();
				for (int j = 0; j < keyNames.Length; j++)
				{
					MapContent mc = (MapContent)Map[keyNames[j]];
					string name = mc.GetName();
					Assert.AreEqual(valueNames[j], name);
				}
			}
		}

		public virtual void Test()
		{
			ActualTest();
		}
	}
}
