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

using System.IO;
using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.Tests.CLI1.Foundation.Net.SSL
{
	internal class PassThroughSocket : Socket4Decorator
	{
		public PassThroughSocket(ISocket4 socket) : base(socket)
		{
		}

		public override void Write(byte[] bytes, int offset, int count)
		{
			AppendBytes(bytes, offset, count);
			base.Write(bytes, offset, count);
		}

		private void AppendBytes(byte[] bytes, int offset, int count)
		{
			_written.Write(bytes, offset, count);
		}

		public byte[] Written
		{
			get
			{
				_written.Seek(0, SeekOrigin.Begin);

				byte[] data = new byte[_written.Length];
				int read = _written.Read(data, 0, data.Length);
				return data;
			}
		}

		private readonly MemoryStream _written = new MemoryStream();
	}
}
#endif