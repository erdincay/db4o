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
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	public class Cat : AbstractDb4oTestCase, IOptOutSilverlight
	{
		public string name;

		public Cat()
		{
		}

		public Cat(string name)
		{
			this.name = name;
		}

		override protected void Store()
		{
			Store(new Cat("Tom"));
			Store(new Cat("Occam"));
			Store(new Cat("Fritz"));
			Store(new Cat("Garfield"));
			Store(new Cat("Zora"));
		}

		class CatComparer : IComparer
		{
			public static readonly IComparer Instance = new CatComparer();

			public int Compare(object x, object y)
			{
				return ((Cat)x).name.CompareTo(((Cat)y).name);
			}
		}

		class AllCatsPredicate : Predicate
		{
			public static readonly Predicate Instance = new AllCatsPredicate();

			public bool Match(Cat candidate)
			{
				return true;
			}
		}

		public void TestComparer()
		{
			IObjectSet result = Db().Query(AllCatsPredicate.Instance, CatComparer.Instance);
			AssertCatOrder(result, "Fritz", "Garfield", "Occam", "Tom", "Zora");
		}

		public void TestOrPredicate()
		{
			IObjectContainer objectContainer = Db();
			IObjectSet objectSet = objectContainer.Query(new OrPredicate());
			Assert.AreEqual(2, objectSet.Count);
			EnsureContains(objectSet, "Occam");
			EnsureContains(objectSet, "Zora");
		}

		public class OrPredicate : Predicate
		{
			public bool Match(Cat cat)
			{
				return cat.name == "Occam" || cat.name == "Zora";
			}
		}

		public void TestGenericPredicate()
		{
			IObjectContainer objectContainer = Db();
			System.Collections.Generic.IList<Cat> found = objectContainer.Query<Cat>(delegate(Cat c)
			{
				return c.name == "Occam" || c.name == "Zora";
			});
			Assert.AreEqual(2, found.Count);
			EnsureContains(found, "Occam");
			EnsureContains(found, "Zora");
		}

		class GenericCatComparer : System.Collections.Generic.IComparer<Cat>
		{
			public static readonly System.Collections.Generic.IComparer<Cat> Instance = new GenericCatComparer();

			public int Compare(Cat x, Cat y)
			{
				return x.name.CompareTo(y.name);
			}
		}

		public void TestGenericComparer()
		{
			System.Collections.Generic.IList<Cat> result = Db().Query(GenericCatComparer.Instance);
			AssertCatOrder(result, "Fritz", "Garfield", "Occam", "Tom", "Zora");
		}

		public void TestGenericComparison()
		{
			System.Collections.Generic.IList<Cat> result = Db().Query<Cat>(
				delegate(Cat candidate) { return true; },
				delegate(Cat x, Cat y) { return x.name.CompareTo(y.name); });
			AssertCatOrder(result, "Fritz", "Garfield", "Occam", "Tom", "Zora");
		}

		private void AssertCatOrder(IEnumerable cats, params string[] catNames)
		{
			IEnumerator e = cats.GetEnumerator();
			for (int i = 0; i < catNames.Length; ++i)
			{
				Assert.IsTrue(e.MoveNext());
				Assert.AreEqual(catNames[i], ((Cat)e.Current).name);
			}
		}

		private void EnsureContains(IEnumerable objectSet, string catName)
		{
			foreach (Cat cat in objectSet)
			{
				if (cat.name == catName) return;
			}
			Assert.Fail(catName + " expected!");
		}
	}
}
