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
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Tests.Common.Events;

namespace Db4objects.Db4o.Tests.Common.Events
{
	public class EventsTestCaseBase : AbstractDb4oTestCase
	{
		public sealed class Item
		{
			public int id;

			public Item()
			{
			}

			public Item(int id)
			{
				this.id = id;
			}
		}

		protected sealed class EventLog
		{
			public bool xing;

			public bool xed;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new EventsTestCaseBase.Item(1));
		}

		protected virtual void AssertClientTransaction(EventArgs args)
		{
			Assert.AreSame(Trans(), ((TransactionalEventArgs)args).Transaction());
		}
	}
}
