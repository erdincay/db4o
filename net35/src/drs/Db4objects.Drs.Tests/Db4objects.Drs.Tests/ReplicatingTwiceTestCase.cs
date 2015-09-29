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
using Db4oUnit;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class ReplicatingTwiceTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			Pilot pilot = new Pilot("one", 1);
			A().Provider().StoreNew(pilot);
			A().Provider().Commit();
			ReplicateAll(A().Provider(), B().Provider(), null);
			pilot.SetName("modified");
			A().Provider().Update(pilot);
			A().Provider().Commit();
			ReplicateAll(A().Provider(), B().Provider(), null);
			Pilot pilotFromB = (Pilot)GetOneInstance(B(), typeof(Pilot));
			Assert.AreEqual("modified", pilotFromB.Name());
		}
	}
}
