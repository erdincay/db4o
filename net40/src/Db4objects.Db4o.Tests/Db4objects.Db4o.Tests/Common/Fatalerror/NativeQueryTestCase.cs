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
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fatalerror;

namespace Db4objects.Db4o.Tests.Common.Fatalerror
{
	public class NativeQueryTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public string str;

			public Item(string s)
			{
				str = s;
			}
		}

		public static void Main(string[] args)
		{
			new NativeQueryTestCase().RunSoloAndClientServer();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new NativeQueryTestCase.Item("hello"));
		}

		public virtual void _test()
		{
			Assert.Expect(typeof(NativeQueryTestCase.NQError), new _ICodeBlock_29(this));
			Assert.IsTrue(Db().IsClosed());
		}

		private sealed class _ICodeBlock_29 : ICodeBlock
		{
			public _ICodeBlock_29(NativeQueryTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Predicate fatalErrorPredicate = new NativeQueryTestCase.FatalErrorPredicate();
				this._enclosing.Db().Query(fatalErrorPredicate);
			}

			private readonly NativeQueryTestCase _enclosing;
		}

		[System.Serializable]
		public class FatalErrorPredicate : Predicate
		{
			public virtual bool Match(object item)
			{
				throw new NativeQueryTestCase.NQError("nq error!");
			}
		}

		[System.Serializable]
		public class NQError : Exception
		{
			public NQError(string msg) : base(msg)
			{
			}
		}
	}
}
