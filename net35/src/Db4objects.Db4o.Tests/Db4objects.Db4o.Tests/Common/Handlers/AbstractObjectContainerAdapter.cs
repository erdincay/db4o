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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public abstract class AbstractObjectContainerAdapter : IObjectContainerAdapter
	{
		protected IExtObjectContainer db;

		public virtual IObjectContainerAdapter ForContainer(IExtObjectContainer db)
		{
			this.db = db;
			return this;
		}

		public virtual void Commit()
		{
			db.Commit();
		}

		public virtual void Delete(object obj)
		{
			db.Delete(obj);
		}

		public virtual IQuery Query()
		{
			return db.Query();
		}

		public AbstractObjectContainerAdapter() : base()
		{
		}

		public abstract void Store(object arg1);

		public abstract void Store(object arg1, int arg2);
	}
}
