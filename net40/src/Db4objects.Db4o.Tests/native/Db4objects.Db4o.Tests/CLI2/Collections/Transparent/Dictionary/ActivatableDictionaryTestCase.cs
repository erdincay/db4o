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
using Db4objects.Db4o.Collections;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent.Dictionary
{
	public partial class ActivatableDictionaryTestCase : AbstractActivatableCollectionApiTestCase<IDictionary<string, ICollectionElement>, KeyValuePair<string, ICollectionElement>>
	{
		#region Tests for IDictionary<TKey, TValue> members

		public void TestEqualityComparerConstructor()
		{
			ActivatableDictionary<string, int> dictionary = new ActivatableDictionary<string, int>(StringComparer.OrdinalIgnoreCase);
			string key = "ten";

			dictionary[key] = 10;
			Assert.AreEqual(10, dictionary[key.ToUpper()]);
		}

		public void TestConstructorWithDictionaryAndEqualityComparer()
		{
			ActivatableDictionary<string, ICollectionElement> dictionary = new ActivatableDictionary<string, ICollectionElement>(NewPopulatedPlainCollection(), StringComparer.OrdinalIgnoreCase);
			Assert.IsTrue(dictionary.ContainsKey(ExistingKey.ToUpper()));
		}

		public void TestAdd()
		{
			AssertCollectionChange(delegate(IDictionary<string, ICollectionElement> dict)
			{
				KeyValuePair<string, ICollectionElement> element = NewElement("typed-element");
				dict.Add(element.Key, element.Value);
			});
		}
		
		public void TestSuccessfulContainsKey()
		{
			Assert.IsTrue(SingleCollection().ContainsKey(ExistingKey));
			Assert.IsTrue(NewPopulatedPlainCollection().ContainsKey(ExistingKey));
		}

		public void TestFailingContainsKey()
		{
			Assert.IsFalse(SingleCollection().ContainsKey(NonExistingKey));
			Assert.IsFalse(NewPopulatedPlainCollection().ContainsKey(NonExistingKey));
		}

		public void TestSuccessfulContains()
		{
			KeyValuePair<string, ICollectionElement> toBeFound = NewElement(ExistingKey);
			
			Assert.IsTrue(SingleCollection().Contains(toBeFound));
			Assert.IsTrue(NewPopulatedPlainCollection().Contains(toBeFound));
		}

		public void TestFailingContains()
		{
			KeyValuePair<string, ICollectionElement> nonExistingItem = NewElement(NonExistingKey);

			Assert.IsFalse(SingleCollection().Contains(nonExistingItem));
			Assert.IsFalse(NewPopulatedPlainCollection().Contains(nonExistingItem));
		}

		public void TestSuccessfulTryGetValue()
		{
			IDictionary<string, ICollectionElement> dictionary = SingleCollection();
			
			ICollectionElement item;
			Assert.IsTrue(dictionary.TryGetValue(ExistingKey, out item));
			Assert.AreEqual(NewElement(ExistingKey).Value, item);
		}
		
		public void TestFailingTryGetValue()
		{
			IDictionary<string, ICollectionElement> dictionary = SingleCollection();
			
			ICollectionElement item;
			Assert.IsFalse(dictionary.TryGetValue(NonExistingKey, out item));
			Assert.IsNull(item);
		}

		public void TestSetterIndexer()
		{
			AssertCollectionChange(delegate(IDictionary<string, ICollectionElement> dictionary)
			{
				dictionary[ExistingKey] = NewItem(ExistingKey + "-New");
			});
		}

		public void TestGetterIndexer()
		{
			IDictionary<string, ICollectionElement> dictionary = SingleCollection();
			Assert.AreEqual(NewItem(ExistingKey), SingleCollection()[ExistingKey]);
		}

		public void TestKeys()
		{
			IteratorAssert.AreEqual(NewPopulatedPlainCollection().Keys, SingleCollection().Keys);
		}

		public void TestValues()
		{
			IteratorAssert.SameContent(NewPopulatedPlainCollection().Values, SingleCollection().Values);
		}

		public void TestSuccessfulRemoveByKey()
		{
			AssertCollectionChange(delegate(IDictionary<string, ICollectionElement> dictionary)
			{
				Assert.IsTrue(dictionary.Remove(ExistingKey));
			});
		}

		public void TestFailingRemoveByKey()
		{
			AssertCollectionChange(delegate(IDictionary<string, ICollectionElement> dictionary)
			{
				Assert.IsFalse(dictionary.Remove(NonExistingKey));
			});
		}

		#endregion

		#region Tests for IDictionary members

		public void TestIDictionary_Add()
		{
			AssertCollectionChange(delegate(IDictionary<string, ICollectionElement> dict)
			{
				KeyValuePair<string, ICollectionElement> element = NewElement("typed-element");
				IDictionary nonGenericDict = (IDictionary)dict;

				nonGenericDict.Add(element.Key, element.Value);
			});
		}

		public void TestIDictionary_Remove()
		{
			AssertCollectionChange(delegate(IDictionary<string, ICollectionElement> dict)
			{
				IDictionary nonGenericDict = (IDictionary)dict;
				nonGenericDict.Remove(ExistingKey);
			});
		}
		
		public void TestIDictionary_Keys()
		{
			IDictionary actual = (IDictionary) SingleCollection();
			IDictionary expected = (IDictionary) NewPopulatedPlainCollection();
			
			IteratorAssert.AreEqual(expected.Keys, actual.Keys);
		}

		public void TestIDictionary_Values()
		{
			IDictionary actual = (IDictionary)SingleCollection();
			IDictionary expected = (IDictionary)NewPopulatedPlainCollection();
			
			IteratorAssert.AreEqual(expected.Values, actual.Values);
		}

		public void TestIDictionary_Contains()
		{
			IDictionary actual = (IDictionary)SingleCollection();
			Assert.IsTrue(actual.Contains(ExistingKey));
		}
		
		public void TestIDictionary_CopyTo()
		{
			IDictionary plainDictionary = (IDictionary) NewPopulatedPlainCollection();
			KeyValuePair<string, ICollectionElement>[] expectedlPairs = new KeyValuePair<string, ICollectionElement>[plainDictionary.Count];

			plainDictionary.CopyTo(expectedlPairs, 0);


			IDictionary actual = (IDictionary)SingleCollection();
			KeyValuePair<string, ICollectionElement>[] actualPairs = new KeyValuePair<string, ICollectionElement>[plainDictionary.Count];
			actual.CopyTo(actualPairs, 0);

			IteratorAssert.AreEqual(expectedlPairs, actualPairs);
		}

		public void TestIDictionary_GetEnumerator()
		{
			IDictionary expected = (IDictionary) NewPopulatedPlainCollection();
			IDictionary actual = (IDictionary) SingleCollection();
			IteratorAssert.AreEqual(expected.GetEnumerator(), actual.GetEnumerator());
		}

		#endregion

		#region Tests for Dictionary<TKey, TValue> members

		public void TestContainsValue()
		{
			ActivatableDictionary<string, ICollectionElement> dict = (ActivatableDictionary<string, ICollectionElement>) SingleCollection();
			Assert.IsTrue(dict.ContainsValue(NewElement(ExistingKey).Value));
			Assert.IsFalse(dict.ContainsValue(NewElement(NonExistingKey).Value));
		}

		//[Ignored("MapTypeHandler doesn't store comparer information")]
		public void _TestComparerProperty()
		{
			Store(new CollectionHolder<ActivatableDictionary<string, int>>(new ActivatableDictionary<string, int>(new MyStringComparer("foo.comparer"))));
			Reopen();
			CollectionHolder<ActivatableDictionary<string, int>>  instance = (CollectionHolder<ActivatableDictionary<string, int>>) RetrieveOnlyInstance(typeof(CollectionHolder<ActivatableDictionary<string, int>>));

			Assert.AreEqual(new MyStringComparer("foo.comparer"), instance.Collection.Comparer);
		}

#if !CF && !SILVERLIGHT
		public void TestSerialization()
		{
			AssertSerializable(SingleCollection());
			AssertSerializable(new ActivatableDictionary<string, ICollectionElement>(NewPopulatedPlainCollection()));
		}
#endif

		#endregion

		#region Tests for IEnumerable members

		public void TestIEnumerable_GetEnumerator()
		{
			IEnumerable expected = NewPopulatedPlainCollection();
			IEnumerable actual = SingleCollection();

			IteratorAssert.AreEqual(expected, actual);
		}

		#endregion

		private static readonly string ExistingKey = Names[2];
		private const string NonExistingKey = "ogro";
	}

	public class MyStringComparer : IEqualityComparer<string>
	{
		public MyStringComparer(string name)
		{
			_name = name;
		}

		#region Implementation of IEqualityComparer<string>

		public bool Equals(string x, string y)
		{
			return StringComparer.OrdinalIgnoreCase.Equals(x, y);
		}

		public int GetHashCode(string obj)
		{
			return StringComparer.OrdinalIgnoreCase.GetHashCode(obj);
		}

		private readonly string _name;

		public override bool Equals(object obj)
		{
			MyStringComparer other = obj as MyStringComparer;
			if (other == null) return false;

			return StringComparer.OrdinalIgnoreCase.Equals(_name, other._name);
		}

		#endregion
	}
}
