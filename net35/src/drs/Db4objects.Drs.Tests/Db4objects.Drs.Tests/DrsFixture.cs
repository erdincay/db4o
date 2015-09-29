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
using Db4objects.Db4o.Reflect;
using Db4objects.Drs.Tests;

namespace Db4objects.Drs.Tests
{
	public class DrsFixture
	{
		public readonly IDrsProviderFixture a;

		public readonly IDrsProviderFixture b;

		public readonly IReflector reflector;

		public DrsFixture(IDrsProviderFixture fixtureA, IDrsProviderFixture fixtureB) : this
			(fixtureA, fixtureB, null)
		{
		}

		public DrsFixture(IDrsProviderFixture fixtureA, IDrsProviderFixture fixtureB, IReflector
			 reflector)
		{
			if (null == fixtureA)
			{
				throw new ArgumentException("fixtureA");
			}
			if (null == fixtureB)
			{
				throw new ArgumentException("fixtureB");
			}
			a = fixtureA;
			b = fixtureB;
			this.reflector = reflector;
		}
	}
}
