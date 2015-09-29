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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Soda
{
	public class CollectionIndexedJoinTestCase : AbstractDb4oTestCase
	{
		private static readonly string Collectionfieldname = "_data";

		private static readonly string Idfieldname = "_id";

		private const int Numentries = 3;

		public class DataHolder
		{
			public ArrayList _data;

			public DataHolder(int id)
			{
				_data = new ArrayList();
				_data.Add(new CollectionIndexedJoinTestCase.Data(id));
			}
		}

		public class Data
		{
			public int _id;

			public Data(int id)
			{
				this._id = id;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(CollectionIndexedJoinTestCase.Data)).ObjectField(Idfieldname
				).Indexed(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < Numentries; i++)
			{
				Store(new CollectionIndexedJoinTestCase.DataHolder(i));
			}
		}

		public virtual void TestIndexedOrTwo()
		{
			AssertIndexedOr(new int[] { 0, 1, -1 }, 2);
		}

		private void AssertIndexedOr(int[] values, int expectedResultCount)
		{
			CollectionIndexedJoinTestCase.TestConfig config = new CollectionIndexedJoinTestCase.TestConfig
				(values.Length);
			while (config.MoveNext())
			{
				AssertIndexedOr(values, expectedResultCount, config.RootIndex(), config.ConnectLeft
					());
			}
		}

		public virtual void TestIndexedOrAll()
		{
			AssertIndexedOr(new int[] { 0, 1, 2 }, 3);
		}

		public virtual void TestTwoJoinLegs()
		{
			IQuery query = NewQuery(typeof(CollectionIndexedJoinTestCase.DataHolder)).Descend
				(Collectionfieldname);
			IConstraint left = query.Descend(Idfieldname).Constrain(0);
			left.Or(query.Descend(Idfieldname).Constrain(1));
			IConstraint right = query.Descend(Idfieldname).Constrain(2);
			right.Or(query.Descend(Idfieldname).Constrain(-1));
			left.Or(right);
			IObjectSet result = query.Execute();
			Assert.AreEqual(3, result.Count);
		}

		public virtual void AssertIndexedOr(int[] values, int expectedResultCount, int rootIdx
			, bool connectLeft)
		{
			IQuery query = NewQuery(typeof(CollectionIndexedJoinTestCase.DataHolder)).Descend
				(Collectionfieldname);
			IConstraint constraint = query.Descend(Idfieldname).Constrain(values[rootIdx]);
			for (int idx = 0; idx < values.Length; idx++)
			{
				if (idx != rootIdx)
				{
					IConstraint curConstraint = query.Descend(Idfieldname).Constrain(values[idx]);
					if (connectLeft)
					{
						constraint.Or(curConstraint);
					}
					else
					{
						curConstraint.Or(constraint);
					}
				}
			}
			IObjectSet result = query.Execute();
			Assert.AreEqual(expectedResultCount, result.Count);
		}

		private class TestConfig : PermutingTestConfig
		{
			public TestConfig(int numValues) : base(new object[][] { new object[] { 0, numValues
				 - 1 }, new object[] { false, true } })
			{
			}

			public virtual int RootIndex()
			{
				return ((int)Current(0));
			}

			public virtual bool ConnectLeft()
			{
				return ((bool)Current(1));
			}
		}
	}
}
