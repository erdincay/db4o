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
using System.Reflection;

using Db4oTool;
using Db4oTool.Core;
using Db4oTool.Tests.Core;

namespace Db4objects.Db4o.Linq.Instrumentation.Tests
{
	// TODO: integrate into the build
	class Program
	{
		const string TestAssemblyFileName = "Db4objects.Db4o.Linq.Tests.exe";

		static void Main(string[] args)
		{
			var path = ExecutingAssemblyPath();
			CopyToTemp(Path.Combine(path, TestAssemblyFileName));
			CopyAllToTemp(Directory.GetFiles(path, "*.dll"));

			var testAssemblyFile = Path.Combine(GetTempPath(), TestAssemblyFileName);
			InstrumentAssembly(testAssemblyFile);

			var domain = AppDomain.CreateDomain("LinqTests", null, new AppDomainSetup { ApplicationBase = GetTempPath() });
			domain.ExecuteAssembly(testAssemblyFile);
		}

		private static string GetTempPath()
		{
			return ShellUtilities.GetTempPath();
		}

		private static void InstrumentAssembly(string testAssemblyFile)
		{
			var options = new ProgramOptions()
			{
				Target = testAssemblyFile,
				TransparentPersistence = true,
			};

			const string attribute_type = "Db4objects.Db4o.Linq.Tests.CodeAnalysis.DoNotInstrumentAttribute";

			options.Filters.Add(delegate { return new NotFilter(new ByAttributeFilter(attribute_type)); });
			Db4oTool.Program.Run(options);
		}

		private static void CopyAllToTemp(string[] files)
		{
			foreach (var fname in files)
			{
				CopyToTemp(fname);
			}
		}

		private static void CopyToTemp(string fname)
		{
			ShellUtilities.CopyToTemp(fname);
		}

		private static string ExecutingAssemblyPath()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().ManifestModule.FullyQualifiedName);
		}
	}
}
