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
namespace Db4objects.Db4o.Tests.Common.Persistent
{
	public class SimpleObject
	{
		public string _s;

		public int _i;

		public SimpleObject(string s, int i)
		{
			_s = s;
			_i = i;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Db4objects.Db4o.Tests.Common.Persistent.SimpleObject))
			{
				return false;
			}
			Db4objects.Db4o.Tests.Common.Persistent.SimpleObject another = (Db4objects.Db4o.Tests.Common.Persistent.SimpleObject
				)obj;
			return _s.Equals(another._s) && (_i == another._i);
		}

		public virtual int GetI()
		{
			return _i;
		}

		public virtual void SetI(int i)
		{
			_i = i;
		}

		public virtual string GetS()
		{
			return _s;
		}

		public virtual void SetS(string s)
		{
			_s = s;
		}

		public override string ToString()
		{
			return _s + ":" + _i;
		}
	}
}
