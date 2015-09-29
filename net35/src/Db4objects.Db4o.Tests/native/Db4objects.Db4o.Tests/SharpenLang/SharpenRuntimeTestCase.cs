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
#if !CF
using System;
using System.Linq;
using System.Linq.Expressions;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.SharpenLang
{
	public class SharpenRuntimeTestCase : ITestCase
	{
		public void Test()
		{
			//The following expression triggers this bug in CLR
			//https://connect.microsoft.com/VisualStudio/feedback/details/566678/field-of-generic-class-missing-when-getmembers-called-with-declaredonly-if-it-was-used-in-an-expression
			
			Expression<Func<Item<string>, bool>> exp = item => item.a == 100;

			var declaredFieldNames = Sharpen.Runtime.GetDeclaredFields(typeof (Item<string>)).Select( f => f.Name );
			IteratorAssert.SameContent(new [] {"a", "b"}, declaredFieldNames);
		}

		class Item<T>
		{
			public int a;
			public int b;
		}
	}
}
#endif