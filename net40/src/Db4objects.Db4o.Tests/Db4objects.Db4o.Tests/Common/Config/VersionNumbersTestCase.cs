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
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class VersionNumbersTestCase : AbstractDb4oTestCase
	{
		private static readonly string Original = "original";

		private static readonly string Newer = "newer";

		public class Item
		{
			public Item(string _name) : base()
			{
				this._name = _name;
			}

			public string _name;
		}

		public static void Main(string[] args)
		{
			new VersionNumbersTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.GenerateCommitTimestamps(true);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new VersionNumbersTestCase.Item(Original));
		}

		public virtual void TestVersionIncrease()
		{
			VersionNumbersTestCase.Item item = (VersionNumbersTestCase.Item)((VersionNumbersTestCase.Item
				)RetrieveOnlyInstance(typeof(VersionNumbersTestCase.Item)));
			IObjectInfo objectInfo = Db().GetObjectInfo(item);
			long version1 = objectInfo.GetCommitTimestamp();
			item._name = "modified";
			Db().Store(item);
			Db().Commit();
			long version2 = objectInfo.GetCommitTimestamp();
			Assert.IsGreater(version1, version2);
			Db().Store(item);
			Db().Commit();
			objectInfo = Db().GetObjectInfo(item);
			long version3 = objectInfo.GetCommitTimestamp();
			Assert.IsGreater(version2, version3);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestTransactionConsistentVersion()
		{
			Store(new VersionNumbersTestCase.Item(Newer));
			Db().Commit();
			VersionNumbersTestCase.Item newer = ItemByName(Newer);
			VersionNumbersTestCase.Item original = ItemByName(Original);
			Assert.IsGreater(Version(original), Version(newer));
			newer._name += " modified";
			original._name += " modified";
			Store(newer);
			Store(original);
			Db().Commit();
			Assert.AreEqual(Version(newer), Version(original));
			Reopen();
			newer = ItemByName(newer._name);
			original = ItemByName(original._name);
			Assert.AreEqual(Version(newer), Version(original));
		}

		private long Version(object obj)
		{
			return Db().GetObjectInfo(obj).GetCommitTimestamp();
		}

		private VersionNumbersTestCase.Item ItemByName(string @string)
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(VersionNumbersTestCase.Item));
			q.Descend("_name").Constrain(@string);
			object @object = q.Execute().Next();
			return (VersionNumbersTestCase.Item)@object;
		}

		public virtual void TestQueryForVersionNumber()
		{
			Store(new VersionNumbersTestCase.Item(Newer));
			Db().Commit();
			VersionNumbersTestCase.Item newer = ItemByName(Newer);
			long version = Version(newer);
			IQuery query = Db().Query();
			query.Descend(VirtualField.CommitTimestamp).Constrain(version).Smaller().Not();
			IObjectSet set = query.Execute();
			Assert.AreEqual(1, set.Count);
			Assert.AreSame(newer, ((VersionNumbersTestCase.Item)set.Next()));
		}
	}
}
