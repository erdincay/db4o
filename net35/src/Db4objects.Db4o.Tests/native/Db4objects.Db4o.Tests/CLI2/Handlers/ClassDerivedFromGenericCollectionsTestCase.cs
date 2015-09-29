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
using System.Collections.Generic;
using System.Reflection;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.CLI2.Handlers
{
	public class ClassDerivedFromGenericCollectionsTestSuite : FixtureBasedTestSuite
	{
		public override Type[] TestUnits()
		{
			return new Type[]
			       	{
			       		typeof(ClassDerivedFromGenericCollectionsTestUnit),
			       	};
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			
			return new IFixtureProvider[]
			       	{
			       		new Db4oFixtureProvider(),
																
						new SimpleFixtureProvider(COLLECTION_INHERITED_TYPE_VARIABLE, 
														new object[] 
														{
															new CollectionObjectProvider<DirectlyConcrete>(),
                                                            new CollectionObjectProvider<DirectlyGeneric<string>>(),
                                                            new CollectionObjectProvider<IndirectlyConcrete>(),
                                                            new CollectionObjectProvider<IndirectlyGeneric<string>>(),
															//new CollectionObjectProvider<IndirectlyInheritedGenericWithDifferentParamTypes<int>>()
														})
			       	};
		}

		internal static readonly FixtureVariable COLLECTION_INHERITED_TYPE_VARIABLE = FixtureVariable.NewInstance("T");
	}

	public class IndirectlyInheritedGenericWithDifferentParamTypes<T> : DirectlyGeneric<string>
	{
		public IndirectlyInheritedGenericWithDifferentParamTypes()
		{
		}

		public IndirectlyInheritedGenericWithDifferentParamTypes(string[] args) : base(args)
		{
		}
	}

	public class IndirectlyGeneric<T> : DirectlyGeneric<T>
	{
		public IndirectlyGeneric()
		{
		}

		public IndirectlyGeneric(T []args) : base(args)
		{
		}
	}

	public class IndirectlyConcrete : DirectlyConcrete, ISomeInterface
	{
		public IndirectlyConcrete()
		{
		}

		public IndirectlyConcrete(string []args) : base(args)
		{
		}
	}

	public interface ISomeInterface
	{
	}

	public class DirectlyGeneric<T> : List<T>
	{
		public DirectlyGeneric()
		{
		}

		public DirectlyGeneric(T[] args)
		{
			AddRange(args);
		}
	}

	public class DirectlyConcrete : List<string>
	{
		public DirectlyConcrete()
		{
		}
		
		public DirectlyConcrete(string[] values)
		{
			AddRange(values);
		}
	}

	public class CollectionObjectProvider<T> : ILabeled, ICollectionInstantiator
	{
		public ICollection New(params string[] args)
		{
		    ConstructorInfo ctor = typeof(T).GetConstructor(new Type[] {typeof (string[])});
		    return (ICollection) ctor.Invoke(new object[] {args});
		}

	    public string Label()
		{
			return typeof(T).Name;
		}
	}

	public interface ICollectionInstantiator
	{
		ICollection New(params string[] args);
	}

	public class ClassDerivedFromGenericCollectionsTestUnit : AbstractDb4oTestCase
	{
		protected override void Store()
		{
			Store(new CollectionHolder(NewCurrentCollectionFor("foo", "bar")));
		}

		private static ICollection NewCurrentCollectionFor(params string[] args)
		{
			return CollectionInstantiatorVariable().New(args);
		}

		public void TestCollectionTypeHandlerDoesntThrows()
		{
			CollectionHolder holder = RetrieveOnlyInstance<CollectionHolder>();
			Iterator4Assert.SameContent(new object[] {"foo", "bar"}, holder._collection.GetEnumerator());
		}

		private static ICollectionInstantiator CollectionInstantiatorVariable()
		{
			return (ICollectionInstantiator) ClassDerivedFromGenericCollectionsTestSuite.COLLECTION_INHERITED_TYPE_VARIABLE.Value;
		}
	}

	public class CollectionHolder
	{
		public CollectionHolder()
		{
			_collection = null;	
		}

		public CollectionHolder(ICollection collection)
		{
			_collection = collection;
		}

		public ICollection _collection;
	}
}
