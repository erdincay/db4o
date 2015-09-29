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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Tests.Common.Internal;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class ClassMetadataTypeHandlerIntegrationTestCase : AbstractDb4oTestCase
	{
		public class Item
		{
		}

		public class MyReferenceType
		{
		}

		public class MyReferenceTypeHandler : IReferenceTypeHandler
		{
			public virtual void Activate(IReferenceActivationContext context)
			{
			}

			public virtual void Defragment(IDefragmentContext context)
			{
			}

			/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
			public virtual void Delete(IDeleteContext context)
			{
			}

			public virtual void Write(IWriteContext context, object obj)
			{
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Configure(IConfiguration config)
		{
			config.RegisterTypeHandler(new SingleClassTypeHandlerPredicate(typeof(ClassMetadataTypeHandlerIntegrationTestCase.MyReferenceType
				)), new ClassMetadataTypeHandlerIntegrationTestCase.MyReferenceTypeHandler());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new ClassMetadataTypeHandlerIntegrationTestCase.Item());
			Store(new ClassMetadataTypeHandlerIntegrationTestCase.MyReferenceType());
		}

		public virtual void TestIsValueType()
		{
			Pair[] typeDescriptorArray = TypeDescriptors();
			for (int typeDescriptorIndex = 0; typeDescriptorIndex < typeDescriptorArray.Length
				; ++typeDescriptorIndex)
			{
				Pair typeDescriptor = typeDescriptorArray[typeDescriptorIndex];
				ClassMetadata classMetadata = Container().ClassMetadataForObject(typeDescriptor.first
					);
				Assert.AreEqual(((bool)typeDescriptor.second), classMetadata.IsValueType(), classMetadata
					.ToString());
			}
		}

		private Pair[] TypeDescriptors()
		{
			return new Pair[] { Pair(1, true), Pair(new DateTime(), true), Pair("astring", true
				), Pair(new ClassMetadataTypeHandlerIntegrationTestCase.Item(), false), Pair((new 
				int[] { 1 }), false), Pair((new DateTime[] { new DateTime() }), false), Pair((new 
				ClassMetadataTypeHandlerIntegrationTestCase.Item[] { new ClassMetadataTypeHandlerIntegrationTestCase.Item
				() }), false), Pair(new ClassMetadataTypeHandlerIntegrationTestCase.MyReferenceType
				(), false) };
		}

		private Pair Pair(object first, object second)
		{
			return new Pair(first, second);
		}
	}
}
