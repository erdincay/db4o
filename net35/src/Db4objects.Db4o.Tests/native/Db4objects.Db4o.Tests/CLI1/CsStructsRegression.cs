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

namespace Db4objects.Db4o.Tests.CLI1
{
	public class CsStructsRegression : AbstractDb4oTestCase
	{
		override protected void Store()
		{
			Store(new Item());
			Store(new Item(1));
			Store(new Item(2));
		}

		public void TestConstrainOnNullableValue()
		{
			CheckQueryById(1);
			CheckQueryById(2);
		}

		private void CheckQueryById(int id)
		{
			IObjectSet os = QueryById(id);
			Assert.AreEqual(1, os.Count);
			Assert.AreEqual(id, ((Item)os.Next()).Id);
		}

		private IObjectSet QueryById(int id)
		{
            IQuery q = NewQuery(typeof(Item));
			q.Descend("_id").Descend("_value").Constrain(id);
			return q.Execute();
		}
	}

	public class Item
	{
		public NullableInt32 _id;

		public Item(int id)
		{
			_id = new NullableInt32(id);
		}

		public Item()
		{	
		}

		public int Id
		{
			get
			{
				return _id.Value;
			}
		}
	}

	public struct NullableInt32
	{
		public int _value;
		public bool _hasValue;

		public NullableInt32(int value)
		{
			_value = value;
			_hasValue = true;
		}

		public int Value
		{
			get { return _value; }
		}

		public bool HasValue
		{
			get { return _hasValue; }
		}
	}
}
