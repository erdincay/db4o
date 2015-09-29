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
using Db4objects.Drs;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class MixedTypesCollectionReplicationTest : DrsTestCase
	{
		protected virtual void ActualTest()
		{
			if (!A().Provider().SupportsHybridCollection())
			{
				return;
			}
			if (!B().Provider().SupportsHybridCollection())
			{
				return;
			}
			CollectionHolder h1 = new CollectionHolder("h1");
			CollectionHolder h2 = new CollectionHolder("h2");
			h1.Map()["key"] = "value";
			h1.Map()["key2"] = h1;
			h1.Map()[h1] = "value2";
			h2.Map()["key"] = h1;
			h2.Map()[h2] = h1;
			h1.List().Add("one");
			h1.List().Add(h1);
			h2.List().Add("two");
			h2.List().Add(h1);
			h2.List().Add(h2);
			h1.Set().Add("one");
			h1.Set().Add(h1);
			h2.Set().Add("two");
			h2.Set().Add(h1);
			h2.Set().Add(h2);
			B().Provider().StoreNew(h2);
			B().Provider().StoreNew(h1);
			IReplicationSession replication = new GenericReplicationSession(A().Provider(), B
				().Provider());
			replication.Replicate(h2);
			//Traverses to h1.
			replication.Commit();
			IEnumerator objects = A().Provider().GetStoredObjects(typeof(CollectionHolder)).GetEnumerator
				();
			Check(NextCollectionHolder(objects), h1, h2);
			Check(NextCollectionHolder(objects), h1, h2);
		}

		private CollectionHolder NextCollectionHolder(IEnumerator objects)
		{
			Assert.IsTrue(objects.MoveNext());
			return (CollectionHolder)objects.Current;
		}

		private void Check(CollectionHolder holder, CollectionHolder original1, CollectionHolder
			 original2)
		{
			Assert.IsTrue(holder != original1);
			Assert.IsTrue(holder != original2);
			if (holder.Name().Equals("h1"))
			{
				CheckH1(holder);
			}
			else
			{
				CheckH2(holder);
			}
		}

		private void CheckH1(CollectionHolder holder)
		{
			Assert.AreEqual("value", holder.Map()["key"]);
			Assert.AreEqual(holder, holder.Map()["key2"]);
			Assert.AreEqual("value2", holder.Map()[holder]);
			Assert.AreEqual("one", holder.List()[0]);
			Assert.AreEqual(holder, holder.List()[1]);
			Assert.IsTrue(holder.Set().Contains("one"));
			Assert.IsTrue(holder.Set().Contains(holder));
		}

		private void CheckH2(CollectionHolder holder)
		{
			Assert.AreEqual("h1", ((CollectionHolder)holder.Map()["key"]).Name());
			Assert.AreEqual("h1", ((CollectionHolder)holder.Map()[holder]).Name());
			Assert.AreEqual("two", holder.List()[0]);
			Assert.AreEqual("h1", ((CollectionHolder)holder.List()[1]).Name());
			Assert.AreEqual(holder, holder.List()[2]);
			Assert.IsTrue(holder.Set().Remove("two"));
			Assert.IsTrue(holder.Set().Remove(holder));
			CollectionHolder remaining = NextCollectionHolder(holder.Set().GetEnumerator());
			Assert.AreEqual("h1", remaining.Name());
		}

		public virtual void Test()
		{
			ActualTest();
		}
	}
}
