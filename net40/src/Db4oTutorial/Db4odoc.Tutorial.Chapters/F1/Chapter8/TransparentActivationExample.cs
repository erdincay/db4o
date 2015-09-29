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
using Db4objects.Db4o.TA;
using Db4odoc.Tutorial.F1;

namespace Db4odoc.Tutorial.F1.Chapter8
{
    public class TransparentActivationExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
        public static void Main(String[] args)
        {
            File.Delete(YapFileName);
            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                StoreCarAndSnapshots(db);
            }

            using(IObjectContainer db = Db4oEmbedded.OpenFile(YapFileName))
            {
                RetrieveSnapshotsSequentially(db);
            }

            RetrieveSnapshotsSequentiallyTA();

            DemonstrateTransparentActivation();
        }


        public static void StoreCarAndSnapshots(IObjectContainer db)
        {
            Pilot pilot = new Pilot("Kimi Raikkonen", 110);
            Car car = new Car("Ferrari");
            car.Pilot = pilot;
            for (int i = 0; i < 5; i++)
            {
                car.snapshot();
            }
            db.Store(car);
        }

        public static void RetrieveSnapshotsSequentially(IObjectContainer db)
        {
            IObjectSet result = db.QueryByExample(typeof (Car));
            Car car = (Car) result.Next();
            SensorReadout readout = car.History;
            while (readout != null)
            {
                Console.WriteLine(readout);
                readout = readout.Next;
            }
        }

        public static void RetrieveSnapshotsSequentiallyTA()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentActivationSupport());
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                IObjectSet result = db.QueryByExample(typeof(Car));
                Car car = (Car)result.Next();
                SensorReadout readout = car.History;
                while (readout != null)
                {
                    Console.WriteLine(readout);
                    readout = readout.Next;
                }
            }
        }

        public static void DemonstrateTransparentActivation()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentActivationSupport());
            
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName)){
                IObjectSet result = db.QueryByExample(typeof (Car));
                Car car = (Car) result.Next();

                Console.WriteLine("#PilotWithoutActivation before the car is activated");
                Console.WriteLine(car.PilotWithoutActivation);

                Console.WriteLine("accessing 'Pilot' property activates the car object");
                Console.WriteLine(car.Pilot);

                Console.WriteLine("Accessing PilotWithoutActivation property after the car is activated");
                Console.WriteLine(car.PilotWithoutActivation);

            }
        }
    }
}