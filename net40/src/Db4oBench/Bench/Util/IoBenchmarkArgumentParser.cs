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
using Sharpen;

namespace Db4objects.Db4o.Bench.Util
{
	public class IoBenchmarkArgumentParser
	{
		private string _resultsFile2;

		private string _resultsFile1;

		private int _objectCount;

		private bool _delayed;

		public IoBenchmarkArgumentParser(string[] arguments)
		{
			ValidateArguments(arguments);
		}

		private void ValidateArguments(string[] arguments)
		{
			if (arguments.Length != 1 && arguments.Length != 3)
			{
				Sharpen.Runtime.Out.WriteLine("Usage: IoBenchmark <object-count> [<results-file-1> <results-file-2>]"
					);
                throw new System.Exception("Usage: IoBenchmark <object-count> [<results-file-1> <results-file-2>]");
			}
			_objectCount = int.Parse(arguments[0]);
			if (arguments.Length > 1)
			{
				_resultsFile1 = arguments[1];
				_resultsFile2 = arguments[2];
				_delayed = true;
			}
		}

		public virtual int ObjectCount()
		{
			return _objectCount;
		}

		public virtual string ResultsFile1()
		{
			return _resultsFile1;
		}

		public virtual string ResultsFile2()
		{
			return _resultsFile2;
		}

		public virtual bool Delayed()
		{
			return _delayed;
		}
	}
}
