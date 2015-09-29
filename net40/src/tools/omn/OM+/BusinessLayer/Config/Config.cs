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
using System.Reflection;
using Db4objects.Db4o.Query;
using Db4objects.Db4o;
using System.IO;
using OManager.BusinessLayer.Login;
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.Connection;
using OME.Logging.Common;

namespace OManager.BusinessLayer.Config
{
	public class Config
	{
		public ISearchPath AssemblySearchPath
		{
			get
			{
				if (null == _searchPath)
				{
					_searchPath = LoadSearchPath();
				}

				return _searchPath;
			}
		}

		public static Config Instance
		{
			get { return _config; }
		}

		public bool SaveRecentConnection(RecentQueries recentQueries)
		{
			try
			{
				recentQueries.Timestamp = DateTime.Now;
				RecentQueries temprc = Db4oClient.CurrentRecentConnection.ChkIfRecentConnIsInDb();
                if (temprc != null)
                {
                    temprc.Timestamp = DateTime.Now;
                    temprc.QueryList = recentQueries.QueryList;
                	temprc.ConnParam.ConnectionReadOnly = recentQueries.ConnParam.ConnectionReadOnly;
                }
                else
                {
                    temprc = recentQueries;
                    temprc.TimeOfCreation = Sharpen.Runtime.CurrentTimeMillis(); 
                }
			    IObjectContainer dbrecentConn = Db4oClient.RecentConn;
				dbrecentConn.Store(temprc);
				dbrecentConn.Commit();
				Db4oClient.CloseRecentConnectionFile();
			}

			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				return false;
			}

			return true;
		}




		public List<RecentQueries> GetRecentQueries(bool remote)
		{
			List<RecentQueries> recConnections = null;
			try
			{
#if DEBUG
				CreateOMNDirectory();
#endif
				IObjectContainer dbrecentConn = Db4oClient.RecentConn;
				IQuery query = dbrecentConn.Query();
				query.Constrain(typeof (RecentQueries));
				if (remote)
				{
					query.Descend("m_connParam").Descend("m_host").Constrain(null).Not();
					query.Descend("m_connParam").Descend("m_port").Constrain(0).Not();
				}
				else
				{
					query.Descend("m_connParam").Descend("m_host").Constrain(null);
					query.Descend("m_connParam").Descend("m_port").Constrain(0);
				}
				IObjectSet os = query.Execute();

				if (os.Count > 0)
				{
					recConnections = new List<RecentQueries>();
					while (os.HasNext())
					{
						recConnections.Add((RecentQueries) os.Next());
					}
				}
				Db4oClient.CloseRecentConnectionFile();
			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
				Db4oClient.CloseRecentConnectionFile();
				return null;
			}
			return recConnections;

		}

		private void CreateOMNDirectory()
		{
			string omnConfigFolderPath = Path.GetDirectoryName(OMNConfigDatabasePath());
			if (!Directory.Exists(omnConfigFolderPath))
			{
				Directory.CreateDirectory(omnConfigFolderPath);
			}
		}

		public void SaveAssemblySearchPath()
		{

			PathContainer pathContainer = PathContainerFor(Db4oClient.RecentConn);
			Db4oClient.RecentConn.Delete(pathContainer.SearchPath);
			pathContainer.SearchPath = AssemblySearchPath;
			Db4oClient.RecentConn.Store(pathContainer);
			Db4oClient.CloseRecentConnectionFile();
		}

		public static string OMNConfigDatabasePath()
		{
			string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			return Path.Combine(applicationDataPath, Path.Combine("db4objects", Path.Combine("ObjectManagerEnterprise", "ObjectManagerPlus.yap")));
		}
		
		private static ISearchPath LoadSearchPath()
		{
			ISearchPath searchPath = PathContainerFor(Db4oClient.RecentConn).SearchPath;
			Db4oClient.CloseRecentConnectionFile();
			return searchPath;
		}

		private static PathContainer PathContainerFor(IObjectContainer database)
		{
			IList<PathContainer> paths = database.Query<PathContainer>();
			return paths.Count == 0 ? new PathContainer(new SearchPathImpl()) : paths[0];
		}

		private Config()
		{
		}
		
		private ISearchPath _searchPath;
		private static readonly Config _config = new Config();
	}
	
}
