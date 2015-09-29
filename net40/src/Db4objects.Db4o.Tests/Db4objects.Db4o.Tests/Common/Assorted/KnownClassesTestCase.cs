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
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class KnownClassesTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new KnownClassesTestCase().RunAll();
		}

		public static readonly Type[] InternalClasses = new Type[] { typeof(Db4oDatabase)
			, typeof(StaticClass) };

		public class Item
		{
		}

		protected override void Store()
		{
			Assert.IsFalse(IsKnownClass(typeof(KnownClassesTestCase.Item)));
			Store(new KnownClassesTestCase.Item());
			Assert.IsTrue(IsKnownClass(typeof(KnownClassesTestCase.Item)));
		}

		public virtual void TestNoPrimitives()
		{
			IReflectClass[] knownClassArray = Container().KnownClasses();
			for (int knownClassIndex = 0; knownClassIndex < knownClassArray.Length; ++knownClassIndex)
			{
				IReflectClass knownClass = knownClassArray[knownClassIndex];
				Assert.IsFalse(knownClass.IsPrimitive(), knownClass.GetName());
			}
		}

		public virtual void TestValueTypes()
		{
			Container().Reflector().ForName("System.Guid, mscorlib");
			bool found = false;
			IReflectClass[] knownClassArray = Container().KnownClasses();
			for (int knownClassIndex = 0; knownClassIndex < knownClassArray.Length; ++knownClassIndex)
			{
				IReflectClass knownClass = knownClassArray[knownClassIndex];
				if (knownClass.GetName().Equals("System.Guid, mscorlib"))
				{
					found = true;
				}
			}
			Assert.IsTrue(found);
		}

		public virtual void TestInternalClassesAreNotVisible()
		{
			IReflectClass[] knownClassArray = Container().KnownClasses();
			for (int knownClassIndex = 0; knownClassIndex < knownClassArray.Length; ++knownClassIndex)
			{
				IReflectClass knownClass = knownClassArray[knownClassIndex];
				AssertIsNotInternal(knownClass.GetName());
			}
		}

		public virtual void TestNewClassIsFound()
		{
			Assert.IsTrue(IsKnownClass(typeof(KnownClassesTestCase.Item)));
		}

		private bool IsKnownClass(Type klass)
		{
			return IsKnownClass(ReflectPlatform.FullyQualifiedName(klass));
		}

		private bool IsKnownClass(string expected)
		{
			IReflectClass[] knownClassArray = Container().KnownClasses();
			for (int knownClassIndex = 0; knownClassIndex < knownClassArray.Length; ++knownClassIndex)
			{
				IReflectClass knownClass = knownClassArray[knownClassIndex];
				string className = knownClass.GetName();
				if (className.Equals(expected))
				{
					return true;
				}
			}
			return false;
		}

		private void AssertIsNotInternal(string className)
		{
			for (int j = 0; j < InternalClasses.Length; j++)
			{
				Assert.AreNotEqual(InternalClasses[j].FullName, className);
			}
		}
	}
}
