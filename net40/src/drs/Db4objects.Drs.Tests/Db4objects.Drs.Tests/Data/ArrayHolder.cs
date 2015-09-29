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
	public class ArrayHolder
	{
		public string _name;

		public Db4objects.Drs.Tests.Data.ArrayHolder[] _array;

		public Db4objects.Drs.Tests.Data.ArrayHolder[][] _arrayN;

		public ArrayHolder()
		{
		}

		public ArrayHolder(string name)
		{
			_name = name;
		}

		public override string ToString()
		{
			return _name + ", hashcode = " + GetHashCode();
		}

		public virtual string GetName()
		{
			return _name;
		}

		public virtual Db4objects.Drs.Tests.Data.ArrayHolder[] Array()
		{
			return _array;
		}

		public virtual Db4objects.Drs.Tests.Data.ArrayHolder[][] ArrayN()
		{
			return _arrayN;
		}
	}
}
