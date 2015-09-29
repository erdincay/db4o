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
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class CreationEventsTestCase : EventsTestCaseBase
	{
		public virtual void TestObjectInfoIsNotAvailableOnCreatingHandler()
		{
			ByRef executed = ByRef.NewInstance(false);
			EventRegistry().Creating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_15(executed).OnEvent);
			Store(new EventsTestCaseBase.Item());
			Assert.IsTrue((((bool)executed.value)));
		}

		private sealed class _IEventListener4_15
		{
			public _IEventListener4_15(ByRef executed)
			{
				this.executed = executed;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_18(executed, args
					));
			}

			private sealed class _ICodeBlock_18 : ICodeBlock
			{
				public _ICodeBlock_18(ByRef executed, EventArgs args)
				{
					this.executed = executed;
					this.args = args;
				}

				/// <exception cref="System.Exception"></exception>
				public void Run()
				{
					executed.value = true;
					this.UsefulForCSharp(((CancellableObjectEventArgs)args).Info);
				}

				private void UsefulForCSharp(IObjectInfo info)
				{
					Assert.Fail();
				}

				private readonly ByRef executed;

				private readonly EventArgs args;
			}

			private readonly ByRef executed;
		}
	}
}
