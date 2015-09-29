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
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Messaging;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class PrimitiveMessageTestCase : MessagingTestCaseBase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(PrimitiveMessageTestCase)).Run();
		}

		public virtual void Test()
		{
			Collection4 expected = new Collection4(new object[] { "PING", true, 42 });
			MessagingTestCaseBase.MessageCollector recipient = new MessagingTestCaseBase.MessageCollector
				();
			IObjectServer server = OpenServerWith(recipient);
			try
			{
				IObjectContainer client = OpenClient("client", server);
				try
				{
					IMessageSender sender = MessageSender(client);
					SendAll(expected, sender);
				}
				finally
				{
					client.Close();
				}
			}
			finally
			{
				server.Close();
			}
			Assert.AreEqual(expected.ToString(), recipient.messages.ToString());
		}

		private void SendAll(IEnumerable messages, IMessageSender sender)
		{
			IEnumerator iterator = messages.GetEnumerator();
			while (iterator.MoveNext())
			{
				sender.Send(iterator.Current);
			}
		}
	}
}
#endif // !SILVERLIGHT
