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
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class Db4oIOExceptionTestCaseBase : AbstractDb4oTestCase, IOptOutMultiSession
		, IOptOutTA
	{
		private ExceptionSimulatingStorage _storage;

		protected override void Configure(IConfiguration config)
		{
			config.LockDatabaseFile(false);
			_storage = new ExceptionSimulatingStorage(new FileStorage(), new _IExceptionFactory_19
				());
			config.Storage = _storage;
		}

		private sealed class _IExceptionFactory_19 : IExceptionFactory
		{
			public _IExceptionFactory_19()
			{
			}

			public void ThrowException()
			{
				throw new Db4oIOException();
			}

			public void ThrowOnClose()
			{
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oSetupBeforeStore()
		{
			TriggerException(false);
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Db4oTearDownBeforeClean()
		{
			TriggerException(false);
		}

		protected virtual void TriggerException(bool value)
		{
			_storage.TriggerException(value);
		}
	}
}
