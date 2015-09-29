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
#if !SILVERLIGHT
using Db4objects.Db4o;
using Db4objects.Db4o.CS;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.CS
{
	/// <summary>required for CsSchemaUpdateTestCase</summary>
	public class CsSchemaMigrationSourceCode
	{
		public class Item
		{
			//update
			//assert
		}

		private static readonly string File = Runtime.GetProperty("java.io.tmpdir", ".") 
			+ Sharpen.IO.File.separator + "csmig.db4o";

		private const int Port = 4447;

		/// <exception cref="System.IO.IOException"></exception>
		public static void Main(string[] arguments)
		{
			new CsSchemaMigrationSourceCode().Run();
		}

		public virtual void Run()
		{
			//store
			IServerConfiguration conf = Db4oClientServer.NewServerConfiguration();
			IObjectServer server = Db4oClientServer.OpenServer(conf, File, Port);
			server.GrantAccess("db4o", "db4o");
			//store
			//update
			//assert
			server.Close();
		}

		//assert
		private void StoreItem()
		{
			IObjectContainer client = OpenClient();
			CsSchemaMigrationSourceCode.Item item = new CsSchemaMigrationSourceCode.Item();
			client.Store(item);
			client.Close();
		}

		//store
		private void UpdateItem()
		{
			IObjectContainer client = OpenClient();
			IQuery query = client.Query();
			query.Constrain(typeof(CsSchemaMigrationSourceCode.Item));
			CsSchemaMigrationSourceCode.Item item = (CsSchemaMigrationSourceCode.Item)query.Execute
				().Next();
			//update
			//assert
			client.Store(item);
			client.Close();
		}

		private IObjectContainer OpenClient()
		{
			return Db4oClientServer.OpenClient("localhost", Port, "db4o", "db4o");
		}

		private void AssertItem()
		{
			IObjectContainer client = OpenClient();
			IQuery query = client.Query();
			query.Constrain(typeof(CsSchemaMigrationSourceCode.Item));
			CsSchemaMigrationSourceCode.Item item = (CsSchemaMigrationSourceCode.Item)query.Execute
				().Next();
			Sharpen.Runtime.Out.WriteLine(item);
			client.Close();
		}
	}
}
#endif // !SILVERLIGHT
