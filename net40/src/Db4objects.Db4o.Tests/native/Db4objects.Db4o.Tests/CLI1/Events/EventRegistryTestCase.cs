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
using System.Collections.Generic;
using Db4objects.Db4o.Events;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Events
{
	public class EventRegistryTestCase : AbstractDb4oTestCase
	{
		public class Item
		{	
		}
		
		class EventRecorder
		{
			readonly List<string> _events = new List<string>();

			public EventRecorder(IObjectContainer container)
			{
				IEventRegistry registry = EventRegistryFactory.ForObjectContainer(container);
				registry.Creating += OnCreating;
			}
			
			public string this[int index]
			{
				get { return _events[index];  }
			}

			void OnCreating(object sender, CancellableObjectEventArgs args)
			{
				_events.Add("Creating");
				Assert.IsFalse(args.IsCancelled);
				args.Cancel();
			}
		}
		
		public void TestCreating()
		{
			EventRecorder recorder = new EventRecorder(Db());

			Store(new Item());

			Assert.AreEqual(0, Db().QueryByExample(typeof(Item)).Count);
			Assert.AreEqual("Creating", recorder[0]);
		}
	}
}
