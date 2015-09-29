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
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped
{
	public class STStringUTestCase : SodaBaseTestCase
	{
		public object str;

		public STStringUTestCase()
		{
		}

		public STStringUTestCase(string str)
		{
			this.str = str;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"aaa"), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"bbb"), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"dod") };
		}

		public virtual void TestEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[2]);
			SodaTestUtil.ExpectOne(q, _array[2]);
		}

		public virtual void TestNotEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(_array[2]);
			q.Descend("str").Constraints().Not();
			Expect(q, new int[] { 0, 1, 3 });
		}

		public virtual void TestDescendantEquals()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				());
			q.Descend("str").Constrain("bbb");
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("bbb"));
		}

		public virtual void TestContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("od"));
			q.Descend("str").Constraints().Contains();
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("dod"));
		}

		public virtual void TestNotContains()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("od"));
			q.Descend("str").Constraints().Contains().Not();
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"aaa"), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"bbb") });
		}

		public virtual void TestLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("do"));
			q.Descend("str").Constraints().Like();
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("dod"));
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("od"));
			q.Descend("str").Constraints().Like();
			SodaTestUtil.ExpectOne(q, _array[3]);
		}

		public virtual void TestNotLike()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("aaa"));
			q.Descend("str").Constraints().Like().Not();
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"bbb"), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"dod") });
			q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("xxx"));
			q.Descend("str").Constraints().Like();
			Expect(q, new int[] {  });
		}

		public virtual void TestIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("aaa"));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase)set.Next
				();
			identityConstraint.str = "hihs";
			q = NewQuery();
			q.Constrain(identityConstraint).Identity();
			identityConstraint.str = "aaa";
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("aaa"));
		}

		public virtual void TestNotIdentity()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("aaa"));
			IObjectSet set = q.Execute();
			Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase identityConstraint
				 = (Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase)set.Next
				();
			identityConstraint.str = null;
			q = NewQuery();
			q.Constrain(identityConstraint).Identity().Not();
			identityConstraint.str = "aaa";
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"bbb"), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase(
				"dod") });
		}

		public virtual void TestNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				(null));
			q.Descend("str").Constrain(null);
			SodaTestUtil.ExpectOne(q, new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				(null));
		}

		public virtual void TestNotNull()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				(null));
			q.Descend("str").Constrain(null).Not();
			SodaTestUtil.Expect(q, new object[] { new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("aaa"), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("bbb"), new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("dod") });
		}

		public virtual void TestConstraints()
		{
			IQuery q = NewQuery();
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("aaa"));
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Wrapper.Untyped.STStringUTestCase
				("bbb"));
			IConstraints cs = q.Constraints();
			IConstraint[] csa = cs.ToArray();
			if (csa.Length != 2)
			{
				Assert.Fail("Constraints not returned");
			}
		}
	}
}
