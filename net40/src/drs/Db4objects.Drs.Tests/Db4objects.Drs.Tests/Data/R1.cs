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
	public class R1 : R0
	{
		internal R0 circle1;

		internal R2 r2;

		public virtual R0 GetCircle1()
		{
			return circle1;
		}

		public virtual void SetCircle1(R0 circle1)
		{
			this.circle1 = circle1;
		}

		public virtual R2 GetR2()
		{
			return r2;
		}

		public virtual void SetR2(R2 r2)
		{
			this.r2 = r2;
		}
	}
}
