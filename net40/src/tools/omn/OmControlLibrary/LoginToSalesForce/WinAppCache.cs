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
using System.Windows.Forms;
using OMControlLibrary.Common;
using System.Reflection;
using EnvDTE80;

using OME.Logging.Common;

namespace OMControlLibrary.LoginToSalesForce
{
	public partial class WinAppCache : Form
	{
		readonly CustomCookies customCookies;
		public static bool isPasswordEmpty;
		public static bool isUserNameEmpty;

		public WinAppCache(DTE2 AppObj)
		{
			customCookies = new CustomCookies();

			InitializeComponent();
		}

		public WinAppCache()
		{
			customCookies = new CustomCookies();
			SetStyle(
				ControlStyles.CacheText |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.Opaque,
				true);

			InitializeComponent();
		}
		private void buttonLogin_Click(object sender, EventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(textBoxUserID.Text.Trim())
						   || string.IsNullOrEmpty(textBoxPassword.Text.Trim()))
				{
					MessageBox.Show(
						Helper.GetResourceString(Constants.VALIDATION_MSG_MANDATORY_FIELDS),
						Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						MessageBoxButtons.OK,
						MessageBoxIcon.Warning);

					if (string.IsNullOrEmpty(textBoxUserID.Text.Trim()))
					{
						textBoxUserID.Focus();
					}
					else if (string.IsNullOrEmpty(textBoxPassword.Text.Trim()))
					{
						textBoxPassword.Focus();
					}

					DialogResult = DialogResult.None;

					return;
				}

				if (checkBoxRememberMe.Checked)
				{
					string logininfo = textBoxUserID.Text + "~" + textBoxPassword.Text;
					customCookies.SetCookies(logininfo);
				}
				else
				{
					customCookies.SetCookies(string.Empty);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{

		}

		private void linkLabelForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://www.db4o.com/users/retrievePassword.aspx");
		}

		private void linkLabelPurchase_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string filepath = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);

			filepath = filepath.Remove(filepath.Length - 21, 21);
			filepath = filepath + @"/ContactSales/ContactSales.htm";
			System.Diagnostics.Process.Start(filepath);

		}

		private void textBoxUserID_TextChanged(object sender, EventArgs e)
		{
			try
			{
				lblUserName.Visible = string.IsNullOrEmpty(textBoxUserID.Text.Trim());
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void textBoxPassword_TextChanged(object sender, EventArgs e)
		{
			try
			{
				lblPassword.Visible = string.IsNullOrEmpty(textBoxPassword.Text.Trim());
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
	}
}