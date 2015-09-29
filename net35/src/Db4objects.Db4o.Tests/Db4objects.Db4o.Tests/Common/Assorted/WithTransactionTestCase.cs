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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class WithTransactionTestCase : AbstractDb4oTestCase
	{
		public virtual void Test()
		{
			Transaction originalTransaction = Container().Transaction;
			Transaction transaction = null;
			lock (Container().Lock())
			{
				transaction = Container().NewUserTransaction();
			}
			Transaction finalTransaction = transaction;
			Container().WithTransaction(transaction, new _IRunnable_19(this, finalTransaction
				));
			Assert.AreSame(originalTransaction, Container().Transaction);
		}

		private sealed class _IRunnable_19 : IRunnable
		{
			public _IRunnable_19(WithTransactionTestCase _enclosing, Transaction finalTransaction
				)
			{
				this._enclosing = _enclosing;
				this.finalTransaction = finalTransaction;
			}

			public void Run()
			{
				Assert.AreSame(finalTransaction, this._enclosing.Container().Transaction);
			}

			private readonly WithTransactionTestCase _enclosing;

			private readonly Transaction finalTransaction;
		}
	}
}
