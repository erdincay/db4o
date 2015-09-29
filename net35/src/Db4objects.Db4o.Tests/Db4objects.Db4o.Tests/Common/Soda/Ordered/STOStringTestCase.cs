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
	public class STOStringTestCase : SodaBaseTestCase
	{
		public string foo;

		public STOStringTestCase()
		{
		}

		public STOStringTestCase(string str)
		{
			this.foo = str;
		}

		public override string ToString()
		{
			return foo;
		}

		public override object[] CreateData()
		{
			return new object[] { new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase
				(null), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase("bbb"), 
				new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase("dod"), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase
				("aaa"), new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase("Xbb"), 
				new Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase("bbq") };
		}

		public virtual void TestAscending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase));
			q.Descend("foo").OrderAscending();
			ExpectOrdered(q, new int[] { 0, 4, 3, 1, 5, 2 });
		}

		public virtual void TestDescending()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase));
			q.Descend("foo").OrderDescending();
			ExpectOrdered(q, new int[] { 2, 5, 1, 3, 4, 0 });
		}

		public virtual void TestAscendingLike()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase));
			IQuery qStr = q.Descend("foo");
			qStr.Constrain("b").Like();
			qStr.OrderAscending();
			ExpectOrdered(q, new int[] { 4, 1, 5 });
		}

		public virtual void TestDescendingContains()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Ordered.STOStringTestCase));
			IQuery qStr = q.Descend("foo");
			qStr.Constrain("b").Contains();
			qStr.OrderDescending();
			ExpectOrdered(q, new int[] { 5, 1, 4 });
		}
	}
}
