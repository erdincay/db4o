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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Assorted;
using Db4oUnit;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.CLI1.Reflect.Net
{
	public class UnavailableClassPrimitiveRepresentationTestCase : UnavailableClassTestCaseBase
	{
		protected override void Store()
		{
			foreach (Type type in Platform4.PrimitiveTypes())
			{
				Store(type);
			}
		}

		private void Store(Type type)
		{
			Type genericItemType = GenericItemTypeFor(type);
			object instance = Activator.CreateInstance(genericItemType);

			Store(instance);
		}

		public void TestPrimitiveTypeRepresentation()
		{
			
			ReopenHidingClasses(ExcludedTypes());

			IReflector reflector = Db().Ext().Reflector();

			foreach (Type type in Platform4.PrimitiveTypes())
			{
				IReflectClass klass = reflector.ForName(TypeReference.FromType(GenericItemTypeFor(type)).GetUnversionedName());
				AssertType(reflector, type, FieldType(klass, "simpleField"));
				AssertType(reflector, NullableTypeFor(type), FieldType(klass, "nullableField"));
			}
		}

		private static Type[] ExcludedTypes()
		{
			Type[] excludedTypes = new Type[Platform4.PrimitiveTypes().Length];
			int i = 0;
			foreach (Type type in Platform4.PrimitiveTypes())
			{
				excludedTypes[i++] = GenericItemTypeFor(type);
			}

			return excludedTypes;
		}

		private static Type NullableTypeFor(Type type)
		{
			return typeof (Nullable<>).MakeGenericType(type);
		}

		private static IReflectClass FieldType(IReflectClass klass, string fieldName)
		{
			return Field(klass, fieldName).GetFieldType();
		}

		private static void AssertType(IReflector reflector, Type expected, IReflectClass actual)
		{
			Assert.AreEqual(reflector.ForClass(expected), actual);
		}

		private static IReflectField Field(IReflectClass klass, string fieldName)
		{
			return klass.GetDeclaredField(fieldName);
		}

		private static Type GenericItemTypeFor(Type type)
		{
			return typeof(Item<>).MakeGenericType(type);
		}
	}

	public class Item<T> where T : struct
	{
		public T simpleField;
		public T? nullableField;
	}
}
