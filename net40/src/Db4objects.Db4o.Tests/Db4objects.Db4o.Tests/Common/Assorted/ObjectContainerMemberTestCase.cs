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
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class ObjectContainerMemberTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public IObjectContainer _typedObjectContainer;

			public object _untypedObjectContainer;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IEventRegistry eventRegistryFactory = EventRegistryFactory.ForObjectContainer(Db(
				));
			eventRegistryFactory.Creating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_23().OnEvent);
			ObjectContainerMemberTestCase.Item item = new ObjectContainerMemberTestCase.Item(
				);
			item._typedObjectContainer = Db();
			item._untypedObjectContainer = Db();
			Store(item);
			// Special case: Cascades activation to existing ObjectContainer member
			Db().QueryByExample(typeof(ObjectContainerMemberTestCase.Item)).Next();
		}

		private sealed class _IEventListener4_23
		{
			public _IEventListener4_23()
			{
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				object obj = ((CancellableObjectEventArgs)args).Object;
				Assert.IsFalse(obj is IObjectContainer);
			}
		}
	}
}
