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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Foundation;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class EnvironmentsTestCase : ITestCase
	{
		public interface IWhatever
		{
		}

		// FIXME: db4ounit tests always run in an environment now (required to keep the test executor)
		public virtual void _testNoEnvironment()
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_14());
		}

		private sealed class _ICodeBlock_14 : ICodeBlock
		{
			public _ICodeBlock_14()
			{
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Environments.My(typeof(EnvironmentsTestCase.IWhatever));
			}
		}

		public virtual void TestRunWith()
		{
			EnvironmentsTestCase.IWhatever whatever = new _IWhatever_22();
			IEnvironment environment = new _IEnvironment_23(whatever);
			ByRef ran = ByRef.NewInstance();
			Environments.RunWith(environment, new _IRunnable_29(ran, whatever));
			Assert.IsTrue((((bool)ran.value)));
		}

		private sealed class _IWhatever_22 : EnvironmentsTestCase.IWhatever
		{
			public _IWhatever_22()
			{
			}
		}

		private sealed class _IEnvironment_23 : IEnvironment
		{
			public _IEnvironment_23(EnvironmentsTestCase.IWhatever whatever)
			{
				this.whatever = whatever;
			}

			public object Provide(Type service)
			{
				return (object)whatever;
			}

			private readonly EnvironmentsTestCase.IWhatever whatever;
		}

		private sealed class _IRunnable_29 : IRunnable
		{
			public _IRunnable_29(ByRef ran, EnvironmentsTestCase.IWhatever whatever)
			{
				this.ran = ran;
				this.whatever = whatever;
			}

			public void Run()
			{
				ran.value = true;
				Assert.AreSame(whatever, ((EnvironmentsTestCase.IWhatever)Environments.My(typeof(
					EnvironmentsTestCase.IWhatever))));
			}

			private readonly ByRef ran;

			private readonly EnvironmentsTestCase.IWhatever whatever;
		}

		public virtual void TestNestedEnvironments()
		{
			EnvironmentsTestCase.IWhatever whatever = new _IWhatever_39();
			IEnvironment environment1 = new _IEnvironment_41(whatever);
			IEnvironment environment2 = new _IEnvironment_47();
			Environments.RunWith(environment1, new _IRunnable_53(whatever, environment2));
		}

		private sealed class _IWhatever_39 : EnvironmentsTestCase.IWhatever
		{
			public _IWhatever_39()
			{
			}
		}

		private sealed class _IEnvironment_41 : IEnvironment
		{
			public _IEnvironment_41(EnvironmentsTestCase.IWhatever whatever)
			{
				this.whatever = whatever;
			}

			public object Provide(Type service)
			{
				return (object)whatever;
			}

			private readonly EnvironmentsTestCase.IWhatever whatever;
		}

		private sealed class _IEnvironment_47 : IEnvironment
		{
			public _IEnvironment_47()
			{
			}

			public object Provide(Type service)
			{
				return null;
			}
		}

		private sealed class _IRunnable_53 : IRunnable
		{
			public _IRunnable_53(EnvironmentsTestCase.IWhatever whatever, IEnvironment environment2
				)
			{
				this.whatever = whatever;
				this.environment2 = environment2;
			}

			public void Run()
			{
				Assert.AreSame(whatever, ((EnvironmentsTestCase.IWhatever)Environments.My(typeof(
					EnvironmentsTestCase.IWhatever))));
				Environments.RunWith(environment2, new _IRunnable_56());
				Assert.AreSame(whatever, ((EnvironmentsTestCase.IWhatever)Environments.My(typeof(
					EnvironmentsTestCase.IWhatever))));
			}

			private sealed class _IRunnable_56 : IRunnable
			{
				public _IRunnable_56()
				{
				}

				public void Run()
				{
					Assert.IsNull(((EnvironmentsTestCase.IWhatever)Environments.My(typeof(EnvironmentsTestCase.IWhatever
						))));
				}
			}

			private readonly EnvironmentsTestCase.IWhatever whatever;

			private readonly IEnvironment environment2;
		}
	}
}
