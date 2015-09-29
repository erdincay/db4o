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
	public class SPCParent
	{
		private SPCChild child;

		private string name;

		public SPCParent()
		{
		}

		public SPCParent(string name)
		{
			this.name = name;
		}

		public SPCParent(SPCChild child, string name)
		{
			this.child = child;
			this.name = name;
		}

		public virtual SPCChild GetChild()
		{
			return child;
		}

		public virtual void SetChild(SPCChild child)
		{
			this.child = child;
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return "SPCParent{" + "child=" + child + ", name='" + name + '\'' + '}';
		}
	}
}
