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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Updatedepth;

namespace Db4objects.Db4o.Tests.Common.Updatedepth
{
	public class NegativeUpdateDepthTestCase : ITestCase
	{
		public class Item
		{
		}

		public virtual void TestNegativeUpdateDepthIsIllegal()
		{
			ICommonConfiguration config = Db4oEmbedded.NewConfiguration().Common;
			Assert.Expect(typeof(ArgumentException), new _ICodeBlock_17(config));
		}

		private sealed class _ICodeBlock_17 : ICodeBlock
		{
			public _ICodeBlock_17(ICommonConfiguration config)
			{
				this.config = config;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				config.ObjectClass(typeof(NegativeUpdateDepthTestCase.Item)).UpdateDepth(Const4.Unspecified
					);
			}

			private readonly ICommonConfiguration config;
		}
	}
}
