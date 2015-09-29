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

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Api;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Inside
{
	public class LegacyDb4oAssemblyNameRenamingTestCase : TestWithTempFile
	{
		public void TestAssemblyNameEndingWithDb4o()
		{
			AssertNameRead("SomeType, exdb4o");
			AssertNameRead("SomeType, ex.db4o");
		}

		public void TestSimpleAssemblyName()
		{
			AssertNameRead("SomeType, some assembly");
		}

		public void TestLegacyDb4oAssemblyNames()
		{
			AssertNameRead("desktop.1, db4o-4.0-net1", "desktop.1, Db4objects.Db4o");
			AssertNameRead("cf1, db4o-4.0-compact1", "cf1, Db4objects.Db4o");
		}

		private void AssertNameRead(string name)
		{
			AssertNameRead(name, name);	
		}

		private void AssertNameRead(string originalName, string newName)
		{
			using (IObjectContainer db = Db4oEmbedded.OpenFile(TempFile()))
			{
				LocalObjectContainer localObjectContainer = (LocalObjectContainer) db;
				AssertNameRead(localObjectContainer, originalName, newName);
			}
		}

		private static void AssertNameRead(ObjectContainerBase localObjectContainer, string originalName, string newName)
		{
			byte[] originalBytes = localObjectContainer.StringIO().Write(originalName);
			byte[] updatedClassNameBytes = Platform4.UpdateClassName(originalBytes);
			Assert.AreEqual(newName, localObjectContainer.StringIO().Read(updatedClassNameBytes));
		}
	}
}

#endif