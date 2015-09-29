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
namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	using System;

	using Db4o;
	using Query;

	using Db4oUnit;
	using Db4oUnit.Extensions;

	public class NullableContainer
	{
		public int? intValue;
		public DateTime? dateValue;

		public NullableContainer(int value)
		{
			intValue = value;
		}

		public NullableContainer(DateTime value)
		{
			dateValue = value;
		}
	}

	public class NullableTypes : AbstractDb4oTestCase
	{
		static readonly DateTime TheDate = new DateTime(1983, 3, 7);

		protected override void Store()
		{
			Db().Store(new NullableContainer(42));
			Db().Store(new NullableContainer(TheDate));
		}

		public void TestGlobalQuery()
		{
			IQuery query = NewQuery();
			query.Constrain(typeof(NullableContainer));

			IObjectSet os = query.Execute();
			Assert.AreEqual(2, os.Count);

			bool foundInt = false;
			bool foundDate = false;
			while (os.HasNext())
			{
				NullableContainer item = (NullableContainer)os.Next();
				if (item.intValue.HasValue)
				{
					Assert.AreEqual(42, item.intValue.Value);
					Assert.IsFalse(item.dateValue.HasValue);
					foundInt = true;
				}
				else if (item.dateValue.HasValue)
				{
					Assert.AreEqual(TheDate, item.dateValue.Value);
					Assert.IsFalse(item.intValue.HasValue);
					foundDate = true;
				}
			}

			Assert.IsTrue(foundInt);
			Assert.IsTrue(foundDate);
		}

		public void TestDateQuery()
		{
			IObjectSet os = Db().QueryByExample(new NullableContainer(TheDate));
			CheckDateValueQueryResult(os);
		}

		private static void CheckDateValueQueryResult(IObjectSet os)
		{
			Assert.AreEqual(1, os.Count);
			NullableContainer found = (NullableContainer)os.Next();
			Assert.AreEqual(TheDate, found.dateValue.Value);
			EnsureIsNull(found.intValue);
		}

		public void TestIntQuery()
		{
			IObjectSet os = Db().QueryByExample(new NullableContainer(42));
			CheckIntValueQueryResult(os);
		}

		public void TestSodaQuery()
		{
			IQuery q = NewQuery(typeof(NullableContainer));
			q.Descend("intValue").Constrain(42);
			CheckIntValueQueryResult(q.Execute());
		}

		public void TestSodaQueryWithNullConstrain()
		{
			IQuery q = NewQuery(typeof(NullableContainer));
			q.Descend("intValue").Constrain(null);
			CheckDateValueQueryResult(q.Execute());
		}

		private static void CheckIntValueQueryResult(IObjectSet os)
		{
			Assert.AreEqual(1, os.Count);
			NullableContainer found = (NullableContainer)os.Next();
			Assert.AreEqual(42, found.intValue.Value);
			EnsureIsNull(found.dateValue);
		}

		private static void EnsureIsNull<T>(Nullable<T> value) where T : struct
		{
			Assert.IsFalse(value.HasValue, "!nullable.HasValue");
		}
	}
}
