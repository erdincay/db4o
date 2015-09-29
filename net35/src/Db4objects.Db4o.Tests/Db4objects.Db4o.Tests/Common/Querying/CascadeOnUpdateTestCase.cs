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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class CascadeOnUpdateTestCase : AbstractDb4oTestCase
	{
		public class Holder
		{
			public object child;

			public Holder(object child)
			{
				this.child = child;
			}
		}

		public class Atom
		{
			public CascadeOnUpdateTestCase.Atom child;

			public string name;

			public Atom()
			{
			}

			public Atom(CascadeOnUpdateTestCase.Atom child)
			{
				this.child = child;
			}

			public Atom(string name)
			{
				this.name = name;
			}

			public Atom(CascadeOnUpdateTestCase.Atom child, string name) : this(child)
			{
				this.name = name;
			}
		}

		public object child;

		protected override void Configure(IConfiguration conf)
		{
			conf.ObjectClass(typeof(CascadeOnUpdateTestCase.Holder)).CascadeOnUpdate(true);
		}

		protected override void Store()
		{
			CascadeOnUpdateTestCase.Holder cou = new CascadeOnUpdateTestCase.Holder(new CascadeOnUpdateTestCase.Atom
				(new CascadeOnUpdateTestCase.Atom("storedChild"), "stored"));
			Db().Store(cou);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Foreach(GetType(), new _IVisitor4_55(this));
			Reopen();
			Foreach(GetType(), new _IVisitor4_66());
		}

		private sealed class _IVisitor4_55 : IVisitor4
		{
			public _IVisitor4_55(CascadeOnUpdateTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object obj)
			{
				CascadeOnUpdateTestCase.Holder cou = (CascadeOnUpdateTestCase.Holder)obj;
				((CascadeOnUpdateTestCase.Atom)cou.child).name = "updated";
				((CascadeOnUpdateTestCase.Atom)cou.child).child.name = "updated";
				this._enclosing.Db().Store(cou);
			}

			private readonly CascadeOnUpdateTestCase _enclosing;
		}

		private sealed class _IVisitor4_66 : IVisitor4
		{
			public _IVisitor4_66()
			{
			}

			public void Visit(object obj)
			{
				CascadeOnUpdateTestCase.Holder cou = (CascadeOnUpdateTestCase.Holder)obj;
				CascadeOnUpdateTestCase.Atom atom = (CascadeOnUpdateTestCase.Atom)cou.child;
				Assert.AreEqual("updated", atom.name);
				Assert.AreNotEqual("updated", atom.child.name);
			}
		}
	}
}
