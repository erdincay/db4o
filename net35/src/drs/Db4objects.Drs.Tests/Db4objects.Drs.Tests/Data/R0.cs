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
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class R0
	{
		internal string name;

		internal R0 r0;

		internal R1 r1;

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public virtual R0 GetR0()
		{
			return r0;
		}

		public virtual void SetR0(R0 r0)
		{
			this.r0 = r0;
		}

		public virtual R1 GetR1()
		{
			return r1;
		}

		public virtual void SetR1(R1 r1)
		{
			this.r1 = r1;
		}
	}
}
