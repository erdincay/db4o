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
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Events;

namespace Db4objects.Db4o.Tests.Common.TA.Events
{
	public class ActivationEventsTestCase : TransparentActivationTestCaseBase
	{
		public class NonActivatableItem
		{
			public string name;

			public NonActivatableItem(string name_)
			{
				name = name_;
			}

			public NonActivatableItem()
			{
			}
		}

		public class ActivatableItem : IActivatable
		{
			public string name;

			public ActivationEventsTestCase.NonActivatableItem child;

			[System.NonSerialized]
			private IActivator _activator;

			public ActivatableItem(string name_, ActivationEventsTestCase.NonActivatableItem 
				child_)
			{
				name = name_;
				child = child_;
			}

			public ActivatableItem()
			{
			}

			public virtual void Activate(ActivationPurpose purpose)
			{
				_activator.Activate(purpose);
			}

			public virtual void Bind(IActivator activator)
			{
				_activator = activator;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			ActivationEventsTestCase.NonActivatableItem nonActivatable = new ActivationEventsTestCase.NonActivatableItem
				("Eric Idle");
			Store(nonActivatable);
			Store(new ActivationEventsTestCase.ActivatableItem("John Cleese", nonActivatable)
				);
		}

		public virtual void TestActivatingCancelNonActivatableDepth0()
		{
			AddCancelAnyListener();
			ActivationEventsTestCase.NonActivatableItem item = QueryNonActivatableItem();
			Assert.IsNull(item.name);
		}

		public virtual void TestActivatingCancelActivatableDepth0()
		{
			AddCancelAnyListener();
			ActivationEventsTestCase.ActivatableItem item = QueryActivatableItem();
			item.Activate(ActivationPurpose.Read);
			Assert.IsNull(item.name);
		}

		public virtual void TestActivatingCancelDepth1()
		{
			AddCancelNonActivatableListener();
			ActivationEventsTestCase.ActivatableItem item = QueryActivatableItem();
			item.Activate(ActivationPurpose.Read);
			Assert.IsNotNull(item.name);
			Assert.IsNotNull(item.child);
			Assert.IsNull(item.child.name);
		}

		private void AddCancelNonActivatableListener()
		{
			EventRegistry().Activating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_79().OnEvent);
		}

		private sealed class _IEventListener4_79
		{
			public _IEventListener4_79()
			{
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				object obj = ((ObjectEventArgs)args).Object;
				if (obj is ActivationEventsTestCase.NonActivatableItem)
				{
					((ICancellableEventArgs)args).Cancel();
				}
			}
		}

		private void AddCancelAnyListener()
		{
			EventRegistry().Activating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_90().OnEvent);
		}

		private sealed class _IEventListener4_90
		{
			public _IEventListener4_90()
			{
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				((ICancellableEventArgs)args).Cancel();
			}
		}

		private ActivationEventsTestCase.NonActivatableItem QueryNonActivatableItem()
		{
			return (ActivationEventsTestCase.NonActivatableItem)((ActivationEventsTestCase.NonActivatableItem
				)RetrieveOnlyInstance(typeof(ActivationEventsTestCase.NonActivatableItem)));
		}

		private ActivationEventsTestCase.ActivatableItem QueryActivatableItem()
		{
			return (ActivationEventsTestCase.ActivatableItem)((ActivationEventsTestCase.ActivatableItem
				)RetrieveOnlyInstance(typeof(ActivationEventsTestCase.ActivatableItem)));
		}
	}
}
