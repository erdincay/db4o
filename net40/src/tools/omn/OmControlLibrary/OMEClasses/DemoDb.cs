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
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.Connection;
using OMControlLibrary.Common;
using OManager.BusinessLayer.Login;
using OME.Logging.Common;
using System.IO;

namespace OMControlLibrary
{
	public class DemoDb
	{
	    private static string demoFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "db4objects"
			+ Path.DirectorySeparatorChar + "ObjectManagerEnterprise" + Path.DirectorySeparatorChar + "DemoDb.yap";

	    public static void Create()
		{
			try
			{
				dbInteraction.CreateDemoDb(demoFilePath);
			    ConnParams conparam = new ConnParams(demoFilePath,false);
				
				RecentQueries currRecentConnection = new RecentQueries(conparam);
				RecentQueries tempRc = currRecentConnection.ChkIfRecentConnIsInDb();
				if (tempRc != null)
					currRecentConnection = tempRc;
				currRecentConnection.Timestamp = DateTime.Now;
				currRecentConnection.ConnParam = conparam;
				dbInteraction.ConnectoToDB(currRecentConnection);
				dbInteraction.SetCurrentRecentConnection(currRecentConnection);


			}
			catch (Exception e)
			{
				LoggingHelper.ShowMessage(e);
			}
		}


	}
}
