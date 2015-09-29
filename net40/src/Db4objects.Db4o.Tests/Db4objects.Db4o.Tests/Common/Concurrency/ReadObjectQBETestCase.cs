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
using Db4objects.Db4o.Tests.Common.Concurrency;
using Db4objects.Db4o.Tests.Common.Persistent;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ReadObjectQBETestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new ReadObjectQBETestCase().RunConcurrency();
		}

		private static string testString = "simple test string";

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
			SimpleObject example = new SimpleObject(testString + mid, mid);
			IObjectSet result = oc.QueryByExample(example);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(example, result.Next());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void ConcReadDifferentObject(IExtObjectContainer oc, int seq)
		{
			SimpleObject example = new SimpleObject(testString + seq, seq);
			IObjectSet result = oc.QueryByExample(example);
			Assert.AreEqual(1, result.Count);
			Assert.AreEqual(example, result.Next());
		}
	}
}
#endif // !SILVERLIGHT
