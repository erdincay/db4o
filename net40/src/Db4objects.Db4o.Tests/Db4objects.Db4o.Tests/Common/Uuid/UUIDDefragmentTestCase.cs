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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Uuid;

namespace Db4objects.Db4o.Tests.Common.Uuid
{
	/// <exclude></exclude>
	public class UUIDDefragmentTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
			public string name;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateUUIDs(ConfigScope.Globally);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			UUIDDefragmentTestCase.Item item = new UUIDDefragmentTestCase.Item();
			item.name = "one";
			Store(item);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Db4oUUID uuidBeforeDefragment = SingleItemUUID();
			byte[] signatureBeforeDefragment = uuidBeforeDefragment.GetSignaturePart();
			long longPartBeforeDefragment = uuidBeforeDefragment.GetLongPart();
			Defragment();
			Db4oUUID uuidAfterDefragment = SingleItemUUID();
			byte[] signatureAfterDefragment = uuidAfterDefragment.GetSignaturePart();
			long longPartAfterDefragment = uuidAfterDefragment.GetLongPart();
			ArrayAssert.AreEqual(signatureBeforeDefragment, signatureAfterDefragment);
			Assert.AreEqual(longPartBeforeDefragment, longPartAfterDefragment);
		}

		private Db4oUUID SingleItemUUID()
		{
			UUIDDefragmentTestCase.Item item = (UUIDDefragmentTestCase.Item)((UUIDDefragmentTestCase.Item
				)RetrieveOnlyInstance(typeof(UUIDDefragmentTestCase.Item)));
			IObjectInfo objectInfo = Db().GetObjectInfo(item);
			Db4oUUID uuid = objectInfo.GetUUID();
			return uuid;
		}
	}
}
