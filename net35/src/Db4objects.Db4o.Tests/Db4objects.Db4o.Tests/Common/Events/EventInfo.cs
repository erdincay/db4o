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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Events
{
	internal class EventInfo
	{
		public EventInfo(string eventFirerName, IProcedure4 eventListenerSetter) : this(eventFirerName
			, true, eventListenerSetter)
		{
		}

		public EventInfo(string eventFirerName, bool isClientServerEvent, IProcedure4 eventListenerSetter
			)
		{
			_listenerSetter = eventListenerSetter;
			_eventFirerName = eventFirerName;
			_isClientServerEvent = isClientServerEvent;
		}

		public virtual IProcedure4 ListenerSetter()
		{
			return _listenerSetter;
		}

		public virtual string EventFirerName()
		{
			return _eventFirerName;
		}

		public virtual bool IsClientServerEvent()
		{
			return _isClientServerEvent;
		}

		private readonly IProcedure4 _listenerSetter;

		private readonly string _eventFirerName;

		private readonly bool _isClientServerEvent;
	}
}
