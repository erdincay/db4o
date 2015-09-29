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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;
using Db4objects.Db4o.Tests.Common.Persistent;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class CascadeOnUpdate2TestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new CascadeOnUpdate2TestCase().RunConcurrency();
		}

		private const int AtomCount = 10;

		public class Item
		{
			public Atom[] child;
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(CascadeOnUpdate2TestCase.Item)).CascadeOnUpdate(true);
			config.ObjectClass(typeof(Atom)).CascadeOnUpdate(false);
		}

		protected override void Store()
		{
			CascadeOnUpdate2TestCase.Item item = new CascadeOnUpdate2TestCase.Item();
			item.child = new Atom[AtomCount];
			for (int i = 0; i < AtomCount; i++)
			{
				item.child[i] = new Atom(new Atom("storedChild"), "stored");
			}
			Store(item);
		}

		public virtual void Conc(IExtObjectContainer oc, int seq)
		{
			CascadeOnUpdate2TestCase.Item item = (CascadeOnUpdate2TestCase.Item)((CascadeOnUpdate2TestCase.Item
				)RetrieveOnlyInstance(oc, typeof(CascadeOnUpdate2TestCase.Item)));
			for (int i = 0; i < AtomCount; i++)
			{
				item.child[i].name = "updated" + seq;
				item.child[i].child.name = "updated" + seq;
				oc.Store(item);
			}
		}

		public virtual void Check(IExtObjectContainer oc)
		{
			CascadeOnUpdate2TestCase.Item item = (CascadeOnUpdate2TestCase.Item)((CascadeOnUpdate2TestCase.Item
				)RetrieveOnlyInstance(oc, typeof(CascadeOnUpdate2TestCase.Item)));
			string name = item.child[0].name;
			Assert.IsTrue(name.StartsWith("updated"));
			for (int i = 0; i < AtomCount; i++)
			{
				Assert.AreEqual(name, item.child[i].name);
				Assert.AreEqual("storedChild", item.child[i].child.name);
			}
		}
	}
}
#endif // !SILVERLIGHT
