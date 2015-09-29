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
	public class RefreshTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase().RunConcurrency();
		}

		public string name;

		public Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase child;

		public RefreshTestCase()
		{
		}

		public RefreshTestCase(string name, Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase
			 child)
		{
			this.name = name;
			this.child = child;
		}

		protected override void Store()
		{
			Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase r3 = new Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase
				("o3", null);
			Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase r2 = new Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase
				("o2", r3);
			Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase r1 = new Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase
				("o1", r2);
			Store(r1);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase r11 = GetRoot(oc);
			r11.name = "cc";
			oc.Refresh(r11, 0);
			Assert.AreEqual("cc", r11.name);
			oc.Refresh(r11, 1);
			Assert.AreEqual("o1", r11.name);
			r11.child.name = "cc";
			oc.Refresh(r11, 1);
			Assert.AreEqual("cc", r11.child.name);
			oc.Refresh(r11, 2);
			Assert.AreEqual("o2", r11.child.name);
		}

		private Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase GetRoot(IObjectContainer
			 oc)
		{
			return GetByName(oc, "o1");
		}

		private Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase GetByName(IObjectContainer
			 oc, string name)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase));
			q.Descend("name").Constrain(name);
			IObjectSet objectSet = q.Execute();
			return (Db4objects.Db4o.Tests.Common.Concurrency.RefreshTestCase)objectSet.Next();
		}
	}
}
#endif // !SILVERLIGHT
