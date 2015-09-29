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
#if !SILVERLIGHT
using System.Collections;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Fieldindex;

namespace Db4objects.Db4o.Tests.Common.Fieldindex
{
	public class CommitAfterDroppedFieldIndexTestCase : Db4oClientServerTestCase
	{
		public class Item
		{
			public int _id;

			public Item(int id)
			{
				_id = id;
			}
		}

		private const int ObjectCount = 100;

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.ClientServer().PrefetchIDCount(1);
			config.ClientServer().BatchMessages(false);
			config.BTreeNodeSize(4);
		}

		public virtual void Test()
		{
			for (int i = 0; i < ObjectCount; i++)
			{
				Store(new CommitAfterDroppedFieldIndexTestCase.Item(1));
			}
			IStoredField storedField = FileSession().StoredClass(typeof(CommitAfterDroppedFieldIndexTestCase.Item
				)).StoredField("_id", null);
			storedField.CreateIndex();
			FileSession().Commit();
			IExtObjectContainer session = OpenNewSession();
			IObjectSet allItems = session.Query(typeof(CommitAfterDroppedFieldIndexTestCase.Item
				));
			for (IEnumerator itemIter = allItems.GetEnumerator(); itemIter.MoveNext(); )
			{
				CommitAfterDroppedFieldIndexTestCase.Item item = ((CommitAfterDroppedFieldIndexTestCase.Item
					)itemIter.Current);
				item._id++;
				session.Store(item);
			}
			// Making sure all storing has been processed.
			session.SetSemaphore("anySemaphore", 0);
			storedField.DropIndex();
			session.Commit();
			storedField.CreateIndex();
		}
	}
}
#endif // !SILVERLIGHT
