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
using Sharpen;

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
	public class Logger4
	{
		public virtual void LogIdentity(object obj, string message)
		{
			if (obj == null)
			{
				Log(message + " [null]");
			}
			Log(message + " " + obj.GetType() + " " + Runtime.IdentityHashCode(obj));
		}

		public virtual void Log(string message)
		{
			return;
			Sharpen.Runtime.Out.WriteLine(StackAnalyzer.MethodCallAboveAsString(typeof(Logger4Support
				)) + " " + message);
			Sharpen.Runtime.Out.Flush();
		}
	}
}
