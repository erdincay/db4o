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
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.CS;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class IsAliveConcurrencyTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		private volatile bool processingMessage = false;

		/// <exception cref="System.Exception"></exception>
		public virtual void TestIsAliveInMultiThread()
		{
			IBlockingQueue4 barrier = new BlockingQueue();
			client = (ClientObjectContainer)OpenNewSession();
			client.MessageListener(new _IMessageListener_23(this, barrier));
			Thread workThread = new Thread(new _IRunnable_38(), "Quering");
			workThread.SetDaemon(true);
			workThread.Start();
			barrier.Next();
			client.IsAlive();
		}

		private sealed class _IMessageListener_23 : ClientObjectContainer.IMessageListener
		{
			public _IMessageListener_23(IsAliveConcurrencyTestCase _enclosing, IBlockingQueue4
				 barrier)
			{
				this._enclosing = _enclosing;
				this.barrier = barrier;
			}

			public void OnMessage(Msg msg)
			{
				if (msg is MQueryExecute)
				{
					this._enclosing.processingMessage = true;
					barrier.Add(new object());
					Runtime4.Sleep(500);
					this._enclosing.processingMessage = false;
				}
				else
				{
					if (msg is MIsAlive)
					{
						Assert.IsFalse(this._enclosing.processingMessage);
					}
				}
			}

			private readonly IsAliveConcurrencyTestCase _enclosing;

			private readonly IBlockingQueue4 barrier;
		}

		private sealed class _IRunnable_38 : IRunnable
		{
			public _IRunnable_38()
			{
			}

			public void Run()
			{
				IsAliveConcurrencyTestCase.client.QueryByExample(typeof(IsAliveConcurrencyTestCase.Item
					));
			}
		}

		protected override void Store()
		{
			for (int i = 0; i < 10; ++i)
			{
				Store(new IsAliveConcurrencyTestCase.Item());
			}
		}

		public class Item
		{
		}

		private static ClientObjectContainer client;
	}
}
#endif // !SILVERLIGHT
