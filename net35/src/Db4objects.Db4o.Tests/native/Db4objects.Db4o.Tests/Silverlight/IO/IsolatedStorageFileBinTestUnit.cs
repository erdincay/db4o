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
#if SILVERLIGHT

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4oUnit;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.Silverlight.IO
{
	public class IsolatedStorageFileBinTestSuite : FixtureBasedTestSuite
	{
		public static FixtureVariable LOCK_FILE_VARIABLE = FixtureVariable.NewInstance("LockFile");
		public static FixtureVariable READONLY_VARIABLE = FixtureVariable.NewInstance("ReadOnly");

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(IsolatedStorageFileBinTestUnit), };
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[]
			       	{
			       		new SimpleFixtureProvider(LOCK_FILE_VARIABLE, new object[] {true, false}),
						new SimpleFixtureProvider(READONLY_VARIABLE, new object[]  {true, false}),
					};
		}
	}

	public class IsolatedStorageFileBinTestUnit : ITestCase
	{
		public void TestReadOnlyBin()
		{
			IBin bin = OpenBin();
			try
			{
				bin.Write(0, new byte[] {1,2,3}, 3);
			}
			catch (Db4oIOException)
			{
				_exceptionCaught = true;
				if (!IsReadOnly())
				{
					throw;
				}
			}
			finally
			{
				bin.Close();
			}
			Assert.AreEqual(IsReadOnly(), _exceptionCaught, "Exceptions are expected in ReadOnly mode.");
		}

		public void TestLockedBin()
		{
			IBin bin = OpenBin();
			try
			{
				OpenBin().Close();
			}
			catch (DatabaseFileLockedException)
			{
				_exceptionCaught = true;
				if (!IsLocked())
				{
					throw;
				}
			}
			bin.Close();
			Assert.AreEqual(IsLocked(), _exceptionCaught, "Exceptions are expected in locked mode.");
		}

		private IBin OpenBin()
		{
			return _storage.Open(new BinConfiguration("testBin", IsLocked(), 0, IsReadOnly()));
		}

		private static bool IsReadOnly()
		{
			return (bool) IsolatedStorageFileBinTestSuite.READONLY_VARIABLE.Value;
		}

		private static bool IsLocked()
		{
			return (bool) IsolatedStorageFileBinTestSuite.LOCK_FILE_VARIABLE.Value;
		}
		
		private readonly IsolatedStorageStorage _storage = new IsolatedStorageStorage();
		private bool _exceptionCaught;
	}
}

#endif