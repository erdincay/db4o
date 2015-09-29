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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Diagnostics;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class DescendIntoTranslatorTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(DescendIntoTranslatorTestCase.Item)).Translate(new DescendIntoTranslatorTestCase.TItem
				());
			config.Diagnostic().AddListener(_collector);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new DescendIntoTranslatorTestCase.Item("foo"));
		}

		public virtual void TestDiagnostic()
		{
			IQuery query = NewQuery(typeof(DescendIntoTranslatorTestCase.Item));
			query.Descend("_name").Constrain("foo").StartsWith(true);
			query.Execute();
			IList diagnostics = NativeCollections.Filter(_collector.Diagnostics(), new _IPredicate4_36
				());
			Assert.AreEqual(1, diagnostics.Count);
			DescendIntoTranslator diagnostic = (DescendIntoTranslator)((IDiagnostic)diagnostics
				[0]);
			Assert.AreEqual(ReflectPlatform.FullyQualifiedName(typeof(DescendIntoTranslatorTestCase.Item
				)) + "." + "_name", diagnostic.Reason());
		}

		private sealed class _IPredicate4_36 : IPredicate4
		{
			public _IPredicate4_36()
			{
			}

			public bool Match(object candidate)
			{
				return ((IDiagnostic)candidate) is DescendIntoTranslator;
			}
		}

		public class Item
		{
			public Item(string name)
			{
				_name = name;
			}

			public virtual string GetName()
			{
				return _name;
			}

			public virtual void SetName(string name)
			{
				_name = name;
			}

			private string _name;
		}

		public class TItem : IObjectTranslator
		{
			public virtual void OnActivate(IObjectContainer container, object applicationObject
				, object storedObject)
			{
				DescendIntoTranslatorTestCase.Item item = (DescendIntoTranslatorTestCase.Item)applicationObject;
				item.SetName((string)storedObject);
			}

			public virtual object OnStore(IObjectContainer container, object applicationObject
				)
			{
				string name = ((DescendIntoTranslatorTestCase.Item)applicationObject).GetName();
				return name;
			}

			public virtual Type StoredClass()
			{
				return typeof(string);
			}
		}

		private DiagnosticCollector _collector = new DiagnosticCollector();
	}
}
