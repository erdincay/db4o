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
using System.Threading;
using System.Windows.Forms;
using Timer=System.Threading.Timer;

namespace Db4objects.Db4o.SilverlightTestHost
{
	public partial class WebBrowerHost : Form
	{
		private string _url;
		private Timer _timer;

		public WebBrowerHost()
		{
			InitializeComponent();
		}

		public int ErrorCount { get; set; }

		public int Navigate(string url)
		{
			_url = url;
			ShowDialog();

			return ErrorCount;
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			silverlightTestHost.Navigate(_url);
			_timer = new Timer(state => Invoke((ThreadStart) CheckTestsCompletion), null, 1000, 1000);
		}

		private void CheckTestsCompletion()
		{
			HtmlElement element = silverlightTestHost.Document.GetElementById("completed");
			if (element != null)
			{
				_timer.Dispose();
				string testResults = silverlightTestHost.Document.GetElementById("result").InnerText;

				ErrorCount = Int32.Parse(ErrorCountFromHtmlPage());
				Console.Error.WriteLine(testResults);

				Close();
			}
		}

		private string ErrorCountFromHtmlPage()
		{
			return silverlightTestHost.Document.InvokeScript("getErrorCount").ToString();
		}
	}
}
