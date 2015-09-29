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

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	abstract partial class AbstractActivatableCollectionApiTestCase<TColl, TElem>
	{
		protected void AssertCopy(Action<TElem[]> copyAction)
		{
			TElem[] elements = new TElem[NewPopulatedPlainCollection().Count];
			copyAction(elements);

			foreach (string name in Names)
			{
				Assert.IsGreaterOrEqual(0, Array.IndexOf<TElem>(elements, NewElement(name) ));
				Assert.IsGreaterOrEqual(0, Array.IndexOf<TElem>(elements, NewActivatableElement(name) ));
			}
		}

		protected void AssertCollectionChange(Action<TColl> action)
		{
			action(SingleCollection());
			Reopen();

			TColl expected = NewPopulatedPlainCollection();
			action(expected);

			IteratorAssert.AreEqual(expected.GetEnumerator(), SingleCollection().GetEnumerator());
		}

		private TColl NewPopulatedActivatableCollection()
		{
			return NewActivatableCollection(NewPopulatedPlainCollection());
		}
		
		protected TColl NewPopulatedPlainCollection()
		{
			TColl coll = NewPlainCollection();
			return PopulateNewCollection(coll);
		}

		private TColl PopulateNewCollection(TColl coll)
		{
			foreach (string name in Names)
			{
				coll.Add(NewElement(name));
			}

			foreach (string name in Names)
			{
				coll.Add(NewActivatableElement(name));
			}

			return coll;
		}

	}
}
