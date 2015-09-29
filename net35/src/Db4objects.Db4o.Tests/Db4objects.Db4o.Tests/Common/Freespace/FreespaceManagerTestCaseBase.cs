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
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Common.Freespace;

namespace Db4objects.Db4o.Tests.Common.Freespace
{
	public abstract class FreespaceManagerTestCaseBase : FileSizeTestCaseBase, IOptOutMultiSession
	{
		protected IFreespaceManager[] fm;

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupAfterStore()
		{
			LocalObjectContainer container = (LocalObjectContainer)Db();
			BTreeFreespaceManager btreeFm = new BTreeFreespaceManager(container, null, container
				.ConfigImpl.DiscardFreeSpace(), AbstractFreespaceManager.RemainderSizeLimit);
			btreeFm.Start(0);
			fm = new IFreespaceManager[] { new InMemoryFreespaceManager(null, container.ConfigImpl
				.DiscardFreeSpace(), AbstractFreespaceManager.RemainderSizeLimit), btreeFm };
		}

		protected virtual void Clear(IFreespaceManager freespaceManager)
		{
			Slot slot = null;
			do
			{
				slot = freespaceManager.AllocateSlot(1);
			}
			while (slot != null);
			Assert.AreEqual(0, freespaceManager.SlotCount());
			Assert.AreEqual(0, freespaceManager.TotalFreespace());
		}

		protected virtual void AssertSame(IFreespaceManager fm1, IFreespaceManager fm2)
		{
			Assert.AreEqual(fm1.SlotCount(), fm2.SlotCount());
			Assert.AreEqual(fm1.TotalFreespace(), fm2.TotalFreespace());
		}

		protected virtual void AssertSlot(Slot expected, Slot actual)
		{
			Assert.AreEqual(expected, actual);
		}

		protected virtual void ProduceSomeFreeSpace()
		{
			int length = 300;
			Slot slot = LocalContainer().AllocateSlot(length);
			ByteArrayBuffer buffer = new ByteArrayBuffer(length);
			LocalContainer().WriteBytes(buffer, slot.Address(), 0);
			LocalContainer().Free(slot);
		}

		protected virtual IFreespaceManager CurrentFreespaceManager()
		{
			return LocalContainer().FreespaceManager();
		}

		public class Item
		{
			public int _int;
		}

		protected virtual void StoreSomeItems()
		{
			for (int i = 0; i < 3; i++)
			{
				Store(new FreespaceManagerTestCaseBase.Item());
			}
			Db().Commit();
		}

		protected virtual LocalObjectContainer LocalContainer()
		{
			return Fixture().FileSession();
		}
	}
}
