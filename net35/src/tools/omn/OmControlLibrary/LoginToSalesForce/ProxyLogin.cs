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
using OManager.BusinessLayer.Login;
using System.Net;
using OManager.BusinessLayer.UIHelper;
using OMControlLibrary.Common;
using OME.Logging.Common;

namespace OMControlLibrary.LoginToSalesForce
{
	public partial class ProxyLogin : Form
	{
		private const string CONST_BACKSLASH = "\\";
		public ProxyLogin()
		{
			InitializeComponent();
			try
			{
				ProxyAuthentication proxy = dbInteraction.RetrieveProxyInfo();
				if (proxy != null)
				{
					textBoxUserID.Text = proxy.UserName;
					textBoxPassword.Focus();
					textBoxPort.Text = proxy.Port;
					textBoxProxy.Text = proxy.ProxyAddress;
					textBoxPassword.Text = Helper.DecryptPass(proxy.PassWord);
				}
				else
				{
					string domain = Environment.UserDomainName;
					string username = Environment.UserName;
					textBoxUserID.Text = domain + CONST_BACKSLASH + username;
					if (((WebProxy)GlobalProxySelection.Select).Address != null)
					{
						int colonIndex = ((WebProxy)GlobalProxySelection.Select).Address.ToString().LastIndexOf(':');
						string proxystr = ((WebProxy)GlobalProxySelection.Select).Address.ToString().Substring(0, colonIndex);
						string port = ((WebProxy)GlobalProxySelection.Select).Address.ToString().Substring(colonIndex + 1, ((WebProxy)GlobalProxySelection.Select).Address.ToString().Length - colonIndex - 1);
						port.TrimEnd('/');
						textBoxPassword.Text = string.Empty;
						textBoxProxy.Text = proxystr;
						textBoxPort.Text = port.Substring(0, 4);
					}
				}
			}
			catch (Exception e)
			{
				LoggingHelper.ShowMessage(e);
			}
		}




	}
}