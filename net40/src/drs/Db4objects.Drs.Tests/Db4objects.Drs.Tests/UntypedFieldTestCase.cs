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
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class UntypedFieldTestCase : DrsTestCase
	{
		public virtual void TestUntypedString()
		{
			AssertUntypedReplication("42");
		}

		public virtual void TestUntypedStringArray()
		{
			AssertUntypedReplication(new object[] { "42" });
		}

		public virtual void TestUntypedStringJaggedArray()
		{
			AssertJaggedArray("42");
		}

		public virtual void TestUntypedReferenceTypeJaggedArray()
		{
			AssertJaggedArray(new UntypedFieldData(42));
		}

		public virtual void TestUntypedDate()
		{
			AssertUntypedReplication(new DateTime(100, 2, 2));
		}

		public virtual void TestUntypedDateArray()
		{
			AssertUntypedReplication(new object[] { new DateTime(100, 2, 2) });
		}

		public virtual void TestUntypedMixedArray()
		{
			AssertUntypedReplication(new object[] { "42", new UntypedFieldData(42) });
			Assert.AreEqual(42, ((UntypedFieldData)SingleReplicatedInstance(typeof(UntypedFieldData
				))).GetId());
		}

		public virtual void TestArrayAsCloneable()
		{
			object[] array = new object[] { "42", new UntypedFieldData(42) };
			ItemWithCloneable replicated = (ItemWithCloneable)Replicate(new ItemWithCloneable
				(array));
			AssertEquals(array, replicated.value);
		}

		private void AssertUntypedReplication(object data)
		{
			AssertEquals(data, ReplicateItem(data).GetUntyped());
		}

		private void AssertJaggedArray(object data)
		{
			object[] expected = new object[] { new object[] { data } };
			object[] actual = (object[])ReplicateItem(expected).GetUntyped();
			Assert.AreEqual(expected.Length, actual.Length);
			object[] nested = (object[])actual[0];
			object actualValue = nested[0];
			Assert.AreEqual(data, actualValue);
			AssertNotSame(data, actualValue);
		}

		private void AssertNotSame(object expectedReference, object actual)
		{
			if (!IsPrimitive(expectedReference.GetType()))
			{
				Assert.AreNotSame(expectedReference, actual);
			}
		}

		private bool IsPrimitive(Type klass)
		{
			if (klass.IsPrimitive)
			{
				return true;
			}
			if (klass == typeof(string))
			{
				return true;
			}
			if (klass == typeof(DateTime))
			{
				return true;
			}
			return false;
		}

		private void AssertEquals(object expected, object actual)
		{
			if (expected is object[])
			{
				AssertEquals((object[])expected, (object[])actual);
			}
			else
			{
				Assert.AreEqual(expected, actual);
				AssertNotSame(expected, actual);
			}
		}

		private void AssertEquals(object[] expectedArray, object[] actualArray)
		{
			ArrayAssert.AreEqual(expectedArray, actualArray);
			for (int i = 0; i < expectedArray.Length; ++i)
			{
				AssertNotSame(expectedArray[i], actualArray[i]);
			}
		}

		private UntypedFieldItem ReplicateItem(object data)
		{
			return (UntypedFieldItem)Replicate(new UntypedFieldItem(data));
		}

		private object Replicate(object item)
		{
			A().Provider().StoreNew(item);
			A().Provider().Commit();
			ReplicateAll(A().Provider(), B().Provider());
			return SingleReplicatedInstance(item.GetType());
		}

		private object SingleReplicatedInstance(Type klass)
		{
			IObjectSet found = B().Provider().GetStoredObjects(klass);
			Assert.AreEqual(1, found.Count);
			return found[0];
		}
	}
}
