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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class InstantiationEventsTestCase : EventsTestCaseBase
	{
		protected override void Configure(IConfiguration config)
		{
			config.ActivationDepth(0);
		}

		public virtual void TestInstantiationEvents()
		{
			EventsTestCaseBase.EventLog instantiatedLog = new EventsTestCaseBase.EventLog();
			EventRegistry().Instantiated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_20(this, instantiatedLog).OnEvent);
			RetrieveOnlyInstance(typeof(EventsTestCaseBase.Item));
			Assert.IsFalse(instantiatedLog.xing);
			Assert.IsTrue(instantiatedLog.xed);
		}

		private sealed class _IEventListener4_20
		{
			public _IEventListener4_20(InstantiationEventsTestCase _enclosing, EventsTestCaseBase.EventLog
				 instantiatedLog)
			{
				this._enclosing = _enclosing;
				this.instantiatedLog = instantiatedLog;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				this._enclosing.AssertClientTransaction(((ObjectInfoEventArgs)args));
				instantiatedLog.xed = true;
				object obj = ((ObjectInfoEventArgs)args).Object;
				ObjectReference objectReference = this._enclosing.Trans().ReferenceSystem().ReferenceForObject
					(obj);
				Assert.IsNotNull(objectReference);
				Assert.AreSame(objectReference, ((ObjectInfoEventArgs)args).Info);
			}

			private readonly InstantiationEventsTestCase _enclosing;

			private readonly EventsTestCaseBase.EventLog instantiatedLog;
		}
	}
}
