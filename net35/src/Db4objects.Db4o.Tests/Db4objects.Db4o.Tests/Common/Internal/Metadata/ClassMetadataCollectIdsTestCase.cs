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
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Internal.Metadata;

namespace Db4objects.Db4o.Tests.Common.Internal.Metadata
{
	public class ClassMetadataCollectIdsTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		public class Item
		{
			public string name;

			public ClassMetadataCollectIdsTestCase.Item typedRef;

			public object untypedRef;

			public object untypedArray;

			public Item(string name, ClassMetadataCollectIdsTestCase.Item ref1, ClassMetadataCollectIdsTestCase.Item
				 ref2, object[] untypedArray)
			{
				this.name = name;
				this.typedRef = ref1;
				this.untypedRef = ref2;
				this.untypedArray = untypedArray;
			}

			public virtual object UntypedElementAt(int index)
			{
				return ((object[])untypedArray)[index];
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ClassMetadataCollectIdsTestCase.Item("root", new ClassMetadataCollectIdsTestCase.Item
				("typed", null, null, new object[] {  }), new ClassMetadataCollectIdsTestCase.Item
				("untyped", null, null, new object[] {  }), new object[] { new ClassMetadataCollectIdsTestCase.Item
				("array", null, null, new object[] {  }) }));
		}

		public virtual void TestCollectIdsByFieldName()
		{
			ClassMetadataCollectIdsTestCase.Item root = QueryRootItem();
			CollectIdContext context = CollectIdContext.ForID(Trans(), (int)Db().GetID(root));
			context.ClassMetadata().CollectIDs(context, "typedRef");
			AssertCollectedIds(context, new object[] { root.typedRef });
		}

		public virtual void TestCollectIds()
		{
			ClassMetadataCollectIdsTestCase.Item root = QueryRootItem();
			CollectIdContext context = CollectIdContext.ForID(Trans(), (int)Db().GetID(root));
			context.ClassMetadata().CollectIDs(context);
			AssertCollectedIds(context, new object[] { root.typedRef, root.untypedRef, root.UntypedElementAt
				(0) });
		}

		private void AssertCollectedIds(CollectIdContext context, object[] expectedReferences
			)
		{
			Iterator4Assert.SameContent(Iterators.Map(expectedReferences, new _IFunction4_66(
				this)), new TreeKeyIterator(context.Ids()));
		}

		private sealed class _IFunction4_66 : IFunction4
		{
			public _IFunction4_66(ClassMetadataCollectIdsTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object reference)
			{
				return (int)this._enclosing.Db().GetID(reference);
			}

			private readonly ClassMetadataCollectIdsTestCase _enclosing;
		}

		private ClassMetadataCollectIdsTestCase.Item QueryRootItem()
		{
			return ((ClassMetadataCollectIdsTestCase.Item)QueryItemByName("root").Next());
		}

		private IObjectSet QueryItemByName(string itemName)
		{
			IQuery query = NewQuery(typeof(ClassMetadataCollectIdsTestCase.Item));
			query.Descend("name").Constrain(itemName);
			return query.Execute();
		}
	}
}
