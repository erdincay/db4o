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
namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class Logger
	{
		public static void Log(string s)
		{
		}

		//		System.err.println(s);
		public static void LogMethodCall(object target, string methodName)
		{
			Log(target.ToString() + "." + methodName + "()");
		}

		public static void LogMethodCall(object target, string methodName, object arg)
		{
			Log(target.ToString() + "." + methodName + "(" + arg + ")");
		}

		public static void LogMethodCall(object target, string methodName, object arg1, object
			 arg2)
		{
			Log(target.ToString() + "." + methodName + "(" + arg1 + ", " + arg2 + ")");
		}
	}
}
