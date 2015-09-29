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

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.Tests.CLI1.Foundation.Net.SSL
{
	internal class PassThroughSocketFactory : ISocket4Factory
	{
		public PassThroughSocketFactory(ISocket4Factory delegating)
		{
			_delegating = delegating;	
		}

		public PassThroughSocketFactory()
		{
			_delegating = new StandardSocket4Factory();
		}

		public ISocket4 CreateSocket(string hostName, int port)
		{
			ISocket4 socket = _delegating.CreateSocket(hostName, port);
			_lastClientSocket = new PassThroughSocket(socket);
			
			return _lastClientSocket;
		}

		public IServerSocket4 CreateServerSocket(int port)
		{
			return _delegating.CreateServerSocket(port);
		}

		public PassThroughSocket LastClient
		{
			get { return _lastClientSocket; }
		}

		private readonly ISocket4Factory _delegating;
		private PassThroughSocket _lastClientSocket;
	}
}

#endif