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
	public class ObjectSetIDsTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new ObjectSetIDsTestCase().RunConcurrency();
		}

		internal const int Count = 11;

		protected override void Store()
		{
			for (int i = 0; i < Count; i++)
			{
				Store(new ObjectSetIDsTestCase());
			}
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			IQuery q = oc.Query();
			q.Constrain(this.GetType());
			IObjectSet res = q.Execute();
			Assert.AreEqual(Count, res.Count);
			long[] ids1 = new long[res.Count];
			int i = 0;
			while (res.HasNext())
			{
				ids1[i++] = oc.GetID(res.Next());
			}
			res.Reset();
			long[] ids2 = res.Ext().GetIDs();
			Assert.AreEqual(Count, ids1.Length);
			Assert.AreEqual(Count, ids2.Length);
			for (int j = 0; j < ids1.Length; j++)
			{
				bool found = false;
				for (int k = 0; k < ids2.Length; k++)
				{
					if (ids1[j] == ids2[k])
					{
						found = true;
						break;
					}
				}
				Assert.IsTrue(found);
			}
		}
	}
}
#endif // !SILVERLIGHT
