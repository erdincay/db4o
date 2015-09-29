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
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class RefactorFieldToTransientTestCase : AbstractDb4oTestCase
	{
		public class Before
		{
			public int _id;

			public Before(int id)
			{
				// COR-1721
				_id = id;
			}
		}

		public class After
		{
			[System.NonSerialized]
			public int _id;

			public After(int id)
			{
				_id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new RefactorFieldToTransientTestCase.Before(42));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestRetrieval()
		{
			Fixture().ResetConfig();
			IConfiguration config = Fixture().Config();
			IReflector reflector = new ExcludingReflector(new Type[] { typeof(RefactorFieldToTransientTestCase.Before
				) });
			config.ReflectWith(reflector);
			TypeAlias alias = new TypeAlias(typeof(RefactorFieldToTransientTestCase.Before), 
				typeof(RefactorFieldToTransientTestCase.After));
			config.AddAlias(alias);
			Reopen();
			RefactorFieldToTransientTestCase.After after = ((RefactorFieldToTransientTestCase.After
				)RetrieveOnlyInstance(typeof(RefactorFieldToTransientTestCase.After)));
			Assert.AreEqual(0, after._id);
			config = Fixture().Config();
			config.ReflectWith(new ExcludingReflector(new Type[] {  }));
			config.RemoveAlias(alias);
			Reopen();
			RefactorFieldToTransientTestCase.Before before = ((RefactorFieldToTransientTestCase.Before
				)RetrieveOnlyInstance(typeof(RefactorFieldToTransientTestCase.Before)));
			Assert.AreEqual(42, before._id);
		}
	}
}
