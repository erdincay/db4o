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
using Db4objects.Db4o.Config;

namespace Db4odoc.Tutorial.F1.Chapter6
{
	public class DeepExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
        public static void Main(string[] args)
        {
            File.Delete(YapFileName);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                StoreCar(db);
            }
            TakeManySnapshots();
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                RetrieveAllSnapshots(db);
            }
            using(IObjectContainer db =  Db4oEmbedded.OpenFile(YapFileName))
            {
                RetrieveSnapshotsSequentially(db);
                RetrieveSnapshotsSequentiallyImproved(db);
            }
            RetrieveSnapshotsSequentiallyCascade();
        }
        
        public static void StoreCar(IObjectContainer db)
        {
            Pilot pilot = new Pilot("Rubens Barrichello", 99);
            Car car = new Car("BMW");
            car.Pilot = pilot;
            db.Store(car);
        }
        
        public static void TakeManySnapshots()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(Car)).CascadeOnUpdate(true);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                IObjectSet result = db.QueryByExample(typeof(Car));
                Car car = (Car)result.Next();
                for (int i = 0; i < 5; i++)
                {
                    car.Snapshot();
                }
                db.Store(car);
            }
        }
        
        public static void RetrieveAllSnapshots(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof(SensorReadout));
            while (result.HasNext())
            {
                Console.WriteLine(result.Next());
            }
        }
        
        public static void RetrieveSnapshotsSequentially(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof(Car));
            Car car = (Car)result.Next();
            SensorReadout readout = car.GetHistory();
            while (readout != null)
            {
                Console.WriteLine(readout);
                readout = readout.Next;
            }
        }
        
        
        public static void RetrieveSnapshotsSequentiallyCascade()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.ObjectClass(typeof(TemperatureSensorReadout))
                .CascadeOnActivate(true);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                IObjectSet result = db.QueryByExample(typeof(Car));
                Car car = (Car)result.Next();
                SensorReadout readout = car.GetHistory();
                while (readout != null)
                {
                    Console.WriteLine(readout);
                    readout = readout.Next;
                }
            }
        }
        
        public static void RetrieveSnapshotsSequentiallyImproved(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof(Car));
            Car car = (Car)result.Next();
            SensorReadout readout = car.GetHistory();
            while (readout != null)
            {
                db.Activate(readout, 1);
                Console.WriteLine(readout);
                readout = readout.Next;
            }
        }
        
    }
}
