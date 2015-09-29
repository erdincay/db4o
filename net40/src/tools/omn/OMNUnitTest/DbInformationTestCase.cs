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
using NUnit.Framework;
using OManager.DataLayer.Modal;
using Sharpen.Lang;

namespace OMNUnitTest
{
	[TestFixture]
	public class DbInformationTestCase : OMNTestCaseBase
	{
		[Test]
		public void TestStoredClasses()
		{
			Hashtable classes = new DbInformation().StoredClasses();
			CollectionAssert.AreEqual(ClassesCollection(typeof(Item), typeof(Element), typeof(ArrayList)), classes);
		}

		[Test]
		public void TestStoredClassesByAssembly()
		{
			Hashtable classesByAssembly = new DbInformation().StoredClassesByAssembly();

			foreach (DictionaryEntry entry in ClassesCollectionByAssembly(typeof(Item), typeof(Element), typeof(ArrayList)))
			{
				Assert.IsTrue(classesByAssembly.ContainsKey(entry.Key));
				CollectionAssert.AreEqual((IEnumerable) entry.Value, (IEnumerable) classesByAssembly[entry.Key]);
			}
		}

		private static Hashtable ClassesCollectionByAssembly(params Type[] types)
		{
			Hashtable classesByAssembly = new Hashtable();
			foreach (var type in types)
			{
				string assemblyName = type.Assembly.GetName().Name;
				if (!classesByAssembly.ContainsKey(assemblyName))
				{
					classesByAssembly[assemblyName] = new List<string>();
				}

				((List<string>)classesByAssembly[assemblyName]).Add(TypeReference.FromType(type).GetUnversionedName());
			}

			return classesByAssembly;
		}

		private static IEnumerable ClassesCollection(params Type[] types)
		{
			Hashtable coll = new Hashtable();

			foreach (var type in types)
			{
				TypeReference reference = TypeReference.FromType(type);
				coll.Add(reference.GetUnversionedName(), type.FullName);
			}

			return coll;
		}

		protected override void Store()
		{
			foreach (object item in Items())
			{
				Store(item);
			}
		}

		private static IEnumerable Items()
		{
			return new object[]
				{
					new Item("foo"),
					new Element("bar"),
					new Item("baz"),
					new ArrayList(new[] {1, 2, 3})
				};
		}
	}

	public class Element : Item
	{
		public Element(string name) : base(name)
		{
		}
	}

	public class Item
	{
		private readonly string _name;

		public Item(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}
	}
}
