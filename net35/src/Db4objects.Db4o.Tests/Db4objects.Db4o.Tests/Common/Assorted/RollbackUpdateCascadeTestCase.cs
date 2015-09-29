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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class RollbackUpdateCascadeTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new RollbackUpdateCascadeTestCase().RunNetworking();
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(Atom)).CascadeOnUpdate(true);
			config.ObjectClass(typeof(Atom)).CascadeOnDelete(true);
		}

		protected override void Store()
		{
			Atom atom = new Atom("root");
			atom.child = new Atom("child");
			atom.child.child = new Atom("child.child");
			Store(atom);
		}

		public virtual void Test()
		{
			IExtObjectContainer oc1 = OpenNewSession();
			IExtObjectContainer oc2 = OpenNewSession();
			IExtObjectContainer oc3 = OpenNewSession();
			try
			{
				IQuery query1 = oc1.Query();
				query1.Descend("name").Constrain("root");
				IObjectSet os1 = query1.Execute();
				Assert.AreEqual(1, os1.Count);
				Atom o1 = (Atom)os1.Next();
				o1.child.child.name = "o1";
				oc1.Store(o1);
				IQuery query2 = oc2.Query();
				query2.Descend("name").Constrain("root");
				IObjectSet os2 = query2.Execute();
				Assert.AreEqual(1, os2.Count);
				Atom o2 = (Atom)os2.Next();
				Assert.AreEqual("child.child", o2.child.child.name);
				oc1.Rollback();
				oc2.Purge(o2);
				os2 = query2.Execute();
				Assert.AreEqual(1, os2.Count);
				o2 = (Atom)os2.Next();
				Assert.AreEqual("child.child", o2.child.child.name);
				oc1.Store(o1);
				oc1.Commit();
				os2 = query2.Execute();
				Assert.AreEqual(1, os2.Count);
				o2 = (Atom)os2.Next();
				oc2.Refresh(o2, int.MaxValue);
				Assert.AreEqual("o1", o2.child.child.name);
				IQuery query3 = oc3.Query();
				query3.Descend("name").Constrain("root");
				IObjectSet os3 = query1.Execute();
				Assert.AreEqual(1, os3.Count);
				Atom o3 = (Atom)os3.Next();
				Assert.AreEqual("o1", o3.child.child.name);
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
