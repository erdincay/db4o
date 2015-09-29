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
using Db4objects.Db4o;
using System.Collections;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.UIHelper;
using OME.Logging.Common;

namespace OManager.DataLayer.QueryParser
{
    public class RunQuery
    {
    	public static long[] ExecuteQuery(OMQuery query)
        {
            QueryParser qParser = new QueryParser(query);
            IObjectSet objSet = qParser.ExecuteOMQueryList();
            
			return objSet != null ? objSet.Ext().GetIDs() : null;
        }

        public static List<Hashtable> ReturnResults(PagingData pgData, bool refresh,string baseclass,Hashtable AttributeList)
        {
            try
            {
            	if (pgData.ObjectId.Count <= 0)
            		return null;

            	IObjectsetConverter objSetConvertor = new IObjectsetConverter(baseclass, refresh);
            	return objSetConvertor.ObjectIDToUIObjects(pgData, AttributeList);
            }
            catch (Exception oEx)
            {
                LoggingHelper.HandleException(oEx);
                return null;
            }
        }
    }
}
