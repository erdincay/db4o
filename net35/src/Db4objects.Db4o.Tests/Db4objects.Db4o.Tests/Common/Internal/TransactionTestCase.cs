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
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class TransactionTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		private const int TestId = 5;

		public virtual void TestRemoveReferenceSystemOnClose()
		{
			LocalObjectContainer container = (LocalObjectContainer)Db();
			IReferenceSystem referenceSystem = container.CreateReferenceSystem();
			Transaction transaction = container.NewTransaction(container.SystemTransaction(), 
				referenceSystem, false);
			referenceSystem.AddNewReference(new ObjectReference(TestId));
			referenceSystem.AddNewReference(new ObjectReference(TestId + 1));
			container.ReferenceSystemRegistry().RemoveId(TestId);
			Assert.IsNull(referenceSystem.ReferenceForId(TestId));
			transaction.Close(false);
			container.ReferenceSystemRegistry().RemoveId(TestId + 1);
			Assert.IsNotNull(referenceSystem.ReferenceForId(TestId + 1));
		}
	}
}
