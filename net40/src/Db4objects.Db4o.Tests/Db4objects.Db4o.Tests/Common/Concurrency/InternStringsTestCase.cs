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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class InternStringsTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase().RunConcurrency
				();
		}

		public string _name;

		public InternStringsTestCase() : this(null)
		{
		}

		public InternStringsTestCase(string name)
		{
			_name = name;
		}

		protected override void Configure(IConfiguration config)
		{
			config.InternStrings(true);
		}

		protected override void Store()
		{
			string name = "Foo";
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase(name));
			Store(new Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase(name));
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery query = oc.Query();
			query.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase
				));
			IObjectSet result = query.Execute();
			Assert.AreEqual(2, result.Count);
			Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase first = (Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase
				)result.Next();
			Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase second = (Db4objects.Db4o.Tests.Common.Concurrency.InternStringsTestCase
				)result.Next();
			Assert.AreSame(first._name, second._name);
		}
	}
}
#endif // !SILVERLIGHT
