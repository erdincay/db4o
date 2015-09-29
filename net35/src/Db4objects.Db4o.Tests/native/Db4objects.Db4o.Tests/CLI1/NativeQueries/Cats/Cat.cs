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
namespace Db4objects.Db4o.Tests.CLI1.NativeQueries.Cats
{
	public class Cat : Animal
	{
		public string _firstName;

		public string _lastName;

		public int _age;

		public Cat _father;

		public Cat _mother;

		public string GetFirstName()
		{
			return _firstName;
		}

		public int GetAge()
		{
			return _age;
		}

		public string GetFullName()
		{
			return _firstName + " " + _lastName;
		}

		public Cat GetFather()
		{
			return _father;
		}
	}
}
