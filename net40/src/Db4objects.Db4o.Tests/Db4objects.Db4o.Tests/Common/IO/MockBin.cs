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
using Db4oUnit.Mocking;
using Db4objects.Db4o.IO;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class MockBin : MethodCallRecorder, IBin
	{
		private int _returnValue;

		public virtual void Close()
		{
			Record("close");
		}

		public virtual long Length()
		{
			Record("length");
			return _returnValue;
		}

		private void Record(string methodName)
		{
			Record(new MethodCall(methodName, new object[] {  }));
		}

		public virtual int Read(long position, byte[] buffer, int bytesToRead)
		{
			Record(new MethodCall("read", new object[] { position, buffer, bytesToRead }));
			return _returnValue;
		}

		public virtual void Sync()
		{
			Record("sync");
		}

		public virtual int SyncRead(long position, byte[] buffer, int bytesToRead)
		{
			Record(new MethodCall("syncRead", new object[] { position, buffer, bytesToRead })
				);
			return _returnValue;
		}

		public virtual void Write(long position, byte[] bytes, int bytesToWrite)
		{
			Record(new MethodCall("write", new object[] { position, bytes, bytesToWrite }));
		}

		public virtual void ReturnValueForNextCall(int value)
		{
			_returnValue = value;
		}

		public virtual void Sync(IRunnable runnable)
		{
			Sync();
			runnable.Run();
			Sync();
		}
	}
}
