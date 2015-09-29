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
using System;
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Foundation.Collections;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Foundation.Collections
{
	class CollectionInitializerTestCase : ITestCase
	{
		private static readonly object[] Values = new object[] { 10, 20 };

		public void Test()
		{
			object list = new LinkedList<int>();
			ICollectionInitializer initializer = CollectionInitializer.For(list);

			Assert.IsNotNull(initializer);

			foreach(object item in Values)
			{
				initializer.Add(item);
			}

			initializer.FinishAdding();

			Iterator4Assert.AreEqual(Values, ((IEnumerable) list).GetEnumerator());
		}

		public void TestNotACollection()
		{
			Assert.Expect(
				typeof(ArgumentException), 
				delegate
					{
						CollectionInitializer.For(10);
					}
				);
		}
	}
}
