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
using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Foundation
{
	/// <exclude></exclude>
	public class ListenerRegistry
	{
		public static ListenerRegistry NewInstance()
		{
			return new ListenerRegistry();
		}

		private IdentitySet4 _listeners;

		public virtual void Register(IListener4 listener)
		{
			if (_listeners == null)
			{
				_listeners = new IdentitySet4();
			}
			_listeners.Add(listener);
		}

		public virtual void NotifyListeners(object @event)
		{
			if (_listeners == null)
			{
				return;
			}
			IEnumerator i = _listeners.GetEnumerator();
			while (i.MoveNext())
			{
				((IListener4)i.Current).OnEvent(@event);
			}
		}

		public virtual void Remove(IListener4 listener)
		{
			if (_listeners == null)
			{
				return;
			}
			_listeners.Remove(listener);
		}
	}
}
