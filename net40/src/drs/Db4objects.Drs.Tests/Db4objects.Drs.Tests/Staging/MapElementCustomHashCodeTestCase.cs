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
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Staging;

namespace Db4objects.Drs.Tests.Staging
{
	public class MapElementCustomHashCodeTestCase : EqualsHashCodeOverriddenTestCaseBase
	{
		public class Holder
		{
			internal IDictionary _map = new Hashtable();

			public Holder(EqualsHashCodeOverriddenTestCaseBase.Item itemA, EqualsHashCodeOverriddenTestCaseBase.Item
				 itemB)
			{
				// DRS-118
				// NOTE: This test does not necessarily reproduce the symptom.
				_map[itemA] = itemA;
				_map[itemB] = itemB;
			}
		}

		public virtual void TestReplicatesMap()
		{
			AssertReplicates(new MapElementCustomHashCodeTestCase.Holder(new EqualsHashCodeOverriddenTestCaseBase.Item
				("item"), new EqualsHashCodeOverriddenTestCaseBase.Item("item")));
		}
	}
}
