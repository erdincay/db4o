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
namespace Db4objects.Drs.Tests.Data
{
	public class NewPilot
	{
		internal string name;

		internal int points;

		internal int[] arr;

		public NewPilot()
		{
		}

		public NewPilot(string name, int points, int[] arr)
		{
			this.name = name;
			this.points = points;
			this.arr = arr;
		}

		public virtual int[] GetArr()
		{
			return arr;
		}

		public virtual void SetArr(int[] arr)
		{
			this.arr = arr;
		}

		public virtual string GetName()
		{
			return name;
		}

		public virtual void SetName(string name)
		{
			this.name = name;
		}

		public virtual int GetPoints()
		{
			return points;
		}

		public virtual void SetPoints(int points)
		{
			this.points = points;
		}

		public override string ToString()
		{
			return name + "/" + points;
		}
	}
}
