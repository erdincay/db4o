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
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class TimeoutBlockingQueueTestCase : Queue4TestCaseBase
	{
		public virtual void TestTimeoutNext()
		{
			ITimeoutBlockingQueue4 queue = new TimeoutBlockingQueue(300);
			queue.Pause();
			queue.Add(new object());
			queue.Check();
			Assert.IsNull(queue.TryNext());
			Runtime4.SleepThrowsOnInterrupt(500);
			queue.Check();
			Assert.IsNotNull(queue.TryNext());
		}
	}
}
