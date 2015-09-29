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
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class PeekPersistedTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new PeekPersistedTestCase().RunConcurrency();
		}

		public string name;

		public PeekPersistedTestCase child;

		protected override void Store()
		{
			PeekPersistedTestCase current = this;
			current.name = "1";
			for (int i = 2; i < 11; i++)
			{
				current.child = new PeekPersistedTestCase();
				current.child.name = string.Empty + i;
				current = current.child;
			}
			Store(this);
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(typeof(PeekPersistedTestCase));
			q.Descend("name").Constrain("1");
			IObjectSet objectSet = q.Execute();
			PeekPersistedTestCase pp = (PeekPersistedTestCase)objectSet.Next();
			for (int i = 0; i < 10; i++)
			{
				Peek(oc, pp, i);
			}
		}

		private void Peek(IExtObjectContainer oc, PeekPersistedTestCase original, int depth
			)
		{
			PeekPersistedTestCase peeked = (PeekPersistedTestCase)((PeekPersistedTestCase)oc.
				PeekPersisted(original, depth, true));
			for (int i = 0; i <= depth; i++)
			{
				Assert.IsNotNull(peeked);
				Assert.IsFalse(oc.IsStored(peeked));
				peeked = peeked.child;
			}
			Assert.IsNull(peeked);
		}
	}
}
#endif // !SILVERLIGHT
