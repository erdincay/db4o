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
using Db4objects.Db4o.Tests.Common.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class AllTests : ReflectionTestSuite
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(Db4objects.Db4o.Tests.Common.Foundation.AllTests)).Run
				();
		}

		protected override Type[] TestCases()
		{
			return new Type[] { typeof(ArrayIterator4TestCase), typeof(Arrays4TestCase), typeof(
				BitMap4TestCase), typeof(BlockingQueueTestCase), typeof(PausableBlockingQueueTestCase
				), typeof(BufferTestCase), typeof(CircularBufferTestCase), typeof(Collection4TestCase
				), typeof(CompositeIterator4TestCase), typeof(Runtime4TestCase), typeof(DynamicVariableTestCase
				), typeof(EnvironmentsTestCase), typeof(HashSet4TestCase), typeof(Hashtable4TestCase
				), typeof(IdentityHashtable4TestCase), typeof(IdentitySet4TestCase), typeof(IntArrayListTestCase
				), typeof(IntMatcherTestCase), typeof(Iterable4AdaptorTestCase), typeof(IteratorsTestCase
				), typeof(Map4TestCase), typeof(NoDuplicatesQueueTestCase), typeof(NonblockingQueueTestCase
				), typeof(ObjectPoolTestCase), typeof(Path4TestCase), typeof(SortedCollection4TestCase
				), typeof(Stack4TestCase), typeof(TimeStampIdGeneratorTestCase), typeof(TreeKeyIteratorTestCase
				), typeof(TreeNodeIteratorTestCase), typeof(TreeTestCase) };
		}
	}
}
