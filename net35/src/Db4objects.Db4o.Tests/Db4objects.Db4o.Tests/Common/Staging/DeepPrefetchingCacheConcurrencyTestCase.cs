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
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.CS.Config;
using Db4objects.Db4o.CS.Internal.Config;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	/// <summary>COR-1762</summary>
	public class DeepPrefetchingCacheConcurrencyTestCase : AbstractDb4oTestCase, IOptOutAllButNetworkingCS
	{
		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			IClientConfiguration clientConfiguration = Db4oClientServerLegacyConfigurationBridge
				.AsClientConfiguration(config);
			clientConfiguration.PrefetchDepth = 3;
			clientConfiguration.PrefetchObjectCount = 3;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			for (int i = 0; i < 2; i++)
			{
				DeepPrefetchingCacheConcurrencyTestCase.Item item = new DeepPrefetchingCacheConcurrencyTestCase.Item
					("original");
				Store(item);
			}
		}

		public virtual void Test()
		{
			int[] ids = new int[2];
			IObjectSet originalResult = NewQuery(typeof(DeepPrefetchingCacheConcurrencyTestCase.Item
				)).Execute();
			DeepPrefetchingCacheConcurrencyTestCase.Item firstOriginalItem = ((DeepPrefetchingCacheConcurrencyTestCase.Item
				)originalResult.Next());
			Db().Purge(firstOriginalItem);
			IExtObjectContainer otherClient = OpenNewSession();
			IObjectSet updateResult = otherClient.Query(typeof(DeepPrefetchingCacheConcurrencyTestCase.Item
				));
			int idx = 0;
			for (IEnumerator updateItemIter = updateResult.GetEnumerator(); updateItemIter.MoveNext
				(); )
			{
				DeepPrefetchingCacheConcurrencyTestCase.Item updateItem = ((DeepPrefetchingCacheConcurrencyTestCase.Item
					)updateItemIter.Current);
				ids[idx] = (int)otherClient.GetID(updateItem);
				updateItem._name = "updated";
				otherClient.Store(updateItem);
				idx++;
			}
			otherClient.Commit();
			otherClient.Close();
			for (int i = 0; i < ids.Length; i++)
			{
				DeepPrefetchingCacheConcurrencyTestCase.Item checkItem = ((DeepPrefetchingCacheConcurrencyTestCase.Item
					)Db().GetByID(ids[i]));
				Db().Activate(checkItem);
				Assert.AreEqual("updated", checkItem._name);
			}
		}
		//		ObjectSet<Item> checkResult = newQuery(Item.class).execute();
		//		for (Item checkItem : checkResult) {
		//			Assert.areEqual("updated", checkItem._name);
		//		}
	}
}
#endif // !SILVERLIGHT
