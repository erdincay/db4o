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
using Db4objects.Drs;

namespace Db4objects.Drs
{
	/// <summary>Defines an event class for the replication of an entity.</summary>
	/// <remarks>Defines an event class for the replication of an entity.</remarks>
	/// <since>dRS 1.2</since>
	public interface IReplicationEvent
	{
		/// <summary>Does a conflict occur?</summary>
		/// <returns>true when a conflict occur</returns>
		bool IsConflict();

		/// <summary>Overrides default replication behaviour with some state chosen by the user.
		/// 	</summary>
		/// <remarks>Overrides default replication behaviour with some state chosen by the user.
		/// 	</remarks>
		/// <param name="chosen">the ObjectState of the prevailing object or null if replication should ignore this object and not traverse to its referenced objects.
		/// 	</param>
		void OverrideWith(IObjectState chosen);

		/// <summary>The ObjectState in provider A.</summary>
		/// <remarks>The ObjectState in provider A.</remarks>
		/// <returns>ObjectState in provider A</returns>
		IObjectState StateInProviderA();

		/// <summary>The ObjectState in provider B.</summary>
		/// <remarks>The ObjectState in provider B.</remarks>
		/// <returns>ObjectState in provider B</returns>
		IObjectState StateInProviderB();

		/// <summary>The time when the object is created in one provider.</summary>
		/// <remarks>The time when the object is created in one provider.</remarks>
		/// <returns>time when the object is created in one provider.</returns>
		long ObjectCreationDate();

		/// <summary>The replication process will not traverse to objects referenced by the current one.
		/// 	</summary>
		/// <remarks>The replication process will not traverse to objects referenced by the current one.
		/// 	</remarks>
		void StopTraversal();
	}
}
