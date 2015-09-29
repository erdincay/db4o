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
	public class NoDuplicatesQueueTestCase : ITestLifeCycle
	{
		private IQueue4 _queue;

		public virtual void Test()
		{
			_queue.Add("A");
			_queue.Add("B");
			_queue.Add("B");
			_queue.Add("A");
			Assert.AreEqual("A", _queue.Next());
			Assert.AreEqual("B", _queue.Next());
			Assert.IsFalse(_queue.HasNext());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			_queue = new NoDuplicatesQueue(new NonblockingQueue());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			_queue = null;
		}
	}
}
