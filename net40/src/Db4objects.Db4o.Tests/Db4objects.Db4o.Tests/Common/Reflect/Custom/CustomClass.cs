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
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Reflect.Custom;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class CustomClass : IReflectClass
	{
		public CustomClassRepository _repository;

		public string _name;

		public IReflectField[] _fields;

		public CustomClass()
		{
		}

		public CustomClass(CustomClassRepository repository, string name, string[] fieldNames
			, Type[] fieldTypes)
		{
			// fields must be public so test works on less capable runtimes
			_repository = repository;
			_name = name;
			_fields = CreateFields(fieldNames, fieldTypes);
		}

		private IReflectField[] CreateFields(string[] fieldNames, Type[] fieldTypes)
		{
			IReflectField[] fields = new IReflectField[fieldNames.Length + 1];
			for (int i = 0; i < fieldNames.Length; ++i)
			{
				fields[i] = new Db4objects.Db4o.Tests.Common.Reflect.Custom.CustomField(_repository
					, i, fieldNames[i], fieldTypes[i]);
			}
			fields[fields.Length - 1] = new CustomUidField(_repository);
			return fields;
		}

		public virtual IReflectClass GetComponentType()
		{
			throw new NotImplementedException();
		}

		public virtual Db4objects.Db4o.Tests.Common.Reflect.Custom.CustomField CustomField
			(string name)
		{
			return (Db4objects.Db4o.Tests.Common.Reflect.Custom.CustomField)GetDeclaredField(
				name);
		}

		public virtual IReflectField GetDeclaredField(string name)
		{
			for (int i = 0; i < _fields.Length; ++i)
			{
				IReflectField field = _fields[i];
				if (field.GetName().Equals(name))
				{
					return field;
				}
			}
			return null;
		}

		public virtual IReflectField[] GetDeclaredFields()
		{
			return _fields;
		}

		public virtual IReflectClass GetDelegate()
		{
			return this;
		}

		public virtual IReflectMethod GetMethod(string methodName, IReflectClass[] paramClasses
			)
		{
			return null;
		}

		public virtual string GetName()
		{
			return _name;
		}

		public virtual IReflectClass GetSuperclass()
		{
			return null;
		}

		//		return _repository.reflectClass(java.lang.Object.class);
		public virtual bool IsAbstract()
		{
			return false;
		}

		public virtual bool IsArray()
		{
			return false;
		}

		public virtual bool IsAssignableFrom(IReflectClass type)
		{
			return Equals(type);
		}

		public virtual bool IsCollection()
		{
			return false;
		}

		public virtual bool IsInstance(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual bool IsInterface()
		{
			return false;
		}

		public virtual bool IsPrimitive()
		{
			return false;
		}

		public virtual object NewInstance()
		{
			return new PersistentEntry(_name, null, new object[_fields.Length - 1]);
		}

		public virtual IReflector Reflector()
		{
			return _repository._reflector;
		}

		public virtual IEnumerator CustomFields()
		{
			return Iterators.Filter(_fields, new _IPredicate4_109());
		}

		private sealed class _IPredicate4_109 : IPredicate4
		{
			public _IPredicate4_109()
			{
			}

			public bool Match(object candidate)
			{
				return candidate is Db4objects.Db4o.Tests.Common.Reflect.Custom.CustomField;
			}
		}

		public virtual object NullValue()
		{
			return null;
		}

		public virtual bool EnsureCanBeInstantiated()
		{
			return true;
		}

		public virtual bool IsImmutable()
		{
			return false;
		}
	}
}
