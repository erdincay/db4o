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
using System.ComponentModel;

namespace OMControlLibrary.Helpers
{
	/// <summary>
	/// Represents the method that will handle the event that has no data.
	/// </summary>
	public delegate void CollectionClear();

	/// <summary>
	/// Represents the method that will handle the event that has item data.
	/// </summary>
	public delegate void CollectionChange(int index, object value);

	/// <summary>
	/// Extend collection base class by generating change events.
	/// </summary>
	public abstract class CollectionWithEvents : CollectionBase
	{
		// Instance fields
		private int _suspendCount;

		/// <summary>
		/// Occurs just before the collection contents are cleared.
		/// </summary>
		[Browsable(false)]
		public event CollectionClear Clearing;

		/// <summary>
		/// Occurs just after the collection contents are cleared.
		/// </summary>
		[Browsable(false)]
		public event CollectionClear Cleared;

		/// <summary>
		/// Occurs just before an item is added to the collection.
		/// </summary>
		[Browsable(false)]
		public event CollectionChange Inserting;

		/// <summary>
		/// Occurs just after an item has been added to the collection.
		/// </summary>
		[Browsable(false)]
		public event CollectionChange Inserted;

		/// <summary>
		/// Occurs just before an item is removed from the collection.
		/// </summary>
		[Browsable(false)]
		public event CollectionChange Removing;

		/// <summary>
		/// Occurs just after an item has been removed from the collection.
		/// </summary>
		[Browsable(false)]
		public event CollectionChange Removed;

		/// <summary>
		/// Initializes DrawTab new instance of the CollectionWithEvents class.
		/// </summary>
		public CollectionWithEvents()
		{
			// Default to not suspended
			_suspendCount = 0;
		}

		/// <summary>
		/// Do not generate change events until resumed.
		/// </summary>
		public void SuspendEvents()
		{
			_suspendCount++;
		}

		/// <summary>
		/// Safe to resume change events.
		/// </summary>
		public void ResumeEvents()
		{
			--_suspendCount;
		}

		/// <summary>
		/// Gets DrawTab value indicating if events are currently suspended.
		/// </summary>
		[Browsable(false)]
		public bool IsSuspended
		{
			get { return (_suspendCount > 0); }
		}

		/// <summary>
		/// Raises the Clearing event when not suspended.
		/// </summary>
		protected override void OnClear()
		{
			if (!IsSuspended)
			{
				// Any attached event handlers?
				if (Clearing != null)
					Clearing();
			}
		}

		/// <summary>
		/// Raises the Cleared event when not suspended.
		/// </summary>
		protected override void OnClearComplete()
		{
			if (!IsSuspended)
			{
				// Any attached event handlers?
				if (Cleared != null)
					Cleared();
			}
		}

		/// <summary>
		/// Raises the Inserting event when not suspended.
		/// </summary>
		/// <param name="index">Index of object being inserted.</param>
		/// <param name="value">The object that is being inserted.</param>
		protected override void OnInsert(int index, object value)
		{
			if (!IsSuspended)
			{
				// Any attached event handlers?
				if (Inserting != null)
					Inserting(index, value);
			}
		}

		/// <summary>
		/// Raises the Inserted event when not suspended.
		/// </summary>
		/// <param name="index">Index of inserted object.</param>
		/// <param name="value">The object that has been inserted.</param>
		protected override void OnInsertComplete(int index, object value)
		{
			if (!IsSuspended)
			{
				// Any attached event handlers?
				if (Inserted != null)
					Inserted(index, value);
			}
		}

		/// <summary>
		/// Raises the Removing event when not suspended.
		/// </summary>
		/// <param name="index">Index of object being removed.</param>
		/// <param name="value">The object that is being removed.</param>
		protected override void OnRemove(int index, object value)
		{
			if (!IsSuspended)
			{
				// Any attached event handlers?
				if (Removing != null)
					Removing(index, value);
			}
		}

		/// <summary>
		/// Raises the Removed event when not suspended.
		/// </summary>
		/// <param name="index">Index of removed object.</param>
		/// <param name="value">The object that has been removed.</param>
		protected override void OnRemoveComplete(int index, object value)
		{
			if (!IsSuspended)
			{
				// Any attached event handlers?
				if (Removed != null)
					Removed(index, value);
			}
		}

		/// <summary>
		/// Returns the index of the first occurrence of DrawTab value.
		/// </summary>
		/// <param name="value">The object to locate.</param>
		/// <returns>Index of object; otherwise -1</returns>
		protected int IndexOf(object value)
		{
			// Find the 0 based index of the requested entry
			return List.IndexOf(value);
		}
	}
}
