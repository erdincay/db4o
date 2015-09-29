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

namespace Db4odoc.Tutorial.F1.Chapter5
{   
    public class InheritanceExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
        public static void Main(string[] args)
        {
            File.Delete(YapFileName);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                StoreFirstCar(db);
                StoreSecondCar(db);
                RetrieveTemperatureReadoutsQBE(db);
                RetrieveAllSensorReadoutsQBE(db);
                RetrieveAllSensorReadoutsQBEAlternative(db);
                RetrieveAllSensorReadoutsQuery(db);
                RetrieveAllObjects(db);
            }
        }
        
        public static void StoreFirstCar(IObjectContainer db)
        {
            Car car1 = new Car("Ferrari");
            Pilot pilot1 = new Pilot("Michael Schumacher", 100);
            car1.Pilot = pilot1;
            db.Store(car1);
        }
        
        public static void StoreSecondCar(IObjectContainer db)
        {
            Pilot pilot2 = new Pilot("Rubens Barrichello", 99);
            Car car2 = new Car("BMW");
            car2.Pilot = pilot2;
            car2.Snapshot();
            car2.Snapshot();
            db.Store(car2);
        }
        
        public static void RetrieveAllSensorReadoutsQBE(IObjectContainer db)
        {
            SensorReadout proto = new SensorReadout(DateTime.MinValue, null, null);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }
        
        public static void RetrieveTemperatureReadoutsQBE(IObjectContainer db)
        {
            SensorReadout proto = new TemperatureSensorReadout(DateTime.MinValue, null, null, 0.0);
            IObjectSet result = db.QueryByExample(proto);
            ListResult(result);
        }
        
        public static void RetrieveAllSensorReadoutsQBEAlternative(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof(SensorReadout));
            ListResult(result);
        }
        
        public static void RetrieveAllSensorReadoutsQuery(IObjectContainer db)
        {
            IQuery query = db.Query();
            query.Constrain(typeof(SensorReadout));
            IObjectSet result = query.Execute();
            ListResult(result);
        }
        
        public static void RetrieveAllObjects(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(new object());
            ListResult(result);
        }
    }
}
