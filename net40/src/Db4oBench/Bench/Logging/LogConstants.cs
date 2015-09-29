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
using Sharpen.Util;

namespace Db4objects.Db4o.Bench.Logging
{
	public class LogConstants
	{
		public static readonly string ReadEntry = "READ ";

		public static readonly string WriteEntry = "WRITE ";

		public static readonly string SyncEntry = "SYNC ";

		public static readonly string SeekEntry = "SEEK ";

		public static readonly string[] AllConstants = new string[] { ReadEntry, WriteEntry
			, SyncEntry, SeekEntry };

		public static readonly string Separator = ",";

		public static Sharpen.Util.ISet AllEntries()
		{
			HashSet entries = new HashSet();
			Sharpen.Collections.AddAll(entries, Arrays.AsList(AllConstants));
			return entries;
		}
	}
}
