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

using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Api;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Silverlight.IO
{
	public class IsolatedStorageStorageTestCase : TestWithTempFile
	{
		private readonly IsolatedStorageStorage _storage = new IsolatedStorageStorage();

		public void TestDeletingNonExistingFileDontThrows()
		{
			_storage.Delete("WhoWouldNameAFileLikeThis.question-mark");
		}

		public void TestRetrievingSizeOnAOpenFileDoenstThrow()
		{
			IBin bin = _storage.Open(new BinConfiguration(TempFile(), true, 0, false));
			try
			{
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes("Test String");
				bin.Write(0, bytes, bytes.Length);

				Assert.AreEqual(bytes.Length, IsolatedStorageStorage.FileSize(TempFile()));
			}
			finally
			{
				bin.Close();
			}
		}
	}
}

#endif