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
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Tests.CLI1
{
	class MyTestClass
	{
		public MyTestClass()
		{ }

		public MyTestClass(string value1, string value2, string value3)
		{
			field1 = value1;
			field2 = value2;
			field3 = value3;
		}

		public string field1;
		public string field2;
		public string field3;
	}


	class JavaUUIDCompatibilityTestCase : JavaCompatibilityTestCaseBase
	{
		public void Test()
		{
			RunTest();
		}

		protected override string ExpectedJavaOutput()
		{
			return @"Db4objects.Db4o.Tests.CLI1.MyTestClass, Db4objects.Db4o.Tests
	v4ouuid
	field1
	field2
	field3";
		}

		protected override JavaSnippet JavaCode()
		{
			return new JavaSnippet("com.db4o.test.uuidcompatibility.Program", @"
package com.db4o.test.uuidcompatibility;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.config.*;

public class Program {
	public static void main(String[] args) {
		Configuration config = Db4o.newConfiguration();
		config.add(new DotnetSupport());

		ObjectContainer container = Db4o.openFile(config, args[0]);
		StoredClass[] storedClasses = container.ext().storedClasses();
		for (int i = 0; i < storedClasses.length; i++) {
			StoredClass storedClass = storedClasses[i];
			System.out.println(storedClass.getName());
			StoredField[] storedFields = storedClass.getStoredFields();
			for (int j = 0; j < storedFields.length; j++) {
				StoredField storedField = storedFields[j];
				System.out.println(""\t"" + storedField.getName());
			}
		}
		container.close();
		System.out.println(""success"");
	}
}");
		}


		protected override void PopulateContainer(IObjectContainer container)
		{
			MyTestClass test1 = new MyTestClass();
			container.Store(test1);

			MyTestClass test2 = new MyTestClass();
			container.Store(test2);
		}

		protected override IConfiguration GetConfiguration()
		{
			IConfiguration config = base.GetConfiguration();
			config.GenerateUUIDs(ConfigScope.Globally);
			return config;
		}
	}
}
