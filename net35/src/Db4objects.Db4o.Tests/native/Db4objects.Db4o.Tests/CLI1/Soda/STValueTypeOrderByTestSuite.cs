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
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI1.Soda 
{
	public class STValueTypeOrderByTestSuite : FixtureBasedTestSuite
	{
		public static FixtureVariable VALUE_TYPE_TYPE_VARIABLE = FixtureVariable.NewInstance("Type");

		public override Type[] TestUnits()
		{
			return new Type[]
			       	{
			       		typeof(STValueTypesOrderByTestCase)
			       	};
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[]
						{
			       			new SimpleFixtureProvider(
										VALUE_TYPE_TYPE_VARIABLE, 
										new object[]
										{
#if NET_3_5
											new ValueTypeFixture<DateTimeOffset>(delegate(int i) { return DateTimeOffset.Now.AddHours(i); }),
#endif
#if !SILVERLIGHT
											new ValueTypeFixture<Guid>(delegate(int i) { return new Guid(i, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0); }),
											new ValueTypeFixture<TimeSpan>(delegate(int i) { return TimeSpan.FromHours(i); }),
#endif
										}),
						};
		}
	}

	public class Thing<T> where T : struct, IComparable<T>
	{
		public T _value;
		public string _name;

		public Thing()
		{
		}

		public Thing(string name, T value)
		{
			_value = value;
			_name = name;
		}

		public override bool Equals(object obj)
		{
			Thing<T> other = obj as Thing<T>;
			if (other == null) return false;

			return _value.CompareTo(other._value) == 0;
		}

		public override string ToString()
		{
			return "Thing(" + _name + ", " + _value + ")";
		}
	}
}
