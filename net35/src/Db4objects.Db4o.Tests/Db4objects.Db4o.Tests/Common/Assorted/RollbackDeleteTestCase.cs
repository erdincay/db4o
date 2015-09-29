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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class RollbackDeleteTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new RollbackDeleteTestCase().RunNetworking();
		}

		protected override void Store()
		{
			Store(new SimpleObject("hello", 1));
		}

		public virtual void TestDRDC()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			IExtObjectContainer oc3 = OpenNewSession();
			try
			{
				SimpleObject o1 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc1, typeof(SimpleObject
					)));
				oc1.Delete(o1);
				SimpleObject o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject
					)));
				Assert.AreEqual("hello", o2.GetS());
				oc1.Rollback();
				o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject))
					);
				oc2.Refresh(o2, int.MaxValue);
				Assert.AreEqual("hello", o2.GetS());
				oc1.Commit();
				o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject))
					);
				oc2.Refresh(o2, int.MaxValue);
				Assert.AreEqual("hello", o2.GetS());
				oc1.Delete(o1);
				oc1.Commit();
				AssertOccurrences(oc3, typeof(SimpleObject), 0);
				AssertOccurrences(oc2, typeof(SimpleObject), 0);
			}
			finally
			{
				oc1.Close();
				oc2.Close();
				oc3.Close();
			}
		}

		public virtual void TestSRDC()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			IExtObjectContainer oc3 = OpenNewSession();
			try
			{
				SimpleObject o1 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc1, typeof(SimpleObject
					)));
				oc1.Store(o1);
				SimpleObject o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject
					)));
				Assert.AreEqual("hello", o2.GetS());
				oc1.Rollback();
				o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject))
					);
				oc2.Refresh(o2, int.MaxValue);
				Assert.AreEqual("hello", o2.GetS());
				oc1.Commit();
				o2 = (SimpleObject)((SimpleObject)RetrieveOnlyInstance(oc2, typeof(SimpleObject))
					);
				oc2.Refresh(o2, int.MaxValue);
				Assert.AreEqual("hello", o2.GetS());
				oc1.Delete(o1);
				oc1.Commit();
				AssertOccurrences(oc3, typeof(SimpleObject), 0);
				AssertOccurrences(oc2, typeof(SimpleObject), 0);
			}
			finally
			{
				oc1.Close();
				oc2.Close();
				oc3.Close();
			}
		}
	}
}
