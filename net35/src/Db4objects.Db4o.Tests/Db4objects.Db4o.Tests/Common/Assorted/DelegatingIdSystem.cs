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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class DelegatingIdSystem : IIdSystem
	{
		protected readonly IIdSystem _delegate;

		public DelegatingIdSystem(LocalObjectContainer container)
		{
			_delegate = new InMemoryIdSystem(container);
		}

		public virtual void Close()
		{
			_delegate.Close();
		}

		public virtual void Commit(IVisitable slotChanges, FreespaceCommitter freespaceCommitter
			)
		{
			_delegate.Commit(slotChanges, freespaceCommitter);
		}

		public virtual Slot CommittedSlot(int id)
		{
			return _delegate.CommittedSlot(id);
		}

		public virtual void CompleteInterruptedTransaction(int transactionId1, int transactionId2
			)
		{
			_delegate.CompleteInterruptedTransaction(transactionId1, transactionId2);
		}

		public virtual int NewId()
		{
			return _delegate.NewId();
		}

		public virtual void ReturnUnusedIds(IVisitable visitable)
		{
			_delegate.ReturnUnusedIds(visitable);
		}

		public virtual void TraverseOwnSlots(IProcedure4 block)
		{
			_delegate.TraverseOwnSlots(block);
		}
	}
}
