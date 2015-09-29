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
using Db4objects.Db4o.Tests.Common.Soda.Arrays;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays
{
	public class ArrayDescendSubQueryTestCase : AbstractDb4oTestCase
	{
		public class Person
		{
			public string _name;

			public Person(string name)
			{
				//COR-1977
				_name = name;
			}

			public override string ToString()
			{
				return _name;
			}
		}

		public class Book
		{
			public string _title;

			public ArrayDescendSubQueryTestCase.Person _author;

			public ArrayDescendSubQueryTestCase.Book[] _cites;

			public Book(string title, ArrayDescendSubQueryTestCase.Person author, ArrayDescendSubQueryTestCase.Book
				[] cites)
			{
				_title = title;
				_author = author;
				_cites = cites;
			}

			public override string ToString()
			{
				return _title;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			ArrayDescendSubQueryTestCase.Person erich = new ArrayDescendSubQueryTestCase.Person
				("Erich");
			ArrayDescendSubQueryTestCase.Person kent = new ArrayDescendSubQueryTestCase.Person
				("Kent");
			ArrayDescendSubQueryTestCase.Person bill = new ArrayDescendSubQueryTestCase.Person
				("Bill");
			ArrayDescendSubQueryTestCase.Book gof = new ArrayDescendSubQueryTestCase.Book("gof"
				, erich, new ArrayDescendSubQueryTestCase.Book[0]);
			ArrayDescendSubQueryTestCase.Book xp = new ArrayDescendSubQueryTestCase.Book("xp"
				, kent, new ArrayDescendSubQueryTestCase.Book[] { gof });
			ArrayDescendSubQueryTestCase.Book ddd = new ArrayDescendSubQueryTestCase.Book("ddd"
				, bill, new ArrayDescendSubQueryTestCase.Book[] { gof, xp });
			Store(ddd);
		}

		// all books cited in ddd - works
		public virtual void TestSimpleDescend()
		{
			IQuery topQuery = NewQuery(typeof(ArrayDescendSubQueryTestCase.Book));
			topQuery.Descend("_title").Constrain("ddd");
			IQuery subQuery = topQuery.Descend("_cites");
			Assert.AreEqual(2, subQuery.Execute().Count);
		}

		// all authors of books cited in ddd - only selects one array element as root for the second descend
		public virtual void TestDoubleDescend()
		{
			IQuery topQuery = NewQuery(typeof(ArrayDescendSubQueryTestCase.Book));
			topQuery.Descend("_title").Constrain("ddd");
			IQuery subQuery = topQuery.Descend("_cites").Descend("_author");
			IObjectSet result = subQuery.Execute();
			//		while(result.hasNext()) {
			//			System.out.println(result.next());
			//		}
			Assert.AreEqual(2, result.Count);
		}
	}
}
