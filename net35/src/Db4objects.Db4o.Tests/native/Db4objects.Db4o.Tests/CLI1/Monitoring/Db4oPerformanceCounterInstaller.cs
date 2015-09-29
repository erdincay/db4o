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
#if !CF && !SILVERLIGHT

using System;
using System.Security.Principal;
using System.Threading;
using Db4objects.Db4o.Monitoring;

namespace Db4objects.Db4o.Tests.CLI1.Monitoring
{
	public class Db4oPerformanceCounterInstaller
	{
		private static bool _installed;

		public static void ReInstall()
		{
			if (_installed)
			{
				return;
			}

			if (IsCurrentUserAnAdministrator())
			{
				Db4oPerformanceCounters.ReInstall();
				_installed = true;
			}
		}

		private static bool IsCurrentUserAnAdministrator()
		{
			AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
			WindowsPrincipal principal = (WindowsPrincipal)Thread.CurrentPrincipal;
			return principal.IsInRole(WindowsBuiltInRole.Administrator);
		}
	}
}

#endif