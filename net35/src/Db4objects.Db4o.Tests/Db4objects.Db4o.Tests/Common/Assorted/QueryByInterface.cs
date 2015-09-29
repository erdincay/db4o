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
	public class QueryByInterface : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new QueryByInterface().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			QueryByInterface.Ferrari f430 = new QueryByInterface.Ferrari("F430");
			QueryByInterface.Ferrari f450 = new QueryByInterface.Ferrari("F450");
			Store(f430);
			Store(f450);
			QueryByInterface.Bmw serie5 = new QueryByInterface.Bmw("Serie 5");
			QueryByInterface.Bmw serie7 = new QueryByInterface.Bmw("Serie 7");
			Store(serie5);
			Store(serie7);
		}

		public virtual void Test()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(QueryByInterface.ICar));
			q.Descend("name").Constrain("F450");
			IObjectSet result = q.Execute();
			Assert.AreEqual(1, result.Count);
			QueryByInterface.Ferrari car = (QueryByInterface.Ferrari)result.Next();
			Assert.AreEqual("F450", car.name);
		}

		public interface ICar
		{
		}

		public class Ferrari : QueryByInterface.ICar
		{
			public string name;

			public Ferrari(string n)
			{
				name = n;
			}

			public override string ToString()
			{
				return "Ferrari " + name;
			}
		}

		public class Bmw : QueryByInterface.ICar
		{
			public string name;

			public Bmw(string n)
			{
				name = n;
			}

			public override string ToString()
			{
				return "BMW " + name;
			}
		}
	}
}
