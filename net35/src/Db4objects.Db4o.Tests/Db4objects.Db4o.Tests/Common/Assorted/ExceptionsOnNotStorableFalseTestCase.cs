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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	/// <exclude></exclude>
	public class ExceptionsOnNotStorableFalseTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ExceptionsOnNotStorableFalseTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ExceptionsOnNotStorable(false);
			config.CallConstructors(true);
		}

		public class Item
		{
			public Item(object obj)
			{
				if (obj == null)
				{
					throw new Exception();
				}
			}

			public static ExceptionsOnNotStorableFalseTestCase.Item NewItem()
			{
				return new ExceptionsOnNotStorableFalseTestCase.Item(new object());
			}
		}

		public virtual void TestObjectContainerAliveAfterObjectNotStorableException()
		{
			ExceptionsOnNotStorableFalseTestCase.Item item = ExceptionsOnNotStorableFalseTestCase.Item
				.NewItem();
			bool exceptionOccurred = false;
			try
			{
				Store(item);
			}
			catch (ObjectNotStorableException)
			{
				exceptionOccurred = true;
			}
			Assert.IsFalse(exceptionOccurred);
		}
	}
}
