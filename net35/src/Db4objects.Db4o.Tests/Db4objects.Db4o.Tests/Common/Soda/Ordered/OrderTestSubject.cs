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
namespace Db4objects.Db4o.Tests.Common.Soda.Ordered
{
	public class OrderTestSubject
	{
		public string _name;

		public int _seniority;

		public int _age;

		public OrderTestSubject(string name, int age, int seniority)
		{
			this._name = name;
			this._age = age;
			this._seniority = seniority;
		}

		public override string ToString()
		{
			return _name + " " + _age + " " + _seniority;
		}

		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (o.GetType() != GetType())
			{
				return false;
			}
			Db4objects.Db4o.Tests.Common.Soda.Ordered.OrderTestSubject ots = (Db4objects.Db4o.Tests.Common.Soda.Ordered.OrderTestSubject
				)o;
			bool ret = (_age == ots._age) && (_name.Equals(ots._name)) && (_seniority == ots.
				_seniority);
			return ret;
		}
	}
}
