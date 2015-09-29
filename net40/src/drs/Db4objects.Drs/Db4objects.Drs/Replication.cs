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
using Db4objects.Db4o.Reflect;
using Db4objects.Drs;
using Db4objects.Drs.Inside;

namespace Db4objects.Drs
{
	/// <summary>Factory to create ReplicationSessions.</summary>
	/// <remarks>Factory to create ReplicationSessions.</remarks>
	/// <version>1.3</version>
	/// <seealso cref="com.db4o.drs.hibernate.HibernateReplication">com.db4o.drs.hibernate.HibernateReplication
	/// 	</seealso>
	/// <seealso cref="IReplicationProvider">IReplicationProvider</seealso>
	/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
	/// <since>dRS 1.0</since>
	public class Replication
	{
		/// <summary>
		/// Begins a replication session between two ReplicationProviders without a
		/// ReplicationEventListener and with no Reflector provided.
		/// </summary>
		/// <remarks>
		/// Begins a replication session between two ReplicationProviders without a
		/// ReplicationEventListener and with no Reflector provided.
		/// </remarks>
		/// <exception cref="ReplicationConflictException">when conflicts occur</exception>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		public static IReplicationSession Begin(IReplicationProvider providerA, IReplicationProvider
			 providerB)
		{
			return Begin(providerA, providerB, null, null);
		}

		/// <summary>
		/// Begins a replication session between two ReplicationProviders using a
		/// ReplicationEventListener and with no Reflector provided.
		/// </summary>
		/// <remarks>
		/// Begins a replication session between two ReplicationProviders using a
		/// ReplicationEventListener and with no Reflector provided.
		/// </remarks>
		/// <exception cref="ReplicationConflictException">when conflicts occur</exception>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		public static IReplicationSession Begin(IReplicationProvider providerA, IReplicationProvider
			 providerB, IReplicationEventListener listener)
		{
			return Begin(providerA, providerB, listener, null);
		}

		/// <summary>
		/// Begins a replication session between two ReplicationProviders without a
		/// ReplicationEventListener and with a Reflector provided.
		/// </summary>
		/// <remarks>
		/// Begins a replication session between two ReplicationProviders without a
		/// ReplicationEventListener and with a Reflector provided.
		/// </remarks>
		/// <exception cref="ReplicationConflictException">when conflicts occur</exception>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		public static IReplicationSession Begin(IReplicationProvider providerFrom, IReplicationProvider
			 providerTo, IReflector reflector)
		{
			return Begin(providerFrom, providerTo, null, reflector);
		}

		/// <summary>
		/// Begins a replication session between two ReplicationProviders using a
		/// ReplicationEventListener and with a Reflector provided.
		/// </summary>
		/// <remarks>
		/// Begins a replication session between two ReplicationProviders using a
		/// ReplicationEventListener and with a Reflector provided.
		/// </remarks>
		/// <exception cref="ReplicationConflictException">when conflicts occur</exception>
		/// <seealso cref="IReplicationEventListener">IReplicationEventListener</seealso>
		public static IReplicationSession Begin(IReplicationProvider providerFrom, IReplicationProvider
			 providerTo, IReplicationEventListener listener, IReflector reflector)
		{
			if (listener == null)
			{
				listener = new DefaultReplicationEventListener();
			}
			ReplicationReflector rr = new ReplicationReflector(providerFrom, providerTo, reflector
				);
			providerFrom.ReplicationReflector(rr);
			providerTo.ReplicationReflector(rr);
			return new GenericReplicationSession(providerFrom, providerTo, listener, reflector
				);
		}
	}
}
