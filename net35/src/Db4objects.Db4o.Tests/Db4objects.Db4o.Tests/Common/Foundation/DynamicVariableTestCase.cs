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
	public class DynamicVariableTestCase : ITestCase
	{
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(DynamicVariableTestCase)).Run();
		}

		public virtual void TestSingleThread()
		{
			DynamicVariable variable = new DynamicVariable();
			CheckVariableBehavior(variable);
		}

		public virtual void TestMultiThread()
		{
			DynamicVariable variable = new DynamicVariable();
			Collection4 failures = new Collection4();
			variable.With("mine", new _IRunnable_23(this, variable, failures));
			Assert.IsNull(variable.Value);
			Assert.IsTrue(failures.IsEmpty(), failures.ToString());
		}

		private sealed class _IRunnable_23 : IRunnable
		{
			public _IRunnable_23(DynamicVariableTestCase _enclosing, DynamicVariable variable
				, Collection4 failures)
			{
				this._enclosing = _enclosing;
				this.variable = variable;
				this.failures = failures;
			}

			public void Run()
			{
				Thread[] threads = this._enclosing.CreateThreads(variable, failures);
				this._enclosing.StartAll(threads);
				for (int i = 0; i < 10; ++i)
				{
					Assert.AreEqual("mine", variable.Value);
				}
				this._enclosing.JoinAll(threads);
			}

			private readonly DynamicVariableTestCase _enclosing;

			private readonly DynamicVariable variable;

			private readonly Collection4 failures;
		}

		private void JoinAll(Thread[] threads)
		{
			for (int i = 0; i < threads.Length; i++)
			{
				try
				{
					threads[i].Join();
				}
				catch (Exception e)
				{
					Sharpen.Runtime.PrintStackTrace(e);
				}
			}
		}

		private void StartAll(Thread[] threads)
		{
			for (int i = 0; i < threads.Length; i++)
			{
				threads[i].Start();
			}
		}

		private Thread[] CreateThreads(DynamicVariable variable, Collection4 failures)
		{
			Thread[] threads = new Thread[5];
			for (int i = 0; i < threads.Length; i++)
			{
				threads[i] = new Thread(new _IRunnable_56(this, variable, failures), "DynamicVariableTestCase.checkVariableBehavior Thread["
					 + i + "]");
			}
			return threads;
		}

		private sealed class _IRunnable_56 : IRunnable
		{
			public _IRunnable_56(DynamicVariableTestCase _enclosing, DynamicVariable variable
				, Collection4 failures)
			{
				this._enclosing = _enclosing;
				this.variable = variable;
				this.failures = failures;
			}

			public void Run()
			{
				try
				{
					for (int i = 0; i < 10; ++i)
					{
						this._enclosing.CheckVariableBehavior(variable);
					}
				}
				catch (Exception failure)
				{
					lock (failures)
					{
						failures.Add(failure);
					}
				}
			}

			private readonly DynamicVariableTestCase _enclosing;

			private readonly DynamicVariable variable;

			private readonly Collection4 failures;
		}

		private void CheckVariableBehavior(DynamicVariable variable)
		{
			Assert.IsNull(variable.Value);
			variable.With("foo", new _IRunnable_75(variable));
			Assert.IsNull(variable.Value);
		}

		private sealed class _IRunnable_75 : IRunnable
		{
			public _IRunnable_75(DynamicVariable variable)
			{
				this.variable = variable;
			}

			public void Run()
			{
				Assert.AreEqual("foo", variable.Value);
				variable.With("bar", new _IRunnable_78(variable));
				Assert.AreEqual("foo", variable.Value);
			}

			private sealed class _IRunnable_78 : IRunnable
			{
				public _IRunnable_78(DynamicVariable variable)
				{
					this.variable = variable;
				}

				public void Run()
				{
					Assert.AreEqual("bar", variable.Value);
				}

				private readonly DynamicVariable variable;
			}

			private readonly DynamicVariable variable;
		}
	}
}
