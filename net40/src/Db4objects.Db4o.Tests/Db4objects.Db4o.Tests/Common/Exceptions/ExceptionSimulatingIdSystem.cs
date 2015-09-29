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
using Db4objects.Db4o.Tests.Common.Assorted;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class ExceptionSimulatingIdSystem : DelegatingIdSystem
	{
		private readonly IExceptionFactory _exceptionFactory;

		private readonly ExceptionSimulatingStorage.ExceptionTriggerCondition _triggerCondition
			 = new ExceptionSimulatingStorage.ExceptionTriggerCondition();

		public ExceptionSimulatingIdSystem(LocalObjectContainer container, IExceptionFactory
			 exceptionFactory) : base(container)
		{
			_exceptionFactory = exceptionFactory;
		}

		private void ResetShutdownState()
		{
			_triggerCondition._isClosed = false;
		}

		public virtual void TriggerException(bool exception)
		{
			ResetShutdownState();
			_triggerCondition._triggersException = exception;
		}

		public virtual bool TriggersException()
		{
			return this._triggerCondition._triggersException;
		}

		public virtual bool IsClosed()
		{
			return _triggerCondition._isClosed;
		}

		public override Slot CommittedSlot(int id)
		{
			if (TriggersException())
			{
				_exceptionFactory.ThrowException();
			}
			return base.CommittedSlot(id);
		}

		public override int NewId()
		{
			if (TriggersException())
			{
				_exceptionFactory.ThrowException();
			}
			return base.NewId();
		}

		public override void Close()
		{
			base.Close();
			if (TriggersException())
			{
				_exceptionFactory.ThrowOnClose();
			}
		}

		public override void CompleteInterruptedTransaction(int transactionId1, int transactionId2
			)
		{
			if (TriggersException())
			{
				_exceptionFactory.ThrowException();
			}
			base.CompleteInterruptedTransaction(transactionId1, transactionId2);
		}

		public override void Commit(IVisitable slotChanges, FreespaceCommitter freespaceCommitter
			)
		{
			if (TriggersException())
			{
				_exceptionFactory.ThrowException();
			}
			base.Commit(slotChanges, freespaceCommitter);
		}
	}
}
