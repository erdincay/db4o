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
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	/// <exclude></exclude>
	public class StringIndexTestCase : StringIndexTestCaseBase, IOptOutMultiSession
	{
		public static void Main(string[] args)
		{
			new StringIndexTestCase().RunSolo();
		}

		public virtual void TestNotEquals()
		{
			Add("foo");
			Add("bar");
			Add("baz");
			Add(null);
			IQuery query = NewQuery(typeof(StringIndexTestCaseBase.Item));
			query.Descend("name").Constrain("bar").Not();
			AssertItems(new string[] { "foo", "baz", null }, query.Execute());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestCancelRemovalRollback()
		{
			PrepareCancelRemoval(Trans(), "original");
			Rename("original", "updated");
			Db().Rollback();
			GrafittiFreeSpace();
			Reopen();
			AssertExists("original");
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestCancelRemovalRollbackForMultipleTransactions()
		{
			Transaction trans1 = NewTransaction();
			Transaction trans2 = NewTransaction();
			PrepareCancelRemoval(trans1, "original");
			AssertExists(trans2, "original");
			trans1.Rollback();
			AssertExists(trans2, "original");
			Add(trans2, "second");
			AssertExists(trans2, "original");
			trans2.Commit();
			AssertExists(trans2, "original");
			GrafittiFreeSpace();
			Reopen();
			AssertExists("original");
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestCancelRemoval()
		{
			PrepareCancelRemoval(Trans(), "original");
			Db().Commit();
			GrafittiFreeSpace();
			Reopen();
			AssertExists("original");
		}

		private void PrepareCancelRemoval(Transaction transaction, string itemName)
		{
			Add(itemName);
			Db().Commit();
			Rename(transaction, itemName, "updated");
			AssertExists(transaction, "updated");
			Rename(transaction, "updated", itemName);
			AssertExists(transaction, itemName);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestCancelRemovalForMultipleTransactions()
		{
			Transaction trans1 = NewTransaction();
			Transaction trans2 = NewTransaction();
			PrepareCancelRemoval(trans1, "original");
			Rename(trans2, "original", "updated");
			trans1.Commit();
			GrafittiFreeSpace();
			Reopen();
			AssertExists("original");
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeletingAndReaddingMember()
		{
			Add("original");
			AssertExists("original");
			Rename("original", "updated");
			AssertExists("updated");
			Assert.IsNull(Query("original"));
			Reopen();
			AssertExists("updated");
			Assert.IsNull(Query("original"));
		}
	}
}
