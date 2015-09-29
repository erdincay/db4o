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
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
	public class Map4TestCase : ITestCase
	{
		private readonly IMap4 subject = new Hashtable4();

		public virtual void TestRemove()
		{
			for (int i = 0; i < 5; ++i)
			{
				string key = "key" + i;
				string value = "value" + i;
				subject.Put(key, value);
				Assert.AreEqual(value, subject.Remove(key));
			}
		}

		public virtual void TestContainsKey()
		{
			string key1 = "foo";
			string key2 = "bar";
			subject.Put(key1, "v");
			subject.Put(key2, "v");
			Assert.IsTrue(subject.ContainsKey(key1));
			Assert.IsTrue(subject.ContainsKey(key2));
			Assert.IsFalse(subject.ContainsKey(null));
			Assert.IsFalse(subject.ContainsKey(key1.ToUpper()));
			Assert.IsFalse(subject.ContainsKey(key2.ToUpper()));
		}

		public virtual void TestValuesIterator()
		{
			object[] values = new object[5];
			for (int i = 0; i < values.Length; ++i)
			{
				values[i] = ("value" + i);
			}
			for (int vIndex = 0; vIndex < values.Length; ++vIndex)
			{
				object v = values[vIndex];
				subject.Put("key4" + v, v);
			}
			Iterator4Assert.SameContent(values, subject.Values().GetEnumerator());
		}
	}
}
