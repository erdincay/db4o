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
using System.Reflection;
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public partial class Pre7_1ObjectContainerAdapter : AbstractObjectContainerAdapter
	{
		public override void Store(object obj)
		{
			StoreObject(obj);
		}

		public override void Store(object obj, int depth)
		{
			StoreObject(obj, depth);
		}

		private void StoreObject(object obj)
		{
			try
			{
				StoreInternal(ResolveSetMethod(), new object[] { obj });
			}
			catch (Exception e)
			{
				Assert.Fail("Call to set method failed.", e);
			}
		}

		private void StoreObject(object obj, int depth)
		{
			try
			{
				StoreInternal(ResolveSetWithDepthMethod(), new object[] { obj, depth });
			}
			catch (Exception e)
			{
				Assert.Fail("Call to set method failed.", e);
			}
		}

		public virtual void StoreInternal(MethodInfo method, object[] args)
		{
			try
			{
				method.Invoke(db, args);
			}
			catch (Exception e)
			{
				Assert.Fail(e.ToString());
				Sharpen.Runtime.PrintStackTrace(e);
			}
		}

		/// <exception cref="System.Exception"></exception>
		private MethodInfo ResolveSetWithDepthMethod()
		{
			if (setWithDepthMethod != null)
			{
				return setWithDepthMethod;
			}
			setWithDepthMethod = db.GetType().GetMethod(SetMethodName(), new Type[] { typeof(
				object), typeof(int) });
			return setWithDepthMethod;
		}

		/// <exception cref="System.Exception"></exception>
		private MethodInfo ResolveSetMethod()
		{
			if (setMethod != null)
			{
				return setMethod;
			}
			setMethod = db.GetType().GetMethod(SetMethodName(), new Type[] { typeof(object) }
				);
			return setMethod;
		}

		private MethodInfo setWithDepthMethod = null;

		private MethodInfo setMethod = null;
	}
}
