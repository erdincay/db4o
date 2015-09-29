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
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Diagnostics;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class ClassHasNoFieldsTestCase : AbstractDb4oTestCase, ICustomClientServerConfiguration
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Diagnostic().AddListener(_collector);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureClient(IConfiguration config)
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConfigureServer(IConfiguration config)
		{
			Configure(config);
		}

		public virtual void TestDiagnostic()
		{
			Store(new ClassHasNoFieldsTestCase.Item());
			IList diagnostics = NativeCollections.Filter(_collector.Diagnostics(), new _IPredicate4_34
				());
			Assert.AreEqual(1, diagnostics.Count);
			ClassHasNoFields diagnostic = (ClassHasNoFields)((IDiagnostic)diagnostics[0]);
			Assert.AreEqual(ReflectPlatform.FullyQualifiedName(typeof(ClassHasNoFieldsTestCase.Item
				)), diagnostic.Reason());
		}

		private sealed class _IPredicate4_34 : IPredicate4
		{
			public _IPredicate4_34()
			{
			}

			public bool Match(object candidate)
			{
				return ((IDiagnostic)candidate) is ClassHasNoFields;
			}
		}

		private DiagnosticCollector _collector = new DiagnosticCollector();

		public class Item
		{
		}
	}
}
