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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class ReAddFieldTestCase : AbstractDb4oTestCase, IOptOutDefragSolo
	{
		public class Version1
		{
			public string name;

			public int id;

			public Version1(string name, int id)
			{
				this.name = name;
				this.id = id;
			}

			public Version1()
			{
			}
		}

		public class Version2
		{
			public int id;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ReAddFieldTestCase.Version1("ltuae", 42));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			TypeAlias alias = new TypeAlias(typeof(ReAddFieldTestCase.Version1), typeof(ReAddFieldTestCase.Version2
				));
			Fixture().Config().AddAlias(alias);
			Reopen();
			Assert.AreEqual(42, ((ReAddFieldTestCase.Version2)RetrieveOnlyInstance(typeof(ReAddFieldTestCase.Version2
				))).id);
			Fixture().Config().RemoveAlias(alias);
			Reopen();
			ReAddFieldTestCase.Version1 original = ((ReAddFieldTestCase.Version1)RetrieveOnlyInstance
				(typeof(ReAddFieldTestCase.Version1)));
			Assert.AreEqual("ltuae", original.name);
			Assert.AreEqual(42, original.id);
		}
	}
}
