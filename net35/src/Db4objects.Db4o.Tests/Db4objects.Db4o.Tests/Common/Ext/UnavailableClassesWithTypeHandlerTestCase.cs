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
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Ext;

namespace Db4objects.Db4o.Tests.Common.Ext
{
	public class UnavailableClassesWithTypeHandlerTestCase : TestWithTempFile, IOptOutNetworkingCS
		, IOptOutExcludingClassLoaderIssue
	{
		public class HolderForClassWithTypeHandler
		{
			public HolderForClassWithTypeHandler(Stack stack)
			{
				_fieldWithTypeHandler = stack;
			}

			public Stack _fieldWithTypeHandler;
		}

		public virtual void TestStoredClasses()
		{
			AssertStoredClasses(TempFile());
		}

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			base.SetUp();
			Store(TempFile(), new UnavailableClassesWithTypeHandlerTestCase.HolderForClassWithTypeHandler
				(new Stack()));
		}

		private void AssertStoredClasses(string databaseFileName)
		{
			IObjectContainer db = OpenFileExcludingStackClass(databaseFileName);
			try
			{
				Assert.IsGreater(2, db.Ext().StoredClasses().Length);
			}
			finally
			{
				db.Close();
			}
		}

		private IEmbeddedObjectContainer OpenFileExcludingStackClass(string databaseFileName
			)
		{
			return Db4oEmbedded.OpenFile(ConfigExcludingStack(), databaseFileName);
		}

		private void Store(string databaseFileName, object obj)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(), databaseFileName
				);
			try
			{
				db.Store(obj);
			}
			finally
			{
				db.Close();
			}
		}

		private IEmbeddedConfiguration ConfigExcludingStack()
		{
			return ConfigWith(new ExcludingReflector(new Type[] { typeof(Stack) }));
		}

		private IEmbeddedConfiguration ConfigWith(IReflector reflector)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.Common.ReflectWith(reflector);
			return config;
		}
	}
}
#endif // !SILVERLIGHT
