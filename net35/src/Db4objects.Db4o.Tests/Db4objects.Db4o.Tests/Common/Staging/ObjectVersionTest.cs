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
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class ObjectVersionTest : AbstractDb4oTestCase
	{
		protected override void Configure(IConfiguration config)
		{
			config.GenerateUUIDs(ConfigScope.Globally);
			config.GenerateCommitTimestamps(true);
		}

		public virtual void Test()
		{
			IExtObjectContainer oc = this.Db();
			ObjectVersionTest.Item @object = new ObjectVersionTest.Item("c1");
			oc.Store(@object);
			IObjectInfo objectInfo1 = oc.GetObjectInfo(@object);
			long oldVer = objectInfo1.GetCommitTimestamp();
			//Update
			@object.SetName("c3");
			oc.Store(@object);
			IObjectInfo objectInfo2 = oc.GetObjectInfo(@object);
			long newVer = objectInfo2.GetCommitTimestamp();
			Assert.IsNotNull(objectInfo1.GetUUID());
			Assert.IsNotNull(objectInfo2.GetUUID());
			Assert.IsTrue(oldVer > 0);
			Assert.IsTrue(newVer > 0);
			Assert.AreEqual(objectInfo1.GetUUID(), objectInfo2.GetUUID());
			Assert.IsTrue(newVer > oldVer);
		}

		public class Item
		{
			public string name;

			public Item()
			{
			}

			public Item(string name_)
			{
				this.name = name_;
			}

			public virtual string GetName()
			{
				return name;
			}

			public virtual void SetName(string name_)
			{
				name = name_;
			}
		}
	}
}
