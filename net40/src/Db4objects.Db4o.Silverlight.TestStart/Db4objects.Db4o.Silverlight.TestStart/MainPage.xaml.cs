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
using System.IO.IsolatedStorage;
using System.Threading;
using System.Windows.Browser;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;

namespace Db4objects.Db4o.Silverlight.TestStart
{
	public partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();
			ThreadPool.QueueUserWorkItem(unused => TestRunnerEntry());
		}

		private void TestRunnerEntry()
		{
			try
			{
				Type[] testCases = new[]
				                   	{
										typeof(Tests.Silverlight.AllTests),
										typeof(Linq.Tests.AllTests),
										typeof(Tests.Common.AllTests), 
										typeof(Tests.CLI1.AllTests),
										typeof(Tests.CLI2.AllTests),
				                   	};


				InitializeVersion();
				new TestRunner(SilverlightSuite(testCases)).Run(new SilverlightTestListener(Dispatcher));
				Complete();
			}
			catch(Exception ex)
			{
				AppendException(ex);
			}
		}

		private void InitializeVersion()
		{
			var version = Environment.Version.ToString();
			Dispatcher.BeginInvoke(() => HtmlPage.Window.Eval(string.Format("initialize('{0}');", version)));
		}

		private void Complete()
		{
			Dispatcher.BeginInvoke(() => HtmlPage.Window.Eval("completed();"));
		}

		private void AppendException(Exception exception)
		{
			Dispatcher.BeginInvoke(() => HtmlPage.Window.Eval("appendException(\"" + exception.ToJScriptString() + "\");"));
		}

		private static Db4oTestSuiteBuilder SilverlightSuite(params Type[] testCases)
		{
			return new Db4oTestSuiteBuilder(new SilverlightFixture(), testCases);
		}

		private void OnIncreaseDiskQuotaClick(object sender, System.Windows.RoutedEventArgs e)
		{
			var isolatedStorageManager = IsolatedStorageFile.GetUserStoreForApplication();
			isolatedStorageManager.IncreaseQuotaTo(1024*1024*100);
		}
	}
}
