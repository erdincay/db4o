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
	public class AliasesQueryingTestCase : TestWithTempFile
	{
		public virtual void TestQuery_B_Before_A_AfterAliasing()
		{
			CreateData();
			Querydb(AliasConfig(), new int[] { 1, 1 }, typeof(AliasesQueryingTestCase.B), typeof(
				AliasesQueryingTestCase.A));
		}

		public virtual void TestQuery_A_Before_B_AfterAliasing()
		{
			CreateData();
			Querydb(AliasConfig(), new int[] { 1, 1 }, typeof(AliasesQueryingTestCase.A), typeof(
				AliasesQueryingTestCase.B));
		}

		public virtual void TestQuery()
		{
			CreateData();
			IEmbeddedObjectContainer database = Db4oEmbedded.OpenFile(AliasConfig(), TempFile
				());
			database.Query(typeof(AliasesQueryingTestCase.B));
			database.Query(typeof(AliasesQueryingTestCase.A));
			database.Close();
			Querydb(Db4oEmbedded.NewConfiguration(), new int[] { 1, 0 }, typeof(AliasesQueryingTestCase.A
				), typeof(AliasesQueryingTestCase.B));
		}

		private void CreateData()
		{
			IEmbeddedObjectContainer database = Db4oEmbedded.OpenFile(TempFile());
			database.Store(new AliasesQueryingTestCase.A("Item1"));
			database.Commit();
			database.Close();
		}

		public virtual void Querydb(IEmbeddedConfiguration config, int[] count, Type class1
			, Type class2)
		{
			IEmbeddedObjectContainer database = Db4oEmbedded.OpenFile(config, TempFile());
			try
			{
				IList list = database.Query(class1);
				Assert.AreEqual(count[0], list.Count, "Unexpected result querying for " + class1.
					Name);
				if (count[0] > 0)
				{
				}
				// System.out.println("Querying for " + class1.getSimpleName() + " getting " + list.get(0).getClass().getSimpleName());
				IList list1 = database.Query(class2);
				Assert.AreEqual(count[1], list1.Count, "Unexpected result querying for " + class2
					.Name);
				if (count[1] > 0)
				{
				}
			}
			finally
			{
				// System.out.println("Querying for " + class2.getSimpleName() + " getting " + list1.get(0).getClass().getSimpleName());
				database.Close();
			}
		}

		private IEmbeddedConfiguration AliasConfig()
		{
			IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
			configuration.Common.AddAlias(new TypeAlias(CrossPlatformServices.FullyQualifiedName
				(typeof(AliasesQueryingTestCase.A)), CrossPlatformServices.FullyQualifiedName(typeof(
				AliasesQueryingTestCase.B))));
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
