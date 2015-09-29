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
using System;
using Db4oUnit;
using Db4oUnit.Mocking;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class NonFlushingStorageTestCase : ITestCase
	{
		public virtual void Test()
		{
			MockBin mock = new MockBin();
			BinConfiguration binConfig = new BinConfiguration("uri", true, 42L, false);
			IBin storage = new NonFlushingStorage(new _IStorage_19(mock)).Open(binConfig);
			byte[] buffer = new byte[5];
			storage.Read(1, buffer, 4);
			storage.Write(2, buffer, 3);
			mock.ReturnValueForNextCall(42);
			Assert.AreEqual(42, mock.Length());
			storage.Sync();
			storage.Close();
			mock.Verify(new MethodCall[] { new MethodCall("open", new object[] { binConfig })
				, new MethodCall("read", new object[] { 1L, buffer, 4 }), new MethodCall("write"
				, new object[] { 2L, buffer, 3 }), new MethodCall("length", new object[] {  }), 
				new MethodCall("close", new object[] {  }) });
		}

		private sealed class _IStorage_19 : IStorage
		{
			public _IStorage_19(MockBin mock)
			{
				this.mock = mock;
			}

			public bool Exists(string uri)
			{
				throw new NotImplementedException();
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public IBin Open(BinConfiguration config)
			{
				mock.Record(new MethodCall("open", new object[] { config }));
				return mock;
			}

			/// <exception cref="System.IO.IOException"></exception>
			public void Delete(string uri)
			{
				throw new NotImplementedException();
			}

			/// <exception cref="System.IO.IOException"></exception>
			public void Rename(string oldUri, string newUri)
			{
				throw new NotImplementedException();
			}

			private readonly MockBin mock;
		}
	}
}
