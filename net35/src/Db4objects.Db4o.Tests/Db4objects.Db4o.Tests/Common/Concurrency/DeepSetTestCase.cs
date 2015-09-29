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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class DeepSetTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new DeepSetTestCase().RunConcurrency();
		}

		public DeepSetTestCase child;

		public string name;

		protected override void Store()
		{
			name = "1";
			child = new DeepSetTestCase();
			child.name = "2";
			child.child = new DeepSetTestCase();
			child.child.name = "3";
			Store(this);
		}

		public virtual void Conc(IExtObjectContainer oc, int seq)
		{
			DeepSetTestCase example = new DeepSetTestCase();
			example.name = "1";
			DeepSetTestCase ds = (DeepSetTestCase)oc.QueryByExample(example).Next();
			Assert.AreEqual("1", ds.name);
			Assert.AreEqual("3", ds.child.child.name);
			ds.name = "1";
			ds.child.name = "12" + seq;
			ds.child.child.name = "13" + seq;
			oc.Store(ds, 2);
		}

		public virtual void Check(IExtObjectContainer oc)
		{
			DeepSetTestCase example = new DeepSetTestCase();
			example.name = "1";
			DeepSetTestCase ds = (DeepSetTestCase)oc.QueryByExample(example).Next();
			Assert.IsTrue(ds.child.name.StartsWith("12"));
			Assert.IsTrue(ds.child.name.Length > "12".Length);
			Assert.AreEqual("3", ds.child.child.name);
		}
	}
}
#endif // !SILVERLIGHT
