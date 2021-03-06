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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;

namespace Db4objects.Db4o.Tests.Common.Soda.Util
{
	public abstract class SodaBaseTestCase : AbstractDb4oTestCase
	{
		[System.NonSerialized]
		protected object[] _array;

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupBeforeStore()
		{
			_array = CreateData();
		}

		protected override void Store()
		{
			object[] data = CreateData();
			for (int idx = 0; idx < data.Length; idx++)
			{
				Db().Store(data[idx]);
			}
		}

		public abstract object[] CreateData();

		protected virtual void Expect(IQuery query, int[] indices)
		{
			SodaTestUtil.Expect(query, CollectCandidates(indices), false);
		}

		protected virtual void ExpectOrdered(IQuery query, int[] indices)
		{
			SodaTestUtil.ExpectOrdered(query, CollectCandidates(indices));
		}

		private object[] CollectCandidates(int[] indices)
		{
			object[] data = new object[indices.Length];
			for (int idx = 0; idx < indices.Length; idx++)
			{
				data[idx] = _array[indices[idx]];
			}
			return data;
		}
	}
}
