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
using Db4objects.Db4o.Bench;
using Db4objects.Db4o.Bench.Crud;
using Db4objects.Db4o.Bench.Delaying;
using Db4objects.Db4o.Bench.Logging;
using Db4objects.Db4o.Bench.Logging.Replay;
using Db4objects.Db4o.Bench.Util;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Sharpen;
using Sharpen.IO;
using Sharpen.Lang;
using Sharpen.Util;

namespace Db4objects.Db4o.Bench
{
	/// <summary>IoBenchmark is a benchmark that measures I/O performance as seen by db4o.
	/// 	</summary>
	/// <remarks>
	/// IoBenchmark is a benchmark that measures I/O performance as seen by db4o.
	/// The benchmark can also run in delayed mode which allows simulating the I/O behaviour of a slower machine
	/// on a faster one.
	/// For further information and usage instructions please refer to README.htm.
	/// </remarks>
	public class IoBenchmark
	{
		private static readonly string _doubleLine = "=============================================================";

		private static readonly string _singleLine = "-------------------------------------------------------------";

		private static readonly string _dbFileName = "IoBenchmark.db4o";

		private Delays _delays = null;

		/// <exception cref="IOException"></exception>
		public static void Main(string[] args)
		{
			IoBenchmarkArgumentParser argumentParser = new IoBenchmarkArgumentParser(args);
			PrintBenchmarkHeader();
			IoBenchmark ioBenchmark = new IoBenchmark();
			if (argumentParser.Delayed())
			{
				ioBenchmark.ProcessResultsFiles(argumentParser.ResultsFile1(), argumentParser.ResultsFile2
					());
			}
			ioBenchmark.Run(argumentParser);
		}

		/// <exception cref="IOException"></exception>
		private void Run(IoBenchmarkArgumentParser argumentParser)
		{
			RunTargetApplication(argumentParser.ObjectCount());
			PrepareDbFile(argumentParser.ObjectCount());
			RunBenchmark(argumentParser.ObjectCount());
		}

		private void RunTargetApplication(int itemCount)
		{
			Sysout("Running target application ...");
			new CrudApplication().Run(itemCount);
		}

		private void PrepareDbFile(int itemCount)
		{
			Sysout("Preparing DB file ...");
			DeleteFile(_dbFileName);
			Db4objects.Db4o.IO.IoAdapter rafFactory = new RandomAccessFileAdapter();
			Db4objects.Db4o.IO.IoAdapter raf = rafFactory.Open(_dbFileName, false, 0, false);
			LogReplayer replayer = new LogReplayer(CrudApplication.LogFileName(itemCount), raf
				);
			try
			{
				replayer.ReplayLog();
			}
			catch (IOException)
			{
				ExitWithError("Error reading I/O operations log file");
			}
			finally
			{
				raf.Close();
			}
		}

		/// <exception cref="IOException"></exception>
		private void RunBenchmark(int itemCount)
		{
			Sysout("Running benchmark ...");
			DeleteBenchmarkResultsFile(itemCount);
            TextWriter @out = new StreamWriter(new FileStream(ResultsFileName(itemCount), FileMode.Append, FileAccess.Write));
			PrintRunHeader(itemCount, @out);
			for (int i = 0; i < LogConstants.AllConstants.Length; i++)
			{
				string currentCommand = LogConstants.AllConstants[i];
				BenchmarkCommand(currentCommand, itemCount, _dbFileName, @out);
			}
			DeleteFile(_dbFileName);
			DeleteCrudLogFile(itemCount);
		}

		/// <exception cref="IOException"></exception>
		private void BenchmarkCommand(string command, int itemCount, string dbFileName, TextWriter
			 @out)
		{
			HashSet commands = CommandSet(command);
			Db4objects.Db4o.IO.IoAdapter io = IoAdapter(dbFileName);
			LogReplayer replayer = new LogReplayer(CrudApplication.LogFileName(itemCount), io
				, commands);
			List4 commandList = replayer.ReadCommandList();
			StopWatch watch = new StopWatch();
			watch.Start();
			replayer.PlayCommandList(commandList);
			watch.Stop();
			io.Close();
			long timeElapsed = watch.Elapsed();
			long operationCount = ((long)replayer.OperationCounts()[command]);
			PrintStatisticsForCommand(@out, command, timeElapsed, operationCount);
		}

