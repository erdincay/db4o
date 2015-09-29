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
using Db4objects.Db4o.Tests.Common.Persistent;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class IndexedByIdentityTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new IndexedByIdentityTestCase().RunConcurrency();
		}

		public Atom atom;

		internal const int Count = 10;

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(this).ObjectField("atom").Indexed(true);
			config.ObjectClass(typeof(IndexedByIdentityTestCase)).CascadeOnUpdate(true);
		}

		protected override void Store()
		{
			for (int i = 0; i < Count; i++)
			{
				IndexedByIdentityTestCase ibi = new IndexedByIdentityTestCase();
				ibi.atom = new Atom("ibi" + i);
				Store(ibi);
			}
		}

		public virtual void ConcRead(IExtObjectContainer oc)
		{
			for (int i = 0; i < Count; i++)
			{
				IQuery q = oc.Query();
				q.Constrain(typeof(Atom));
				q.Descend("name").Constrain("ibi" + i);
				IObjectSet objectSet = q.Execute();
				Assert.AreEqual(1, objectSet.Count);
				Atom child = (Atom)objectSet.Next();
				q = oc.Query();
				q.Constrain(typeof(IndexedByIdentityTestCase));
				q.Descend("atom").Constrain(child).Identity();
				objectSet = q.Execute();
				Assert.AreEqual(1, objectSet.Count);
				IndexedByIdentityTestCase ibi = (IndexedByIdentityTestCase)objectSet.Next();
				Assert.AreSame(child, ibi.atom);
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConcUpdate(IExtObjectContainer oc, int seq)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(IndexedByIdentityTestCase));
			IObjectSet os = q.Execute();
			Assert.AreEqual(Count, os.Count);
			while (os.HasNext())
			{
				IndexedByIdentityTestCase idi = (IndexedByIdentityTestCase)os.Next();
				idi.atom.name = "updated" + seq;
				oc.Store(idi);
				Thread.Sleep(100);
			}
		}

		public virtual void CheckUpdate(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(IndexedByIdentityTestCase));
			IObjectSet os = q.Execute();
			Assert.AreEqual(Count, os.Count);
			string expected = null;
			while (os.HasNext())
			{
				IndexedByIdentityTestCase idi = (IndexedByIdentityTestCase)os.Next();
				if (expected == null)
				{
					expected = idi.atom.name;
					Assert.IsTrue(expected.StartsWith("updated"));
					Assert.IsTrue(expected.Length > "updated".Length);
				}
				Assert.AreEqual(expected, idi.atom.name);
			}
		}
	}
}
#endif // !SILVERLIGHT
