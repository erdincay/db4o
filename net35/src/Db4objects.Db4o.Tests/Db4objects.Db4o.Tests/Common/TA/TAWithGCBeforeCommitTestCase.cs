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
using Db4objects.Db4o.TA;
using Db4objects.Db4o.Tests.Common.TA;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class TAWithGCBeforeCommitTestCase : AbstractDb4oTestCase
	{
		private static readonly string UpdatedId = "X";

		private static readonly string OrigId = "U";

		public class Item : IActivatable
		{
			public string _id;

			public Item(string id)
			{
				_id = id;
			}

			public virtual void Id(string id)
			{
				Activate(ActivationPurpose.Write);
				_id = id;
			}

			public virtual string Id()
			{
				Activate(ActivationPurpose.Read);
				return _id;
			}

			[System.NonSerialized]
			private IActivator _activator;

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

			public virtual void Activate(ActivationPurpose purpose)
			{
				if (this._activator == null)
				{
					return;
				}
				this._activator.Activate(purpose);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.Add(new TransparentPersistenceSupport());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new TAWithGCBeforeCommitTestCase.Item(OrigId));
		}

		public virtual void TestCommit()
		{
			TAWithGCBeforeCommitTestCase.Item item = (TAWithGCBeforeCommitTestCase.Item)((TAWithGCBeforeCommitTestCase.Item
				)RetrieveOnlyInstance(typeof(TAWithGCBeforeCommitTestCase.Item)));
			item.Id(UpdatedId);
			item = null;
			Runtime.Gc();
			Db().Commit();
			item = (TAWithGCBeforeCommitTestCase.Item)((TAWithGCBeforeCommitTestCase.Item)RetrieveOnlyInstance
				(typeof(TAWithGCBeforeCommitTestCase.Item)));
			Db().Refresh(item, int.MaxValue);
			Assert.AreEqual(UpdatedId, item.Id());
		}

		public virtual void TestRollback()
		{
			TAWithGCBeforeCommitTestCase.Item item = (TAWithGCBeforeCommitTestCase.Item)((TAWithGCBeforeCommitTestCase.Item
				)RetrieveOnlyInstance(typeof(TAWithGCBeforeCommitTestCase.Item)));
			item.Id(UpdatedId);
			item = null;
			Runtime.Gc();
			Db().Rollback();
			item = (TAWithGCBeforeCommitTestCase.Item)((TAWithGCBeforeCommitTestCase.Item)RetrieveOnlyInstance
				(typeof(TAWithGCBeforeCommitTestCase.Item)));
			Db().Refresh(item, int.MaxValue);
			Assert.AreEqual(OrigId, item.Id());
		}

		public static void Main(string[] args)
		{
			new TAWithGCBeforeCommitTestCase().RunAll();
		}
	}
}
