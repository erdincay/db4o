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
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Tests.Optional.Monitoring.CS;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	class MockServerSocket4 : IServerSocket4
	{
		public void SetSoTimeout(int timeout)
		{
		}

		public int GetLocalPort()
		{
			return 0xDB40;
		}

		public ISocket4 Accept()
		{
			CountingSocket4 accepted = new CountingSocket4(new NullSocket());
			_acceptedSockets.Add(accepted);
			return accepted;
		}

		public void Close()
		{
			throw new NotImplementedException();
		}

		public double BytesSent()
		{
			double total = 0.0;
			foreach (CountingSocket4 countingSocket in _acceptedSockets)
			{
				total += countingSocket.BytesSent();
			}

			return total;
		}

		public double BytesReceived()
		{
			double total = 0.0;
			foreach (CountingSocket4 countingSocket in _acceptedSockets)
			{
				total += countingSocket.BytesReceived();
			}

			return total;
		}

		public double MessagesSent()
		{
			double total = 0.0;
			foreach (CountingSocket4 countingSocket in _acceptedSockets)
			{
				total += countingSocket.MessagesSent();
			}

			return total;
		}

		private readonly IList<CountingSocket4> _acceptedSockets = new List<CountingSocket4>();
	}
}

#endif