		/// <exception cref="NumberFormatException"></exception>
		/// <exception cref="IOException"></exception>
		/// <exception cref="Db4oIOException"></exception>
		private Db4objects.Db4o.IO.IoAdapter IoAdapter(string dbFileName)
		{
			if (Delayed())
			{
				return DelayingIoAdapter(dbFileName);
			}
			Db4objects.Db4o.IO.IoAdapter rafFactory = new RandomAccessFileAdapter();
			return rafFactory.Open(dbFileName, false, 0, false);
		}

		/// <exception cref="NumberFormatException"></exception>
		private Db4objects.Db4o.IO.IoAdapter DelayingIoAdapter(string dbFileName)
		{
			Db4objects.Db4o.IO.IoAdapter rafFactory = new RandomAccessFileAdapter();
			Db4objects.Db4o.IO.IoAdapter delFactory = new Db4objects.Db4o.Bench.Delaying.DelayingIoAdapter
				(rafFactory, _delays);
			return delFactory.Open(dbFileName, false, 0, false);
		}

		/// <exception cref="NumberFormatException"></exception>
		private void ProcessResultsFiles(string resultsFile1, string resultsFile2)
		{
			Sysout("Delaying:");
			try
			{
				DelayCalculation calculation = new DelayCalculation(resultsFile1, resultsFile2);
				calculation.ValidateData();
				if (!calculation.IsValidData())
				{
					ExitWithError("> Result files are invalid for delaying!");
				}
				_delays = calculation.CalculatedDelays();
				Sysout("> Required delays:");
				Sysout("> " + _delays);
				Sysout("> Adjusting delay timer to match required delays...");
				calculation.AdjustDelays(_delays);
				Sysout("> Adjusted delays:");
				Sysout("> " + _delays);
			}
			catch (IOException)
			{
				ExitWithError("> Could not open results file(s)!\n" + "> Please check the file name settings in IoBenchmark.properties."
					);
			}
		}

		private void ExitWithError(string error)
		{
			Sharpen.Runtime.Err.WriteLine(error + "\n Aborting execution!");
            throw new System.Exception(error + "\n Aborting execution!");
		}

		private string ResultsFileName(int itemCount)
		{
			string fileName = "db4o-IoBenchmark-results-" + itemCount;
			if (Delayed())
			{
				fileName += "-delayed";
			}
			fileName += ".log";
			return fileName;
		}

		private bool Delayed()
		{
			return _delays != null;
		}

		private HashSet CommandSet(string command)
		{
			HashSet commands = new HashSet();
			commands.Add(command);
			return commands;
		}

		private void DeleteBenchmarkResultsFile(int itemCount)
		{
			DeleteFile(ResultsFileName(itemCount));
		}

		private void DeleteCrudLogFile(int itemCount)
		{
			DeleteFile(CrudApplication.LogFileName(itemCount));
		}

		private void DeleteFile(string fileName)
		{
			new Sharpen.IO.File(fileName).Delete();
		}

		private static void PrintBenchmarkHeader()
		{
			PrintDoubleLine();
			Sysout("Running db4o IoBenchmark");
			PrintDoubleLine();
		}

		private void PrintRunHeader(int itemCount, TextWriter @out)
		{
			Output(@out, _singleLine);
			Output(@out, "db4o IoBenchmark results with " + itemCount + " items");
			Sysout("Statistics written to " + ResultsFileName(itemCount));
			Output(@out, _singleLine);
			Output(@out, string.Empty);
		}

		private void PrintStatisticsForCommand(TextWriter @out, string currentCommand, long
			 timeElapsed, long operationCount)
		{
			double avgTimePerOp = (double)timeElapsed / (double)operationCount;
			double opsPerMs = (double)operationCount / (double)timeElapsed;
			double nanosPerMilli = Math.Pow(10, 6);
			string output = "Results for " + currentCommand + "\r\n" + "> operations executed: "
				 + operationCount + "\r\n" + "> time elapsed: " + timeElapsed + " ms\r\n" + "> operations per millisecond: "
				 + opsPerMs + "\r\n" + "> average duration per operation: " + avgTimePerOp + " ms\r\n"
				 + currentCommand + (int)(avgTimePerOp * nanosPerMilli) + " ns\r\n";
			Output(@out, output);
			Sysout(" ");
		}

		private void Output(TextWriter @out, string text)
		{
			@out.WriteLine(text);
			Sysout(text);
		}

		private static void PrintDoubleLine()
		{
			Sysout(_doubleLine);
		}

		private static void Sysout(string text)
		{
			Sharpen.Runtime.Out.WriteLine(text);
		}
	}
}
