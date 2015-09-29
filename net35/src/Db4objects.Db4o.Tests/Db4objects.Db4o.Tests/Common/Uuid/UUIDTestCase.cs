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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Uuid;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Uuid
{
	public class UUIDTestCase : AbstractDb4oTestCase
	{
		private static long storeStartTime;

		private static long storeEndTime;

		public static void Main(string[] args)
		{
			new UUIDTestCase().RunAll();
		}

		public class Item
		{
			public string name;

			public Item(string name_)
			{
				this.name = name_;
			}
		}

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(UUIDTestCase.Item)).GenerateUUIDs(true);
		}

		protected override void Store()
		{
			storeStartTime = Runtime.CurrentTimeMillis();
			Db().Store(new UUIDTestCase.Item("one"));
			Db().Commit();
			storeEndTime = Runtime.CurrentTimeMillis();
			Db().Store(new UUIDTestCase.Item("two"));
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestRetrieve()
		{
			Hashtable4 uuidCache = new Hashtable4();
			AssertItemsCanBeRetrievedByUUID(uuidCache);
			Reopen();
			AssertItemsCanBeRetrievedByUUID(uuidCache);
		}

		public virtual void TestTimeStamp()
		{
			IQuery q = NewItemQuery();
			q.Descend("name").Constrain("one");
			UUIDTestCase.Item item = (UUIDTestCase.Item)q.Execute().Next();
			Db4oUUID uuid = Uuid(item);
			long longPart = uuid.GetLongPart();
			long creationTime = TimeStampIdGenerator.IdToMilliseconds(longPart);
			Assert.IsGreaterOrEqual(storeStartTime, creationTime);
			Assert.IsSmallerOrEqual(storeEndTime, creationTime);
		}

		protected virtual void AssertItemsCanBeRetrievedByUUID(Hashtable4 uuidCache)
		{
			IQuery q = NewItemQuery();
			IObjectSet objectSet = q.Execute();
			while (objectSet.HasNext())
			{
				UUIDTestCase.Item item = (UUIDTestCase.Item)objectSet.Next();
				Db4oUUID uuid = Uuid(item);
				Assert.IsNotNull(uuid);
				Assert.AreSame(item, Db().GetByUUID(uuid));
				Db4oUUID cached = (Db4oUUID)uuidCache.Get(item.name);
				if (cached != null)
				{
					Assert.AreEqual(cached, uuid);
				}
				else
				{
					uuidCache.Put(item.name, uuid);
				}
			}
		}

		private Db4oUUID Uuid(object obj)
		{
			return Db().GetObjectInfo(obj).GetUUID();
		}

		private IQuery NewItemQuery()
		{
			return NewQuery(typeof(UUIDTestCase.Item));
		}
	}
}
