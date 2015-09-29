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
using System.Collections;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Jre5.Collections.Typehandler;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
	public class ListTypeHandlerPersistedCountTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ListTypeHandlerPersistedCountTestCase().RunAll();
		}

		public class TypedItem
		{
			internal ArrayList list;
		}

		public class InterfaceItem
		{
			internal IList list;
		}

		public class UntypedItem
		{
			internal object list;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(ArrayList))
				, new CollectionTypeHandler());
		}

		public virtual void TestTypedItem()
		{
			ListTypeHandlerPersistedCountTestCase.TypedItem typedItem = new ListTypeHandlerPersistedCountTestCase.TypedItem
				();
			typedItem.list = new ArrayList();
			Store(typedItem);
			Db4oAssert.PersistedCount(1, typeof(ArrayList));
		}

		public virtual void TestInterFaceItem()
		{
			ListTypeHandlerPersistedCountTestCase.InterfaceItem interfaceItem = new ListTypeHandlerPersistedCountTestCase.InterfaceItem
				();
			interfaceItem.list = new ArrayList();
			Store(interfaceItem);
			Db4oAssert.PersistedCount(1, typeof(ArrayList));
		}

		public virtual void TestUntypedItem()
		{
			ListTypeHandlerPersistedCountTestCase.UntypedItem untypedItem = new ListTypeHandlerPersistedCountTestCase.UntypedItem
				();
			untypedItem.list = new ArrayList();
			Store(untypedItem);
			Db4oAssert.PersistedCount(1, typeof(ArrayList));
		}
	}
}
