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
using Db4objects.Db4o.Foundation;
using Db4oUnit;
using Db4oUnit.Data;
using Db4oUnit.Extensions;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Collections
{
	public class GenericDictionaryTestSuite : FixtureBasedTestSuite
	{
		public override Type[] TestUnits()
		{
			return new Type[] { typeof(DictionaryTestUnit) };
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] {
				new SubjectFixtureProvider(Dictionaries(DictionaryTypes(), KeyTypes(), ValueTypes())),
			};
		}

		private IEnumerable<IDictionary> Dictionaries(IEnumerable<Type> dictionaryTypes, IEnumerable<Type> keyTypes, IEnumerable<Type> valueTypes)
		{
			foreach (IEnumerable tuple in Iterators.CrossProduct(new IEnumerable[] { dictionaryTypes, keyTypes, valueTypes }))
			{
				IEnumerator tupleEnumerator = tuple.GetEnumerator();
				Type dictionaryType = (Type) Iterators.Next(tupleEnumerator);
				Type keyType = (Type) Iterators.Next(tupleEnumerator);
				Type valueType = (Type)Iterators.Next(tupleEnumerator);
				IDictionary dictionary = (IDictionary)Activator.CreateInstance(dictionaryType.MakeGenericType(keyType, valueType));
				Populate(dictionary, UniqueValuesOf(keyType), ValuesOf(valueType));
				yield return dictionary;
			}
		}

		private IEnumerable UniqueValuesOf(Type keyType)
		{
			return Iterators.Unique(ValuesOf(keyType));
		}

		private IEnumerable ValuesOf(Type type)
		{
//			return Generators.Trace(Generators.ArbitraryValuesOf(type));
			return Generators.ArbitraryValuesOf(type);
		}

		IEnumerable<Type> KeyTypes()
		{
			yield return typeof(int);
			yield return typeof(string);
		}

		IEnumerable<Type> ValueTypes()
		{
			foreach (Type keyType in KeyTypes()) yield return keyType;
			yield return typeof(int?);
		}

		IEnumerable<Type> DictionaryTypes()
		{
			yield return typeof(Dictionary<,>);
#if !SILVERLIGHT
			yield return typeof(SortedList<,>);
#endif
#if !CF && !SILVERLIGHT
			yield return typeof(SortedDictionary<,>);
#endif
		}

		private void Populate(IDictionary subject, IEnumerable keys, IEnumerable values)
		{
			foreach (Db4o.Foundation.Tuple<object, object> entry in Iterators.Zip(keys, values))
			{
				subject.Add(entry.a, entry.b);
			}
		}

		public class DictionaryTestUnit : AbstractDb4oTestCase
		{
			public class Item
			{
				public IDictionary dictionary;

				public Item(IDictionary d)
				{
					dictionary = d;
				}
			}

			protected override void Store()
			{
				Store(new Item(Subject()));
			}

			public void Test()
			{
				IDictionary actual = RetrieveOnlyInstance<Item>().dictionary;
				Iterator4Assert.AreEqual(Subject().Values, actual.Values);
				Iterator4Assert.AreEqual(Subject().Keys, actual.Keys);
			}

			private IDictionary Subject()
			{
				return (IDictionary)SubjectFixtureProvider.Value();
			}
		}
	}
}
