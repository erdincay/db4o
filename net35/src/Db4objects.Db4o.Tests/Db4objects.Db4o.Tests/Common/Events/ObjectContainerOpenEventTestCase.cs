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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class ObjectContainerOpenEventTestCase : ITestCase
	{
		private BooleanByRef eventReceived = new BooleanByRef(false);

		internal sealed class OpenListenerConfigurationItem : IConfigurationItem
		{
			private BooleanByRef _eventReceived;

			internal OpenListenerConfigurationItem(ObjectContainerOpenEventTestCase _enclosing
				, BooleanByRef eventReceived)
			{
				this._enclosing = _enclosing;
				this._eventReceived = eventReceived;
			}

			public void Prepare(IConfiguration configuration)
			{
			}

			public void Apply(IInternalObjectContainer container)
			{
				EventRegistryFactory.ForObjectContainer(container).Opened += new System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs>
					(new _IEventListener4_28(this).OnEvent);
			}

			private sealed class _IEventListener4_28
			{
				public _IEventListener4_28(OpenListenerConfigurationItem _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectContainerEventArgs
					 args)
				{
					this._enclosing._eventReceived.value = true;
				}

				private readonly OpenListenerConfigurationItem _enclosing;
			}

			private readonly ObjectContainerOpenEventTestCase _enclosing;
		}

		public virtual void Test()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = new MemoryStorage();
			config.Common.Add(new ObjectContainerOpenEventTestCase.OpenListenerConfigurationItem
				(this, eventReceived));
			Assert.IsFalse(eventReceived.value);
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(config, string.Empty);
			Assert.IsTrue(eventReceived.value);
			db.Close();
		}
	}
}
