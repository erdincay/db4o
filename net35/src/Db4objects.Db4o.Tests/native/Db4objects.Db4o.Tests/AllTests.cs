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
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests
{
	public class AllTests : Db4oTestSuite
	{
		public static int Main(string[] args)
		{
#if CF
			return new AllTests().RunSolo();
//            return new AllTests().RunClientServer();
#else
//			return RunInMemory();
//			return new AllTests().RunSolo();
//			return new AllTests().RunClientServer();
//			return new AllTestsConcurrency().RunConcurrencyAll();
//			return new Jre12.Collections.BigSetTestCase().RunAll();
		    return new AllTests().RunAll();
#endif
		}

		private static int RunInMemory()
		{
			return new ConsoleTestRunner(new Db4oTestSuiteBuilder(new Db4oInMemory(), typeof(AllTests))).Run();
		}
		
		protected override Type[] TestCases()
		{
			return new[]
				{	
#if CF_3_5 || NET_3_5
					typeof(Linq.Tests.AllTests),
#endif
                    typeof(Common.Migration.AllTests),
                    typeof(Common.TA.AllTests),
                    typeof(Common.AllTests),
                    typeof(Jre5.Annotation.AllTests),
                    typeof(Jre5.Collections.Typehandler.AllTests),
                    typeof(CLI1.AllTests),
                    typeof(CLI2.AllTests),
                    typeof(SharpenLang.AllTests),
                    typeof(AllTestsConcurrency),
				};
		}
	}
}
