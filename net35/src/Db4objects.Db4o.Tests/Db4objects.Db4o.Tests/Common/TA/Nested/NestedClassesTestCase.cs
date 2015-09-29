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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA.Nested;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.TA.Nested
{
	public class NestedClassesTestCase : AbstractDb4oTestCase, IOptOutTA
	{
		public static void Main(string[] args)
		{
			new NestedClassesTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			OuterClass outerObject = new OuterClass();
			outerObject._foo = 42;
			IActivatable objOne = (IActivatable)outerObject.CreateInnerObject();
			Store(objOne);
			IActivatable objTwo = (IActivatable)outerObject.CreateInnerObject();
			Store(objTwo);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			string property = Runtime.GetProperty("java.version");
			if (property != null && property.StartsWith("1.3"))
			{
				Sharpen.Runtime.Err.WriteLine("IGNORED: " + GetType() + " will fail when run against JDK1.3/JDK1.4"
					);
				return;
			}
			IObjectSet query = Db().Query(typeof(OuterClass.InnerClass));
			while (query.HasNext())
			{
				OuterClass.InnerClass innerObject = (OuterClass.InnerClass)query.Next();
				Assert.IsNull(innerObject.GetOuterObjectWithoutActivation());
				Assert.AreEqual(42, innerObject.GetOuterObject().Foo());
			}
		}
	}
}
