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
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class CloseUnlocksFileTestCase : Db4oTestWithTempFile
	{
		[System.ObsoleteAttribute]
		public virtual void Test()
		{
			File4.Delete(TempFile());
			Assert.IsFalse(Exists(TempFile()));
			IObjectContainer oc = Db4oEmbedded.OpenFile(NewConfiguration(), TempFile());
			oc.Close();
			File4.Delete(TempFile());
			Assert.IsFalse(Exists(TempFile()));
		}

		private bool Exists(string fileName)
		{
			return new Sharpen.IO.File(fileName).Exists();
		}
	}
}
