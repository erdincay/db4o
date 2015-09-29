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
using Db4objects.Db4o.Internal.Metadata;
using Db4objects.Db4o.Tests.Common.Internal.Metadata;

namespace Db4objects.Db4o.Tests.Common.Internal.Metadata
{
	public class ClassMetadataIntegrationTestCase : AbstractDb4oTestCase
	{
		public class SuperClazz
		{
			public int _id;

			public string _name;
		}

		public class SubClazz : ClassMetadataIntegrationTestCase.SuperClazz
		{
			public int _age;
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ClassMetadataIntegrationTestCase.SubClazz());
		}

		public virtual void TestFieldTraversal()
		{
			Collection4 expectedNames = new Collection4(new ArrayIterator4(new string[] { "_id"
				, "_name", "_age" }));
			ClassMetadata classMetadata = ClassMetadataFor(typeof(ClassMetadataIntegrationTestCase.SubClazz
				));
			classMetadata.TraverseAllAspects(new _TraverseFieldCommand_31(expectedNames));
			Assert.IsTrue(expectedNames.IsEmpty());
		}

		private sealed class _TraverseFieldCommand_31 : TraverseFieldCommand
		{
			public _TraverseFieldCommand_31(Collection4 expectedNames)
			{
				this.expectedNames = expectedNames;
			}

			protected override void Process(FieldMetadata field)
			{
				Assert.IsNotNull(expectedNames.Remove(field.GetName()));
			}

			private readonly Collection4 expectedNames;
		}

		public virtual void TestPrimitiveArrayMetadataIsPrimitiveTypeMetadata()
		{
			ClassMetadata byteArrayMetadata = Container().ProduceClassMetadata(ReflectClass(typeof(
				byte[])));
			Assert.IsInstanceOf(typeof(PrimitiveTypeMetadata), byteArrayMetadata);
		}
	}
}
