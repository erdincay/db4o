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
namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	internal class ByExample
	{
		public string Name;
		public ByExample Child;

		public ByExample(string name)
		{
			Name = name;
		}

		public ByExample(string name, ByExample child) : this(name)
		{
			Child = child;
		}

		public override string ToString()
		{
			return string.Format("ByExample(Name='{0}', Child=[{1}])", Name, Child);
		}

		public override bool Equals(object obj)
		{
			ByExample other = obj as ByExample;
			if (obj == null) return false;

			if (other.Name != Name)
				return false;

			if (Child == null)
				return other.Child == null;

			return  other.Child.Equals(Child);
		}
	}
}
