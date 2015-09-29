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
using System.Collections.Generic;

namespace Tests.Subject
{
    public class CFCompatibilityTestSubject<T> : BaseType where T : IComparable
    {
        public CFCompatibilityTestSubject(string name)
            : base(name)
        {
        }

        public IList<T> _ids;
        public long[] _longs;
        public DateTime _dateTime;
        public Nullable<int>[] _nullables;
        public CFCompatibilityTestSubject<string> _child;

        public CFCompatibilityTestSubject<T> Longs(params long[] longs)
        {
            _longs = longs;
            return this;
        }

        public CFCompatibilityTestSubject<T> DateTime(DateTime dateTime)
        {
            _dateTime = dateTime;
            return this;
        }

        public CFCompatibilityTestSubject<T> NullableInts(params int?[] values)
        {
            _nullables = values;
            return this;
        }

        public CFCompatibilityTestSubject<T> List(params T[] ids)
        {
            _ids = new List<T>(ids);
            return this;
        }

        public CFCompatibilityTestSubject<T> Child(CFCompatibilityTestSubject<string> child)
        {
            _child = child;
            return this;
        }

        public override bool Equals(object obj)
        {
            CFCompatibilityTestSubject<T> lhs = obj as CFCompatibilityTestSubject<T>;
            if (null == lhs) return false;

            if (lhs.GetType() != GetType()) return false;

            return
                Equals(lhs._child, _child) &&
                lhs._dateTime == _dateTime &&
                lhs._name == _name &&
                AreEqual(lhs._ids, _ids) &&
                AreEqual(lhs._longs, _longs);
        }

        public override string ToString()
        {
            return "CFCompatibilityTest<" + GetType().GetGenericArguments()[0].Name + ">\r\n(\r\n" +
                   "\tName: " + _name + "\r\n" +
                   "\tDateTime: " + _dateTime + "\r\n" +
                   "\tIds: " + _ids + "\r\n" +
                   "\tLongs: " + _longs + "\r\n" +
                   "\tNullables: " + _nullables + "\r\n" +
                   "\tChild: " + _child + ")\r\n";
        }

        private static bool AreEqual<E>(IEnumerable<E> lhs, IEnumerable<E> rhs) where E : IComparable
        {
            if (rhs == lhs) return true;
            if (null == lhs) return false;
            if (null == rhs) return false;

            IEnumerator<E> e1 = lhs.GetEnumerator();
            IEnumerator<E> e2 = rhs.GetEnumerator();

            while (e1.MoveNext())
            {
                if (!e2.MoveNext())
                {
                    return false;
                }

                if (!Equals(e1.Current, e2.Current))
                {
                    return false;
                }
            }

            return !e2.MoveNext();
        }
    }

	public class BaseType
	{
		public BaseType(string name)
		{
			_name = name;
		}

		public string _name;
	}

	public class Subjects
	{
		private static readonly IDictionary<string, CFCompatibilityTestSubject<string>> _subjects;

		static Subjects()
		{
			_subjects = new Dictionary<string, CFCompatibilityTestSubject<string>>();

			foreach (CFCompatibilityTestSubject<string> subject in SubjectsToTest())
			{
				_subjects[subject._name] = subject;
			}
		}

		public static CFCompatibilityTestSubject<string> Item(string name)
		{
			return _subjects[name];
		}

		public static IEnumerable<CFCompatibilityTestSubject<string>> Objects
		{
			get
			{
				return _subjects.Values;
			}
		}

		private static IEnumerable<CFCompatibilityTestSubject<string>> SubjectsToTest()
		{
			DateTime vid = new DateTime(1971, 5, 1);
			yield return new CFCompatibilityTestSubject<string>("foo").
									List(string.Empty, "foo.2", "foo.3").
									Longs(long.MinValue, 0, long.MaxValue).
									DateTime(vid).
									NullableInts(int.MinValue, 0, int.MaxValue, null);

			yield return new CFCompatibilityTestSubject<string>("bar").
									List("bar.1", string.Empty, "bar.3").
									Longs().
									DateTime(vid).
									NullableInts(null);

			DateTime vid2 = new DateTime(1998, 5, 1);
			CFCompatibilityTestSubject<string> baz = new CFCompatibilityTestSubject<string>("baz").
															List("baz.1", "baz.2", string.Empty).
															Longs().
															DateTime(vid2).
															NullableInts(vid2.Day, vid2.Month, vid2.Year);
			yield return baz;

			yield return new CFCompatibilityTestSubject<string>("foobar").
									List(string.Empty, "foobar.2", "foobar.3").
									Longs().
									DateTime(DateTime.MaxValue).
									NullableInts(null).
									Child(baz);
		}

		public static CFCompatibilityTestSubject<string> NewJohnDoe()
		{
			return new CFCompatibilityTestSubject<string>("john.doe").
									List("john.doe.1", "john.doe.2", "john.doe.3").
									Longs().
									DateTime(DateTime.MinValue).
									NullableInts(null);
		}
	}
}