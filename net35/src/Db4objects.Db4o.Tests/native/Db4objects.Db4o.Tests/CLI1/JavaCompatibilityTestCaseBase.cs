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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Extensions.Util;

namespace Db4objects.Db4o.Tests.CLI1
{
	public class JavaSnippet
	{
		public readonly string MainClassName;

		public readonly string SourceCode;

		public JavaSnippet(string mainClassName, string sourceCode)
		{
			MainClassName = mainClassName;
			SourceCode = sourceCode;
		}

		public string MainClassFile
		{
			get { return MainClassName.Replace('.', '/') + ".java";  }
		}
	}

	public abstract class JavaCompatibilityTestCaseBase : ITestCase, IOptOutSilverlight
	{
		protected abstract JavaSnippet JavaCode();

		protected abstract string ExpectedJavaOutput();

		protected abstract void PopulateContainer(IObjectContainer container);

		protected virtual IConfiguration GetConfiguration()
		{
			return Db4oFactory.NewConfiguration();
		}

		protected void RunTest()
		{
			if (!JavaServices.CanRunJavaCompatibilityTests())
			{
				return;
			}

			GenerateDataFile();
			CompileJavaSnippet();
			string output = RunJavaSnippet();
			AssertJavaOutput(output);
		}

		private void AssertJavaOutput(string output)
		{
//			Console.WriteLine(output);
			string actual = Normalize(output);
			string expected = Normalize(ExpectedJavaOutput());
			if (Contains(actual, expected)) return;

			Assert.Fail(string.Format("Expecting '{0}' got '{1}'", expected, actual));
		}

		private bool Contains(string s, string what)
		{
			return -1 != s.IndexOf(what);
		}

		private string Normalize(string output)
		{
			return output.Trim().Replace("\r\n", "\n");
		}

		private string RunJavaSnippet()
		{
			return JavaServices.java(JavaCode().MainClassName, DataFilePath());
		}

		private void CompileJavaSnippet()
		{
			JavaServices.ResetJavaTempPath();
			JavaSnippet program = JavaCode();
			string stdout = JavaServices.CompileJavaCode(program.MainClassFile, program.SourceCode);
			Console.WriteLine(stdout);
		}

		private void GenerateDataFile()
		{
			System.IO.File.Delete(DataFilePath());
			using (IObjectContainer container = Db4oFactory.OpenFile(GetConfiguration(), DataFilePath()))
			{
				PopulateContainer(container);
				container.Commit();
			}
		}

		private string DataFilePath()
		{
			return IOServices.BuildTempPath(GetType().Name + ".db4o");
		}
	}
}