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
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;

namespace Db4objects.Db4o.Internal.Activation
{
	public class UnknownActivationDepth : IActivationDepth
	{
		public static readonly IActivationDepth Instance = new Db4objects.Db4o.Internal.Activation.UnknownActivationDepth
			();

		private UnknownActivationDepth()
		{
		}

		public virtual ActivationMode Mode()
		{
			throw new InvalidOperationException();
		}

		public virtual IActivationDepth Descend(ClassMetadata metadata)
		{
			throw new InvalidOperationException();
		}

		public virtual bool RequiresActivation()
		{
			throw new InvalidOperationException();
		}
	}
}
