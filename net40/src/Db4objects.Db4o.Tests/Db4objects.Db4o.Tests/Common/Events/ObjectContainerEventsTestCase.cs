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
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class ObjectContainerEventsTestCase : AbstractDb4oTestCase, IOptOutDefragSolo
	{
		/// <exception cref="System.Exception"></exception>
		public virtual void TestClose()
		{
			IExtObjectContainer container = Db();
			LocalObjectContainer session = FileSession();
			Collection4 actual = new Collection4();
			EventRegistry().Closing += new System.EventHandler<Db4objects.Db4o.Events.ObjectContainerEventArgs>
				(new _IEventListener4_21(actual).OnEvent);
			Fixture().Close();
			if (IsEmbedded())
			{
				Iterator4Assert.AreEqual(new object[] { container, session }, actual.GetEnumerator
					());
			}
			else
			{
				Assert.AreSame(container, actual.SingleElement());
			}
		}

		private sealed class _IEventListener4_21
		{
			public _IEventListener4_21(Collection4 actual)
			{
				this.actual = actual;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectContainerEventArgs
				 args)
			{
				actual.Add(((ObjectContainerEventArgs)args).ObjectContainer);
			}

			private readonly Collection4 actual;
		}
	}
}
