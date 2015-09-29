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
using Db4objects.Db4o.Collections;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent.List
{
	partial class ActivatableListTestCase
	{
		private List<ICollectionElement> NewPopulatedPlainList()
		{
			return (List<ICollectionElement>)NewPopulatedPlainCollection();
		}

		private ActivatableList<ICollectionElement> SingleActivatableCollection()
		{
			return (ActivatableList<ICollectionElement>)SingleCollection();
		}

		private List<ICollectionElement> ToBeAdded()
		{
			return NewPopulatedPlainList();
		}

		protected override IList<ICollectionElement> SingleCollection()
		{
			CollectionHolder<IList<ICollectionElement>> holder = (CollectionHolder<IList<ICollectionElement>>)RetrieveOnlyInstance(typeof(CollectionHolder<IList<ICollectionElement>>));
			return holder.Collection;
		}

		protected override IList<ICollectionElement> NewPlainCollection()
		{
			return new List<ICollectionElement>();
		}

		protected override IList<ICollectionElement> NewActivatableCollection(IList<ICollectionElement> template)
		{
			return new ActivatableList<ICollectionElement>(template);
		}

		protected override ICollectionElement NewActivatableElement(string value)
		{
			return new ActivatableElement(value);
		}

		protected ICollectionElement NewElement(int index)
		{
			return new Element(Names[index]);
		}

		protected override ICollectionElement NewElement(string value)
		{
			return new Element(value);
		}
	}

	public class SimpleComparer : IComparer<ICollectionElement>
	{
		public static SimpleComparer Instance = new SimpleComparer();

		public int Compare(ICollectionElement x, ICollectionElement y)
		{
			return x.Name.CompareTo(y.Name);
		}
	}
}
