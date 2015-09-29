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
using Db4objects.Db4o.Diagnostic;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class DeleteStringInUntypedFieldTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] args)
		{
			new DeleteStringInUntypedFieldTestCase().RunAll();
		}

		public class Item
		{
			public DeleteStringInUntypedFieldTestCase.Item _firstChild;

			public string _name;

			public object _untypedName;

			public DeleteStringInUntypedFieldTestCase.Item _secondChild;

			public Item(string name)
			{
				_name = name;
				_untypedName = name;
			}
		}

		public class DeleteListener : IDiagnosticListener
		{
			public bool _called;

			public virtual void OnDiagnostic(IDiagnostic d)
			{
				if (d is DeletionFailed)
				{
					_called = true;
				}
			}
		}

		private DeleteStringInUntypedFieldTestCase.DeleteListener _listener;

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			DeleteStringInUntypedFieldTestCase.Item item = new DeleteStringInUntypedFieldTestCase.Item
				("root");
			item._firstChild = new DeleteStringInUntypedFieldTestCase.Item("first");
			item._secondChild = new DeleteStringInUntypedFieldTestCase.Item("second");
			Store(item);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(DeleteStringInUntypedFieldTestCase.Item)).CascadeOnDelete
				(true);
			_listener = new DeleteStringInUntypedFieldTestCase.DeleteListener();
			config.Diagnostic().AddListener(_listener);
		}

		public virtual void Test()
		{
			IQuery q = ItemQuery();
			q.Descend("_name").Constrain("root");
			IObjectSet objectSet = q.Execute();
			DeleteStringInUntypedFieldTestCase.Item item = ((DeleteStringInUntypedFieldTestCase.Item
				)objectSet.Next());
			Db().Delete(item);
			Assert.IsFalse(_listener._called);
			Assert.AreEqual(0, ItemQuery().Execute().Count);
		}

		private IQuery ItemQuery()
		{
			IQuery q = Db().Query();
			q.Constrain(typeof(DeleteStringInUntypedFieldTestCase.Item));
			return q;
		}
	}
}
