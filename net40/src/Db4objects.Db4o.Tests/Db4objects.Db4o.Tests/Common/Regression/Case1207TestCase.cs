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
using Db4objects.Db4o.Tests.Common.Assorted;
using Db4objects.Db4o.Tests.Common.Regression;

namespace Db4objects.Db4o.Tests.Common.Regression
{
	public class Case1207TestCase : Db4oClientServerTestCase
	{
		/// <exception cref="System.Exception"></exception>
		public static void Main(string[] args)
		{
			new Case1207TestCase().RunNetworking();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IObjectContainer oc1 = OpenNewSession();
			IObjectContainer oc2 = OpenNewSession();
			IObjectContainer oc3 = OpenNewSession();
			try
			{
				for (int i = 0; i < 1000; i++)
				{
					SimpleObject obj1 = new SimpleObject("oc " + i, i);
					SimpleObject obj2 = new SimpleObject("oc2 " + i, i);
					oc1.Store(obj1);
					oc2.Store(obj2);
					oc2.Rollback();
					obj2 = new SimpleObject("oc2.2 " + i, i);
					oc2.Store(obj2);
				}
				oc1.Commit();
				oc2.Rollback();
				Assert.AreEqual(1000, oc1.Query(typeof(SimpleObject)).Count);
				Assert.AreEqual(1000, oc2.Query(typeof(SimpleObject)).Count);
				Assert.AreEqual(1000, oc3.Query(typeof(SimpleObject)).Count);
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
