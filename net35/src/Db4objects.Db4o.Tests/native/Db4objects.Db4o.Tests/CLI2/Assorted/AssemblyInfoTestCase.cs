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
using System.Reflection;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Assorted
{
	class AssemblyInfoTestCase : ITestCase, IOptOutSilverlight
	{
		public void Test()
		{
			Type[] assemblyReferences = new Type[]
				{
					typeof(Db4oFactory),
#if !SILVERLIGHT
					typeof(Db4objects.Db4o.Instrumentation.Api.ITypeEditor),
					typeof(Db4objects.Db4o.NativeQueries.NQOptimizer),
#endif
				};
			foreach (Type type in assemblyReferences)
			{
				AssemblyName assemblyName = type.Assembly.GetName();
				Assert.AreEqual(ExpectedVersion(), assemblyName.Version, assemblyName.FullName);
				Assert.AreNotEqual(0, assemblyName.GetPublicKeyToken().Length, assemblyName.FullName);
			}
		}

		private static Version ExpectedVersion()
		{
			return new Version(Db4oVersion.Major, Db4oVersion.Minor, Db4oVersion.Iteration, Db4oVersion.Revision);
		}
	}
}
