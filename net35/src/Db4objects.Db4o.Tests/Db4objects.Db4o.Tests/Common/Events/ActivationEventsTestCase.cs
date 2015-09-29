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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class ActivationEventsTestCase : EventsTestCaseBase
	{
		protected override void Configure(IConfiguration config)
		{
			config.ActivationDepth(1);
		}

		public virtual void TestActivationEvents()
		{
			EventsTestCaseBase.EventLog activationLog = new EventsTestCaseBase.EventLog();
			EventRegistry().Activating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_19(this, activationLog).OnEvent);
			EventRegistry().Activated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_25(this, activationLog).OnEvent);
			RetrieveOnlyInstance(typeof(EventsTestCaseBase.Item));
			Assert.IsTrue(activationLog.xing);
			Assert.IsTrue(activationLog.xed);
		}

		private sealed class _IEventListener4_19
		{
			public _IEventListener4_19(ActivationEventsTestCase _enclosing, EventsTestCaseBase.EventLog
				 activationLog)
			{
				this._enclosing = _enclosing;
				this.activationLog = activationLog;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				this._enclosing.AssertClientTransaction(((CancellableObjectEventArgs)args));
				activationLog.xing = true;
			}

			private readonly ActivationEventsTestCase _enclosing;

			private readonly EventsTestCaseBase.EventLog activationLog;
		}

		private sealed class _IEventListener4_25
		{
			public _IEventListener4_25(ActivationEventsTestCase _enclosing, EventsTestCaseBase.EventLog
				 activationLog)
			{
				this._enclosing = _enclosing;
				this.activationLog = activationLog;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing.AssertClientTransaction(((ObjectInfoEventArgs)args));
				activationLog.xed = true;
			}

			private readonly ActivationEventsTestCase _enclosing;

			private readonly EventsTestCaseBase.EventLog activationLog;
		}
	}
}
