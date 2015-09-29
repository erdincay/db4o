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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	/// <exclude></exclude>
	public class ExceptionsInCallbackTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new ExceptionsInCallbackTestCase().RunSolo();
		}

		public class Holder
		{
			public IList list;

			public int i;
		}

		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(ExceptionsInCallbackTestCase.Holder)).CascadeOnUpdate(true
				);
			config.ObjectClass(typeof(ExceptionsInCallbackTestCase.Holder)).CascadeOnDelete(true
				);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestExceptionInUpdateCallback()
		{
			BooleanByRef doThrow = new BooleanByRef();
			EventRegistryFactory.ForObjectContainer(Db()).Updated += new System.EventHandler<Db4objects.Db4o.Events.ObjectInfoEventArgs>
				(new _IEventListener4_42(doThrow).OnEvent);
			ExceptionsInCallbackTestCase.Holder holder = new ExceptionsInCallbackTestCase.Holder
				();
			ExceptionsInCallbackTestCase.Item item = new ExceptionsInCallbackTestCase.Item();
			Store(holder);
			Store(item);
			Commit();
			doThrow.value = true;
			holder.list = new ArrayList();
			holder.list.Add(item);
			try
			{
				Db().Store(holder, int.MaxValue);
			}
			catch (Exception)
			{
			}
			// rex.printStackTrace();
			Checkdb();
			Commit();
			Checkdb();
			Reopen();
			Checkdb();
		}

		private sealed class _IEventListener4_42
		{
			public _IEventListener4_42(BooleanByRef doThrow)
			{
				this.doThrow = doThrow;
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.ObjectInfoEventArgs args
				)
			{
				if (doThrow.value)
				{
					if (((ObjectInfoEventArgs)args).Info.GetObject().GetType().Equals(typeof(ExceptionsInCallbackTestCase.Item
						)))
					{
						throw new Exception();
					}
				}
			}

			private readonly BooleanByRef doThrow;
		}

		private void Checkdb()
		{
			ConsistencyReport consistencyReport = new ConsistencyChecker((LocalObjectContainer
				)Container()).CheckSlotConsistency();
			Assert.IsTrue(consistencyReport.Consistent(), consistencyReport.ToString());
		}
	}
}
