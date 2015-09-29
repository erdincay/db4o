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
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	/// <summary>
	/// Design of this case is copied from
	/// com.db4o.db4ounit.common.types.arrays.ByteArrayTestCase.
	/// </summary>
	/// <remarks>
	/// Design of this case is copied from
	/// com.db4o.db4ounit.common.types.arrays.ByteArrayTestCase.
	/// </remarks>
	public class ByteArrayTest : DrsTestCase
	{
		internal const int ArrayLength = 5;

		internal static byte[] initial = CreateByteArray();

		internal static byte[] modInB = new byte[] { 2, 3, 5, 68, 69 };

		internal static byte[] modInA = new byte[] { 15, 36, 55, 8, 9, 28, 65 };

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
			IIByteArrayHolder byteArrayHolder = new ByteArrayHolder(CreateByteArray());
			A().Provider().StoreNew(byteArrayHolder);
			A().Provider().Commit();
			EnsureNames(A(), initial);
		}

		private void Replicate()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureNames(A(), initial);
			EnsureNames(B(), initial);
		}

		private void ModifyInB()
		{
			IIByteArrayHolder c = GetTheObject(B());
			c.SetBytes(modInB);
			B().Provider().Update(c);
			B().Provider().Commit();
			EnsureNames(B(), modInB);
		}

		private void Replicate2()
		{
			ReplicateAll(B().Provider(), A().Provider());
			EnsureNames(A(), modInB);
			EnsureNames(B(), modInB);
		}

		private void ModifyInA()
		{
			IIByteArrayHolder c = GetTheObject(A());
			c.SetBytes(modInA);
			A().Provider().Update(c);
			A().Provider().Commit();
			EnsureNames(A(), modInA);
		}

		private void Replicate3()
		{
			ReplicateAll(A().Provider(), B().Provider());
			EnsureNames(A(), modInA);
			EnsureNames(B(), modInA);
		}

		private void EnsureNames(IDrsProviderFixture fixture, byte[] bs)
		{
			EnsureOneInstance(fixture, typeof(ByteArrayHolder));
			IIByteArrayHolder c = GetTheObject(fixture);
			ArrayAssert.AreEqual(c.GetBytes(), bs);
		}

		private IIByteArrayHolder GetTheObject(IDrsProviderFixture fixture)
		{
			return (ByteArrayHolder)GetOneInstance(fixture, typeof(ByteArrayHolder));
		}

		internal static byte[] CreateByteArray()
		{
			byte[] bytes = new byte[ArrayLength];
			for (byte i = 0; i < bytes.Length; ++i)
			{
				bytes[i] = i;
			}
			return bytes;
		}
	}
}
