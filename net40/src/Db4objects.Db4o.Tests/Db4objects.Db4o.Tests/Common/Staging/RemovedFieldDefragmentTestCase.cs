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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class RemovedFieldDefragmentTestCase : AbstractDb4oTestCase
	{
		public class Before
		{
			public int _id;

			public Before(int id)
			{
				// COR-1740
				_id = id;
			}
		}

		public class After
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new RemovedFieldDefragmentTestCase.Before(42));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestRetrieval()
		{
			Fixture().ResetConfig();
			IConfiguration config = Fixture().Config();
			IReflector reflector = new ExcludingReflector(new Type[] { typeof(RemovedFieldDefragmentTestCase.Before
				) });
			config.ReflectWith(reflector);
			TypeAlias alias = new TypeAlias(typeof(RemovedFieldDefragmentTestCase.Before), typeof(
				RemovedFieldDefragmentTestCase.After));
			config.AddAlias(alias);
			Defragment();
			RemovedFieldDefragmentTestCase.After after = ((RemovedFieldDefragmentTestCase.After
				)RetrieveOnlyInstance(typeof(RemovedFieldDefragmentTestCase.After)));
			config = Fixture().Config();
			config.ReflectWith(new ExcludingReflector(new Type[] {  }));
			config.RemoveAlias(alias);
			Reopen();
			RemovedFieldDefragmentTestCase.Before before = ((RemovedFieldDefragmentTestCase.Before
				)RetrieveOnlyInstance(typeof(RemovedFieldDefragmentTestCase.Before)));
			Assert.AreEqual(42, before._id);
		}
	}
}
