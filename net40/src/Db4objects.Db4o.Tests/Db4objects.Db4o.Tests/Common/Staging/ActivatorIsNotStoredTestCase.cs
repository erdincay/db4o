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
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	/// <summary>
	/// COR-1003
	/// If the Activator is not declared as transient,
	/// it will get restored as null from the database
	/// when activation happens.
	/// </summary>
	/// <remarks>
	/// COR-1003
	/// If the Activator is not declared as transient,
	/// it will get restored as null from the database
	/// when activation happens.
	/// </remarks>
	public class ActivatorIsNotStoredTestCase : AbstractDb4oTestCase
	{
		public class Item : IActivatable
		{
			public string _name;

			public IActivator _activator;

			public virtual void Activate(ActivationPurpose purpose)
			{
				if (this._activator != null)
				{
					this._activator.Activate(purpose);
				}
			}

			public virtual void Bind(IActivator activator)
			{
				if (this._activator == activator)
				{
					return;
				}
				if (activator != null && this._activator != null)
				{
					throw new InvalidOperationException();
				}
				this._activator = activator;
			}

			internal Item(ActivatorIsNotStoredTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly ActivatorIsNotStoredTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentActivationSupport());
			config.Add(new TransparentActivationSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			ActivatorIsNotStoredTestCase.Item item = new ActivatorIsNotStoredTestCase.Item(this
				);
			item._name = "one";
			Store(item);
		}

		public virtual void Test()
		{
			ActivatorIsNotStoredTestCase.Item item = ((ActivatorIsNotStoredTestCase.Item)RetrieveOnlyInstance
				(typeof(ActivatorIsNotStoredTestCase.Item)));
			item.Activate(ActivationPurpose.Write);
			Assert.IsNotNull(item._activator);
			Store(item);
			IQuery q = NewQuery(typeof(IActivator));
			Assert.AreEqual(0, q.Execute().Count);
		}
	}
}
