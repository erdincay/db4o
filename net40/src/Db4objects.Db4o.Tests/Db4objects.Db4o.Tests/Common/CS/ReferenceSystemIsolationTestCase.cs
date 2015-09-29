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
#if !SILVERLIGHT
using Db4oUnit;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.CS;

namespace Db4objects.Db4o.Tests.Common.CS
{
	public class ReferenceSystemIsolationTestCase : EmbeddedAndNetworkingClientTestCaseBase
	{
		[System.Serializable]
		public sealed class IncludeAllEvaluation : IEvaluation
		{
			public void Evaluate(ICandidate candidate)
			{
				candidate.Include(true);
			}
		}

		public class Item
		{
		}

		public virtual void Test()
		{
			ReferenceSystemIsolationTestCase.Item item = new ReferenceSystemIsolationTestCase.Item
				();
			NetworkingClient().Store(item);
			int id = (int)NetworkingClient().GetID(item);
			IQuery query = NetworkingClient().Query();
			query.Constrain(typeof(ReferenceSystemIsolationTestCase.Item));
			query.Constrain(new ReferenceSystemIsolationTestCase.IncludeAllEvaluation());
			query.Execute();
			Assert.IsNull(EmbeddedClient().Transaction.ReferenceForId(id));
		}
	}
}
#endif // !SILVERLIGHT
