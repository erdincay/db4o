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
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Reflect.Net
{
	public class UninitializedObjectsWithFinalizerTestCase : AbstractDb4oTestCase
	{
#if !CF
		protected override void Store()
		{
			Store(new TestSubject("Test"));

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		public void TestUninitilizedObjects()
		{
			Reopen();
			
			IList <TestSubject> result = Db().Query<TestSubject>();
				
			Assert.AreEqual(1, result.Count);
			Db().Activate(result[0], 2);
			Assert.AreEqual("Test", result[0].name);
		}
	}

	public class TestSubject
	{
		public string name;

		public TestSubject(string _name)
		{
			name = _name;
		}

		~TestSubject()
		{
            // Just access an object method...
            name = name.ToUpper();
		}
#endif
    }
}
