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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class PersistStaticFieldValuesTestSuite : FixtureBasedTestSuite, IDb4oTestCase
	{
		private static FixtureVariable StackDepth = new FixtureVariable("stackDepth");

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(PersistStaticFieldValuesTestSuite)).Run();
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new Db4oFixtureProvider(), new SimpleFixtureProvider
				(StackDepth, new object[] { 2, Const4.DefaultMaxStackDepth }) };
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit
				) };
		}

		public class PersistStaticFieldValuesTestUnit : AbstractDb4oTestCase
		{
			public class Data
			{
				public static readonly PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					 One = new PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					("ONE");

				public static readonly PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					 Two = new PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					("TWO");

				[System.NonSerialized]
				public static readonly PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					 Three = new PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					("THREE");

				public PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					 one;

				public PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					 two;

				public PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.PsfvHelper
					 three;
			}

			protected override void Configure(IConfiguration config)
			{
				config.ObjectClass(typeof(PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data
					)).PersistStaticFieldValues();
				config.MaxStackDepth((((int)StackDepth.Value)));
			}

			protected override void Store()
			{
				PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data psfv = new 
					PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data();
				psfv.one = PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data
					.One;
				psfv.two = PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data
					.Two;
				psfv.three = PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data
					.Three;
				Store(psfv);
			}

			public virtual void Test()
			{
				PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data psfv = (PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data
					)((PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data)RetrieveOnlyInstance
					(typeof(PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data)
					));
				Assert.AreSame(PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data
					.One, psfv.one);
				Assert.AreSame(PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data
					.Two, psfv.two);
				Assert.AreNotSame(PersistStaticFieldValuesTestSuite.PersistStaticFieldValuesTestUnit.Data
					.Three, psfv.three);
			}

			public class PsfvHelper
			{
				public string name;

				public PsfvHelper(string name) : base()
				{
					this.name = name;
				}

				public override string ToString()
				{
					// TODO Auto-generated method stub
					return "PsfvHelper[" + name + "]";
				}
			}
		}
	}
}
