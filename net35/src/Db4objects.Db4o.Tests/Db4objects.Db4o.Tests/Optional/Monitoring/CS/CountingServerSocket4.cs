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
using Db4objects.Db4o.CS.Foundation;
using Db4objects.Db4o.Tests.Optional.Monitoring.CS;

namespace Db4objects.Db4o.Tests.Optional.Monitoring.CS
{
	public class CountingServerSocket4 : IServerSocket4
	{
		public CountingServerSocket4(IServerSocket4 serverSocket)
		{
			_serverSocket = serverSocket;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual ISocket4 Accept()
		{
			CountingSocket4 socket = new CountingSocket4(_serverSocket.Accept());
			_clients.Add(socket);
			return socket;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Close()
		{
			_serverSocket.Close();
		}

		public virtual int GetLocalPort()
		{
			return _serverSocket.GetLocalPort();
		}

		public virtual void SetSoTimeout(int timeout)
		{
			_serverSocket.SetSoTimeout(timeout);
		}

		public virtual IList ConnectedClients()
		{
			return _clients;
		}

		private IServerSocket4 _serverSocket;

		private IList _clients = new ArrayList();
	}
}
#endif // !SILVERLIGHT
