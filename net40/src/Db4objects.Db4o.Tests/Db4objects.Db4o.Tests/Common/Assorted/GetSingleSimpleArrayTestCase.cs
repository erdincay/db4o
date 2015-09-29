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
using Db4objects.Db4o;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class GetSingleSimpleArrayTestCase : AbstractDb4oTestCase
	{
		public virtual void Test()
		{
			IObjectSet result = Db().QueryByExample(new double[] { 0.6, 0.4 });
			Assert.IsFalse(result.HasNext());
			Assert.IsFalse(result.HasNext());
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_17(result));
		}

		private sealed class _ICodeBlock_17 : ICodeBlock
		{
			public _ICodeBlock_17(IObjectSet result)
			{
				this.result = result;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				result.Next();
			}

			private readonly IObjectSet result;
		}
	}
}
