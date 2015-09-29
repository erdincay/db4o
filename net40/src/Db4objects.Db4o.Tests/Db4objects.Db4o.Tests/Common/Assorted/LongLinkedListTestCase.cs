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
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class LongLinkedListTestCase : AbstractDb4oTestCase
	{
		private const int Count = 1000;

		public class LinkedList
		{
			public LongLinkedListTestCase.LinkedList _next;

			public int _depth;
		}

		private static LongLinkedListTestCase.LinkedList NewLongCircularList()
		{
			LongLinkedListTestCase.LinkedList head = new LongLinkedListTestCase.LinkedList();
			LongLinkedListTestCase.LinkedList tail = head;
			for (int i = 1; i < Count; i++)
			{
				tail._next = new LongLinkedListTestCase.LinkedList();
				tail = tail._next;
				tail._depth = i;
			}
			tail._next = head;
			return head;
		}

		/// <exception cref="System.Exception"></exception>
		public static void Main(string[] args)
		{
			new LongLinkedListTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(NewLongCircularList());
		}

		public virtual void Test()
		{
			IQuery q = NewQuery(typeof(LongLinkedListTestCase.LinkedList));
			q.Descend("_depth").Constrain(0);
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(1, objectSet.Count);
			LongLinkedListTestCase.LinkedList head = (LongLinkedListTestCase.LinkedList)objectSet
				.Next();
			Db().Activate(head, int.MaxValue);
			AssertListIsComplete(head);
			Db().Deactivate(head, int.MaxValue);
			Db().Activate(head, int.MaxValue);
			AssertListIsComplete(head);
			Db().Deactivate(head, int.MaxValue);
			Db().Refresh(head, int.MaxValue);
			AssertListIsComplete(head);
		}

		// TODO: The following produces a stack overflow. That's OK for now, peekPersisted is rarely
		//		 used and users can control behaviour with the depth parameter. 
		// 		 
		//		LinkedList peeked = (LinkedList) db().ext().peekPersisted(head, Integer.MAX_VALUE, true);
		//		assertListIsComplete(peeked);
		private void AssertListIsComplete(LongLinkedListTestCase.LinkedList head)
		{
			int count = 1;
			LongLinkedListTestCase.LinkedList tail = head._next;
			while (tail != head)
			{
				count++;
				tail = tail._next;
			}
			Assert.AreEqual(Count, count);
		}
	}
}
