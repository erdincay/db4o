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
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.Tests.Common.TA;
using Db4objects.Db4o.Tests.Common.TA.Nested;

namespace Db4objects.Db4o.Tests.Common.TA.Nested
{
	public partial class OuterClass : ActivatableImpl
	{
		public int _foo;

		public virtual int Foo()
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			return _foo;
		}

		public virtual OuterClass.InnerClass CreateInnerObject()
		{
			return new OuterClass.InnerClass(this);
		}

		public partial class InnerClass : ActivatableImpl
		{
			public virtual OuterClass GetOuterObject()
			{
				// TA BEGIN
				this.Activate(ActivationPurpose.Read);
				// TA END
				return this._enclosing;
			}

			public virtual OuterClass GetOuterObjectWithoutActivation()
			{
				return this._enclosing;
			}

			internal InnerClass(OuterClass _enclosing)
			{
				this._enclosing = _enclosing;
			}

			private readonly OuterClass _enclosing;
		}
	}
}
