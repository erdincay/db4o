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
using Db4objects.Db4o;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Util
{
	public class SodaTestUtil
	{
		public static void ExpectOne(IQuery query, object @object)
		{
			Expect(query, new object[] { @object });
		}

		public static void ExpectNone(IQuery query)
		{
			Expect(query, null);
		}

		public static void Expect(IQuery query, object[] results)
		{
			Expect(query, results, false);
		}

		public static void ExpectOrdered(IQuery query, object[] results)
		{
			Expect(query, results, true);
		}

		public static void Expect(IQuery query, object[] results, bool ordered)
		{
			IObjectSet set = query.Execute();
			if (results == null || results.Length == 0)
			{
				if (set.Count > 0)
				{
					Assert.Fail("No content expected.");
				}
				return;
			}
			int j = 0;
			Assert.AreEqual(results.Length, set.Count);
			while (set.HasNext())
			{
				object obj = set.Next();
				bool found = false;
				if (ordered)
				{
					if (TCompare.IsEqual(results[j], obj))
					{
						results[j] = null;
						found = true;
					}
					j++;
				}
				else
				{
					for (int i = 0; i < results.Length; i++)
					{
						if (results[i] != null)
						{
							if (TCompare.IsEqual(results[i], obj))
							{
								results[i] = null;
								found = true;
								break;
							}
						}
					}
				}
				if (ordered)
				{
					Assert.IsTrue(found, "Expected '" + SafeToString(results[j - 1]) + "' but got '" 
						+ SafeToString(obj) + "' at index " + (j - 1));
				}
				else
				{
					Assert.IsTrue(found, "Object not expected: " + SafeToString(obj));
				}
			}
			for (int i = 0; i < results.Length; i++)
			{
				if (results[i] != null)
				{
					Assert.Fail("Expected object not returned: " + results[i]);
				}
			}
		}

		private static string SafeToString(object obj)
		{
			return obj != null ? obj.ToString() : string.Empty;
		}

		private SodaTestUtil()
		{
		}
	}
}
