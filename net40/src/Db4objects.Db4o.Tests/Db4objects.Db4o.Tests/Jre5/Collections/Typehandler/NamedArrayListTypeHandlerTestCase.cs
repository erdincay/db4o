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
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class NamedArrayListTypeHandlerTestCase : AbstractDb4oTestCase
	{
		private static string Name = "listname";

		private static object[] Data = new object[] { "one", "two", 3, System.Convert.ToInt64
			(4), null };

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(CreateNamedArrayList());
		}

		private NamedArrayList CreateNamedArrayList()
		{
			NamedArrayList namedArrayList = new NamedArrayList();
			namedArrayList.name = Name;
			for (int i = 0; i < Data.Length; i++)
			{
				namedArrayList.Add(Data[i]);
			}
			return namedArrayList;
		}

		private void AssertRetrievedOK(NamedArrayList namedArrayList)
		{
			Assert.AreEqual(Name, namedArrayList.name);
			object[] listElements = new object[namedArrayList.Count];
			int idx = 0;
			IEnumerator i = namedArrayList.GetEnumerator();
			while (i.MoveNext())
			{
				listElements[idx++] = i.Current;
			}
			ArrayAssert.AreEqual(Data, listElements);
		}

		public virtual void TestRetrieve()
		{
			NamedArrayList namedArrayList = (NamedArrayList)RetrieveOnlyInstance(typeof(NamedArrayList
				));
			AssertRetrievedOK(namedArrayList);
		}

		public virtual void TestQuery()
		{
			IQuery query = NewQuery(typeof(NamedArrayList));
			query.Descend("name").Constrain(Name);
			IObjectSet objectSet = query.Execute();
			Assert.AreEqual(1, objectSet.Count);
			NamedArrayList namedArrayList = (NamedArrayList)objectSet.Next();
			AssertRetrievedOK(namedArrayList);
		}
	}
}
