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
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class TransactionLocalTestCase : AbstractInMemoryDb4oTestCase
	{
		internal class Item
		{
			public readonly Transaction transaction;

			public Item(Transaction transaction)
			{
				this.transaction = transaction;
			}
		}

		private sealed class _TransactionLocal_19 : TransactionLocal
		{
			public _TransactionLocal_19()
			{
			}

			public override object InitialValueFor(Transaction transaction)
			{
				return new TransactionLocalTestCase.Item(transaction);
			}
		}

		private readonly TransactionLocal _subject = new _TransactionLocal_19();

		private Transaction t1;

		private Transaction t2;

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupAfterStore()
		{
			t1 = NewTransaction();
			t2 = NewTransaction();
		}

		public virtual void TestValueRemainsTheSame()
		{
			Assert.AreSame(ItemFor(t1), ItemFor(t1));
			Assert.AreSame(ItemFor(t2), ItemFor(t2));
		}

		public virtual void TestDifferentValuesForDifferentTransactions()
		{
			Assert.AreNotSame(ItemFor(t1), ItemFor(t2));
		}

		public virtual void TestInitialValueTransaction()
		{
			Assert.AreSame(t1, ItemFor(t1).transaction);
			Assert.AreSame(t2, ItemFor(t2).transaction);
		}

		public virtual void TestValuesAreDisposedOfOnCommit()
		{
			TransactionLocalTestCase.Item itemBeforeCommit = ItemFor(t1);
			t1.Commit();
			TransactionLocalTestCase.Item itemAfterCommit = ItemFor(t1);
			Assert.AreNotSame(itemAfterCommit, itemBeforeCommit);
		}

		public virtual void TestValuesAreDisposedOfOnRollback()
		{
			TransactionLocalTestCase.Item itemBeforeRollback = ItemFor(t1);
			t1.Rollback();
			TransactionLocalTestCase.Item itemAfterRollback = ItemFor(t1);
			Assert.AreNotSame(itemAfterRollback, itemBeforeRollback);
		}

		private TransactionLocalTestCase.Item ItemFor(Transaction transaction)
		{
			return ((TransactionLocalTestCase.Item)transaction.Get(_subject).value);
		}
	}
}
