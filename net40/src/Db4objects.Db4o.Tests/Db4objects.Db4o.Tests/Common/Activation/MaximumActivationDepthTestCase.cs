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
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Activation;

namespace Db4objects.Db4o.Tests.Common.Activation
{
	public class MaximumActivationDepthTestCase : AbstractDb4oTestCase, IOptOutTA
	{
		public class Data
		{
			public int _id;

			public MaximumActivationDepthTestCase.Data _prev;

			public Data(int id, MaximumActivationDepthTestCase.Data prev)
			{
				_id = id;
				_prev = prev;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ActivationDepth(int.MaxValue);
			config.ObjectClass(typeof(MaximumActivationDepthTestCase.Data)).MaximumActivationDepth
				(1);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			MaximumActivationDepthTestCase.Data data = new MaximumActivationDepthTestCase.Data
				(2, null);
			data = new MaximumActivationDepthTestCase.Data(1, data);
			data = new MaximumActivationDepthTestCase.Data(0, data);
			Store(data);
		}

		public virtual void TestActivationRestricted()
		{
			IQuery query = NewQuery(typeof(MaximumActivationDepthTestCase.Data));
			query.Descend("_id").Constrain(0);
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			MaximumActivationDepthTestCase.Data data = (MaximumActivationDepthTestCase.Data)result
				.Next();
			Assert.IsNotNull(data._prev);
			Assert.IsNull(data._prev._prev);
		}
	}
}
