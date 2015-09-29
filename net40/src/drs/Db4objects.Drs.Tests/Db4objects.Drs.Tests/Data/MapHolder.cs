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
using System.Collections;

namespace Db4objects.Drs.Tests.Data
{
	public class MapHolder
	{
		private string name;

		private IDictionary map;

		public MapHolder()
		{
		}

		public MapHolder(string name)
		{
			this.name = name;
			this.map = new Hashtable();
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public virtual IDictionary GetMap()
		{
			return map;
		}

		public virtual void SetMap(IDictionary map)
		{
			this.map = map;
		}

		public virtual void Put(object key, object value)
		{
			map[key] = value;
		}

		public override string ToString()
		{
			return "name = " + name + ", map = " + map;
		}
	}
}
