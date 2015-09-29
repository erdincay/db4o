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
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Query;

namespace Db4objects.Drs.Db4o
{
	/// <summary>
	/// tracks the version of the last replication between
	/// two Objectcontainers.
	/// </summary>
	/// <remarks>
	/// tracks the version of the last replication between
	/// two Objectcontainers.
	/// </remarks>
	/// <exclude></exclude>
	/// <persistent></persistent>
	public class ReplicationRecord : IInternal4
	{
		public Db4oDatabase _youngerPeer;

		public Db4oDatabase _olderPeer;

		public long _version;

		public ReplicationRecord()
		{
		}

		public ReplicationRecord(Db4oDatabase younger, Db4oDatabase older)
		{
			_youngerPeer = younger;
			_olderPeer = older;
		}

		public virtual void SetVersion(long version)
		{
			_version = version;
		}

		public virtual void Store(ObjectContainerBase container)
		{
			container.ShowInternalClasses(true);
			try
			{
				Transaction trans = container.CheckTransaction();
				container.StoreAfterReplication(trans, this, container.UpdateDepthProvider().ForDepth
					(1), false);
				container.Commit(trans);
			}
			finally
			{
				container.ShowInternalClasses(false);
			}
		}

		public static Db4objects.Drs.Db4o.ReplicationRecord BeginReplication(Transaction 
			transA, Transaction transB)
		{
			ObjectContainerBase peerA = transA.Container();
			ObjectContainerBase peerB = transB.Container();
			Db4oDatabase dbA = ((IInternalObjectContainer)peerA).Identity();
			Db4oDatabase dbB = ((IInternalObjectContainer)peerB).Identity();
			dbB.Bind(transA);
			dbA.Bind(transB);
			Db4oDatabase younger = null;
			Db4oDatabase older = null;
			if (dbA.IsOlderThan(dbB))
			{
				younger = dbB;
				older = dbA;
			}
			else
			{
				younger = dbA;
				older = dbB;
			}
			Db4objects.Drs.Db4o.ReplicationRecord rrA = QueryForReplicationRecord(peerA, transA
				, younger, older);
			Db4objects.Drs.Db4o.ReplicationRecord rrB = QueryForReplicationRecord(peerB, transB
				, younger, older);
			if (rrA == null)
			{
				if (rrB == null)
				{
					return new Db4objects.Drs.Db4o.ReplicationRecord(younger, older);
				}
				rrB.Store(peerA);
				return rrB;
			}
			if (rrB == null)
			{
				rrA.Store(peerB);
				return rrA;
			}
			if (rrA != rrB)
			{
				peerB.ShowInternalClasses(true);
				try
				{
					int id = peerB.GetID(transB, rrB);
					peerB.Bind(transB, rrA, id);
				}
				finally
				{
					peerB.ShowInternalClasses(false);
				}
			}
			return rrA;
		}

		public static Db4objects.Drs.Db4o.ReplicationRecord QueryForReplicationRecord(ObjectContainerBase
			 container, Transaction trans, Db4oDatabase younger, Db4oDatabase older)
		{
			container.ShowInternalClasses(true);
			try
			{
				IQuery q = container.Query(trans);
				q.Constrain(typeof(Db4objects.Drs.Db4o.ReplicationRecord));
				q.Descend("_youngerPeer").Constrain(younger).Identity();
				q.Descend("_olderPeer").Constrain(older).Identity();
				IObjectSet objectSet = q.Execute();
				return objectSet.HasNext() ? (Db4objects.Drs.Db4o.ReplicationRecord)objectSet.Next
					() : null;
			}
			finally
			{
				container.ShowInternalClasses(false);
			}
		}
	}
}
