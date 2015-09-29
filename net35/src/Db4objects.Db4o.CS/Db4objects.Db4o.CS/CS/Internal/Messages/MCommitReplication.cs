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
using System.Collections;
using Db4objects.Db4o.CS.Internal;
using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Drs.Db4o;

namespace Db4objects.Db4o.CS.Internal.Messages
{
	public class MCommitReplication : MCommit, IMessageWithResponse
	{
		public override Msg ReplyFromServer()
		{
			IServerMessageDispatcher dispatcher = ServerMessageDispatcher();
			lock (ContainerLock())
			{
				LocalTransaction trans = ServerTransaction();
				long replicationRecordId = ReadLong();
				long timestamp = ReadLong();
				IList concurrentTimestamps = trans.ConcurrentReplicationTimestamps();
				ServerMessageDispatcher().Server().BroadcastReplicationCommit(timestamp, concurrentTimestamps
					);
				ReplicationRecord replicationRecord = (ReplicationRecord)Container().GetByID(trans
					, replicationRecordId);
				Container().Activate(trans, replicationRecord, new FixedActivationDepth(int.MaxValue
					));
				replicationRecord.SetVersion(timestamp);
				replicationRecord.ConcurrentTimestamps(concurrentTimestamps);
				replicationRecord.Store(trans);
				Container().StoreAfterReplication(trans, replicationRecord, Container().UpdateDepthProvider
					().ForDepth(int.MaxValue), false);
				trans.Commit(dispatcher);
				committedInfo = dispatcher.CommittedInfo();
				Transaction().UseDefaultTransactionTimestamp();
			}
			return Msg.Ok;
		}
	}
}
