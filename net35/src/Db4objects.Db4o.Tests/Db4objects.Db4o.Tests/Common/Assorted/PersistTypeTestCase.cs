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
#if !SILVERLIGHT
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class PersistTypeTestCase : AbstractDb4oTestCase
	{
		public sealed class Item
		{
			public Type type;

			public Item()
			{
			}

			public Item(Type type_)
			{
				type = type_;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new PersistTypeTestCase.Item(typeof(string)));
		}

		public virtual void Test()
		{
			Assert.AreEqual(typeof(string), ((PersistTypeTestCase.Item)((PersistTypeTestCase.Item
				)RetrieveOnlyInstance(typeof(PersistTypeTestCase.Item)))).type);
		}
	}
}
#endif // !SILVERLIGHT
