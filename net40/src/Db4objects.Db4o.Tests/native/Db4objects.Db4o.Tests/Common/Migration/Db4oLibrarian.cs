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
using System.IO;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Migration
{
	public class Db4oLibrarian
	{
		private const string MinimumVersionToTest = "6.0";
		private readonly Db4oLibraryEnvironmentProvider _provider;

		public Db4oLibrarian(Db4oLibraryEnvironmentProvider provider)
		{
			_provider = provider;
		}

		public Db4oLibrary[] Libraries()
		{
#if !SILVERLIGHT
            List <Db4oLibrary> libraries = new List<Db4oLibrary>();
			foreach (string path in Directory.GetDirectories(LibraryPath()))
			{
				// comment out the next line to run against legacy versions
				if (!IsVersionOrGreater(path, MinimumVersionToTest))
					continue;

				string db4oLib = FindLibraryFile(path);
				if (null == db4oLib) continue;
				libraries.Add(ForFile(db4oLib));
			}
			return libraries.ToArray();
#else
            throw new NotImplementedException();
#endif
		}

		private static bool IsVersionOrGreater(string path, string minimumVersion)
		{
#if !CF
			string folderName = Path.GetFileName(path);
			if (folderName == ".svn") return false;

			Version candidate = new Version(folderName);
			Version minimum = new Version(minimumVersion);

			if (candidate.Major > minimum.Major)
				return true;

			return candidate.Major == minimum.Major && candidate.Minor >= minimum.Minor;
#endif
			return false;
		}

		public static string LibraryPath()
		{
			return WorkspaceServices.WorkspacePath("db4o.archives/net-2.0");
		}

		public Db4oLibrary ForVersion(string version)
		{
			return ForFile(FindLibraryFile(Path.Combine(LibraryPath(), version)));
		}

		public Db4oLibrary ForFile(string db4oLib)
		{
			return new Db4oLibrary(db4oLib, EnvironmentFor(db4oLib));
		}

		private static string FindLibraryFile(string directory)
		{
#if !SILVERLIGHT
            string[] found = Directory.GetFiles(directory, "*.dll");
			return found.Length == 1 ? found[0] : null;
#else
            throw new NotImplementedException();
#endif
		}

		private Db4oLibraryEnvironment EnvironmentFor(string db4oLib)
		{
			return _provider.EnvironmentFor(db4oLib);
		}

    }

}