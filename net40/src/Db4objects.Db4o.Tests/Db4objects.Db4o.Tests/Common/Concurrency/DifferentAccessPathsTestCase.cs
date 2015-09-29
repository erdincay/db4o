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
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class DifferentAccessPathsTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new DifferentAccessPathsTestCase().RunConcurrency();
		}

		public string foo;

		protected override void Store()
		{
			DifferentAccessPathsTestCase dap = new DifferentAccessPathsTestCase();
			dap.foo = "hi";
			Store(dap);
			dap = new DifferentAccessPathsTestCase();
			dap.foo = "hi too";
			Store(dap);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Conc(IExtObjectContainer oc)
		{
			DifferentAccessPathsTestCase dap = Query(oc);
			for (int i = 0; i < 10; i++)
			{
				Assert.AreSame(dap, Query(oc));
			}
			oc.Purge(dap);
			Assert.AreNotSame(dap, Query(oc));
		}

		private DifferentAccessPathsTestCase Query(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(DifferentAccessPathsTestCase));
			q.Descend("foo").Constrain("hi");
			IObjectSet os = q.Execute();
			Assert.AreEqual(1, os.Count);
			DifferentAccessPathsTestCase dap = (DifferentAccessPathsTestCase)os.Next();
			return dap;
		}
	}
}
#endif // !SILVERLIGHT
