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
using Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped
{
	public class STArrMixedTestCase : SodaBaseTestCase
	{
		public object[] arr;

		public STArrMixedTestCase()
		{
		}

		public STArrMixedTestCase(object[] arr)
		{
			this.arr = arr;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedTestCase
				(), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedTestCase(new 
				object[0]), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedTestCase
				(new object[] { new STArrMixedTestCase.ReferenceMarker(), 0, 0, "foo", false }), 
				new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedTestCase(new object
				[] { 1, 17, int.MaxValue - 1, "foo", "bar" }), new Db4objects.Db4o.Tests.Common.Soda.Arrays.Untyped.STArrMixedTestCase
				(new object[] { 3, 17, 25, int.MaxValue - 2 }) };
		}

		public class ReferenceMarker
		{
		}

		public virtual void TestContainsReference()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(STArrMixedTestCase));
			q.Descend("arr").Constrain(((STArrMixedTestCase.ReferenceMarker)RetrieveOnlyInstance
				(typeof(STArrMixedTestCase.ReferenceMarker))));
			Expect(q, new int[] { 2 });
		}

		public virtual void TestDefaultContainsInteger()
		{
			IQuery q = NewQuery();
			q.Constrain(new STArrMixedTestCase(new object[] { 17 }));
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDefaultContainsString()
		{
			IQuery q = NewQuery();
			q.Constrain(new STArrMixedTestCase(new object[] { "foo" }));
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestDefaultContainsBoolean()
		{
			IQuery q = NewQuery();
			q.Constrain(new STArrMixedTestCase(new object[] { false }));
			Expect(q, new int[] { 2 });
		}

		public virtual void TestDefaultContainsTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(new STArrMixedTestCase(new object[] { 17, "bar" }));
			Expect(q, new int[] { 3 });
		}

		public virtual void TestDescendOne()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(STArrMixedTestCase));
			q.Descend("arr").Constrain(17);
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDescendTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(STArrMixedTestCase));
			IQuery qElements = q.Descend("arr");
			qElements.Constrain(17);
			qElements.Constrain("bar");
			Expect(q, new int[] { 3 });
		}

		public virtual void TestDescendSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(STArrMixedTestCase));
			IQuery qElements = q.Descend("arr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}
	}
}
