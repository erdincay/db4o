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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Refactor
{
	public class AddIndexedValueTypeFieldTestCase : AbstractDb4oTestCase
	{
		public class Version1
		{
			public Version1(string id)
			{
				this.id = id;
			}

			public string id;
		}

		public class Version2
		{
			public Version2(string id, System.DateTime creationDate)
			{
				this.id = id;
				this.creationDate = creationDate;
			}

			public string id;

			public System.DateTime creationDate;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new AddIndexedValueTypeFieldTestCase.Version1
			      	("version1"));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			IConfiguration config = Fixture().Config();
			config.AddAlias(new TypeAlias(FullyQualifiedName(typeof(AddIndexedValueTypeFieldTestCase.Version1)), FullyQualifiedName(typeof(AddIndexedValueTypeFieldTestCase.Version2))));
			config.ObjectClass(typeof(AddIndexedValueTypeFieldTestCase.Version2)).ObjectField("creationDate").Indexed(true);
			Reopen();
			Store(new AddIndexedValueTypeFieldTestCase.Version2("version2", System.DateTime.MaxValue));
			Assert.AreEqual(2, NewQuery(typeof(AddIndexedValueTypeFieldTestCase.Version2)).Execute().Count);
		}

		private string FullyQualifiedName(System.Type clazz)
		{
			return ReflectPlatform.FullyQualifiedName(clazz);
		}
	}
}