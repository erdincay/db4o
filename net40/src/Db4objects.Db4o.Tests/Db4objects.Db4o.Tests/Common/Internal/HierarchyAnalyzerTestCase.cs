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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Metadata;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Internal;

namespace Db4objects.Db4o.Tests.Common.Internal
{
	public class HierarchyAnalyzerTestCase : AbstractDb4oTestCase
	{
		public class A
		{
		}

		public class BA : HierarchyAnalyzerTestCase.A
		{
		}

		public class CBA : HierarchyAnalyzerTestCase.BA
		{
		}

		public class DA : HierarchyAnalyzerTestCase.A
		{
		}

		public class E
		{
		}

		public virtual void TestRemovedImmediateSuperclass()
		{
			AssertDiffBetween(typeof(HierarchyAnalyzerTestCase.DA), typeof(HierarchyAnalyzerTestCase.CBA
				), new HierarchyAnalyzer.Diff[] { new HierarchyAnalyzer.Removed(ProduceClassMetadata
				(typeof(HierarchyAnalyzerTestCase.BA))), new HierarchyAnalyzer.Same(ProduceClassMetadata
				(typeof(HierarchyAnalyzerTestCase.A))) });
		}

		public virtual void TestRemoveTopLevelSuperclass()
		{
			AssertDiffBetween(typeof(HierarchyAnalyzerTestCase.E), typeof(HierarchyAnalyzerTestCase.BA
				), new HierarchyAnalyzer.Diff[] { new HierarchyAnalyzer.Removed(ProduceClassMetadata
				(typeof(HierarchyAnalyzerTestCase.A))) });
		}

		public virtual void TestAddedImmediateSuperClass()
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_50(this));
		}

		private sealed class _ICodeBlock_50 : ICodeBlock
		{
			public _ICodeBlock_50(HierarchyAnalyzerTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.AssertDiffBetween(typeof(HierarchyAnalyzerTestCase.CBA), typeof(HierarchyAnalyzerTestCase.DA
					), new HierarchyAnalyzer.Diff[] {  });
			}

			private readonly HierarchyAnalyzerTestCase _enclosing;
		}

		public virtual void TestAddedTopLevelSuperClass()
		{
			Assert.Expect(typeof(InvalidOperationException), new _ICodeBlock_58(this));
		}

		private sealed class _ICodeBlock_58 : ICodeBlock
		{
			public _ICodeBlock_58(HierarchyAnalyzerTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.AssertDiffBetween(typeof(HierarchyAnalyzerTestCase.BA), typeof(HierarchyAnalyzerTestCase.E
					), new HierarchyAnalyzer.Diff[] {  });
			}

			private readonly HierarchyAnalyzerTestCase _enclosing;
		}

		private void AssertDiffBetween(Type runtimeClass, Type storedClass, HierarchyAnalyzer.Diff
			[] expectedDiff)
		{
			ClassMetadata classMetadata = ProduceClassMetadata(storedClass);
			IReflectClass reflectClass = ReflectClass(runtimeClass);
			IList ancestors = new HierarchyAnalyzer(classMetadata, reflectClass).Analyze();
			AssertDiff(ancestors, expectedDiff);
		}

		private ClassMetadata ProduceClassMetadata(Type storedClass)
		{
			return Container().ProduceClassMetadata(ReflectClass(storedClass));
		}

		private void AssertDiff(IList actual, HierarchyAnalyzer.Diff[] expected)
		{
			Iterator4Assert.AreEqual(Iterators.Iterate(expected), Iterators.Iterator(actual));
		}
	}
}
