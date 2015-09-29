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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	public class FreespaceRemainderLimitTestCase : ITestCase
	{
		private const int BlockSize = 7;

		private readonly IBlockConverter _blockConverter = new BlockSizeBlockConverter(BlockSize
			);

		private readonly InMemoryFreespaceManager _blocked = new InMemoryFreespaceManager
			(null, 0, 2);

		private readonly BlockAwareFreespaceManager _nonBlocked;

		public virtual void TestAllocateSlotWithRemainder()
		{
			AssertAllocateSlot(3 * BlockSize, BlockSize + 1, 3 * BlockSize);
		}

		public virtual void TestAllocateSlotNoRemainder()
		{
			AssertAllocateSlot(5 * BlockSize, BlockSize + 1, 2 * BlockSize);
		}

		private void AssertAllocateSlot(int freeSlotSize, int requiredSlotSize, int expectedSlotSize
			)
		{
			Slot slot = new Slot(1, freeSlotSize);
			_nonBlocked.Free(slot);
			Slot allocatedSlot = _nonBlocked.AllocateSlot(requiredSlotSize);
			int expectedAllocatedSlotLength = _blockConverter.BlockAlignedBytes(expectedSlotSize
				);
			Slot expectedSlot = new Slot(1, expectedAllocatedSlotLength);
			Assert.AreEqual(expectedSlot, allocatedSlot);
		}

		public FreespaceRemainderLimitTestCase()
		{
			_nonBlocked = new BlockAwareFreespaceManager(_blocked, new BlockSizeBlockConverter
				(BlockSize));
		}
	}
}
