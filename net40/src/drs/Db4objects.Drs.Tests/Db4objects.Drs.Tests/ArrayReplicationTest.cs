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
	public class ArrayReplicationTest : DrsTestCase
	{
		public virtual void Test()
		{
			if (!A().Provider().SupportsMultiDimensionalArrays())
			{
				return;
			}
			if (!B().Provider().SupportsMultiDimensionalArrays())
			{
				return;
			}
			ArrayHolder h1 = new ArrayHolder("h1");
			ArrayHolder h2 = new ArrayHolder("h2");
			h1._array = new ArrayHolder[] { h1 };
			h2._array = new ArrayHolder[] { h1, h2, null };
			h1._arrayN = new ArrayHolder[][] { new ArrayHolder[] { h1 } };
			h2._arrayN = new ArrayHolder[][] { new ArrayHolder[] { h1, null }, new ArrayHolder
				[] { null, h2 }, new ArrayHolder[] { null, null } };
			//TODO Fix ReflectArray.shape() and test with innermost arrays of varying sizes:  {{h1}, {null, h2}, {null}}
			B().Provider().StoreNew(h2);
			B().Provider().StoreNew(h1);
			B().Provider().Commit();
			IReplicationSession replication = new GenericReplicationSession(A().Provider(), B
				().Provider(), null, _fixtures.reflector);
			replication.Replicate(h2);
			//Traverses to h1.
			replication.Commit();
			IEnumerator objects = A().Provider().GetStoredObjects(typeof(ArrayHolder)).GetEnumerator
				();
			CheckNext(objects);
			CheckNext(objects);
			Assert.IsFalse(objects.MoveNext());
		}

		private void CheckNext(IEnumerator objects)
		{
			Assert.IsTrue(objects.MoveNext());
			Check((ArrayHolder)objects.Current);
		}

		private void Check(ArrayHolder holder)
		{
			if (holder.GetName().Equals("h1"))
			{
				CheckH1(holder);
			}
			else
			{
				CheckH2(holder);
			}
		}

		protected virtual void CheckH1(ArrayHolder holder)
		{
			Assert.AreEqual(holder.Array()[0], holder);
			Assert.AreEqual(holder.ArrayN()[0][0], holder);
		}

		protected virtual void CheckH2(ArrayHolder holder)
		{
			Assert.AreEqual(holder.Array()[0].GetName(), "h1");
			Assert.AreEqual(holder.Array()[1], holder);
			Assert.AreEqual(holder.Array()[2], null);
			Assert.AreEqual(holder.ArrayN()[0][0].GetName(), "h1");
			Assert.AreEqual(holder.ArrayN()[1][0], null);
			Assert.AreEqual(holder.ArrayN()[1][1], holder);
			Assert.AreEqual(holder.ArrayN()[2][0], null);
		}
	}
}
