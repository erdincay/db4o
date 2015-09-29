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
using Db4objects.Drs.Foundation;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class UuidConversionTestCase : DrsTestCase
	{
		public virtual void Test()
		{
			SPCChild child = StoreInA();
			Replicate();
			IReplicationReference @ref = A().Provider().ProduceReference(child);
			B().Provider().ClearAllReferences();
			IDrsUUID expectedUuid = @ref.Uuid();
			IReplicationReference referenceByUUID = B().Provider().ProduceReferenceByUUID(expectedUuid
				, null);
			Assert.IsNotNull(referenceByUUID);
			IDrsUUID actualUuid = referenceByUUID.Uuid();
			Assert.AreEqual(expectedUuid.GetLongPart(), actualUuid.GetLongPart());
		}

		private SPCChild StoreInA()
		{
			string name = "c1";
			SPCChild child = CreateChildObject(name);
			A().Provider().StoreNew(child);
			A().Provider().Commit();
			return child;
		}

		private void Replicate()
		{
			ReplicateAll(A().Provider(), B().Provider());
		}

		protected virtual SPCChild CreateChildObject(string name)
		{
			return new SPCChild(name);
		}
	}
}
