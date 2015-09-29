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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class Runtime4TestCase : ITestCase
	{
		public virtual void TestLoopWithTimeoutReturnsWhenBlockIsFalse()
		{
			StopWatch watch = new AutoStopWatch();
			Runtime4.Retry(500, new _IClosure4_14());
			Assert.IsSmaller(500, watch.Peek());
		}

		private sealed class _IClosure4_14 : IClosure4
		{
			public _IClosure4_14()
			{
			}

			public object Run()
			{
				return true;
			}
		}

		public virtual void TestLoopWithTimeoutReturnsAfterTimeout()
		{
			StopWatch watch = new AutoStopWatch();
			Runtime4.Retry(500, new _IClosure4_24());
			watch.Stop();
			Assert.IsGreaterOrEqual(500, watch.Elapsed());
		}

		private sealed class _IClosure4_24 : IClosure4
		{
			public _IClosure4_24()
			{
			}

			public object Run()
			{
				return false;
			}
		}
	}
}
