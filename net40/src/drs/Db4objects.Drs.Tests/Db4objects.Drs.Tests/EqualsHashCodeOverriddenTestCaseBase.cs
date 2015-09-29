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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Drs;
using Db4objects.Drs.Db4o;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class EqualsHashCodeOverriddenTestCaseBase : ITestCase
	{
		public class Item
		{
			public string _name;

			public Item(string name)
			{
				_name = name;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (obj == null || GetType() != obj.GetType())
				{
					return false;
				}
				return _name.Equals(((EqualsHashCodeOverriddenTestCaseBase.Item)obj)._name);
			}

			public override int GetHashCode()
			{
				return _name.GetHashCode();
			}
		}

		private IStorage _storage = new MemoryStorage();

		public EqualsHashCodeOverriddenTestCaseBase() : base()
		{
		}

		protected virtual void AssertReplicates(object holder)
		{
			IEmbeddedObjectContainer sourceDb = OpenContainer("source");
			sourceDb.Store(holder);
			sourceDb.Commit();
			IEmbeddedObjectContainer targetDb = OpenContainer("target");
			try
			{
				Db4oEmbeddedReplicationProvider providerA = new Db4oEmbeddedReplicationProvider(sourceDb
					);
				Db4oEmbeddedReplicationProvider providerB = new Db4oEmbeddedReplicationProvider(targetDb
					);
				IReplicationSession replication = Replication.Begin(providerA, providerB);
				IObjectSet changed = replication.ProviderA().ObjectsChangedSinceLastReplication();
				while (changed.HasNext())
				{
					object o = changed.Next();
					if (holder.GetType() == o.GetType())
					{
						replication.Replicate(o);
						break;
					}
				}
				replication.Commit();
			}
			finally
			{
				sourceDb.Close();
				targetDb.Close();
			}
		}

		private IEmbeddedObjectContainer OpenContainer(string filePath)
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = _storage;
			config.File.GenerateUUIDs = ConfigScope.Globally;
			config.File.GenerateCommitTimestamps = true;
			return Db4oEmbedded.OpenFile(config, filePath);
		}
	}
}
