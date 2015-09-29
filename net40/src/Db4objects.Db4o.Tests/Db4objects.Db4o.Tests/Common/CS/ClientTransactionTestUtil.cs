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
#if !SILVERLIGHT
using System.IO;
using Db4objects.Db4o.Foundation.IO;

namespace Db4objects.Db4o.Tests.Common.CS
{
	internal sealed class ClientTransactionTestUtil
	{
		internal static readonly string FilenameA = Path.GetTempFileName();

		internal static readonly string FilenameB = Path.GetTempFileName();

		public static readonly string MainfileName = Path.GetTempFileName();

		private ClientTransactionTestUtil()
		{
		}

		internal static void DeleteFiles()
		{
			File4.Delete(MainfileName);
			File4.Delete(FilenameA);
			File4.Delete(FilenameB);
		}
	}
}
#endif // !SILVERLIGHT
