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
    public class SeachString
    {
        private DateTime m_timestamp;

        internal DateTime Timestamp
        {
            get { return m_timestamp; }
            set { m_timestamp = value; }
        }
        private string m_SearchString;

        internal string SearchString
        {
            get { return m_SearchString; }
            set { m_SearchString = value; }
        }

        public SeachString(DateTime date,string str)
        {
            m_timestamp = date;
            m_SearchString = str;
        }

    }


}
