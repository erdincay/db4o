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
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Foundation
{
	/// <summary>Experiment field for future db4o logging.</summary>
	/// <remarks>
	/// Experiment field for future db4o logging.
	/// This will become an interface in the future.
	/// It will also allow wrapping to Log4j
	/// For now we are just collecting requirments.
	/// We are not using log4j directly on purpose, so
	/// we can keep the footprint small for embedded
	/// devices
	/// </remarks>
	public class Logger4Support
	{
		private static readonly Logger4 _logger = new Logger4();

		public static void LogIdentity(object obj, string message)
		{
			_logger.LogIdentity(obj, message);
		}

		public static void LogIdentity(object obj)
		{
			LogIdentity(obj, string.Empty);
		}

		public static void Log(string str)
		{
			_logger.Log(str);
		}
	}
}
