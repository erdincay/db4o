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

namespace Db4odoc.Tutorial.F1.Chapter6
{
    public class TransactionExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
        public static void Main(string[] args)
        {
            File.Delete(YapFileName);
            using (IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                StoreCarCommit(db);
            }
            using (IObjectContainer db = Db4oEmbedded.OpenFile( YapFileName))
            {
                ListAllCars(db);
                StoreCarRollback(db);
            }
            using (IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                ListAllCars(db);
                CarSnapshotRollback(db);
                CarSnapshotRollbackRefresh(db);
            }
        }
        
        public static void StoreCarCommit(IObjectContainer db)
        {
            Pilot pilot = new Pilot("Rubens Barrichello", 99);
            Car car = new Car("BMW");
            car.Pilot = pilot;
            db.Store(car);
            db.Commit();
        }
    
        public static void ListAllCars(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof(Car));
            ListResult(result);
        }
        
        public static void StoreCarRollback(IObjectContainer db)
        {
            Pilot pilot = new Pilot("Michael Schumacher", 100);
            Car car = new Car("Ferrari");
            car.Pilot = pilot;
            db.Store(car);
            db.Rollback();
        }
    
        public static void CarSnapshotRollback(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new Car("BMW"));
            Car car = (Car)result.Next();
            car.Snapshot();
            db.Store(car);
            db.Rollback();
            Console.WriteLine(car);
        }
    
        public static void CarSnapshotRollbackRefresh(IObjectContainer db)
        {
            IObjectSet result=db.QueryByExample(new Car("BMW"));
            Car car=(Car)result.Next();
            car.Snapshot();
            db.Store(car);
            db.Rollback();
            db.Ext().Refresh(car, int.MaxValue);
            Console.WriteLine(car);
        }
    }
}
