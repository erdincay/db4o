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
using Db4objects.Db4o.Tests.Common.Reflect.Custom;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class CustomUidField : IReflectField
	{
		public CustomClassRepository _repository;

		public CustomUidField()
		{
		}

		public CustomUidField(CustomClassRepository repository)
		{
			_repository = repository;
		}

		public virtual object Get(object onObject)
		{
			return Entry(onObject).uid;
		}

		private PersistentEntry Entry(object onObject)
		{
			return ((PersistentEntry)onObject);
		}

		public virtual IReflectClass GetFieldType()
		{
			return _repository.ForFieldType(typeof(object));
		}

		public virtual string GetName()
		{
			return "uid";
		}

		public virtual object IndexEntry(object orig)
		{
			throw new NotImplementedException();
		}

		public virtual IReflectClass IndexType()
		{
			throw new NotImplementedException();
		}

		public virtual bool IsPublic()
		{
			return true;
		}

		public virtual bool IsStatic()
		{
			return false;
		}

		public virtual bool IsTransient()
		{
			return false;
		}

		public virtual void Set(object onObject, object value)
		{
			Entry(onObject).uid = value;
		}

		public override string ToString()
		{
			return "CustomUidField()";
		}
	}
}
