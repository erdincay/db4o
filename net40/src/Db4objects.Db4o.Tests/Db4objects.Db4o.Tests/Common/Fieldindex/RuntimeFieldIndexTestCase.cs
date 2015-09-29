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
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class RuntimeFieldIndexTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		private static readonly string Fieldname = "_id";

		public class Data
		{
			public int _id;

			public Data(int id)
			{
				_id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 1; i <= 3; i++)
			{
				Store(new RuntimeFieldIndexTestCase.Data(i));
			}
		}

		public virtual void TestCreateIndexAtRuntime()
		{
			IStoredField field = StoredField();
			Assert.IsFalse(field.HasIndex());
			field.CreateIndex();
			Assert.IsTrue(field.HasIndex());
			AssertQuery();
			field.CreateIndex();
		}

		// ensure that second call is ignored
		private void AssertQuery()
		{
			IQuery query = NewQuery(typeof(RuntimeFieldIndexTestCase.Data));
			query.Descend(Fieldname).Constrain(2);
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
		}

		public virtual void TestDropIndex()
		{
			IStoredField field = StoredField();
			field.CreateIndex();
			AssertQuery();
			field.DropIndex();
			Assert.IsFalse(field.HasIndex());
			AssertQuery();
		}

		private IStoredField StoredField()
		{
			return Db().StoredClass(typeof(RuntimeFieldIndexTestCase.Data)).StoredField(Fieldname
				, null);
		}
	}
}
