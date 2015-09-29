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
using System.Text;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.NativeQueries.Diagnostics;

namespace Db4objects.Db4o.Tests.NativeQueries.Diagnostics
{
	public partial class NativeQueryOptimizerDiagnosticsTestCase : AbstractDb4oTestCase
	{
		private bool _failed = false;

		//	private Object _reason = null;
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(NativeQueryOptimizerDiagnosticsTestCase.Subject)).ObjectField
				("_name").Indexed(true);
			config.Diagnostic().AddListener(new _IDiagnosticListener_24(this));
		}

		private sealed class _IDiagnosticListener_24 : IDiagnosticListener
		{
			public _IDiagnosticListener_24(NativeQueryOptimizerDiagnosticsTestCase _enclosing
				)
			{
				this._enclosing = _enclosing;
			}

			public void OnDiagnostic(IDiagnostic d)
			{
				if (d.GetType() == typeof(NativeQueryNotOptimized))
				{
					//							_reason = ((NativeQueryNotOptimized) d).reason();
					this._enclosing._failed = true;
				}
			}

			private readonly NativeQueryOptimizerDiagnosticsTestCase _enclosing;
		}

		protected override void Store()
		{
			Db().Store(new NativeQueryOptimizerDiagnosticsTestCase.Subject("Test"));
			Db().Store(new NativeQueryOptimizerDiagnosticsTestCase.Subject("Test2"));
		}

		public virtual void TestNativeQueryNotOptimized()
		{
			IObjectSet items = Db().Query(new _Predicate_41());
			Assert.IsNotNull(items);
			Assert.IsTrue(_failed);
		}

		private sealed class _Predicate_41 : Predicate
		{
			public _Predicate_41()
			{
			}

			public bool Match(NativeQueryOptimizerDiagnosticsTestCase.Subject subject)
			{
				return subject.ComplexName().StartsWith("Test");
			}
		}

		private class Subject
		{
			private string _name;

			public Subject(string name)
			{
				_name = name;
			}

			public virtual string ComplexName()
			{
				StringBuilder sb = new StringBuilder(_name);
				for (int i = 0; i < 10; i++)
				{
					sb.Append(i);
				}
				return sb.ToString();
			}
		}
	}
}
