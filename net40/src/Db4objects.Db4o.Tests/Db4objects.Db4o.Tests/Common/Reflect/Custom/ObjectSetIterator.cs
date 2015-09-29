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
using System.Collections;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class ObjectSetIterator : IEnumerator
	{
		private readonly IObjectSet _set;

		private object _current;

		public ObjectSetIterator(IObjectSet set)
		{
			_set = set;
		}

		public virtual object Current
		{
			get
			{
				return _current;
			}
		}

		public virtual bool MoveNext()
		{
			if (_set.HasNext())
			{
				_current = _set.Next();
				return true;
			}
			return false;
		}

		public virtual void Reset()
		{
			throw new NotImplementedException();
		}
	}
}
