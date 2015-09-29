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
using Db4oUnit;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Collections;
using Db4objects.Db4o.Tests.Common.TA.Hierarchy;

namespace Db4objects.Db4o.Tests.Common.TA.Hierarchy
{
	public class TransparentActivationTestCase : TransparentActivationTestCaseBase
	{
		public static void Main(string[] args)
		{
			new TransparentActivationTestCase().RunAll();
		}

		private const int Priority = 42;

		protected override void Configure(IConfiguration config)
		{
			base.Configure(config);
			config.Add(new PagedListSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Project project = new PrioritizedProject("db4o", Priority);
			project.LogWorkDone(new UnitOfWork("ta kick-off", new DateTime(1000), new DateTime
				(2000)));
			Store(project);
		}

		public virtual void Test()
		{
			PrioritizedProject project = (PrioritizedProject)((Project)RetrieveOnlyInstance(typeof(
				Project)));
			Assert.AreEqual(Priority, project.GetPriority());
			// Project.totalTimeSpent needs the UnitOfWork objects to be activated
			Assert.AreEqual(1000, project.TotalTimeSpent());
		}
	}
}
