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
using Db4objects.Db4o.Bench.Logging;
using Db4objects.Db4o.Bench.Logging.Statistics;
using Sharpen;
using Sharpen.IO;
using Sharpen.Text;

namespace Db4objects.Db4o.Bench.Logging.Statistics
{
	public class LogStatistics
	{
		private string _logFilePath;

		private string _logFileName;

		private string _statisticsFilePath;

		private TextWriter _out;

		private StreamReader _in;

		private long _readCount = 0;

		private long _readBytes = 0;

		private long _writeCount = 0;

		private long _writeBytes = 0;

		private long _syncCount = 0;

		private long _seekCount = 0;

		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Sharpen.Runtime.Out.WriteLine("Usage: LogStatistics <log file path> [<statistics file path>]"
					);
                throw new System.Exception("Usage: LogStatistics <log file path> [<statistics file path>]");
			}
			if (args.Length > 1)
			{
				new LogStatistics().Run(args[0], args[1]);
			}
			else
			{
				new LogStatistics().Run(args[0]);
			}
		}

		public virtual void Run(string logFilePath, string statisticsFilePath)
		{
			_logFilePath = logFilePath;
			_statisticsFilePath = statisticsFilePath;
			try
			{
				OpenFiles();
				_logFileName = new Sharpen.IO.File(_logFilePath).GetName();
				Sharpen.Runtime.Out.Write("  Creating statistics for " + _logFileName + "  ...   "
					);
				long start = Runtime.CurrentTimeMillis();
				CreateStatistics();
				long elapsed = Runtime.CurrentTimeMillis() - start;
				string elapsedString = FormatTime(elapsed);
				Sharpen.Runtime.Out.WriteLine("Finished! Time taken: " + elapsedString);
			}
			catch (FileNotFoundException e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
			CloseFiles();
		}

		public virtual void Run(string logFilepath)
		{
			Run(logFilepath, logFilepath + "-stat.htm");
		}

		private void CreateStatistics()
		{
			string line;
			try
			{
				while ((line = _in.ReadLine()) != null)
				{
					HandleLine(line);
				}
			}
			catch (IOException e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
			OutputStatistics();
		}

		private void OutputStatistics()
		{
			DecimalFormat formatPercentage = new DecimalFormat("##.##");
			DecimalFormat formatCount = new DecimalFormat("###,###");
			long totalCount = _readCount + _writeCount + _syncCount + _seekCount;
			string totalCountString = formatCount.Format(totalCount);
			double readCountPercentage = CountPercentage(_readCount, totalCount);
			string readCountPercentageString = formatPercentage.Format(readCountPercentage);
			double writeCountPercentage = CountPercentage(_writeCount, totalCount);
			string writeCountPercentageString = formatPercentage.Format(writeCountPercentage);
			double syncCountPercentage = CountPercentage(_syncCount, totalCount);
			string syncCountPercentageString = formatPercentage.Format(syncCountPercentage);
			double seekCountPercentage = CountPercentage(_seekCount, totalCount);
			string seekCountPercentageString = formatPercentage.Format(seekCountPercentage);
			long totalBytes = _readBytes + _writeBytes;
			string totalBytesString = formatCount.Format(totalBytes);
			double readBytesPercentage = CountPercentage(_readBytes, totalBytes);
			string readBytesPercentageString = formatPercentage.Format(readBytesPercentage);
			double writeBytesPercentage = CountPercentage(_writeBytes, totalBytes);
			string writeBytesPercentageString = formatPercentage.Format(writeBytesPercentage);
			string readCountString = formatCount.Format(_readCount);
			string writeCountString = formatCount.Format(_writeCount);
			string syncCountString = formatCount.Format(_syncCount);
			string seekCountString = formatCount.Format(_seekCount);
			string readBytesString = formatCount.Format(_readBytes);
			string writeBytesString = formatCount.Format(_writeBytes);
			PrintHeader();
			_out.WriteLine("<tr><td>Reads</td><td></td><td align=\"right\">" + readCountString
				 + "</td><td></td><td align=\"right\">" + readCountPercentageString + "</td><td></td><td align=\"right\">"
				 + readBytesString + "</td><td></td><td align=\"right\">" + readBytesPercentageString
				 + "</td></tr>");
			_out.WriteLine("<tr><td>Writes</td><td></td><td align=\"right\">" + writeCountString
				 + "</td><td></td><td align=\"right\">" + writeCountPercentageString + "</td><td></td><td align=\"right\">"
				 + writeBytesString + "</td><td></td><td align=\"right\">" + writeBytesPercentageString
				 + "</td></tr>");
			_out.WriteLine("<tr><td>Seeks</td><td></td><td align=\"right\">" + seekCountString
				 + "</td><td></td><td align=\"right\">" + seekCountPercentageString + "</td><td></td><td></td></tr>"
				);
			_out.WriteLine("<tr><td>Syncs</td><td></td><td align=\"right\">" + syncCountString
				 + "</td><td></td><td align=\"right\">" + syncCountPercentageString + "</td><td></td><td></td></tr>"
				);
			_out.WriteLine("<tr><td colspan=\"9\"></td></tr>");
			_out.WriteLine("<tr><td>Total</td><td></td><td align=\"right\">" + totalCountString
				 + "</td><td></td><td></td><td></td><td>" + totalBytesString + "</td><td></td></tr>"
				);
			_out.WriteLine("</table>");
			double avgBytesPerRead = _readBytes / _readCount;
			string avgBytesPerReadString = formatCount.Format(avgBytesPerRead);
			double avgBytesPerWrite = _writeBytes / _writeCount;
			string avgBytesPerWriteString = formatCount.Format(avgBytesPerWrite);
			_out.WriteLine("<p>");
			_out.WriteLine("Average byte count per read: " + avgBytesPerReadString);
			_out.WriteLine("<br>");
			_out.WriteLine("Average byte count per write: " + avgBytesPerWriteString);
			_out.WriteLine("</p>");
			PrintFooter();
		}

		private void PrintHeader()
		{
			_out.WriteLine("<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\">"
				);
			_out.WriteLine("<html>");
			_out.WriteLine("<head>");
			_out.WriteLine("<title>Log Statistics - " + _logFileName + "</title>");
			_out.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=utf-8\">"
				);
			_out.WriteLine("</head>");
			_out.WriteLine("<body>");
			_out.WriteLine("<p>Statistics for logfile '" + _logFilePath + "'</p>");
			_out.WriteLine("<table border=\"0\" cellpadding=\"4\">");
			_out.WriteLine("<tr><th></th><th></th><th>Count</th><th></th><th>%</th><th></th><th>Bytes</th><th></th><th>%</th></tr>"
				);
		}

		private void PrintFooter()
		{
			_out.WriteLine("</body>");
			_out.WriteLine("</html>");
		}

		private double CountPercentage(long count, long totalCount)
		{
			return 100 * (double)count / (double)totalCount;
		}

		private void HandleLine(string line)
		{
			if (line.StartsWith(LogConstants.WriteEntry))
			{
				HandleWrite(line);
			}
			else
			{
				if (line.StartsWith(LogConstants.ReadEntry))
				{
					HandleRead(line);
				}
				else
				{
					if (line.StartsWith(LogConstants.SyncEntry))
					{
						HandleSync();
					}
					else
					{
						if (line.StartsWith(LogConstants.SeekEntry))
						{
							HandleSeek();
						}
						else
						{
							throw new ArgumentException("Unknown command in log: " + line);
						}
					}
				}
			}
		}

		private void HandleSeek()
		{
			_seekCount++;
		}

		private void HandleSync()
		{
			_syncCount++;
		}

		private void HandleRead(string line)
		{
			_readCount++;
			_readBytes += BytesForLine(line, LogConstants.ReadEntry.Length);
		}

		private void HandleWrite(string line)
		{
			_writeCount++;
			_writeBytes += BytesForLine(line, LogConstants.WriteEntry.Length);
		}

		private long BytesForLine(string line, int commandLength)
		{
			return long.Parse(Sharpen.Runtime.Substring(line, commandLength));
		}

		private void CloseFiles()
		{
			CloseStatisticsFile();
			CloseLogFile();
		}

		/// <exception cref="FileNotFoundException"></exception>
		private void OpenFiles()
		{
			OpenStatisticsFile();
			OpenLogFile();
		}

		private void CloseLogFile()
		{
			try
			{
				_in.Close();
			}
			catch (IOException e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		private void CloseStatisticsFile()
		{
			_out.Flush();
			_out.Close();
		}

		/// <exception cref="FileNotFoundException"></exception>
		private void OpenLogFile()
		{
			_in = new StreamReader(_logFilePath);
		}

		/// <exception cref="FileNotFoundException"></exception>
		private void OpenStatisticsFile()
		{
			_out = new StreamWriter(new FileStream(_statisticsFilePath, FileMode.Append, FileAccess.Write));
		}

		private string FormatTime(long millis)
		{
			DateTime date = new DateTime(millis);
			string elapsedString = String.Format("mm min ss sec S millisec", date);
			return elapsedString;
		}
	}
}
