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
using Db4oUnit.Extensions;
using Db4objects.Db4o.Tests.Common.Exceptions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
	public class AllTests : ComposibleTestSuite
	{
		protected override Type[] TestCases()
		{
			return ComposeTests(new Type[] { typeof(ActivationExceptionBubblesUpTestCase), typeof(
				BackupCSExceptionTestCase), typeof(DatabaseClosedExceptionTestCase), typeof(DatabaseReadonlyExceptionTestCase
				), typeof(GlobalOnlyConfigExceptionTestCase), typeof(ObjectCanActiviateExceptionTestCase
				), typeof(ObjectCanDeleteExceptionTestCase), typeof(ObjectOnDeleteExceptionTestCase
				), typeof(ObjectCanNewExceptionTestCase), typeof(StoreExceptionBubblesUpTestCase
				), typeof(StoredClassExceptionBubblesUpTestCase), typeof(TSerializableOnInstantiateCNFExceptionTestCase
				), typeof(TSerializableOnInstantiateIOExceptionTestCase), typeof(TSerializableOnStoreExceptionTestCase
				) });
		}

		#if !SILVERLIGHT
		protected override Type[] ComposeWith()
		{
			return new Type[] { typeof(BackupDb4oIOExceptionTestCase), typeof(IncompatibleFileFormatExceptionTestCase
				), typeof(InvalidSlotExceptionTestCase), typeof(OldFormatExceptionTestCase), typeof(
				Db4objects.Db4o.Tests.Common.Exceptions.Propagation.AllTests), typeof(InvalidPasswordTestCase
				) };
		}
		#endif // !SILVERLIGHT
	}
}
