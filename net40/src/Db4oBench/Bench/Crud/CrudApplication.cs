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
using Db4objects.Db4o;
using Db4objects.Db4o.Bench.Crud;
using Db4objects.Db4o.Bench.Logging;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Bench.Crud
{
	/// <summary>
	/// Very simple CRUD (Create, Read, Update, Delete) application to
	/// produce log files as an input for I/O-benchmarking.
	/// </summary>
	/// <remarks>
	/// Very simple CRUD (Create, Read, Update, Delete) application to
	/// produce log files as an input for I/O-benchmarking.
	/// </remarks>
	public class CrudApplication
	{
		private static readonly string DatabaseFile = "simplecrud.db4o";

		public virtual void Run(int itemCount)
		{
			DeleteDbFile();
			Create(itemCount, NewConfiguration(itemCount));
			Read(NewConfiguration(itemCount));
			Update(NewConfiguration(itemCount));
			Delete(NewConfiguration(itemCount));
			DeleteDbFile();
		}

		private void Create(int itemCount, IConfiguration config)
		{
			IObjectContainer oc = Open(config);
			for (int i = 0; i < itemCount; i++)
			{
				oc.Store(Item.NewItem(i));
				// preventing heap space problems by committing from time to time
				if (i % 100000 == 0)
				{
					oc.Commit();
				}
			}
			oc.Commit();
			oc.Close();
		}

		private void Read(IConfiguration config)
		{
			IObjectContainer oc = Open(config);
			IObjectSet objectSet = AllItems(oc);
			while (objectSet.HasNext())
			{
				Item item = (Item)objectSet.Next();
			}
			oc.Close();
		}

		private void Update(IConfiguration config)
		{
			IObjectContainer oc = Open(config);
			IObjectSet objectSet = AllItems(oc);
			while (objectSet.HasNext())
			{
				Item item = (Item)objectSet.Next();
				item.Change();
				oc.Store(item);
			}
			oc.Close();
		}

		private void Delete(IConfiguration config)
		{
			IObjectContainer oc = Open(config);
			IObjectSet objectSet = AllItems(oc);
			while (objectSet.HasNext())
			{
				oc.Delete(objectSet.Next());
				// adding commit results in more syncs in the log, 
				// which is necessary for meaningful statistics!
				oc.Commit();
			}
			oc.Close();
		}

		private IConfiguration NewConfiguration(int itemCount)
		{
			RandomAccessFileAdapter rafAdapter = new RandomAccessFileAdapter();
			IoAdapter ioAdapter = new LoggingIoAdapter(rafAdapter, LogFileName(itemCount));
			IConfiguration config = Db4oFactory.NewConfiguration();
			config.Io(ioAdapter);
			return config;
		}

		private void DeleteDbFile()
		{
			new Sharpen.IO.File(DatabaseFile).Delete();
		}

		private IObjectSet AllItems(IObjectContainer oc)
		{
			return oc.Query(typeof(Item));
		}

		private IObjectContainer Open(IConfiguration config)
		{
			return Db4oFactory.OpenFile(config, DatabaseFile);
		}

		public static string LogFileName(int itemCount)
		{
			return "simplecrud_" + itemCount + ".log";
		}
	}
}
