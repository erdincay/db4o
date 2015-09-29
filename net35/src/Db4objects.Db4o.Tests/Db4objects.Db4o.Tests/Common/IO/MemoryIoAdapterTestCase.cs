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
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class MemoryIoAdapterTestCase : ITestCase
	{
		private static readonly string Url = "url";

		private const int GrowBy = 100;

		public virtual void TestGrowth()
		{
			MemoryIoAdapter factory = new MemoryIoAdapter();
			factory.GrowBy(GrowBy);
			IoAdapter io = factory.Open(Url, false, 0, false);
			AssertLength(factory, 0);
			WriteBytes(io, GrowBy - 1);
			AssertLength(factory, GrowBy);
			WriteBytes(io, GrowBy - 1);
			AssertLength(factory, GrowBy * 2);
			WriteBytes(io, GrowBy * 2);
			AssertLength(factory, GrowBy * 4 - 2);
		}

		private void WriteBytes(IoAdapter io, int numBytes)
		{
			io.Write(new byte[numBytes]);
		}

		private void AssertLength(MemoryIoAdapter factory, int expected)
		{
			Assert.AreEqual(expected, factory.Get(Url).Length);
		}
	}
}
