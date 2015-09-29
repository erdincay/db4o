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

namespace Db4objects.Db4o.Tests.CLI1
{
	public class DateCompatibilityItem
	{
		public DateCompatibilityItem()
		{	
		}

		public DateCompatibilityItem(DateTime value1, DateTime value2, DateTime value3)
		{
			field1 = value1;
			field2 = value2;
			field3 = value3;
		}

		public DateTime field1;
		public DateTime field2;
		public DateTime field3;
	}


	public class JavaDateCompatibilityTestCase : JavaCompatibilityTestCaseBase
	{
		public void _Test()
		{
			RunTest();
		}

		protected override string ExpectedJavaOutput()
		{
			return @"foo";
		}

		protected override JavaSnippet JavaCode()
		{
			return new JavaSnippet("com.db4o.test.compatibility.Program", @"
package com.db4o.test.compatibility;

import com.db4o.*;
import com.db4o.ext.*;
import com.db4o.config.*;
import java.util.*;

class Item {
	public Date field1;
	public Date field2;
	public Date field3;

	public Item() {
	}
}

public class Program {
	public static void main(String[] args) {
		Configuration config = Db4o.newConfiguration();
		config.add(new DotnetSupport());
		config.addAlias(
			new TypeAlias(
				""Db4objects.Db4o.Tests.CLI1.DateCompatibilityItem, Db4objects.Db4o.Tests"",
				""com.db4o.test.compatibility.Item""));

		ObjectContainer container = Db4o.openFile(config, args[0]);
		try {
			ObjectSet found = container.get(Item.class);
			while (found.hasNext()) {
				Item item = (Item)found.next();
				print(item.field1);
				print(item.field2);
				print(item.field3);
			}
		} finally {
			container.close();
		}
	}

	static void print(java.util.Date d) {
		    System.out.println(
				""""
				+ (1900 + d.getYear())
				+ ""/""
				+ (d.getMonth()+1)
				+ ""/""
				+ d.getDate()
				+ "" ""
				+ d.getHours()
				+ "":""
				+ d.getMinutes()
				+ "":""
				+ d.getSeconds());
	}
}");
		}


		protected override void PopulateContainer(IObjectContainer container)
		{
			DateCompatibilityItem test1 = new DateCompatibilityItem(DateTime.MinValue, DateTime.MaxValue, new DateTime(2007, 10, 07, 11, 23, 42, 1));
			container.Store(test1);
		}
	}
}
