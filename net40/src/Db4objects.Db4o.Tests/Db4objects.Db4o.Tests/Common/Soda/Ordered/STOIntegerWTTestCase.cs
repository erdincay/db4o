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
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class STOIntegerWTTestCase : SodaBaseTestCase
	{
		public int i_int;

		public STOIntegerWTTestCase()
		{
		}

		private STOIntegerWTTestCase(int a_int)
		{
			i_int = a_int;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				(99), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase(1), new 
				Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase(909), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				(1001), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase(0), new 
				Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase(1010), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				() };
		}

		public virtual void TestDescending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				));
			q.Descend("i_int").OrderDescending();
			ExpectOrdered(q, new int[] { 5, 3, 2, 0, 1, 4, 6 });
		}

		public virtual void TestAscendingGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOIntegerWTTestCase
				));
			IQuery qInt = q.Descend("i_int");
			qInt.Constrain(100).Greater();
			qInt.OrderAscending();
			ExpectOrdered(q, new int[] { 2, 3, 5 });
		}
	}
}
