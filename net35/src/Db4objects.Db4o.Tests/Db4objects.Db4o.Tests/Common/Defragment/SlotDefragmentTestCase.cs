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
using System.IO;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Defragment;

namespace Db4objects.Db4o.Tests.Common.Defragment
{
	public class SlotDefragmentTestCase : DefragmentTestCaseBase
	{
		/// <exception cref="System.Exception"></exception>
		public virtual void TestPrimitiveIndex()
		{
			SlotDefragmentFixture.AssertIndex(SlotDefragmentFixture.PrimitiveFieldname, SourceFile
				(), FreshDb4oConfigurationProvider());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestWrapperIndex()
		{
			SlotDefragmentFixture.AssertIndex(SlotDefragmentFixture.WrapperFieldname, SourceFile
				(), FreshDb4oConfigurationProvider());
		}

		private IClosure4 FreshDb4oConfigurationProvider()
		{
			return new _IClosure4_30(this);
		}

		private sealed class _IClosure4_30 : IClosure4
		{
			public _IClosure4_30(SlotDefragmentTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				return this._enclosing.NewConfiguration();
			}

			private readonly SlotDefragmentTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestTypedObjectIndex()
		{
			SlotDefragmentFixture.ForceIndex(SourceFile(), NewConfiguration());
			Db4objects.Db4o.Defragment.Defragment.Defrag(NewDefragmentConfig(SourceFile(), BackupFile
				()));
			IObjectContainer db = Db4oEmbedded.OpenFile(NewConfiguration(), SourceFile());
			IQuery query = db.Query();
			query.Constrain(typeof(SlotDefragmentFixture.Data));
			query.Descend(SlotDefragmentFixture.TypedobjectFieldname).Descend(SlotDefragmentFixture
				.PrimitiveFieldname).Constrain(SlotDefragmentFixture.Value);
			IObjectSet result = query.Execute();
			Assert.AreEqual(1, result.Count);
			db.Close();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestNoForceDelete()
		{
			Db4objects.Db4o.Defragment.Defragment.Defrag(NewDefragmentConfig(SourceFile(), BackupFile
				()));
			Assert.Expect(typeof(IOException), new _ICodeBlock_51(this));
		}

		private sealed class _ICodeBlock_51 : ICodeBlock
		{
			public _ICodeBlock_51(SlotDefragmentTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4objects.Db4o.Defragment.Defragment.Defrag(this._enclosing.SourceFile(), this._enclosing
					.BackupFile());
			}

			private readonly SlotDefragmentTestCase _enclosing;
		}

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			new Sharpen.IO.File(SourceFile()).Delete();
			new Sharpen.IO.File(BackupFile()).Delete();
			SlotDefragmentFixture.CreateFile(SourceFile(), NewConfiguration());
		}

		private DefragmentConfig NewDefragmentConfig(string sourceFile, string backupFile
			)
		{
			DefragmentConfig config = new DefragmentConfig(sourceFile, backupFile);
			config.Db4oConfig(NewConfiguration());
			return config;
		}
	}
}
