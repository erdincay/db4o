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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.Diagnostics;

namespace Db4objects.Db4o.Tests.Common.Diagnostics
{
	public class DiagnosticsTestCase : AbstractDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Diagnostic().AddListener(new _IDiagnosticListener_15());
		}

		private sealed class _IDiagnosticListener_15 : IDiagnosticListener
		{
			public _IDiagnosticListener_15()
			{
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (!(d is NotTransparentActivationEnabled))
				{
					Assert.Fail("no diagnostic message expected but was " + d);
				}
			}
		}

		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		public virtual void TestNoDiagnosticsForInternalClasses()
		{
			Store(new DiagnosticsTestCase.Item("one"));
			RetrieveOnlyInstance(typeof(DiagnosticsTestCase.Item));
		}
	}
}
