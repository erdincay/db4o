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
	public class RecoverableExceptionPropagationFixture : IExceptionPropagationFixture
	{
		public virtual void ThrowInitialException()
		{
			throw new Db4oRecoverableException(new OutOfMemoryException());
		}

		public virtual void ThrowShutdownException()
		{
			Assert.Fail();
		}

		public virtual void ThrowCloseException()
		{
			Assert.Fail();
		}

		public virtual void AssertExecute(DatabaseContext context, TopLevelOperation op)
		{
			Assert.Expect(typeof(Db4oRecoverableException), new _ICodeBlock_23(op, context));
			Assert.IsFalse(context.StorageIsClosed());
		}

		private sealed class _ICodeBlock_23 : ICodeBlock
		{
			public _ICodeBlock_23(TopLevelOperation op, DatabaseContext context)
			{
				this.op = op;
				this.context = context;
			}

			/// <exception cref="System.Exception"></exception>
			public void Run()
			{
				op.Apply(context);
			}

			private readonly TopLevelOperation op;

			private readonly DatabaseContext context;
		}

		public virtual string Label()
		{
			return "recoverable";
		}
	}
}
