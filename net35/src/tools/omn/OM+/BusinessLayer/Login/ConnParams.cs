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
namespace OManager.BusinessLayer.Login
{
    public class ConnParams
    {
        private readonly string m_connection;
        private readonly string m_host;
        private readonly int m_port;
        private readonly string m_userName;
        private readonly string m_passWord;
		private bool m_readonly;

		public ConnParams(string connection,bool readOnly) : this(connection, null, null, null, 0)
		{
			m_readonly = readOnly;
		}

        public ConnParams(string connection, string host, string username, string password, int port)
        {
            m_connection = connection;
            m_host = host;
            m_userName = username;
            m_passWord = password;
            m_port = port; 

        }
		public bool ConnectionReadOnly
		{
			get { return m_readonly; }
			set { m_readonly = value; }
		}
        public string Connection
        {
            get { return m_connection; }
        }

        public string Host
        {
            get { return m_host; }           

        }
        public int Port
        {
            get { return m_port; }           
        }

        public string UserName
        {
            get { return m_userName; }            
        }
        public string PassWord
        {

            get { return m_passWord; }
        }        
    }
}
