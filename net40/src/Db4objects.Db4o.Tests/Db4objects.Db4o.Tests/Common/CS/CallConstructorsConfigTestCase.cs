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
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class CallConstructorsConfigTestCase : StandaloneCSTestCaseBase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void RunTest()
		{
			WithClient(new _IContainerBlock_15());
			WithClient(new _IContainerBlock_21());
		}

		private sealed class _IContainerBlock_15 : IContainerBlock
		{
			public _IContainerBlock_15()
			{
			}

			public void Run(IObjectContainer client)
			{
				client.Store(new StandaloneCSTestCaseBase.Item());
			}
		}

		private sealed class _IContainerBlock_21 : IContainerBlock
		{
			public _IContainerBlock_21()
			{
			}

			public void Run(IObjectContainer client)
			{
				Assert.AreEqual(1, client.Query(typeof(StandaloneCSTestCaseBase.Item)).Count);
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.CallConstructors(true);
			config.ExceptionsOnNotStorable(true);
		}
	}
}
#endif // !SILVERLIGHT
