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
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Threading;
using Sharpen.Lang;

namespace Db4objects.Db4o.Tests.Common.Threading
{
	public class ThreadPoolTestCase : ITestCase
	{
		internal IThreadPool4 _subject = new ThreadPool4Impl();

		/// <exception cref="System.Exception"></exception>
		public virtual void TestFailureEvent()
		{
			ByRef executed = ByRef.NewInstance(false);
			Exception exception = new Exception();
			_subject.UncaughtException += new System.EventHandler<UncaughtExceptionEventArgs>
				(new _IEventListener4_20(exception, executed).OnEvent);
			_subject.Start(ReflectPlatform.SimpleName(GetType()) + " throwing exception thread"
				, new _IRunnable_27(exception));
			_subject.Join(1000);
			Assert.IsTrue((((bool)executed.value)));
		}

		private sealed class _IEventListener4_20
		{
			public _IEventListener4_20(Exception exception, ByRef executed)
			{
				this.exception = exception;
				this.executed = executed;
			}

			public void OnEvent(object sender, UncaughtExceptionEventArgs args)
			{
				Assert.AreSame(exception, ((UncaughtExceptionEventArgs)args).Exception);
				executed.value = true;
			}

			private readonly Exception exception;

			private readonly ByRef executed;
		}

		private sealed class _IRunnable_27 : IRunnable
		{
			public _IRunnable_27(Exception exception)
			{
				this.exception = exception;
			}

			public void Run()
			{
				throw exception;
			}

			private readonly Exception exception;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDaemon()
		{
			ByRef isDaemon = ByRef.NewInstance();
			_subject.StartLowPriority("Deamon checker", new _IRunnable_60(isDaemon));
			_subject.Join(1000);
			Assert.IsTrue((((bool)isDaemon.value)));
		}

		private sealed class _IRunnable_60 : IRunnable
		{
			public _IRunnable_60(ByRef isDaemon)
			{
				this.isDaemon = isDaemon;
			}

			public void Run()
			{
				isDaemon.value = Thread.CurrentThread().IsDaemon();
			}

			private readonly ByRef isDaemon;
		}
	}
}
