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
using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class ReadOnlyBinTest : StorageTestUnitBase
	{
		public virtual void Test()
		{
			ReopenAsReadOnly();
			AssertReadOnly(_bin);
		}

		private void ReopenAsReadOnly()
		{
			Close();
			Open(true);
		}

		private void AssertReadOnly(IBin adapter)
		{
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_21(adapter));
		}

		private sealed class _ICodeBlock_21 : ICodeBlock
		{
			public _ICodeBlock_21(IBin adapter)
			{
				this.adapter = adapter;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				adapter.Write(0, new byte[] { 0 }, 1);
			}

			private readonly IBin adapter;
		}
	}
}
