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
using System.Collections;
using Db4oUnit;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class ComplexListTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			Store(A(), CreateList());
			ReplicateAndTest(A(), B());
			RoundTripTest();
		}

		private void RoundTripTest()
		{
			ChangeInProviderB();
			B().Provider().Commit();
			ReplicateAndTest(B(), A());
		}

		private void ChangeInProviderB()
		{
			SimpleListHolder simpleListHolder = (SimpleListHolder)GetOneInstance(B(), typeof(
				SimpleListHolder));
			SimpleItem fooBaby = new SimpleItem(simpleListHolder, "foobaby");
			B().Provider().StoreNew(fooBaby);
			simpleListHolder.Add(fooBaby);
			SimpleItem foo = GetItem(simpleListHolder, "foo");
			foo.SetChild(fooBaby);
			B().Provider().Update(foo);
			B().Provider().Update(simpleListHolder.GetList());
			B().Provider().Update(simpleListHolder);
		}

		private void ReplicateAndTest(IDrsProviderFixture source, IDrsProviderFixture target
			)
		{
			ReplicateAll(source.Provider(), target.Provider());
			EnsureContents(target, (SimpleListHolder)GetOneInstance(source, typeof(SimpleListHolder
				)));
		}

		private void Store(IDrsProviderFixture fixture, SimpleListHolder listHolder)
		{
			ITestableReplicationProviderInside provider = fixture.Provider();
			provider.StoreNew(listHolder);
			provider.StoreNew(GetItem(listHolder, "foo"));
			provider.StoreNew(GetItem(listHolder, "foobar"));
			provider.Commit();
			EnsureContents(fixture, listHolder);
		}

		private void EnsureContents(IDrsProviderFixture actualFixture, SimpleListHolder expected
			)
		{
			SimpleListHolder actual = (SimpleListHolder)GetOneInstance(actualFixture, typeof(
				SimpleListHolder));
			IList expectedList = expected.GetList();
			IList actualList = actual.GetList();
			AssertListWithCycles(expectedList, actualList);
		}

		private void AssertListWithCycles(IList expectedList, IList actualList)
		{
			Assert.AreEqual(expectedList.Count, actualList.Count);
			for (int i = 0; i < expectedList.Count; ++i)
			{
				SimpleItem expected = (SimpleItem)expectedList[i];
				SimpleItem actual = (SimpleItem)actualList[i];
				AssertItem(expected, actual);
			}
			AssertCycle(actualList, "foo", "bar", 1);
			AssertCycle(actualList, "foo", "foobar", 1);
			AssertCycle(actualList, "foo", "baz", 2);
		}

		private void AssertCycle(IList list, string childName, string parentName, int level
			)
		{
			SimpleItem foo = GetItem(list, childName);
			SimpleItem bar = GetItem(list, parentName);
			Assert.IsNotNull(foo);
			Assert.IsNotNull(bar);
			Assert.AreSame(foo, bar.GetChild(level));
			Assert.AreSame(foo.GetParent(), bar.GetParent());
		}

		private void AssertItem(SimpleItem expected, SimpleItem actual)
		{
			if (expected == null)
			{
				Assert.IsNull(actual);
				return;
			}
			Assert.AreEqual(expected.GetValue(), actual.GetValue());
			AssertItem(expected.GetChild(), actual.GetChild());
		}

		private SimpleItem GetItem(SimpleListHolder holder, string tbf)
		{
			return GetItem(holder.GetList(), tbf);
		}

		private SimpleItem GetItem(IList list, string tbf)
		{
			int itemIndex = list.IndexOf(new SimpleItem(tbf));
			return (SimpleItem)(itemIndex >= 0 ? list[itemIndex] : null);
		}

		public virtual SimpleListHolder CreateList()
		{
			// list : {foo, bar, baz, foobar}
			//
			// baz -----+
			//          |
			//         bar --> foo
			//                  ^
			//                  |
			// foobar ----------+
			SimpleListHolder listHolder = new SimpleListHolder("root");
			SimpleItem foo = new SimpleItem(listHolder, "foo");
			SimpleItem bar = new SimpleItem(listHolder, foo, "bar");
			listHolder.Add(foo);
			listHolder.Add(bar);
			listHolder.Add(new SimpleItem(listHolder, bar, "baz"));
			listHolder.Add(new SimpleItem(listHolder, foo, "foobar"));
			return listHolder;
		}
	}
}
