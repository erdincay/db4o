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

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Simple
{
	public class STDoubleTestCase : SodaBaseTestCase
	{
		public double i_double;

		public STDoubleTestCase()
		{
		}

		private STDoubleTestCase(double a_double)
		{
			i_double = a_double;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase
				(0), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase(0), new 
				Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase(1.01), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase
				(99.99), new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase(909.00
				) };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase
				(0));
			// Primitive default values are ignored, so we need an 
			// additional constraint:
			q.Descend("i_double").Constrain(System.Convert.ToDouble(0));
			Expect(q, new int[] { 0, 1 });
		}

		public virtual void TestGreater()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase
				(1));
			q.Descend("i_double").Constraints().Greater();
			Expect(q, new int[] { 2, 3, 4 });
		}

		public virtual void TestSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase
				(1));
			q.Descend("i_double").Constraints().Smaller();
			Expect(q, new int[] { 0, 1 });
		}

		public virtual void TestGreaterOrEqual()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[2]);
			q.Descend("i_double").Constraints().Greater().Equal();
			Expect(q, new int[] { 2, 3, 4 });
		}

		public virtual void TestGreaterAndNot()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Classes.Simple.STDoubleTestCase
				());
			IQuery val = q.Descend("i_double");
			val.Constrain(System.Convert.ToDouble(0)).Greater();
			val.Constrain(99.99).Not();
			Expect(q, new int[] { 2, 4 });
		}
	}
}
