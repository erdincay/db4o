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
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Foundation.Collections;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
	internal class CollectionHelper<T> : ICollectionHelper
	{
		public void AssertCollection(object item)
		{
			AssertPlainContent(CollectionFor(item));
		}

		public void AssertPlainContent(IEnumerable actual)
		{
			IEnumerable expected = ElementSpec<T>()._elements;
			Iterator4Assert.AreEqual(expected.GetEnumerator(), actual.GetEnumerator());
		}

		public object NewItem(object element)
		{
			object item = NewItem();

			ICollectionInitializer initializer = CollectionInitializer.For(CollectionFor(item));

			initializer.Add(element);
			initializer.FinishAdding();

			return item;
		}

		public object NewItem()
		{
			object item = ItemFactory().NewItem<T>();
			Fill(CollectionFor(item), ElementSpec<T>()._elements);

			_itemType = item.GetType();

			return item;
		}

		public Type ItemType
		{
			get
			{
				if (_itemType == null)
				{
					_itemType = ItemFactory().NewItem<T>().GetType();
				}

				return _itemType;
			}
		}

		public object LargeElement
		{
			get
			{
				return ElementSpec<T>()._largeElement;
			}
		}

		public IEnumerable Elements
		{
			get
			{
				return ElementSpec<T>()._elements;
			}
		}

		public object NotContained
		{
			get
			{
				return ElementSpec<T>()._notContained;
			}
		}

		private static void Fill(ICollection collection, IEnumerable<T> elements)
		{
			ICollectionInitializer initializer = CollectionInitializer.For(collection);
			foreach (T item in elements)
			{
				initializer.Add(item);
			}

			initializer.FinishAdding();
		}

		private static ICollection CollectionFor(object item)
		{
			return (ICollection)item.GetType().GetField(GenericCollectionTestFactory.FieldName).GetValue(item);
		}

		private static GenericCollectionTestFactory ItemFactory()
		{
			return (GenericCollectionTestFactory)GenericCollectionTypeHandlerTestVariables.CollectionImplementation.Value;
		}

		private static GenericCollectionTestElementSpec<T> ElementSpec<T>()
		{
			return (GenericCollectionTestElementSpec<T>)GenericCollectionTypeHandlerTestVariables.ElementSpec.Value;
		}

		private Type _itemType;
	}

	public interface ICollectionHelper
	{
		void AssertCollection(object item);
		void AssertPlainContent(IEnumerable enumerable);

		object NewItem();
		object NewItem(object element);

		Type ItemType
		{
			get;
		}

		object LargeElement { get; }
		IEnumerable Elements { get; }
		object NotContained { get; }
	}
}
