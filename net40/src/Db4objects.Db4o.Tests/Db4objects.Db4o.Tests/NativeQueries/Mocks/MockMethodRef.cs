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
using System.Reflection;
using Db4oUnit;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.Tests.NativeQueries.Mocks;

namespace Db4objects.Db4o.Tests.NativeQueries.Mocks
{
	public class MockMethodRef : IMethodRef
	{
		private readonly MethodInfo _method;

		public MockMethodRef(MethodInfo method)
		{
			_method = method;
		}

		public virtual string Name
		{
			get
			{
				return _method.Name;
			}
		}

		public virtual ITypeRef[] ParamTypes
		{
			get
			{
				Type[] paramTypes = Sharpen.Runtime.GetParameterTypes(_method);
				ITypeRef[] types = new ITypeRef[paramTypes.Length];
				for (int i = 0; i < paramTypes.Length; ++i)
				{
					types[i] = TypeRef(paramTypes[i]);
				}
				return types;
			}
		}

		public virtual ITypeRef DeclaringType
		{
			get
			{
				return TypeRef(_method.DeclaringType);
			}
		}

		private ITypeRef TypeRef(Type type)
		{
			return new MockTypeRef(type);
		}

		public virtual ITypeRef ReturnType
		{
			get
			{
				return TypeRef(_method.ReturnType);
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is IMethodRef))
			{
				return false;
			}
			IMethodRef other = (IMethodRef)obj;
			return Name.Equals(other.Name) && Check.ObjectsAreEqual(DeclaringType, other.DeclaringType
				) && Check.ObjectsAreEqual(ReturnType, other.ReturnType) && Check.ArraysAreEqual
				(ParamTypes, other.ParamTypes);
		}
	}
}
#endif // !SILVERLIGHT
