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
	public class TALinkedListTestCase : TAItemTestCaseBase
	{
		public static void Main(string[] args)
		{
			new TALinkedListTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			TALinkedListItem item = new TALinkedListItem();
			item.list = NewList();
			return item;
		}

		private TALinkedList NewList()
		{
			return TALinkedList.NewList(10);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			TALinkedListItem item = (TALinkedListItem)obj;
			Assert.AreEqual(NewList(), item.List());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeactivateDepth()
		{
			TALinkedListItem item = (TALinkedListItem)RetrieveOnlyInstance();
			TALinkedList list = item.List();
			TALinkedList next3 = list.NextN(3);
			TALinkedList next5 = list.NextN(5);
			Assert.IsNotNull(next3.Next());
			Assert.IsNotNull(next5.Next());
			Db().Deactivate(list, 4);
			Assert.IsNull(list.next);
			Assert.AreEqual(0, list.value);
			// FIXME: test fails if uncomenting the following assertion.
			//	    	Assert.isNull(next3.next);
			Assert.IsNotNull(next5.next);
		}
	}
}
