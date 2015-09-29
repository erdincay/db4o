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
using Db4objects.Db4o;
using OManager.DataLayer.Modal;
using OManager.DataLayer.Connection;
using OME.Logging.Common;

namespace OManager.DataLayer.PropertyTable
{
    public class ObjectPropertiesTable
    {
        string m_UUID;
        long m_LocalID;
        int m_Depth;
        long m_version;
        object m_detailsforObject;

        public object DetailsforObject
        {
            get { return m_detailsforObject; }
            set { m_detailsforObject = value; }
        }

        public ObjectPropertiesTable(object obj)
        {
            m_detailsforObject = obj;  
        }
        public long Version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        public string UUID
        {
            get { return m_UUID; }
            set { m_UUID = value; }
        }
        public long LocalID
        {
            get { return m_LocalID; }
            set { m_LocalID = value; }
        }
        public int Depth
        {
            get { return m_Depth; }
            set { m_Depth = value; }
        }

        public ObjectPropertiesTable GetObjectProperties()
        {
            try
            {
                IObjectContainer objContainer = Db4oClient.Client;

                ObjectDetails objDetails = new ObjectDetails(m_detailsforObject);
                UUID = objDetails.GetUUID();

                LocalID = objDetails.GetLocalID();
                Version = objDetails.GetVersion();
                return this;
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }
    }
}
