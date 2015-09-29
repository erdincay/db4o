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
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Stored;

namespace Db4objects.Db4o.Tests.Common.Stored
{
	public class ArrayStoredTypeTestCase : AbstractDb4oTestCase
	{
		public class Data
		{
			public bool[] _primitiveBoolean;

			public bool[] _wrapperBoolean;

			public int[] _primitiveInt;

			public int[] _wrapperInteger;

			public Data(bool[] primitiveBoolean, bool[] wrapperBoolean, int[] primitiveInteger
				, int[] wrapperInteger)
			{
				this._primitiveBoolean = primitiveBoolean;
				this._wrapperBoolean = wrapperBoolean;
				this._primitiveInt = primitiveInteger;
				this._wrapperInteger = wrapperInteger;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			ArrayStoredTypeTestCase.Data data = new ArrayStoredTypeTestCase.Data(new bool[] { 
				true, false }, new bool[] { true, false }, new int[] { 0, 1, 2 }, new int[] { 4, 
				5, 6 });
			Store(data);
		}

		public virtual void TestArrayStoredTypes()
		{
			IStoredClass clazz = Db().StoredClass(typeof(ArrayStoredTypeTestCase.Data));
			AssertStoredType(clazz, "_primitiveBoolean", typeof(bool));
			AssertStoredType(clazz, "_wrapperBoolean", typeof(bool));
			AssertStoredType(clazz, "_primitiveInt", typeof(int));
			AssertStoredType(clazz, "_wrapperInteger", typeof(int));
		}

		private void AssertStoredType(IStoredClass clazz, string fieldName, Type type)
		{
			IStoredField field = clazz.StoredField(fieldName, null);
			Assert.AreEqual(type.FullName, CrossPlatformServices.SimpleName(field.GetStoredType
				().GetName()));
		}
		// getName() also contains the assembly name in .net
		// so we better remove it for comparison
	}
}
