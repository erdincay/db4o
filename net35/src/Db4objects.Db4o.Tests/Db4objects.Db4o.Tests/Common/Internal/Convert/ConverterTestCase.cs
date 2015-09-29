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
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Convert;
using Db4objects.Db4o.Internal.Convert.Conversions;
using Db4objects.Db4o.Tests.Common.Internal.Convert;

namespace Db4objects.Db4o.Tests.Common.Internal.Convert
{
	public class ConverterTestCase : ITestSuiteBuilder
	{
		public virtual IEnumerator GetEnumerator()
		{
			int startingVersion = ClassIndexesToBTrees_5_5.Version;
			return Iterators.Map(Iterators.Range(startingVersion, Converter.Version + 1), new 
				_IFunction4_17(this));
		}

		private sealed class _IFunction4_17 : IFunction4
		{
			public _IFunction4_17(ConverterTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Apply(object version)
			{
				return new _ITest_19(this, version);
			}

			private sealed class _ITest_19 : ITest
			{
				public _ITest_19(_IFunction4_17 _enclosing, object version)
				{
					this._enclosing = _enclosing;
					this.version = version;
				}

				public string Label()
				{
					return "ConverterTestCase: from " + ((int)version) + " to " + Converter.Version;
				}

				public void Run()
				{
					this._enclosing._enclosing.AssertConverterBehaviorForVersion((((int)version)));
				}

				public bool IsLeafTest()
				{
					return true;
				}

				public ITest Transmogrify(IFunction4 fun)
				{
					return ((ITest)fun.Apply(this));
				}

				private readonly _IFunction4_17 _enclosing;

				private readonly object version;
			}

			private readonly ConverterTestCase _enclosing;
		}

		private void AssertConverterBehaviorForVersion(int converterVersion)
		{
			ConverterTestCase.RecordingStage stage = new ConverterTestCase.RecordingStage(converterVersion
				);
			Converter.Convert(stage);
			Iterator4Assert.AreEqual(Iterators.Iterator(ExpectedConversionsFor(converterVersion
				)), Iterators.Iterator(stage.Conversions()));
		}

		private ArrayList ExpectedConversionsFor(int converterVersion)
		{
			ArrayList expected = new ArrayList();
			for (int version = converterVersion + 1; version <= Converter.Version; ++version)
			{
				expected.Add(Converter.Instance().ConversionFor(version));
			}
			return expected;
		}

		private sealed class RecordingStage : ConversionStage
		{
			private readonly int _converterVersion;

			private readonly ArrayList _conversions = new ArrayList();

			public RecordingStage(int converterVersion) : base(null)
			{
				_converterVersion = converterVersion;
			}

			public override void Accept(Conversion conversion)
			{
				Conversions().Add(conversion);
			}

			public override int ConverterVersion()
			{
				return _converterVersion;
			}

			public ArrayList Conversions()
			{
				return _conversions;
			}
		}
	}
}
