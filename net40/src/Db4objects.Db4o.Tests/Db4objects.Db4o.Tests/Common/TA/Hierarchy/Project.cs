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
using System.Collections;
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Collections;
using Db4objects.Db4o.Tests.Common.TA.Hierarchy;

namespace Db4objects.Db4o.Tests.Common.TA.Hierarchy
{
	internal class Project : ActivatableImpl
	{
		internal IList _subProjects = new PagedList();

		internal IList _workLog = new PagedList();

		internal string _name;

		public Project(string name)
		{
			_name = name;
		}

		public virtual void LogWorkDone(UnitOfWork work)
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			_workLog.Add(work);
		}

		public virtual long TotalTimeSpent()
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			long total = 0;
			IEnumerator i = _workLog.GetEnumerator();
			while (i.MoveNext())
			{
				UnitOfWork item = (UnitOfWork)i.Current;
				total += item.TimeSpent();
			}
			return total;
		}
	}
}
