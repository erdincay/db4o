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
using System.Collections.Generic;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	public class PredicateWithCasts : AbstractDb4oTestCase
	{
		private DiagnosticCollector<NativeQueryNotOptimized> diagnosticCollector;
		private static Item[] Items = new Item[] 
											{			
												new Item("Foo", 0),
												new Item("Bar", 1),
												new Item("Baz", 2),
											};

		protected override void Configure(IConfiguration config)
		{
			diagnosticCollector = new DiagnosticCollector<NativeQueryNotOptimized>();
			config.Diagnostic().AddListener(diagnosticCollector);
		}

		protected override void Store()
		{
			foreach (Item item in Items)
			{
				Store(item);
			}
		}

		public void TestSimpleCast()
		{
			object expected = 1;
			Predicate<Item> match = delegate(Item candidate)
									{
										return candidate.value == (int) expected;
									};

			AssertPredicate(match, expected);
		}

		//[Ignore("Static fields not supported.")]
		public void _TestCastInStaticPredicate()
		{
			AssertPredicate(StaticPredicate, _value);
		}

		private void AssertPredicate(Predicate<Item> predicate, object value)
		{
			IList<Item> result = Db().Query(predicate);

			Assert.AreEqual(1, result.Count);			
			Assert.AreEqual(Items[(int) value], result[0]);
			Assert.AreEqual(0, diagnosticCollector.Diagnostics.Count, diagnosticCollector.ToString());
		}

		private static bool StaticPredicate(Item candidate)
		{
			return candidate.value == (int) _value;
		}

		private static readonly object _value = 1;
	}

	class Item
	{
		public int value;
		public string name;

		public Item(string name, int value)
		{
			this.value = value;
			this.name = name;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (obj.GetType() != GetType())
				return false;

			Item other = (Item) obj;
			return other.name.Equals(name) && other.value == value;
		}

		public override string ToString()
		{
			return "Item(" + name + ", " + value + ")";
		}
	}
}
