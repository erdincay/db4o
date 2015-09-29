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

namespace OMControlLibrary
{
	#region TabStripItemClosingEventArgs

	public class TabStripItemClosingEventArgs : EventArgs
	{
		public TabStripItemClosingEventArgs(OMETabStripItem item)
		{
			_item = item;
		}

		private bool _cancel = false;
		private OMETabStripItem _item;

		public OMETabStripItem Item
		{
			get { return _item; }
			set { _item = value; }
		}

		public bool Cancel
		{
			get { return _cancel; }
			set { _cancel = value; }
		}

	}

	#endregion

	#region TabStripItemChangedEventArgs

	public class TabStripItemChangedEventArgs : EventArgs
	{
		OMETabStripItem itm;
		OMETabStripItemChangeTypes changeType;

		public TabStripItemChangedEventArgs(OMETabStripItem item, OMETabStripItemChangeTypes type)
		{
			changeType = type;
			itm = item;
		}

		public OMETabStripItemChangeTypes ChangeType
		{
			get { return changeType; }
		}

		public OMETabStripItem Item
		{
			get { return itm; }
		}
	}

	#endregion

	public delegate void TabStripItemChangedHandler(TabStripItemChangedEventArgs e);
	public delegate void TabStripItemClosingHandler(TabStripItemClosingEventArgs e);

}
