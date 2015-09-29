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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Staging;

namespace Db4objects.Db4o.Tests.Common.Staging
{
	public class OldVersionReflectFieldAfterRefactorTestCase : AbstractDb4oTestCase
	{
		private const int IdValue = 42;

		public class ItemBefore
		{
			public int _id;

			public ItemBefore(int id)
			{
				_id = id;
			}
		}

		public class ItemAfter
		{
			public string _id;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestReflectField()
		{
			Store(new OldVersionReflectFieldAfterRefactorTestCase.ItemBefore(IdValue));
			Reopen();
			FileSession().StoredClass(typeof(OldVersionReflectFieldAfterRefactorTestCase.ItemBefore
				)).Rename(typeof(OldVersionReflectFieldAfterRefactorTestCase.ItemAfter).FullName
				);
			Reopen();
			ClassMetadata classMetadata = Container().ClassMetadataForName(typeof(OldVersionReflectFieldAfterRefactorTestCase.ItemAfter
				).FullName);
			ByRef originalField = new ByRef();
			classMetadata.TraverseDeclaredFields(new _IProcedure4_37(originalField));
			Assert.AreEqual(typeof(int).FullName, ((FieldMetadata)originalField.value).GetStoredType
				().GetName());
		}

		private sealed class _IProcedure4_37 : IProcedure4
		{
			public _IProcedure4_37(ByRef originalField)
			{
				this.originalField = originalField;
			}

			public void Apply(object field)
			{
				if (((FieldMetadata)originalField.value) == null && ((FieldMetadata)field).GetName
					().Equals("_id") && ((FieldMetadata)field).FieldType().GetName().Equals(typeof(int
					).FullName))
				{
					originalField.value = ((FieldMetadata)field);
				}
			}

			private readonly ByRef originalField;
		}
	}
}
