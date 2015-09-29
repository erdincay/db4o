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
using System.Collections.Generic;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	public abstract partial class AbstractActivatableCollectionApiTestCase<TColl, TElem> : AbstractDb4oTestCase where TColl : ICollection<TElem>
	{
		protected static readonly IList<string> Names = new List<string>(new string[] {"one", "two", "three", "four"});

		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentPersistenceSupport());
		}

		protected override void Store()
		{
			CollectionHolder<TColl> item = new CollectionHolder<TColl>(NewPopulatedActivatableCollection());
			Store(item);
		}

		public void TestAdd()
		{
			AssertCollectionChange(delegate(TColl list)
			{
				list.Add( NewElement("five") );
			});
		}

		public void TestClear()
		{
			SingleCollection().Clear();
			Reopen();
			
			TColl expected = NewPopulatedPlainCollection();
			Assert.IsGreater(0, expected.Count);
			expected.Clear();
			IteratorAssert.SameContent(expected, SingleCollection());
		}

		public void TestContains()
		{
			Assert.IsTrue(SingleCollection().Contains( NewElement("one") ));
			Assert.IsFalse(SingleCollection().Contains( NewElement("five") ));
		}

		public void TestCopyTo()
		{
			TColl plainCollection = NewPopulatedPlainCollection();
			TElem[] target = new TElem[plainCollection.Count];

			TColl list = SingleCollection();
			list.CopyTo(target, 0);
			Assert.IsTrue(Db().IsActive(list));
			IteratorAssert.AreEqual(list.GetEnumerator(), target.GetEnumerator());
			IteratorAssert.AreEqual(plainCollection.GetEnumerator(), target.GetEnumerator());
		}

		public void TestIsReadOnly()
		{
			Assert.IsFalse(SingleCollection().IsReadOnly);
		}

		public void TestRemove()
		{
			AssertCollectionChange(delegate(TColl list)
			{
				list.Remove( NewElement("one") );
			});
		}

		public void TestCount()
		{
			Assert.AreEqual(NewPopulatedPlainCollection().Count, SingleCollection().Count);
		}

		protected int LastIndex()
		{
			return Names.Count * 2 - 1;
		}

		protected abstract TColl NewPlainCollection();
		protected abstract TColl SingleCollection();
		protected abstract TColl NewActivatableCollection(TColl template);
		
		protected abstract TElem NewElement(string value);
		protected abstract TElem NewActivatableElement(string value);
	}
}
