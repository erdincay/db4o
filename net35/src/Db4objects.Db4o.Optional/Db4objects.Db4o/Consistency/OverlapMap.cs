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
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Sharpen.Util;

namespace Db4objects.Db4o.Consistency
{
	internal class OverlapMap
	{
		private Sharpen.Util.ISet _dupes = new HashSet();

		private TreeIntObject _slots = null;

		private readonly IBlockConverter _blockConverter;

		public OverlapMap(IBlockConverter blockConverter)
		{
			_blockConverter = blockConverter;
		}

		public virtual void Add(SlotDetail slot)
		{
			if (TreeIntObject.Find(_slots, new TreeIntObject(slot._slot.Address())) != null)
			{
				_dupes.Add(new Pair(ByAddress(slot._slot.Address()), slot));
			}
			_slots = (TreeIntObject)((TreeIntObject)TreeIntObject.Add(_slots, new TreeIntObject
				(slot._slot.Address(), slot)));
		}

		public virtual Sharpen.Util.ISet Overlaps()
		{
			Sharpen.Util.ISet overlaps = new HashSet();
			ByRef prevSlot = ByRef.NewInstance();
			TreeIntObject.Traverse(_slots, new _IVisitor4_29(this, prevSlot, overlaps));
			return overlaps;
		}

		private sealed class _IVisitor4_29 : IVisitor4
		{
			public _IVisitor4_29(OverlapMap _enclosing, ByRef prevSlot, Sharpen.Util.ISet overlaps
				)
			{
				this._enclosing = _enclosing;
				this.prevSlot = prevSlot;
				this.overlaps = overlaps;
			}

			public void Visit(object tree)
			{
				SlotDetail curSlot = (SlotDetail)((TreeIntObject)tree)._object;
				if (this.IsOverlap(((SlotDetail)prevSlot.value), curSlot))
				{
					overlaps.Add(new Pair(((SlotDetail)prevSlot.value), curSlot));
				}
				prevSlot.value = curSlot;
			}

			private bool IsOverlap(SlotDetail prevSlot, SlotDetail curSlot)
			{
				if (prevSlot == null)
				{
					return false;
				}
				return prevSlot._slot.Address() + this._enclosing._blockConverter.BytesToBlocks(prevSlot
					._slot.Length()) > curSlot._slot.Address();
			}

			private readonly OverlapMap _enclosing;

			private readonly ByRef prevSlot;

			private readonly Sharpen.Util.ISet overlaps;
		}

		public virtual Sharpen.Util.ISet Dupes()
		{
			return _dupes;
		}

		private SlotDetail ByAddress(int address)
		{
			TreeIntObject tree = (TreeIntObject)TreeIntObject.Find(_slots, new TreeIntObject(
				address, null));
			return tree == null ? null : (SlotDetail)tree._object;
		}
	}
}
