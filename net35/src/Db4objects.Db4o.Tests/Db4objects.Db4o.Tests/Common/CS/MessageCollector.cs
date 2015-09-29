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
#if !SILVERLIGHT
using System;
using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Events;

namespace Db4objects.Db4o.Tests.Common.CS
{
	internal class MessageCollector
	{
		public static IList ForServerDispatcher(IServerMessageDispatcher dispatcher)
		{
			ArrayList _messages = new ArrayList();
			dispatcher.MessageReceived += new System.EventHandler<MessageEventArgs>(new _IEventListener4_16
				(_messages).OnEvent);
			return _messages;
		}

		private sealed class _IEventListener4_16
		{
			public _IEventListener4_16(ArrayList _messages)
			{
				this._messages = _messages;
			}

			public void OnEvent(object sender, MessageEventArgs args)
			{
				_messages.Add(((MessageEventArgs)args).Message);
			}

			private readonly ArrayList _messages;
		}
	}
}
#endif // !SILVERLIGHT
