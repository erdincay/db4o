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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class Circular1TestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new Circular1TestCase().RunConcurrency();
		}

		protected override void Store()
		{
			Store(new Circular1TestCase.C1C());
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			AssertOccurrences(oc, typeof(Circular1TestCase.C1C), 1);
		}

		public class C1P
		{
			internal Circular1TestCase.C1C c;
		}

		public class C1C : Circular1TestCase.C1P
		{
		}
	}
}
#endif // !SILVERLIGHT
