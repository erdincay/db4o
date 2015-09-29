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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Reflect.Custom;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	/// <summary>Type information is handled by CustomClassRepository.</summary>
	/// <remarks>Type information is handled by CustomClassRepository.</remarks>
	public class CustomReflector : IReflector
	{
		private readonly IReflector _delegate = Platform4.ReflectorForType(typeof(Db4objects.Db4o.Tests.Common.Reflect.Custom.CustomReflector
			));

		private readonly CustomClassRepository _classRepository;

		private IReflector _parent;

		public CustomReflector(CustomClassRepository classRepository)
		{
			classRepository.Initialize(this);
			_classRepository = classRepository;
		}

		public virtual IReflectArray Array()
		{
			return _delegate.Array();
		}

		public virtual IReflectClass ForClass(Type clazz)
		{
			return _delegate.ForClass(clazz);
		}

		public virtual IReflectClass ForName(string className)
		{
			LogMethodCall("forName", className);
			IReflectClass klass = RepositoryForName(className);
			if (null != klass)
			{
				return klass;
			}
			return _delegate.ForName(className);
		}

		private IReflectClass RepositoryForName(string className)
		{
			if (_classRepository == null)
			{
				return null;
			}
			return _classRepository.ForName(className);
		}

		public virtual IReflectClass ForObject(object obj)
		{
			LogMethodCall("forObject", obj);
			IReflectClass klass = RepositoryForObject(obj);
			if (null != klass)
			{
				return klass;
			}
			return _delegate.ForObject(obj);
		}

		private IReflectClass RepositoryForObject(object obj)
		{
			if (_classRepository == null)
			{
				return null;
			}
			if (!(obj is PersistentEntry))
			{
				return null;
			}
			PersistentEntry entry = (PersistentEntry)obj;
			return _classRepository.ForName(entry.className);
		}

		public virtual bool IsCollection(IReflectClass clazz)
		{
			return _delegate.IsCollection(clazz);
		}

		public virtual void SetParent(IReflector reflector)
		{
			LogMethodCall("setParent", reflector);
			_parent = reflector;
			_delegate.SetParent(reflector);
		}

		public virtual object DeepClone(object context)
		{
			LogMethodCall("deepClone", context);
			throw new NotImplementedException();
		}

		public virtual CustomClass DefineClass(string className, string[] fieldNames, string
			[] fieldTypes)
		{
			return _classRepository.DefineClass(className, fieldNames, fieldTypes);
		}

		public override string ToString()
		{
			return "CustomReflector(" + _classRepository + ")";
		}

		private void LogMethodCall(string methodName, object arg)
		{
			Logger.LogMethodCall(this, methodName, arg);
		}

		public virtual IReflectClass ForFieldType(Type type)
		{
			return _parent.ForClass(type);
		}

		public virtual IEnumerator CustomClasses()
		{
			return _classRepository.Iterator();
		}

		public virtual void Configuration(IReflectorConfiguration config)
		{
			_delegate.Configuration(config);
		}
	}
}
