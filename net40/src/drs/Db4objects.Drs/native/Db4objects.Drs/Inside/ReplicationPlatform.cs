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
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Net;

namespace Db4objects.Drs.Inside
{
	/// <summary>
	/// Platform dependent code goes here to minimize manually
	/// converted code.
	/// </summary>
	public class ReplicationPlatform
	{	
		interface ICollectionInitializer
		{
			void Clear();
			void Add(object o);
		}

        public static void CopyCollectionState(object original, object destination, Db4objects.Drs.Inside.ICounterpartFinder
			 counterpartFinder)
		{
			System.Collections.IEnumerable originalCollection = (System.Collections.IEnumerable
				)original;
        	ICollectionInitializer destinationCollection = CollectionInitializerFor(destination);
			destinationCollection.Clear();

			foreach (object element in originalCollection)
			{
				object counterpart = counterpartFinder.FindCounterpart(element);
				destinationCollection.Add(counterpart);
			}
		}

		private static ICollectionInitializer CollectionInitializerFor(object destination)
		{
			if (destination is IList)
			{
				return new ListInitializer((IList) destination);
			}
			Type collectionElementType = CollectionElementTypeFor(destination);
			if (collectionElementType != null)
			{
				Type genericProtocolType = typeof(GenericCollectionInitializer<>).MakeGenericType(collectionElementType);
				return (ICollectionInitializer) Activator.CreateInstance(genericProtocolType, destination);
			}
			throw new ArgumentException("Unknown collection: " + destination);
		}

		private static Type CollectionElementTypeFor(object destination)
		{
			foreach (Type interfaceType in destination.GetType().GetInterfaces())
			{
				if (IsGenericCollection(interfaceType))
				{
					return interfaceType.GetGenericArguments()[0];
				}
			}
			return null;
		}

		private static bool IsGenericCollection(Type type)
		{
			return type.GetGenericTypeDefinition() == typeof(ICollection<>);
		}

		private class GenericCollectionInitializer<T> : ICollectionInitializer
		{
			private ICollection<T> _collection;

			public GenericCollectionInitializer(ICollection<T> collection)
			{
				_collection = collection;
			}

			public void Clear()
			{
				_collection.Clear();
			}

			public void Add(object o)
			{
				_collection.Add((T)o);
			}
		}

		private class ListInitializer : ICollectionInitializer
		{
			private readonly IList _list;

			public ListInitializer(IList list)
			{
				_list = list;
			}

			public void Clear()
			{
				_list.Clear();
			}

			public void Add(object o)
			{
				_list.Add(o);
			}
		}

		public static System.Collections.ICollection EmptyCollectionClone(ICollectionSource source, System.Collections.ICollection
			 original)
		{	
			if (original is System.Collections.ArrayList)
			{
				return new System.Collections.ArrayList(original.Count);
			}
			try
			{
				return (System.Collections.ICollection) Activator.CreateInstance(original.GetType());
			}
			catch (MissingMethodException x)
			{
				throw new ArgumentException(string.Format("Parameterless ctor required for type '{0}'", original.GetType()), x);
			}
		}
		
		public static bool IsValueType(object o)
		{
			if (null == o) return false;
			return o.GetType().IsValueType;
		}

		static readonly Type[] _nonGeneric = new Type[] 
        {
            typeof(ArrayList),
            typeof(SortedList),
            typeof(Queue),
            typeof(Stack),
        };

		static readonly Type[] _generic = new Type[] 
		{
			typeof(List<>),
			typeof(LinkedList<>),
			typeof(Stack<>),
		};

        internal static bool IsBuiltinCollectionClass(ReplicationReflector reflector, IReflectClass claxx)
        {
            Type type = NetReflector.ToNative(claxx);

			if (Contains(_nonGeneric, type)) return true;

			if (!type.IsGenericType) return false;

			if (Contains(_generic, type.GetGenericTypeDefinition())) return true;

			return false;
        }

		private static bool Contains(Type[] array, Type type)
		{
			return ((IList)array).Contains(type);
		}
    }
}

