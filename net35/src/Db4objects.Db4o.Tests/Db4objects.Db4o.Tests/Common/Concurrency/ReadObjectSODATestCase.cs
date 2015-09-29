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
using Db4objects.Db4o.Tests.Common.Persistent;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ReadObjectSODATestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new ReadObjectSODATestCase().RunConcurrency();
		}

		private static string testString = "simple test string";

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < ThreadCount(); i++)
			{
				Store(new SimpleObject(testString + i, i));
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConcReadSameObject(IExtObjectContainer oc)
		{
			int mid = ThreadCount() / 2;
			IQuery query = oc.Query();
			query.Descend("_s").Constrain(testString + mid).And(query.Descend("_i").Constrain
				(mid));
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			SimpleObject expected = new SimpleObject(testString + mid, mid);
			Assert.AreEqual(expected, result.Next());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConcReadDifferentObject(IExtObjectContainer oc, int seq)
		{
			IQuery query = oc.Query();
			query.Descend("_s").Constrain(testString + seq).And(query.Descend("_i").Constrain
				(seq));
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			SimpleObject expected = new SimpleObject(testString + seq, seq);
			Assert.AreEqual(expected, result.Next());
		}
	}
}
#endif // !SILVERLIGHT
