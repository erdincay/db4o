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
#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class GreaterOrEqualTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase().RunConcurrency
				();
		}

		public int val;

		public GreaterOrEqualTestCase()
		{
		}

		public GreaterOrEqualTestCase(int val)
		{
			this.val = val;
		}

		protected override void Store()
		{
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase(1));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase(2));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase(3));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase(4));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase(5));
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			int[] expect = new int[] { 3, 4, 5 };
			IQuery q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase
				));
			q.Descend("val").Constrain(3).Greater().Equal();
			IObjectSet res = q.Execute();
			while (res.HasNext())
			{
				Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase r = (Db4objects.Db4o.Tests.Common.Concurrency.GreaterOrEqualTestCase
					)res.Next();
				for (int i = 0; i < expect.Length; i++)
				{
					if (expect[i] == r.val)
					{
						expect[i] = 0;
					}
				}
			}
			for (int i = 0; i < expect.Length; i++)
			{
				Assert.AreEqual(0, expect[i]);
			}
		}
	}
}
#endif // !SILVERLIGHT
