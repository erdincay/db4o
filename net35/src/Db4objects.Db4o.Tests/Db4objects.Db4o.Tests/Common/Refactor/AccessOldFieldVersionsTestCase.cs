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
	public class AccessOldFieldVersionsTestCase : AccessFieldTestCaseBase, ITestLifeCycle
	{
		private static readonly Type OrigType = typeof(int);

		private static readonly string FieldName = "_value";

		private const int OrigValue = 42;

		public virtual void TestRetypedField()
		{
			Type targetClazz = typeof(AccessOldFieldVersionsTestCase.RetypedFieldData);
			RenameClass(typeof(AccessOldFieldVersionsTestCase.OriginalData), ReflectPlatform.
				FullyQualifiedName(targetClazz));
			AssertField(targetClazz, FieldName, OrigType, OrigValue);
		}

		protected override object NewOriginalData()
		{
			return new AccessOldFieldVersionsTestCase.OriginalData(OrigValue);
		}

		public class OriginalData
		{
			public int _value;

			public OriginalData(int value)
			{
				_value = value;
			}
		}

		public class RetypedFieldData
		{
			public string _value;

			public RetypedFieldData(string value)
			{
				_value = value;
			}
		}
	}
}
