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
using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Drs;
using Db4objects.Drs.Foundation;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs.Inside
{
	public interface IReplicationProviderInside : IReplicationProvider, ICollectionSource
		, ISimpleObjectContainer
	{
		/// <summary>Clear the  ReplicationReference cache</summary>
		void ClearAllReferences();

		void CommitReplicationTransaction();

		/// <summary>Destroys this provider and frees up resources.</summary>
		/// <remarks>Destroys this provider and frees up resources.</remarks>
		void Destroy();

		long GetLastReplicationVersion();

		string GetName();

		IReadonlyReplicationProviderSignature GetSignature();

		IReplicationReference ProduceReference(object obj);

		/// <summary>
		/// Collection version of getting a ReplicationReference:
		/// If the object is not a first class object, we need the
		/// parent object.
		/// </summary>
		/// <remarks>
		/// Collection version of getting a ReplicationReference:
		/// If the object is not a first class object, we need the
		/// parent object.
		/// </remarks>
		/// <returns>null, if there is no reference for this object.</returns>
		IReplicationReference ProduceReference(object obj, object parentObject, string fieldNameOnParent
			);

		/// <summary>Returns the ReplicationReference of an object by specifying the uuid of the object.
		/// 	</summary>
		/// <remarks>Returns the ReplicationReference of an object by specifying the uuid of the object.
		/// 	</remarks>
		/// <param name="uuid">the uuid of the object</param>
		/// <param name="hint">the type of the object</param>
		/// <returns>the ReplicationReference or null if the reference cannot be found</returns>
		IReplicationReference ProduceReferenceByUUID(IDrsUUID uuid, Type hint);

		IReplicationReference ReferenceNewObject(object obj, IReplicationReference counterpartReference
			, IReplicationReference referencingObjRef, string fieldName);

		/// <summary>Rollbacks all changes done during the replication session  and terminates the Transaction.
		/// 	</summary>
		/// <remarks>
		/// Rollbacks all changes done during the replication session  and terminates the Transaction.
		/// Guarantees the changes will not be applied to the underlying databases.
		/// </remarks>
		void RollbackReplication();

		/// <summary>Start a Replication Transaction with another ReplicationProvider</summary>
		/// <param name="peerSignature">the signature of another ReplicationProvider.</param>
		void StartReplicationTransaction(IReadonlyReplicationProviderSignature peerSignature
			);

		/// <summary>Stores the new replicated state of obj.</summary>
		/// <remarks>
		/// Stores the new replicated state of obj. It can also be a new object to this
		/// provider.
		/// </remarks>
		/// <param name="obj">Object with updated state or a clone of new object in the peer.
		/// 	</param>
		void StoreReplica(object obj);

		/// <summary>Visits the object of each cached ReplicationReference.</summary>
		/// <remarks>Visits the object of each cached ReplicationReference.</remarks>
		/// <param name="visitor">implements the visit functions, including copying of object states, and storing of changed objects
		/// 	</param>
		void VisitCachedReferences(IVisitor4 visitor);

		bool WasModifiedSinceLastReplication(IReplicationReference reference);

		void ReplicateDeletion(IDrsUUID uuid);

		object ReplaceIfSpecific(object value);

		bool IsSecondClassObject(object obj);

		void EnsureVersionsAreGenerated();

		Db4objects.Drs.Foundation.TimeStamps TimeStamps();

		void SyncCommitTimestamp(long syncedTimeStamp);
	}
}
