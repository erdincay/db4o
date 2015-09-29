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
using Db4oUnit.Fixtures;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Api;

namespace Db4objects.Db4o.Tests.Common.IO
{
	public class IoAdapterTestUnitBase : TestWithTempFile
	{
		protected IoAdapter _adapter;

		public IoAdapterTestUnitBase() : base()
		{
		}

		/// <exception cref="System.Exception"></exception>
		public override void SetUp()
		{
			Open(false);
		}

		protected virtual void Open(bool readOnly)
		{
			if (null != _adapter)
			{
				throw new InvalidOperationException();
			}
			_adapter = Factory().Open(TempFile(), false, 0, readOnly);
		}

		/// <exception cref="System.Exception"></exception>
		public override void TearDown()
		{
			Close();
			base.TearDown();
		}

		protected virtual void Close()
		{
			if (null != _adapter)
			{
				_adapter.Close();
				_adapter = null;
			}
		}

		private IoAdapter Factory()
		{
			return ((IoAdapter)SubjectFixtureProvider.Value());
		}
	}
}
