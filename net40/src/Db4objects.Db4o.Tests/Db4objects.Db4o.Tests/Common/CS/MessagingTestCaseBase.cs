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
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Messaging;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class MessagingTestCaseBase : ITestCase, IOptOutMultiSession
	{
		public sealed class MessageCollector : IMessageRecipient
		{
			public readonly Collection4 messages = new Collection4();

			public void ProcessMessage(IMessageContext context, object message)
			{
				messages.Add(message);
			}
		}

		protected virtual IMessageSender MessageSender(IObjectContainer client)
		{
			return client.Ext().Configure().ClientServer().GetMessageSender();
		}

		protected virtual IObjectContainer OpenClient(string clientId, IObjectServer server
			)
		{
			server.GrantAccess(clientId, "p");
			return Db4oClientServer.OpenClient(MultithreadedClientConfig(), "127.0.0.1", server
				.Ext().Port(), clientId, "p");
		}

		private IClientConfiguration MultithreadedClientConfig()
		{
			IClientConfiguration config = Db4oClientServer.NewClientConfiguration();
			config.Networking.SingleThreadedClient = false;
			return config;
		}

		protected virtual IObjectServer OpenServerWith(IMessageRecipient recipient)
		{
			IConfiguration config = MemoryIoConfiguration();
			SetMessageRecipient(config, recipient);
			return OpenServer(config);
		}

		protected virtual IConfiguration MemoryIoConfiguration()
		{
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.Storage = new MemoryStorage();
			return config;
		}

		protected virtual IObjectServer OpenServer(IConfiguration config)
		{
			return Db4oClientServer.OpenServer(Db4oClientServerLegacyConfigurationBridge.AsServerConfiguration
				(config), "nofile", unchecked((int)(0xdb40)));
		}

		protected virtual void SetMessageRecipient(IObjectContainer container, IMessageRecipient
			 recipient)
		{
			SetMessageRecipient(container.Ext().Configure(), recipient);
		}

		private void SetMessageRecipient(IConfiguration config, IMessageRecipient recipient
			)
		{
			config.ClientServer().SetMessageRecipient(recipient);
		}
	}
}
#endif // !SILVERLIGHT
