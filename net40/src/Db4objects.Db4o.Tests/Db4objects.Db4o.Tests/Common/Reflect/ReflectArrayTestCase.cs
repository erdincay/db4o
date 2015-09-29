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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Tests.Common.Reflect
{
	public class ReflectArrayTestCase : AbstractDb4oTestCase
	{
		public virtual void TestNewInstance()
		{
			string[][] a23 = NewStringMatrix(2, 3);
			Assert.AreEqual(2, a23.Length);
			for (int i = 0; i < a23.Length; ++i)
			{
				Assert.AreEqual(3, a23[i].Length);
			}
		}

		private string[][] NewStringMatrix(int x, int y)
		{
			return (string[][])NewInstance(typeof(string), new int[] { x, y });
		}

		public virtual void TestIsNDimensional()
		{
			IReflectClass arrayOfArrayOfString = ReflectClass(typeof(string[][]));
			Assert.IsTrue(arrayOfArrayOfString.IsArray());
			IReflectClass arrayOfString = ReflectClass(typeof(string[]));
			Assert.AreSame(arrayOfString, arrayOfArrayOfString.GetComponentType());
			Assert.IsTrue(ArrayReflector().IsNDimensional(arrayOfArrayOfString));
			Assert.IsFalse(ArrayReflector().IsNDimensional(arrayOfString));
		}

		public virtual void TestDimensions()
		{
			string[][] array = NewStringMatrix(3, 4);
			ArrayAssert.AreEqual(new int[] { 3, 4 }, ArrayReflector().Dimensions(array));
		}

		private object NewInstance(Type elementType, int[] dimensions)
		{
			return ArrayReflector().NewInstance(ReflectClass(elementType), dimensions);
		}

		private IReflectArray ArrayReflector()
		{
			return Reflector().Array();
		}
	}
}
