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
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class ConjunctiveQbETestCase : AbstractDb4oTestCase
	{
		public class Sup
		{
			public bool _flag;

			public Sup(bool flag)
			{
				this._flag = flag;
			}

			public virtual IObjectSet Query(IObjectContainer db)
			{
				IQuery query = db.Query();
				query.Constrain(this);
				query.Descend("_flag").Constrain(true).Not();
				return query.Execute();
			}
		}

		public class Sub1 : ConjunctiveQbETestCase.Sup
		{
			public Sub1(bool flag) : base(flag)
			{
			}
		}

		public class Sub2 : ConjunctiveQbETestCase.Sup
		{
			public Sub2(bool flag) : base(flag)
			{
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ConjunctiveQbETestCase.Sub1(false));
			Store(new ConjunctiveQbETestCase.Sub1(true));
			Store(new ConjunctiveQbETestCase.Sub2(false));
			Store(new ConjunctiveQbETestCase.Sub2(true));
		}

		public virtual void TestAndedQbE()
		{
			Assert.AreEqual(1, new ConjunctiveQbETestCase.Sub1(false).Query(Db()).Count);
		}
	}
}
