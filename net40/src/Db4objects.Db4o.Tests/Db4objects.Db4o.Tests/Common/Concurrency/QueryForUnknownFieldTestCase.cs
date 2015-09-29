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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class QueryForUnknownFieldTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Db4objects.Db4o.Tests.Common.Concurrency.QueryForUnknownFieldTestCase().RunConcurrency
				();
		}

		public string _name;

		public QueryForUnknownFieldTestCase()
		{
		}

		public QueryForUnknownFieldTestCase(string name)
		{
			_name = name;
		}

		protected override void Store()
		{
			_name = "name";
			Store(this);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.QueryForUnknownFieldTestCase
				));
			q.Descend("_name").Constrain("name");
			Assert.AreEqual(1, q.Execute().Count);
			q = oc.Query();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Concurrency.QueryForUnknownFieldTestCase
				));
			q.Descend("name").Constrain("name");
			Assert.AreEqual(0, q.Execute().Count);
		}
	}
}
#endif // !SILVERLIGHT
