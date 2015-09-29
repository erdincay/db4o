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
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Reflect;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class ArrayTestSuite : FixtureBasedTestSuite
	{
		public class TestUnit : DrsTestCase
		{
			public virtual void Test()
			{
				ItemWithUntypedField item = new ItemWithUntypedField(Subject());
				StoreToProviderA(item);
				ReplicatedAllToB();
				ItemWithUntypedField replicated = ReplicatedItem();
				Assert.IsNotNull(replicated.Array());
				Iterator4Assert.AreEqual(ArrayIterator(item.Array()), ArrayIterator(replicated.Array
					()));
			}

			private IEnumerator ArrayIterator(object array)
			{
				return ArrayHandler.Iterator(ReflectClass(array), array);
			}

			private IReflectClass ReflectClass(object array)
			{
				return GenericReflector().ForObject(array);
			}

			private Db4objects.Db4o.Reflect.Generic.GenericReflector GenericReflector()
			{
				return new Db4objects.Db4o.Reflect.Generic.GenericReflector(null, Platform4.ReflectorForType
					(GetType()));
			}

			private void ReplicatedAllToB()
			{
				ReplicateAll(A().Provider(), B().Provider());
			}

			private void StoreToProviderA(ItemWithUntypedField item)
			{
				A().Provider().StoreNew(item);
				A().Provider().Commit();
			}

			private ItemWithUntypedField ReplicatedItem()
			{
				IEnumerator iterator = B().Provider().GetStoredObjects(typeof(ItemWithUntypedField
					)).GetEnumerator();
				if (iterator.MoveNext())
				{
					return (ItemWithUntypedField)iterator.Current;
				}
				return null;
			}

			private object Subject()
			{
				return SubjectFixtureProvider.Value();
			}
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new SubjectFixtureProvider(new object[] { new object
				[] {  }, new string[] { "foo", "bar" }, new int[] { 42, -1, 0 }, new int[][] {  }
				, new DateTime[] { new DateTime() } }) };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(ArrayTestSuite.TestUnit) };
		}
	}
}
