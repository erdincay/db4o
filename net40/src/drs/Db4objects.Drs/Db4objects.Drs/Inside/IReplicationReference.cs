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
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Inside
{
	public interface IReplicationReference
	{
		IDrsUUID Uuid();

		/// <summary>
		/// Must return the latests version of the object AND OF
		/// ALL COLLECTIONS IT REFERENCES IN ITS FIELDS because
		/// collections are treated as 2nd class objects
		/// (just like arrays) for Hibernate replication
		/// compatibility purposes.
		/// </summary>
		/// <remarks>
		/// Must return the latests version of the object AND OF
		/// ALL COLLECTIONS IT REFERENCES IN ITS FIELDS because
		/// collections are treated as 2nd class objects
		/// (just like arrays) for Hibernate replication
		/// compatibility purposes.
		/// </remarks>
		long Version();

		object Object();

		object Counterpart();

		void SetCounterpart(object obj);

		void MarkForReplicating(bool flag);

		bool IsMarkedForReplicating();

		void MarkForDeleting();

		bool IsMarkedForDeleting();

		void MarkCounterpartAsNew();

		bool IsCounterpartNew();
	}
}
