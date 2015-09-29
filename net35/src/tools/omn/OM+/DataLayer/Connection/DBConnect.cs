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
using OManager.BusinessLayer.Login;
using Db4objects.Db4o;
using OME.Logging.Common;
using OME.Logging.Tracing;

namespace OManager.DataLayer.Connection
{
    class DBConnect
    {
        IObjectContainer dbConn;

        //
        public string dbConnection(ConnParams login)
        {
            try
            {
                Db4oFactory.Configure().AllowVersionUpdates(true);
                Db4oClient.conn = login;

                dbConn = Db4oClient.Client;
                return Db4oClient.exceptionConnection;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return "";
            }
        }
    }
}
