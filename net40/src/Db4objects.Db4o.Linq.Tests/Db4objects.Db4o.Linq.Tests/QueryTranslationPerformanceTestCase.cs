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
using System.Diagnostics;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests
{
	public class QueryTranslationPerformanceTestCase : AbstractDb4oTestCase
	{
#if CF_3_5
		private const int TIME_LIMIT_WITH_FIRST_TIME_OVERHEAD = 6000;
		private const int TIME_LIMIT_NO_INITIAL_OVERHEAD = 200;
#else
		// Enough time to translate queries.. probably actual time will be shorter.
		private const int TIME_LIMIT_WITH_FIRST_TIME_OVERHEAD = 1000;
		private const int TIME_LIMIT_NO_INITIAL_OVERHEAD = 80;
#endif

		// This test only guarantee that drastic performance degradation doens't pass unnoticed.
		public void Test()
		{
			for (int i = 0; i < 3; i++)
			{
				long translationTime = TimeToTranslateQuery();

				Assert.IsSmaller(i == 0 ? TIME_LIMIT_WITH_FIRST_TIME_OVERHEAD : TIME_LIMIT_NO_INITIAL_OVERHEAD, translationTime);
			}
		}

		private long TimeToTranslateQuery()
		{
		    long start = DateTime.Now.Ticks;
            var result = from TestSubject candidate in Db()
                         where candidate.Name == "Acv"
                         select candidate;

		    long elapsedTicks = DateTime.Now.Ticks - start;
		    return TimeSpan.FromTicks(elapsedTicks).Milliseconds;
		}

		protected override void Store()
		{
			Store(new TestSubject("Acv"));
			Store(new TestSubject("Gcrav"));
		}

		public class TestSubject
		{
			public TestSubject(string name)
			{
				_name = name;	
			}

			public string Name
			{
				get { return _name; }
			}

			public readonly string _name;
		}
	}
}
