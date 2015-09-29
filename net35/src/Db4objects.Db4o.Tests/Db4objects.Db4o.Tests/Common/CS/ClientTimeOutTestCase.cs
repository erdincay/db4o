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
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Messaging;
using Db4objects.Db4o.Tests.Common.CS;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ClientTimeOutTestCase : Db4oClientServerTestCase, IOptOutAllButNetworkingCS
	{
		private const int Timeout = 4000;

		internal static bool _clientWasBlocked;

		internal ClientTimeOutTestCase.TestMessageRecipient recipient = new ClientTimeOutTestCase.TestMessageRecipient
			();

		public static void Main(string[] args)
		{
			new ClientTimeOutTestCase().RunAll();
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().TimeoutClientSocket(Timeout);
		}

		public virtual void TestKeptAliveClient()
		{
			ClientTimeOutTestCase.Item item = new ClientTimeOutTestCase.Item("one");
			Store(item);
			Runtime4.Sleep(Timeout * 2);
			Assert.AreSame(item, ((ClientTimeOutTestCase.Item)RetrieveOnlyInstance(typeof(ClientTimeOutTestCase.Item
				))));
		}

		public virtual void TestTimedoutAndClosedClient()
		{
			Store(new ClientTimeOutTestCase.Item("one"));
			ClientServerFixture().Server().Ext().Configure().ClientServer().SetMessageRecipient
				(recipient);
			IExtObjectContainer client = ClientServerFixture().Db();
			IMessageSender sender = client.Configure().ClientServer().GetMessageSender();
			_clientWasBlocked = false;
			sender.Send(new ClientTimeOutTestCase.Data());
			long start = Runtime.CurrentTimeMillis();
			Assert.Expect(typeof(DatabaseClosedException), new _ICodeBlock_58(client));
			long stop = Runtime.CurrentTimeMillis();
			long duration = stop - start;
			Assert.IsGreaterOrEqual(Timeout / 2, duration);
			Assert.IsTrue(_clientWasBlocked);
		}

		private sealed class _ICodeBlock_58 : ICodeBlock
		{
			public _ICodeBlock_58(IExtObjectContainer client)
			{
				this.client = client;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				client.QueryByExample(null);
			}

			private readonly IExtObjectContainer client;
		}

		public class TestMessageRecipient : IMessageRecipient
		{
			public virtual void ProcessMessage(IMessageContext con, object message)
			{
				_clientWasBlocked = true;
				Runtime4.Sleep(Timeout * 3);
			}
		}

		public class Data
		{
		}
	}
}
#endif // !SILVERLIGHT
