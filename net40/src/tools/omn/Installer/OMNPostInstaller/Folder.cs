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
using System.IO;

namespace OMNPostInstaller
{
	internal class Folder
	{
		public static readonly string DB4OHome = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "db4objects");
		public static readonly string OMNHome = Path.Combine(DB4OHome, "ObjectManagerEnterprise");

		public static void Delete(string path)
		{
			if (Directory.Exists(path))
			{
				foreach (string dir in Directory.GetDirectories(path))
				{
					Delete(dir);
				}

				foreach (string file in Directory.GetFiles(path))
				{
					File.Delete(file);
				}

				Directory.Delete(path);
			}
		}
	}
}
