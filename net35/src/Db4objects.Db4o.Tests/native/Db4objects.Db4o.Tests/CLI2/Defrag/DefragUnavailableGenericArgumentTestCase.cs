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
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI2.Defrag
{
#if !CF && !SILVERLIGHT
	public class DefragUnavailableGenericArgumentTestCase : TestWithAppDomain
	{
		private const string DataGeneratorCode =
			@"
		using Db4objects.Db4o;
		using System.Collections.Generic;

		public class Item
		{
			public List<Thing> things;

			public Item(params Thing[] things)
			{
				this.things = new List<Thing>(things);
			}
		}

		public class Thing
		{
			private string _name;

			public Thing(string name)
			{
				_name = name;
			}
		}

		public class Program
		{
			public static void Main(string[] args)
			{
				string databaseFile = args[0];
				using (IObjectContainer container = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), databaseFile))
				{
					container.Store(new Item(new Thing(""cup"")));
				}
			}
		}
";

		public void Test()
		{
			GenerateDatabaseFile();

			DefragmentConfig config = new DefragmentConfig(TempFile());
			config.ForceBackupDelete(true);

			Exception e = Assert.Expect(typeof(InvalidOperationException), delegate
			{
				Db4objects.Db4o.Defragment.Defragment.Defrag(config);
			});
			Assert.IsTrue(e.Message.Contains("Thing"), "Message should contain the missing type name.");

		}

		private void GenerateDatabaseFile()
		{
			string assemblyFile = Path.Combine(_domain.SetupInformation.ApplicationBase, "ThingDatabaseSetup.exe");

			EmitAssembly(assemblyFile);
			ExecuteAssemblyInAppDomain(assemblyFile, TempFile());
		}

		private void EmitAssembly(string assemblyFile)
		{
			string[] references = new string[] { Db4oAssemblyPath() };
			CompilationServices.EmitAssembly(assemblyFile, references, DataGeneratorCode);
		}
	}
#endif
}
