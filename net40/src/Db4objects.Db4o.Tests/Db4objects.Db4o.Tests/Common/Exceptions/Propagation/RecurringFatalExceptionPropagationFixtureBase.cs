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
using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Exceptions.Propagation;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
	public abstract class RecurringFatalExceptionPropagationFixtureBase : IExceptionPropagationFixture
	{
		protected static readonly string CloseMessage = "B";

		protected static readonly string InitialMessage = "A";

		public virtual void ThrowShutdownException()
		{
			Assert.Fail();
		}

		public virtual void AssertExecute(DatabaseContext context, TopLevelOperation op)
		{
			try
			{
				op.Apply(context);
				Assert.Fail();
			}
			catch (CompositeDb4oException exc)
			{
				Assert.AreEqual(2, exc._exceptions.Length);
				AssertExceptionMessage(exc, InitialMessage, 0);
				AssertExceptionMessage(exc, CloseMessage, 1);
			}
		}

		private void AssertExceptionMessage(CompositeDb4oException exc, string expected, 
			int idx)
		{
			Assert.AreEqual(expected, exc._exceptions[idx].Message);
		}

		protected abstract Type ExceptionType();

		public abstract string Label();

		public abstract void ThrowCloseException();

		public abstract void ThrowInitialException();
	}
}
