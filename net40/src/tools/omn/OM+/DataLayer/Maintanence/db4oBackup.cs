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
using OManager.DataLayer.Connection;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using OME.Logging.Common;
using OME.Logging.Tracing;

namespace OManager.DataLayer.Maintanence
{
    public class db4oBackup
    {
        private string m_newbackupLocation;
        IObjectContainer objectContainer;

        public db4oBackup(string Location)
        {
            objectContainer = Db4oClient.Client; 
            m_newbackupLocation = Location;
        }
        public void db4oBackupDatabase()
        {
            objectContainer.Ext().Backup(m_newbackupLocation);
                       
        }

    }
}
