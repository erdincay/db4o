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
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	public class SimpleGenericType<T>
	{
		public T value;

		public SimpleGenericType(T value)
		{
			this.value = value;
		}
	}

	public class SimpleGenericTypeTestCase : AbstractDb4oTestCase
	{
		override protected void Store()
		{
			Store(new SimpleGenericType<string>("Will it work?"));
			Store(new SimpleGenericType<int>(42));
		}

		public void Test()
		{
			TstGenericType("Will it work?");
			TstGenericType(42);
		}

		private void TstGenericType<T>(T expectedValue)
		{
			IQuery query = NewQuery(typeof(SimpleGenericType<T>));

			EnsureGenericItem(expectedValue, query.Execute());

			query = NewQuery(typeof(SimpleGenericType<T>));
			query.Descend("value").Constrain(expectedValue);
			EnsureGenericItem(expectedValue, query.Execute());
		}

		private static void EnsureGenericItem<T>(T expectedValue, IObjectSet os)
		{
			Assert.AreEqual(1, os.Count);

			SimpleGenericType<T> item = (SimpleGenericType<T>)os.Next();
			Assert.AreEqual(expectedValue, item.value);
		}
	}
}
