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
#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation.IO;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class IncompatibleFileFormatExceptionTestCase : TestWithTempFile, IDb4oTestCase
	{
		/// <exception cref="System.Exception"></exception>
		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(IncompatibleFileFormatExceptionTestCase)).Run();
		}

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			File4.Delete(TempFile());
			IoAdapter adapter = new RandomAccessFileAdapter();
			adapter = adapter.Open(TempFile(), false, 0, false);
			adapter.Write(new byte[] { 1, 2, 3 }, 3);
			adapter.Close();
		}

		public virtual void Test()
		{
			Assert.Expect(typeof(IncompatibleFileFormatException), new _ICodeBlock_31(this));
			File4.Delete(TempFile());
			IoAdapter adapter = new RandomAccessFileAdapter();
			Assert.IsFalse(adapter.Exists(TempFile()));
		}

		private sealed class _ICodeBlock_31 : ICodeBlock
		{
			public _ICodeBlock_31(IncompatibleFileFormatExceptionTestCase _enclosing)
			{
				this._enclosing = _enclosing;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				Db4oFactory.OpenFile(this._enclosing.TempFile());
			}

			private readonly IncompatibleFileFormatExceptionTestCase _enclosing;
		}
	}
}
#endif // !SILVERLIGHT
