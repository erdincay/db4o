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
	public class R3 : R2
	{
		internal R0 circle3;

		internal R4 r4;

		public virtual R0 GetCircle3()
		{
			return circle3;
		}

		public virtual void SetCircle3(R0 circle3)
		{
			this.circle3 = circle3;
		}

		public virtual R4 GetR4()
		{
			return r4;
		}

		public virtual void SetR4(R4 r4)
		{
			this.r4 = r4;
		}
	}
}
