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
#if !SILVERLIGHT
using System;
using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Tests.NativeQueries.Mocks
{
	public class MockTypeRef : ITypeRef
	{
		private readonly Type _type;

		public MockTypeRef(Type type)
		{
			_type = type;
		}

		public virtual ITypeRef ElementType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public virtual bool IsPrimitive
		{
			get
			{
				return _type.IsPrimitive;
			}
		}

		public virtual string Name
		{
			get
			{
				return _type.FullName;
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ITypeRef))
			{
				return false;
			}
			ITypeRef other = (ITypeRef)obj;
			return IsPrimitive == other.IsPrimitive && Name.Equals(other.Name);
		}

		public override int GetHashCode()
		{
			return _type.GetHashCode();
		}
	}
}
#endif // !SILVERLIGHT
