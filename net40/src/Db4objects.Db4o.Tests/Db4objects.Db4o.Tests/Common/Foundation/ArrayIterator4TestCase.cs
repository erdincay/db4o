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
	public class ArrayIterator4TestCase : ITestCase
	{
		public virtual void TestEmptyArray()
		{
			AssertExhausted(new ArrayIterator4(new object[0]));
		}

		public virtual void TestArray()
		{
			ArrayIterator4 i = new ArrayIterator4(new object[] { "foo", "bar" });
			Assert.IsTrue(i.MoveNext());
			Assert.AreEqual("foo", i.Current);
			Assert.IsTrue(i.MoveNext());
			Assert.AreEqual("bar", i.Current);
			AssertExhausted(i);
		}

		private void AssertExhausted(ArrayIterator4 i)
		{
			Assert.IsFalse(i.MoveNext());
			Assert.Expect(typeof(IndexOutOfRangeException), new _ICodeBlock_29(i));
		}

		private sealed class _ICodeBlock_29 : ICodeBlock
		{
			public _ICodeBlock_29(ArrayIterator4 i)
			{
				this.i = i;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Sharpen.Runtime.Out.WriteLine(i.Current);
			}

			private readonly ArrayIterator4 i;
		}
	}
}
