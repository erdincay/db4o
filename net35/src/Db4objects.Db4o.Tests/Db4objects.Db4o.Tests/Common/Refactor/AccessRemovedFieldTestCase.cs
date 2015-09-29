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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class AccessRemovedFieldTestCase : AccessFieldTestCaseBase, ITestLifeCycle
	{
		private static readonly Type FieldType = typeof(int);

		private static readonly string FieldName = "_value";

		private const int FieldValue = 42;

		public virtual void TestRemovedField()
		{
			Type targetClazz = typeof(AccessRemovedFieldTestCase.RemovedFieldData);
			RenameClass(typeof(AccessRemovedFieldTestCase.OriginalData), ReflectPlatform.FullyQualifiedName
				(targetClazz));
			AssertField(targetClazz, FieldName, FieldType, FieldValue);
		}

		protected override object NewOriginalData()
		{
			return new AccessRemovedFieldTestCase.OriginalData(FieldValue);
		}

		public class OriginalData
		{
			public int _value;

			public string _name;

			public OriginalData(int value)
			{
				_value = value;
			}
		}

		public class RemovedFieldData
		{
			public string _name;
		}
	}
}
