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
	public class ByteCoercionTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public byte _b;

			public Item(byte b)
			{
				_b = b;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ByteCoercionTestCase.Item((byte)42));
		}

		public virtual void TestByteCoercion()
		{
			IQuery query = NewQuery(typeof(ByteCoercionTestCase.Item));
			query.Descend("_b").Constrain(42);
			Assert.AreEqual(1, query.Execute().Count);
		}
	}
}
