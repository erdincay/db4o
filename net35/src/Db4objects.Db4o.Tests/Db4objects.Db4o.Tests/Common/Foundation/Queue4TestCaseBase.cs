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
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class Queue4TestCaseBase : ITestCase
	{
		public Queue4TestCaseBase() : base()
		{
		}

		protected virtual void AssertIterator(IQueue4 queue, string[] data, int size)
		{
			IEnumerator iter = queue.Iterator();
			for (int idx = 0; idx < size; idx++)
			{
				Assert.IsTrue(iter.MoveNext(), "should be able to move in iteration #" + idx + " of "
					 + size);
				Assert.AreEqual(data[idx], iter.Current);
			}
			Assert.IsFalse(iter.MoveNext());
		}
	}
}
