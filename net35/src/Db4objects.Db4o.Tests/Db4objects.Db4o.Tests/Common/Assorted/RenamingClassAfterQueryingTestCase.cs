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
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class RenamingClassAfterQueryingTestCase : TestWithTempFile
	{
		public virtual void TestNoQueryBeforeRenaming()
		{
			CreateData();
			QueryDb(RenameConfig(), typeof(RenamingClassAfterQueryingTestCase.A), 0);
			QueryDb(RenameConfig(), typeof(RenamingClassAfterQueryingTestCase.B), 1);
		}

		public virtual void TestQueryBeforeRenaming()
		{
			CreateData();
			QueryDb(Db4oEmbedded.NewConfiguration(), typeof(RenamingClassAfterQueryingTestCase.A
				), 1);
			QueryDb(Db4oEmbedded.NewConfiguration(), typeof(RenamingClassAfterQueryingTestCase.B
				), 0);
			QueryDb(RenameConfig(), typeof(RenamingClassAfterQueryingTestCase.A), 0);
			QueryDb(RenameConfig(), typeof(RenamingClassAfterQueryingTestCase.B), 1);
		}

		private void CreateData()
		{
			IEmbeddedObjectContainer database = Db4oEmbedded.OpenFile(TempFile());
			database.Store(new RenamingClassAfterQueryingTestCase.A("Item1"));
			database.Commit();
			database.Close();
		}

		public virtual void QueryDb(IEmbeddedConfiguration config, Type clazz, int count)
		{
			IEmbeddedObjectContainer database = Db4oEmbedded.OpenFile(config, TempFile());
			try
			{
				IList list = database.Query(clazz);
				Assert.AreEqual(count, list.Count);
			}
			finally
			{
				database.Close();
			}
		}

		private IEmbeddedConfiguration RenameConfig()
		{
			IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
			configuration.Common.ObjectClass(typeof(RenamingClassAfterQueryingTestCase.A)).Rename
				(CrossPlatformServices.FullyQualifiedName(typeof(RenamingClassAfterQueryingTestCase.B
				)));
			return configuration;
		}

		public class A
		{
			private string _name;

			public A(string name)
			{
				_name = name;
			}

			public virtual string GetName()
			{
				return _name;
			}

			public virtual void SetName(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return "Name: " + _name + " Type: " + GetType().FullName;
			}
		}

		public class B
		{
			private string _name;

			public virtual string GetName()
			{
				return _name;
			}

			public virtual void SetName(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return "Name: " + _name + " Type: " + GetType().FullName;
			}
		}
	}
}
