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
using Db4objects.Db4o.Tests.Common.Sampledata;
using Db4objects.Db4o.Tests.Common.Types.Arrays;

namespace Db4objects.Db4o.Tests.Common.Types.Arrays
{
	public class TypedDerivedArrayTestCase : AbstractDb4oTestCase
	{
		private static readonly MoleculeData[] Array = new MoleculeData[] { new MoleculeData
			("TypedDerivedArray") };

		public class Data
		{
			public AtomData[] _array;

			public Data(AtomData[] AtomDatas)
			{
				this._array = AtomDatas;
			}
		}

		protected override void Store()
		{
			Db().Store(new TypedDerivedArrayTestCase.Data(Array));
		}

		public virtual void Test()
		{
			TypedDerivedArrayTestCase.Data data = (TypedDerivedArrayTestCase.Data)((TypedDerivedArrayTestCase.Data
				)RetrieveOnlyInstance(typeof(TypedDerivedArrayTestCase.Data)));
			Assert.IsTrue(data._array is MoleculeData[], "Expected instance of " + typeof(MoleculeData
				[]) + ", but got " + data._array);
			ArrayAssert.AreEqual(Array, data._array);
		}
	}
}
