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

namespace Db4odoc.Tutorial.F1.Chapter9
{
    public class TransparentPersistenceExample : Util
    {
        readonly static string YapFileName = Path.Combine(
                               Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                               "formula1.yap");  
		
        public static void Main(String[] args)
        {
            File.Delete(YapFileName);
            StoreCarAndSnapshots();
            ModifySnapshotHistory();
            ReadSnapshotHistory();

        }

        public static void StoreCarAndSnapshots()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentPersistenceSupport());
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                Car car = new Car("Ferrari");
                for (int i = 0; i < 3; i++)
                {
                    car.snapshot();
                }
                db.Store(car);
            }
        }

        public static void ModifySnapshotHistory()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentPersistenceSupport());
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                System.Console.WriteLine("Read all sensors and modify the description:");
                IObjectSet result = db.QueryByExample(typeof(Car));
                Car car = (Car)result.Next();
                SensorReadout readout = car.History;
                while (readout != null)
                {
                    System.Console.WriteLine(readout);
                    readout.Description = "Modified: " + readout.Description;
                    readout = readout.Next;
                }
                db.Commit();
            }
        }

        public static void ReadSnapshotHistory()
        {
            IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
            config.Common.Add(new TransparentPersistenceSupport());
            using(IObjectContainer db = Db4oEmbedded.OpenFile(config, YapFileName))
            {
                System.Console.WriteLine("Read all modified sensors:");
                IObjectSet result = db.QueryByExample(typeof(Car));
                Car car = (Car)result.Next();
                SensorReadout readout = car.History;
                while (readout != null)
                {
                    System.Console.WriteLine(readout);
                    readout = readout.Next;
                }
            }
        }
    }
}