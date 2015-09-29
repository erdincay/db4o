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
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class BackupDb4oIOExceptionTestCase : Db4oIOExceptionTestCaseBase, IOptOutInMemory
	{
		public static void Main(string[] args)
		{
			new BackupDb4oIOExceptionTestCase().RunAll();
		}

		private static readonly string BackupFile = "backup.db4o";

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupBeforeStore()
		{
			base.Db4oSetupBeforeStore();
			File4.Delete(BackupFile);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oTearDownBeforeClean()
		{
			base.Db4oTearDownBeforeClean();
			File4.Delete(BackupFile);
		}

		public virtual void TestBackup()
		{
			Assert.Expect(typeof(Db4oIOException), new _ICodeBlock_31(this));
		}

		private sealed class _ICodeBlock_31 : ICodeBlock
		{
			public _ICodeBlock_31(BackupDb4oIOExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				this._enclosing.TriggerException(true);
				this._enclosing.Db().Backup(BackupDb4oIOExceptionTestCase.BackupFile);
			}

			private readonly BackupDb4oIOExceptionTestCase _enclosing;
		}
	}
}
