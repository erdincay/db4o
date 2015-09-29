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
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Net;
using Sharpen.Lang;

namespace OManager.DataLayer.Reflection
{
    public class TypeImpl : IType
    {
        private readonly IReflectClass _type;
        private readonly ITypeResolver _parentResolver;

        public TypeImpl(IReflectClass type, ITypeResolver parentResolver)
        {
            _type = type;
            _parentResolver = parentResolver;
        }

        public void SetField(object onObject, string fieldName, object value)
        {
        	IReflectField field = _type.GetDeclaredField(fieldName);
			if (field == null)
				throw new ArgumentException("No field '" + fieldName + "' found in '" + FullName + "'");

			field.Set(onObject, value);
        }

    	public object Cast(object value)
        {
    		System.Type type = NetReflector.ToNative(_type);
            if (null == type)
                return null;

            if (IsNullable)
            {
                type = type.GetGenericArguments()[0];
            }

            return type.IsEnum 
							? ConvertEnum(type, value)
							: Convert.ChangeType(value, type);
        }

    	private static object ConvertEnum(Type type, object value)
    	{
    		return (value is int) ? Enum.ToObject(type, value) : Enum.Parse(type, (string) value);
    	}

    	public string DisplayName
        {
            get
            {
                return NormalizeName(FullName);
            }
        }

        public string FullName
        {
            get
            {
                return _type.GetName();
            }
        }

        public bool HasIdentity
        {
            get
            {
				System.Type type = NetReflector.ToNative(_type);
                return type == null || (!type.IsValueType && !IsString());
            }
        }

        public bool IsEditable
        {
            get
            {
                return (IsPrimitive || IsNullable || IsEnum) && !IsArray && !IsCollection;
            }
        }

    	public bool IsPrimitive
        {
            get { return _type.IsPrimitive() || IsString(); }
        }

        public bool IsCollection
        {
            get { return _type.IsCollection(); }
        }

        public bool IsArray
        {
            get { return _type.IsArray(); }
        }

        public bool IsNullable
        {
            get
            {
                string name = typeof(Nullable<>).FullName;
                return (_type.GetName().StartsWith(name) && !_type.IsArray());
            }
        }

        public bool IsSameAs(Type other)
        {
            return _type.GetName() == TypeReference.FromType(other).GetUnversionedName();
        }

        public override string ToString()
        {
            return "TypeImpl( " + _type + " )";
        }

        public IType UnderlyingType
        {
            get
            {
                if (!IsNullable)
                    throw new InvalidOperationException("Type must be a nullable type in order to have an 'UnderlyingType'.");

                GenericTypeReference typeReference = (GenericTypeReference) TypeReference.FromString(_type.GetName());
                return _parentResolver.Resolve(typeReference.GenericArguments[0].SimpleName);
            }
        }

		private bool IsEnum
		{
			get
			{
				Type type = NetReflector.ToNative(_type);
				return type != null ? type.IsEnum : false;
			}
		}
		
		private bool IsString()
        {
            return _type.GetName().StartsWith(typeof(string).FullName);
        }

        private string NormalizeName(string typeName)
        {
            return RemoveAssemblyName(NormalizeNullable(typeName));
        }

        private string NormalizeNullable(string typeName)
        {
            return IsNullable
                ? DecorateNullableName(typeName)
                : typeName;
        }

        private static string DecorateNullableName(string typeName)
        {
            GenericTypeReference typeRef = (GenericTypeReference)TypeReference.FromString(typeName);
            TypeReference underlyingType = typeRef.GenericArguments[0];

            return "Nullable<" + underlyingType.SimpleName + ">";
        }

        private static string RemoveAssemblyName(string typeName)
        {
            int index = typeName.LastIndexOf(',');
            return index >= 0 ? typeName.Remove(index) : typeName;
        }
    }
}
