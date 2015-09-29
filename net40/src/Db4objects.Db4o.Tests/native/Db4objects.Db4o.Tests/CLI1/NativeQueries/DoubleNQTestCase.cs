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

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	class DoubleItem
	{
		private string _name;
		private double _value;

		public DoubleItem(string name, double value)
		{
			_name = name;
			_value = value;
		}

		public string Name
		{
			get { return _name;  }
		}

		public double Value
		{
			get { return _value;  }
		}


		public override string ToString()
		{
			return "DoubleItem(" + Name + ", " + Value + ")";
		}
	}

	class DoublePredicate
	{
		public bool Match(DoubleItem item)
		{
			return item.Value == 41.99;
		}
	}

	class DoubleNQTestCase : AbstractNativeQueriesTestCase
	{
#if !SILVERLIGHT
		protected override void Store()
		{
			Store(new DoubleItem("foo", 11.5));
			Store(new DoubleItem("bar", 41.99));
		}

		public void Test()
		{
			AssertNQResult(new DoublePredicate(), ItemByName("bar"));
		}

		private object ItemByName(string name)
		{
			IQuery query = NewQuery(typeof(DoubleItem));
			query.Descend("_name").Constrain(name);
			return query.Execute().Next();
		}
#endif
	}
}
