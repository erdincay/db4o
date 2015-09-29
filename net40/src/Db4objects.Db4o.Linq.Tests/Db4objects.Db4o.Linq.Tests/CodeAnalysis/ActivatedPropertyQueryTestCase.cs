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
using System.Collections.Generic;
using System.Linq;

using Db4objects.Db4o;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Linq;
using Db4objects.Db4o.TA;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests.CodeAnalysis
{
	public class DoNotInstrumentAttribute : Attribute {}

	public class ActivatedPropertyQueryTestCase : AbstractDb4oLinqTestCase
	{
		[DoNotInstrument]
		public class Person : IActivatable
		{
			public string _name;
			public int _age;

			public string Name
			{
				get
				{
					Activate(ActivationPurpose.Read);
					return _name;
				}
				set { _name = value; }
			}

			public int Age
			{
				get
				{
					Activate(ActivationPurpose.Read);
					return _age;
				}
				set { _age = value; }
			}

			public void Bind(IActivator activator)
			{
			}

			public void Activate(ActivationPurpose purpose)
			{
			}

			public override bool Equals(object obj)
			{
				Person p = obj as Person;
				if (p == null) return false;

				return p._name == _name && p._age == _age;
			}

			public override int GetHashCode()
			{
				return _age ^ _name.GetHashCode();
			}
		}

		protected override void Store()
		{
			Store(new Person { Name = "Pedro", Age = 17 });
			Store(new Person { Name = "Superman", Age = 34 });
			Store(new Person { Name = "Pedro", Age = 38 });
			Store(new Person { Name = "Spiderman", Age = 24 });
		}

		public void TestActivableProperty()
		{
			AssertQuery("(Person(_name == 'Pedro'))",
				delegate
				{
					var pedros = from Person p in Db()
								 where p.Name == "Pedro"
								 select p;

					AssertSet(new[]
						{
							new Person { Name = "Pedro", Age = 17 },
							new Person { Name = "Pedro", Age = 38 },
						}, pedros);
				});
		}
	}
}
