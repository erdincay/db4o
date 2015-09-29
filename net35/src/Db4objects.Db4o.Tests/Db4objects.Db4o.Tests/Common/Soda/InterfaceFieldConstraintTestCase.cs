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
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class InterfaceFieldConstraintTestCase : AbstractDb4oTestCase
	{
		private const int Id = 42;

		public interface IIFoo
		{
		}

		public class Foo : InterfaceFieldConstraintTestCase.IIFoo
		{
			public int _id;

			public Foo(int id)
			{
				_id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new InterfaceFieldConstraintTestCase.Foo(Id));
		}

		public virtual void TestInterfaceFieldQuery()
		{
			IQuery query = NewQuery(typeof(InterfaceFieldConstraintTestCase.IIFoo));
			query.Descend("_id").Constrain(Id);
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
