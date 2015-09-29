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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class MultiDeleteTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new MultiDeleteTestCase().RunConcurrency();
		}

		public MultiDeleteTestCase child;

		public string name;

		public object forLong;

		public long myLong;

		public object[] untypedArr;

		public long[] typedArr;

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(this).CascadeOnDelete(true);
			config.ObjectClass(this).CascadeOnUpdate(true);
		}

		protected override void Store()
		{
			MultiDeleteTestCase md = new MultiDeleteTestCase();
			md.name = "killmefirst";
			md.SetMembers();
			md.child = new MultiDeleteTestCase();
			md.child.SetMembers();
			Store(md);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(MultiDeleteTestCase));
			q.Descend("name").Constrain("killmefirst");
			IObjectSet objectSet = q.Execute();
			if (objectSet.Count == 0)
			{
				// already deleted by other threads
				return;
			}
			Assert.AreEqual(1, objectSet.Count);
			Thread.Sleep(1000);
			if (!objectSet.HasNext())
			{
				return;
			}
			MultiDeleteTestCase md = (MultiDeleteTestCase)objectSet.Next();
			oc.Delete(md);
			oc.Commit();
			AssertOccurrences(oc, typeof(MultiDeleteTestCase), 0);
		}

		public virtual void Check(IExtObjectContainer oc)
		{
			AssertOccurrences(oc, typeof(MultiDeleteTestCase), 0);
		}

		private void SetMembers()
		{
			forLong = System.Convert.ToInt64(100);
			myLong = System.Convert.ToInt64(100);
			untypedArr = new object[] { System.Convert.ToInt64(10), "hi", new MultiDeleteTestCase
				() };
			typedArr = new long[] { System.Convert.ToInt64(3), System.Convert.ToInt64(7), System.Convert.ToInt64
				(9) };
		}
	}
}
#endif // !SILVERLIGHT
