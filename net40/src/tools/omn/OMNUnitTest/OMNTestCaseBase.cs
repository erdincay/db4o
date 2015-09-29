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
using System.IO;
using Db4objects.Db4o;
using NUnit.Framework;
using OManager.BusinessLayer.Login;
using OManager.DataLayer.Connection;

namespace OMNUnitTest
{
	public abstract class OMNTestCaseBase
	{
		[SetUp]
		public void Setup()
		{
			GenerateDatabase();
			SetupTest();
		}

		[TearDown]
		public void TearDown()
		{
			TearDownTest();
			IObjectContainer client = Db4oClient.Client;
			if (null != client)
			{
				string connection = Db4oClient.conn.Connection;
				Db4oClient.CloseConnection();
				File.Delete(connection);
			}
		}

		protected virtual void TearDownTest()
		{
		}

		protected virtual void SetupTest()
		{
		}

		private void GenerateDatabase()
		{
			string databaseFile = Path.GetTempFileName();
			Db4oClient.conn = new ConnParams(databaseFile,false);
			RecentQueries currRecentQueries = new RecentQueries(Db4oClient.conn);
			Db4oClient.CurrentRecentConnection = currRecentQueries;

			Store();
		}
		
		protected void Reopen()
		{
			string connection = Db4oClient.conn.Connection;
			Db4oClient.CloseConnection();
			Db4oClient.conn = new ConnParams(connection,false);
		}
		
		protected static void Store(object obj)
		{
			Db.Store(obj);
		}

		protected static void Store(object obj, int depth)
		{
			Db.Ext().Store(obj, depth);
		}

		protected static IObjectContainer Db
		{
			get
			{
				return Db4oClient.Client;
			}
		}

		protected abstract void Store();
	}
}
