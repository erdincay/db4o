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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Common.Migration;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class Db4oMigrationSuiteBuilder : ReflectionTestSuiteBuilder
	{
		/// <summary>Runs the tests against all archived libraries + the current one</summary>
		public static readonly string[] All = null;

		/// <summary>Runs the tests against the current version only.</summary>
		/// <remarks>Runs the tests against the current version only.</remarks>
		public static readonly string[] Current = new string[0];

		private readonly Db4oLibraryEnvironmentProvider _environmentProvider = new Db4oLibraryEnvironmentProvider
			(PathProvider.TestCasePath());

		private readonly string[] _specificLibraries;

		/// <summary>
		/// Creates a suite builder for the specific FormatMigrationTestCaseBase derived classes
		/// and specific db4o libraries.
		/// </summary>
		/// <remarks>
		/// Creates a suite builder for the specific FormatMigrationTestCaseBase derived classes
		/// and specific db4o libraries. If no libraries are specified (either null or empty array)
		/// <see cref="Db4oLibrarian.Libraries()">Db4oLibrarian.Libraries()</see>
		/// is used to find archived libraries.
		/// </remarks>
		/// <param name="classes"></param>
		/// <param name="specificLibraries"></param>
		public Db4oMigrationSuiteBuilder(Type[] classes, string[] specificLibraries) : base
			(classes)
		{
			_specificLibraries = specificLibraries;
		}

		public override IEnumerator GetEnumerator()
		{
			return new Db4oMigrationSuiteBuilder.DisposingIterator(base.GetEnumerator(), _environmentProvider
				);
		}

		/// <exception cref="System.Exception"></exception>
		protected override IEnumerator FromClass(Type clazz)
		{
			AssertMigrationTestCase(clazz);
			IEnumerator defaultTestSuite = base.FromClass(clazz);
			IEnumerator migrationTestSuite = MigrationTestSuite(clazz, Db4oLibraries());
			return Iterators.Concat(migrationTestSuite, defaultTestSuite);
		}

		/// <exception cref="System.Exception"></exception>
		private IEnumerator MigrationTestSuite(Type clazz, Db4oLibrary[] libraries)
		{
			return Iterators.Map(libraries, new _IFunction4_55(this, clazz));
		}

		private sealed class _IFunction4_55 : IFunction4
		{
			public _IFunction4_55(Db4oMigrationSuiteBuilder _enclosing, Type clazz)
			{
				this._enclosing = _enclosing;
				this.clazz = clazz;
			}

			public object Apply(object library)
			{
				try
				{
					return this._enclosing.MigrationTest((Db4oLibrary)library, clazz);
				}
				catch (Exception e)
				{
					throw new Db4oException(e);
				}
			}

			private readonly Db4oMigrationSuiteBuilder _enclosing;

			private readonly Type clazz;
		}

		/// <exception cref="System.Exception"></exception>
		private Db4oMigrationSuiteBuilder.Db4oMigrationTest MigrationTest(Db4oLibrary library
			, Type clazz)
		{
			FormatMigrationTestCaseBase instance = (FormatMigrationTestCaseBase)NewInstance(clazz
				);
			return new Db4oMigrationSuiteBuilder.Db4oMigrationTest(instance, library);
		}

		/// <exception cref="System.Exception"></exception>
		private Db4oLibrary[] Db4oLibraries()
		{
			if (HasSpecificLibraries())
			{
				return SpecificLibraries();
			}
			return Librarian().Libraries();
		}

		/// <exception cref="System.Exception"></exception>
		private Db4oLibrary[] SpecificLibraries()
		{
			Db4oLibrary[] libraries = new Db4oLibrary[_specificLibraries.Length];
			for (int i = 0; i < libraries.Length; i++)
			{
				libraries[i] = Librarian().ForFile(_specificLibraries[i]);
			}
			return libraries;
		}

		private bool HasSpecificLibraries()
		{
			return null != _specificLibraries;
		}

		private Db4oLibrarian Librarian()
		{
			return new Db4oLibrarian(_environmentProvider);
		}

		private void AssertMigrationTestCase(Type clazz)
		{
			if (!typeof(FormatMigrationTestCaseBase).IsAssignableFrom(clazz))
			{
				throw new ArgumentException();
			}
		}

		private sealed class Db4oMigrationTest : ITest
		{
			private readonly FormatMigrationTestCaseBase _test;

			private readonly Db4oLibrary _library;

			private readonly string _version;

			/// <exception cref="System.Exception"></exception>
			public Db4oMigrationTest(FormatMigrationTestCaseBase test, Db4oLibrary library)
			{
				_library = library;
				_test = test;
				_version = Environment().Version();
			}

			public string Label()
			{
				return "[" + _version + "] " + _test.GetType().FullName;
			}

			public void Run()
			{
				try
				{
					CreateDatabase();
					Test();
				}
				catch (TestException e)
				{
					throw;
				}
				catch (Exception e)
				{
					throw new TestException(e);
				}
			}

			/// <exception cref="System.IO.IOException"></exception>
			private void Test()
			{
				_test.Test(_version);
			}

			/// <exception cref="System.Exception"></exception>
			private void CreateDatabase()
			{
				Environment().InvokeInstanceMethod(_test.GetType(), "createDatabaseFor", new object
					[] { _version });
			}

			private Db4oLibraryEnvironment Environment()
			{
				return _library.environment;
			}

			public bool IsLeafTest()
			{
				return true;
			}

			public ITest Transmogrify(IFunction4 fun)
			{
				return ((ITest)fun.Apply(this));
			}
		}

		private class DisposingIterator : IEnumerator
		{
			private readonly Db4oLibraryEnvironmentProvider environmentProvider;

			private readonly IEnumerator source;

			public DisposingIterator(IEnumerator source, Db4oLibraryEnvironmentProvider environmentProvider
				)
			{
				this.source = source;
				this.environmentProvider = environmentProvider;
			}

			public virtual bool MoveNext()
			{
				bool result = source.MoveNext();
				if (result == false && environmentProvider != null)
				{
					environmentProvider.DisposeAll();
				}
				return result;
			}

			public virtual object Current
			{
				get
				{
					return source.Current;
				}
			}

			public virtual void Reset()
			{
				throw new NotSupportedException("Once finished, " + GetType().FullName + " cannot be reset."
					);
			}
		}
	}
}
