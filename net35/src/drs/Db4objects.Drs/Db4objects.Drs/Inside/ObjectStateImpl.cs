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

namespace Db4objects.Drs.Inside
{
	public class ObjectStateImpl : IObjectState
	{
		public const long Unknown = -1;

		private object _object;

		private bool _isNew;

		private bool _wasModified;

		private long _modificationDate;

		public virtual object GetObject()
		{
			return _object;
		}

		public virtual bool IsNew()
		{
			return _isNew;
		}

		public virtual bool WasModified()
		{
			return _wasModified;
		}

		public virtual long ModificationDate()
		{
			return _modificationDate;
		}

		internal virtual void SetAll(object obj, bool isNew, bool wasModified, long modificationDate
			)
		{
			_object = obj;
			_isNew = isNew;
			_wasModified = wasModified;
			_modificationDate = modificationDate;
		}

		public override string ToString()
		{
			return "ObjectStateImpl{" + "_object=" + _object + ", _isNew=" + _isNew + ", _wasModified="
				 + _wasModified + ", _modificationDate=" + _modificationDate + '}';
		}

		public virtual bool IsKnown()
		{
			return _modificationDate != Unknown;
		}
	}
}
