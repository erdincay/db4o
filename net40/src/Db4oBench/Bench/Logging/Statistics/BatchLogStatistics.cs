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
using Db4objects.Db4o.Bench.Logging.Statistics;
using Sharpen.IO;

namespace Db4objects.Db4o.Bench.Logging.Statistics
{
	public class BatchLogStatistics
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Sharpen.Runtime.Out.WriteLine("[BATCH] No path given.");
				throw new Exception("[BATCH] No path given.");
			}
			new BatchLogStatistics().Run(args[0]);
		}

		public virtual void Run(string logDirectoryPath)
		{
			Sharpen.IO.File directory = new Sharpen.IO.File(logDirectoryPath);
			if (directory.IsDirectory())
			{
				Sharpen.Runtime.Out.WriteLine("[BATCH] Creating statistics for logs in " + logDirectoryPath
					);
				IFilenameFilter logFilter = new LogFilter();
				Sharpen.IO.File[] logFiles = directory.ListFiles(logFilter);
				int i;
				for (i = 0; i < logFiles.Length; i++)
				{
					new LogStatistics().Run(logFiles[i].GetPath());
				}
				Sharpen.Runtime.Out.WriteLine("[BATCH] Statistics for " + i + " log files have been created!"
					);
			}
			else
			{
				Sharpen.Runtime.Out.WriteLine("[BATCH] Given path is no directory!");
				Sharpen.Runtime.Out.WriteLine("[BATCH] Path: " + logDirectoryPath);
			}
		}
	}

	internal class LogFilter : IFilenameFilter
	{
		public virtual bool Accept(Sharpen.IO.File dir, string name)
		{
			return (name.ToLower().EndsWith(".log"));
		}
	}
}
