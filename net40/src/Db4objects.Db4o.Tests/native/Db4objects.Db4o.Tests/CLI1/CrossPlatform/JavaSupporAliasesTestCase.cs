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

using System;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.Internal;
using Db4oUnit;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	public class JavaSupporAliasesTestCase : ITestCase
	{
		public void TestCSAliases()
		{
			Config4Impl config = new Config4Impl();
			JavaSupport javaSupport = new JavaSupport();

			javaSupport.Prepare(config);

			string resolvedName = config.ResolveAliasRuntimeName(UnversionedNameFor(typeof(ClassInfo)));
			Assert.AreEqual("com.db4o.cs.internal.ClassInfo", resolvedName);
		}

		private static string UnversionedNameFor(Type type)
		{
			return TypeReference.FromType(type).GetUnversionedName();
		}
	}
}

#endif