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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Drs.Inside.Traversal
{
	public class FieldIterators
	{
		public static IEnumerator PersistentFields(IReflectClass claxx)
		{
			return Iterators.Filter(claxx.GetDeclaredFields(), new _IPredicate4_31());
		}

		private sealed class _IPredicate4_31 : IPredicate4
		{
			public _IPredicate4_31()
			{
			}

			public bool Match(object candidate)
			{
				IReflectField field = (IReflectField)candidate;
				if (field.IsStatic())
				{
					return false;
				}
				if (field.IsTransient())
				{
					return false;
				}
				if (Platform4.IsTransient(field.GetFieldType()))
				{
					return false;
				}
				return true;
			}
		}
	}
}
