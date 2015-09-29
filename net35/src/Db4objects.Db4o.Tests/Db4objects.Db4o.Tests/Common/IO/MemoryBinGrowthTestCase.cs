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
using Db4objects.Db4o.Tests.Common.IO;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class MemoryBinGrowthTestCase : ITestCase
	{
		private sealed class MockGrowthStrategy : IGrowthStrategy
		{
			private int[] _values;

			private int _idx;

			public MockGrowthStrategy(int[] values)
			{
				_values = values;
				_idx = 0;
			}

			public long NewSize(long curSize, long requiredSize)
			{
				return _values[_idx++];
			}

			public void Verify()
			{
				Assert.AreEqual(_values.Length, _idx);
			}
		}

		private static readonly string Uri = "growingbin";

		private const int InitialSize = 20;

		public virtual void TestGrowth()
		{
			int[] values = new int[] { 42, 47, 48 };
			MemoryBinGrowthTestCase.MockGrowthStrategy strategy = new MemoryBinGrowthTestCase.MockGrowthStrategy
				(values);
			MemoryBin bin = NewBin(InitialSize, strategy);
			Write(bin, 0, InitialSize + 1, values[0]);
			Write(bin, values[0], 1, values[1]);
			Write(bin, values[1], 1, values[2]);
			strategy.Verify();
		}

		public virtual void TestDoublingStrategy()
		{
			MemoryBin bin = NewBin(0, new DoublingGrowthStrategy());
			Write(bin, 0, 1, 1);
			Write(bin, 0, 2, 2);
			Write(bin, 0, 3, 4);
			bin = NewBin(InitialSize, new DoublingGrowthStrategy());
			Write(bin, 0, InitialSize + 1, 2 * InitialSize);
		}

		public virtual void TestConstantStrategy()
		{
			int growth = 100;
			MemoryBin bin = NewBin(InitialSize, new ConstantGrowthStrategy(growth));
			Write(bin, 0, InitialSize + 1, growth + InitialSize);
			Write(bin, 0, growth + InitialSize + 1, InitialSize + (2 * growth));
		}

		private MemoryBin NewBin(int initialSize, IGrowthStrategy strategy)
		{
			MemoryStorage storage = new MemoryStorage(strategy);
			MemoryBin bin = (MemoryBin)storage.Open(new BinConfiguration(Uri, false, initialSize
				, false));
			return bin;
		}

		private void Write(MemoryBin bin, int pos, int count, int expectedSize)
		{
			bin.Write(pos, new byte[count], count);
			Assert.AreEqual(expectedSize, bin.BufferSize());
		}
	}
}
