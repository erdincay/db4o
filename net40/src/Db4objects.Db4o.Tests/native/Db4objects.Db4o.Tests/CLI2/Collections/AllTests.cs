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
namespace Db4objects.Db4o.Tests.CLI2.Collections
{
	public class AllTests : Db4oUnit.Extensions.Db4oTestSuite
	{
		protected override System.Type[] TestCases()
		{
			return new System.Type[]
			{ 
                typeof(ArrayDictionary4TestCase),
                typeof(ArrayDictionary4TATestCase),
				typeof(ArrayDictionary4TransparentPersistenceTestCase),

                typeof(ArrayList4TATestCase), 
#if !SILVERLIGHT
				typeof(ArrayList4ActivatableTestCase), 
				typeof(ArrayList4TestCase), 
                typeof(BindingListTestCase),
#endif
                typeof(GenericDictionaryTestCase),
				typeof(GenericDictionaryTestSuite),

#if NET_3_5 && ! CF
                typeof(HashSetTestCase),
#endif 
				typeof(Transparent.AllTests),
            };
		}
	}
}
