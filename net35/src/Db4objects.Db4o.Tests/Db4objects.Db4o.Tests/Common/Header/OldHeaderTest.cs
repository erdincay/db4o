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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Header
{
	public class OldHeaderTest : ITestLifeCycle, IOptOutNoFileSystemData, IOptOutWorkspaceIssue
	{
		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Test()
		{
			string originalFilePath = OriginalFilePath();
			string dbFilePath = DbFilePath();
			if (!System.IO.File.Exists(originalFilePath))
			{
				TestPlatform.EmitWarning(originalFilePath + " does not exist. Can not run " + GetType
					().FullName);
				return;
			}
			File4.Copy(originalFilePath, dbFilePath);
			Db4oFactory.Configure().AllowVersionUpdates(true);
			Db4oFactory.Configure().ExceptionsOnNotStorable(false);
			IObjectContainer oc = Db4oFactory.OpenFile(dbFilePath);
			try
			{
				Assert.IsNotNull(oc);
			}
			finally
			{
				oc.Close();
				Db4oFactory.Configure().ExceptionsOnNotStorable(true);
				Db4oFactory.Configure().AllowVersionUpdates(false);
			}
		}

		private static string OriginalFilePath()
		{
			return WorkspaceServices.WorkspaceTestFilePath("db4oVersions/db4o_5.5.2");
		}

		private static string DbFilePath()
		{
			return WorkspaceServices.WorkspaceTestFilePath("db4oVersions/db4o_5.5.2.db4o");
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
			string tempTestFilePath = DbFilePath();
			if (System.IO.File.Exists(tempTestFilePath))
			{
				File4.Delete(tempTestFilePath);
			}
		}
	}
}
