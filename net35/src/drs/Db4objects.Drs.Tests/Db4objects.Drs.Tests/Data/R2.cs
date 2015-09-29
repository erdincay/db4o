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
	public class R2 : R1
	{
		internal R0 circle2;

		internal R3 r3;

		public virtual R0 GetCircle2()
		{
			return circle2;
		}

		public virtual void SetCircle2(R0 circle2)
		{
			this.circle2 = circle2;
		}

		public virtual R3 GetR3()
		{
			return r3;
		}

		public virtual void SetR3(R3 r3)
		{
			this.r3 = r3;
		}
	}
}
