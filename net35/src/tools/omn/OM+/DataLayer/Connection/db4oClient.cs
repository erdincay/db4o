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
using System.Configuration;
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using OManager.BusinessLayer.Login;
using System.IO;
using OManager.DataLayer.Reflection;
using OME.Logging.Common;

namespace OManager.DataLayer.Connection
{
	public class Db4oClient
	{
		private static TypeResolver typeResolver;
		private static IObjectContainer objContainer;
		private static IObjectContainer userConfigDatabase;
		public static ConnParams conn;
		public static string RecentConnFile;
		public static string exceptionConnection = "";
		public static bool boolExceptionForRecentConn;
		private static bool isConnected;
		private static IEmbeddedConfiguration embeddedConfig;
		public static TypeResolver TypeResolver
		{
			get
			{
				return typeResolver;
			}
		}

		private static RecentQueries currentRecentConnection;
		public static RecentQueries CurrentRecentConnection
		{
			get
			{
				return currentRecentConnection;
			}
			set
			{
				currentRecentConnection = value;
			}
		}
       
		public static bool IsConnected
		{
			get
			{
				return isConnected ;


			}
			set
			{
				isConnected=value  ;
			}
		}

		/// <summary>
		/// Static property which either returns a new object container for a specific logon identity or returns the object container already 
		/// allocated to the logon identity.
		/// </summary>
		public static IObjectContainer Client
		{
			get
			{
				exceptionConnection = "";
				try
				{
					if (objContainer == null)
					{
						if (conn != null)
						{
							if (conn.Host != null)
							{
								objContainer=ConnectClient();
							}
							else
							{
							   if (File.Exists(conn.Connection))
                                {
									objContainer=ConnectEmbedded();
                                }
                                else
                                {
                                    exceptionConnection = "File does not exist!";
                                }
							}
							if (objContainer != null)
							{
								typeResolver = new TypeResolver(objContainer.Ext().Reflector());
								isConnected = true;
							}
						}
					}
				}
				catch (InvalidPasswordException)
				{
					exceptionConnection = "Incorrect Credentials. Please enter again.";
				}
				catch (DatabaseFileLockedException)
				{
					exceptionConnection = "Database is locked and is used by another application.";
				}
				catch (IncompatibleFileFormatException ex)
				{
					exceptionConnection = ex.Message;
				}
				catch (System.Net.Sockets.SocketException)
				{
					exceptionConnection = "No connection could be made because the target machine actively refused it.";
				}
				catch (InvalidCastException)
				{
					exceptionConnection = "Java Database is not supproted.";
				}
				catch (Exception oEx)
				{
					exceptionConnection = oEx.Message;
				}

				return objContainer;
			}

		}

		private static IObjectContainer ConnectEmbedded()
		{
			if (embeddedConfig == null)
				embeddedConfig = Db4oEmbedded.NewConfiguration();
			ConfigureCommon(embeddedConfig.Common);
			embeddedConfig.File.ReadOnly = conn.ConnectionReadOnly;
			return Db4oEmbedded.OpenFile(embeddedConfig, conn.Connection);
		}

		private static IObjectContainer  ConnectClient()
		{
			IClientConfiguration config = Db4oClientServer.NewClientConfiguration();
			ConfigureCommon(config.Common);
			return  Db4oClientServer.OpenClient(config, conn.Host, conn.Port, conn.UserName, conn.PassWord);
			
		}
		public static void SetIndex (string fieldname, string className, bool isIndexed)
		{
			if (embeddedConfig == null)
				embeddedConfig = Db4oEmbedded.NewConfiguration();
			embeddedConfig.Common.ObjectClass(className).ObjectField(fieldname).Indexed(isIndexed);
		}

		protected static void ConfigureCommon(ICommonConfiguration config)
		{
			config.Queries.EvaluationMode(QueryEvaluationMode.Lazy);
			config.ActivationDepth = 1;
			config.AllowVersionUpdates = true ;
			config.Add(new JavaSupport());
		}
		
		public static IObjectContainer RecentConn
		{
			get
			{
				try
				{
					RecentConnFile = GetOMNConfigdbPath();
					if (userConfigDatabase == null && RecentConnFile != null)
					{
						IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
						config.Common.Diagnostic.AddListener(new DiagnosticToTrace());
						config.Common.UpdateDepth = int.MaxValue;
						config.Common.ActivationDepth = int.MaxValue;
						userConfigDatabase = Db4oEmbedded.OpenFile(config,RecentConnFile);
					}
				}
				catch (Exception oEx)
				{
					LoggingHelper.HandleException(oEx);
					boolExceptionForRecentConn = true;
				}

				return userConfigDatabase;
			}

		}


		/// <summary>
		/// Static property which closes the corresponding object container for the current logon identity.
		/// </summary>
		public static void CloseConnection()
		{
			try
			{
				if (objContainer != null)
				{
					objContainer.Close();
					objContainer = null;
					isConnected = false;
					conn = null;
				}
				embeddedConfig = null;
				

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);
			}
		}

		public static void CloseRecentConnectionFile()
		{
			
			try
			{
				if (RecentConn != null)
				{
					RecentConn.Close();
					userConfigDatabase = null;
				}

			}
			catch (Exception oEx)
			{
				LoggingHelper.HandleException(oEx);

			}
		}

		private static string GetOMNConfigdbPath()
		{
			string applicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			return Path.Combine(applicationDataPath, Path.Combine("db4objects", Path.Combine("ObjectManagerEnterprise", "ObjectManagerPlus.yap")));
		}
	}
}
