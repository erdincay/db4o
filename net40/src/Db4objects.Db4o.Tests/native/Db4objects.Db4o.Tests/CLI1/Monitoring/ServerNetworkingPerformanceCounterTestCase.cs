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
#if !CF && !SILVERLIGHT

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.CS.Monitoring;
using Db4objects.Db4o.Monitoring;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class ServerNetworkingPerformanceCounterTestCase : PerformanceCounterTestCaseBase, IOptOutAllButNetworkingCS
	{
		private const int ClientCount = 10;

		private delegate void SocketOperation(ISocket4 socket, int byteCount);

		public void TestBytesSent()
		{
			SocketOperation operation = delegate(ISocket4 client, int byteCount) { client.Write(null, 0, byteCount); };
			Func<PerformanceCounter> counterRetriever = delegate { return PerformanceCounterSpec.NetBytesSentPerSec.PerformanceCounter(FileSession()); };
			Func<MockServerSocket4, long> delegatingRetriever = delegate(MockServerSocket4 mockServerSocket) { return (long) mockServerSocket.BytesSent(); };

			AssertCounter(operation, delegatingRetriever, counterRetriever, ExpectedBytesCount());
		}

		public void TestBytesReceived()
		{
			SocketOperation operation = delegate(ISocket4 client, int byteCount) { client.Read(null, 0, byteCount); };
            Func<PerformanceCounter> counterRetriever = delegate { return PerformanceCounterSpec.NetBytesReceivedPerSec.PerformanceCounter(FileSession()); };
			Func<MockServerSocket4, long> delegatingRetriever = delegate(MockServerSocket4 mockServerSocket) { return (long) mockServerSocket.BytesReceived(); };

			AssertCounter(operation, delegatingRetriever, counterRetriever, ExpectedBytesCount());
		}

		public void TestMessagesSent()
		{
			SocketOperation operation = delegate(ISocket4 client, int byteCount) { client.Write(null, 0, 1); };
			Func<PerformanceCounter> counterRetriever = delegate { return PerformanceCounterSpec.NetMessagesSentPerSec.PerformanceCounter(FileSession()); };
			Func<MockServerSocket4, long> delegatingRetriever = delegate(MockServerSocket4 mockServerSocket) { return (long)mockServerSocket.MessagesSent(); };

			AssertCounter(operation, delegatingRetriever, counterRetriever, ClientCount);
		}

		private void AssertCounter(SocketOperation operation, Func<MockServerSocket4, long> delegatingRetriever, Func<PerformanceCounter> counterRetriever, int expectedCount)
		{
			MockServerSocket4 mockServerSocket = new MockServerSocket4();
			IServerSocket4 serverSocket = new MonitoredServerSocket4(mockServerSocket);

			FileSession().WithEnvironment(
				delegate
					{
						IList<ISocket4> clients = new List<ISocket4>();
						for (int i = 0; i < ClientCount; i++)
						{
							clients.Add(serverSocket.Accept());
						}

						for (int i = 0; i < clients.Count; i++)
						{
							operation(clients[i], i);
						}
					});

			Assert.AreEqual(expectedCount, delegatingRetriever(mockServerSocket));
			Assert.AreEqual(expectedCount, counterRetriever().RawValue);
		}

		private static int ExpectedBytesCount()
		{
			return ClientCount * (ClientCount - 1) / 2;
		}
	}
}

#endif