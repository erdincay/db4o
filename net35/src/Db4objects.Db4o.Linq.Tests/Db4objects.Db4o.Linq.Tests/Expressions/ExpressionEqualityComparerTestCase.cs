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
using System.Linq.Expressions;
using System.Text;

using Db4objects.Db4o.Linq.Expressions;

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Linq.Tests.Expressions
{
	public class ExpressionEqualityComparerTestCase : AbstractDb4oTestCase
	{
		interface IParametrable { }

		class Parameter : IParametrable
		{
			public int ID { get; set; }
			public int PID { get; set; }

			public override string ToString() { return null; }
			public string ToNormalizedString() { return null; }
		}

		public void TestComparison()
		{
			AssertAreEqual<int, int>(x => x, x => x);
			AssertAreNotEqual<int, int>(x => -x, x => x);
			AssertAreNotEqual<int, string>(x => "" + x, x => "a" + x);
			AssertAreEqual<int, bool>(x => (x != 2) && (x < 3), x => (x != 2) && (x < 3));
			AssertAreNotEqual<int, bool>(x => (x != 3) && (x <= 3), x => (x != 3) && (x < 3));

			AssertAreEqual<Parameter, bool>(p => p.ID == 2, p => p.ID == 2);
			AssertAreNotEqual<Parameter, bool>(p => p.ID == 2, p => p.PID == 2);

			AssertAreEqual<Parameter, string>(p => p.ToString(), p => p.ToString());
			AssertAreNotEqual<Parameter, string>(p => p.ToString(), p => p.ToNormalizedString());

			AssertAreEqual<Parameter, string>(p => (p as IParametrable).ToString(), p => (p as IParametrable).ToString());
			AssertAreNotEqual<Parameter, string>(p => (p as IParametrable).ToString(), p => p.ToString());
		}

		private static void AssertAreEqual<T>(Expression<Func<T>> a, Expression<Func<T>> b)
		{
			AssertEqual(a, b);
		}

		private static void AssertAreEqual<T1, T2>(Expression<Func<T1, T2>> a, Expression<Func<T1, T2>> b)
		{
			AssertEqual(a, b);
		}

		private static void AssertAreNotEqual<T>(Expression<Func<T>> a, Expression<Func<T>> b)
		{
			AssertNotEqual(a, b);
		}

		private static void AssertAreNotEqual<T1, T2>(Expression<Func<T1, T2>> a, Expression<Func<T1, T2>> b)
		{
			AssertNotEqual(a, b);
		}

		private static void AssertEqual(Expression a, Expression b)
		{
			Assert.IsTrue(AreEqual(a, b), string.Format("'{0}' and '{1}' expected to be equal", a, b));
			AssertHashCodeEqual(a, b);
		}

		private static void AssertNotEqual(Expression a, Expression b)
		{
			Assert.IsFalse(AreEqual(a, b), string.Format("'{0}' and '{1}' expected to be not equal", a, b));
			AssertHashCodeNotEqual(a, b);
		}

		private static bool AreEqual(Expression a, Expression b)
		{
			return ExpressionEqualityComparer.Instance.Equals(a, b);
		}

		private static void AssertHashCodeEqual(Expression a, Expression b)
		{
			Assert.IsTrue(ExpressionEqualityComparer.Instance.GetHashCode(a) == ExpressionEqualityComparer.Instance.GetHashCode(b),
				string.Format("HashCode for '{0}' expected to be the same as for '{1}'", a, b));
		}

		private static void AssertHashCodeNotEqual(Expression a, Expression b)
		{
			Assert.IsFalse(ExpressionEqualityComparer.Instance.GetHashCode(a) == ExpressionEqualityComparer.Instance.GetHashCode(b),
				string.Format("HashCode for '{0}' expected to be the different as for '{1}'", a, b));
		}
	}
}
