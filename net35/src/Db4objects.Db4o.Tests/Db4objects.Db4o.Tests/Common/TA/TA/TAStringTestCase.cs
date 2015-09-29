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
using Db4objects.Db4o.Tests.Common.TA.TA;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	/// <exclude></exclude>
	public class TAStringTestCase : TAItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new TAStringTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			TAStringItem item = new TAStringItem();
			item.value = "42";
			item.obj = "hello";
			return item;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			TAStringItem item = (TAStringItem)obj;
			Assert.AreEqual("42", item.Value());
			Assert.AreEqual("hello", item.Object());
		}

		protected override void AssertRetrievedItem(object obj)
		{
			TAStringItem item = (TAStringItem)obj;
			Assert.IsNull(item.value);
			Assert.IsNull(item.obj);
		}
	}
}
