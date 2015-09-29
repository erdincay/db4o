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
using System.Collections.Generic;
using System.Text;

namespace OManager.BusinessLayer.Login
{
    public class ProxyAuthentication
    {
        string m_UserName;
        string m_ProxyAddress;
        string m_Port;
        byte[] m_pass;

        public byte[] PassWord
        {
            get { return m_pass; }
            set { m_pass = value; }
        }
       

        public string UserName
        {
            get { return m_UserName; }
            set { m_UserName = value; }
        }

        public string Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        public string ProxyAddress
        {
            get { return m_ProxyAddress; }
            set { m_ProxyAddress = value; }
        }

        public ProxyAuthentication(string username, string proxyAddr, string port, byte[] password)
        {
            this.UserName = username;
            this.ProxyAddress = proxyAddr;
            this.Port = port;
            this.PassWord = password;
        }
        public ProxyAuthentication()
        {
            this.UserName = string.Empty;
            this.ProxyAddress = string.Empty;
            this.Port = string.Empty;
           
        }
    }
}
