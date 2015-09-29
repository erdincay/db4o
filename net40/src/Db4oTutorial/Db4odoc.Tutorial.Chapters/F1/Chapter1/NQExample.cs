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
using System.IO;

using Db4objects.Db4o;
using Db4objects.Db4o.Query;

using Db4odoc.Tutorial;

namespace Db4odoc.Tutorial.F1.Chapter1
{
	public class NQExample : Util
	{
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
		public static void Main(string[] args)
		{
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
			{
				StorePilots(db);
				RetrieveComplexSODA(db);
				RetrieveComplexNQ(db);
				RetrieveArbitraryCodeNQ(db);
				ClearDatabase(db);
			}
		}
    
		public static void StorePilots(IObjectContainer db)
		{
			db.Store(new Pilot("Michael Schumacher", 100));
			db.Store(new Pilot("Rubens Barrichello", 99));
		}
    
		public static void RetrieveComplexSODA(IObjectContainer db)
		{
			IQuery query=db.Query();
			query.Constrain(typeof(Pilot));
			IQuery pointQuery=query.Descend("_points");
			query.Descend("_name").Constrain("Rubens Barrichello")
				.Or(pointQuery.Constrain(99).Greater()
				.And(pointQuery.Constrain(199).Smaller()));
			IObjectSet result=query.Execute();
			ListResult(result);
		}

		public static void RetrieveComplexNQ(IObjectContainer db)
		{
			IObjectSet result = db.Query(new ComplexQuery());
			ListResult(result);
		}

		public static void RetrieveArbitraryCodeNQ(IObjectContainer db)
		{
			IObjectSet result = db.Query(new ArbitraryQuery(new int[]{1,100}));
			ListResult(result);
		}
    
		public static void ClearDatabase(IObjectContainer db)
		{
			IObjectSet result = db.QueryByExample(typeof(Pilot));
			while (result.HasNext())
			{
				db.Delete(result.Next());
			}
		}
	}
}
