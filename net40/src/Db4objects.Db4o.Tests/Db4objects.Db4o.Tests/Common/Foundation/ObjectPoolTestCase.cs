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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class ObjectPoolTestCase : ITestCase
	{
		public virtual void Test()
		{
			object o1 = new object();
			object o2 = new object();
			object o3 = new object();
			IObjectPool pool = new SimpleObjectPool(new object[] { o1, o2, o3 });
			Assert.AreSame(o1, pool.BorrowObject());
			Assert.AreSame(o2, pool.BorrowObject());
			Assert.AreSame(o3, pool.BorrowObject());
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_20(pool));
			pool.ReturnObject(o2);
			Assert.AreSame(o2, pool.BorrowObject());
		}

		private sealed class _ICodeBlock_20 : ICodeBlock
		{
			public _ICodeBlock_20(IObjectPool pool)
			{
				this.pool = pool;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				pool.BorrowObject();
			}

			private readonly IObjectPool pool;
		}
	}
}
