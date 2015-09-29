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
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public class MockActivator : IActivator
	{
		private int _readCount;

		private int _writeCount;

		public MockActivator()
		{
		}

		public virtual int Count()
		{
			return _readCount + _writeCount;
		}

		public virtual void Activate(ActivationPurpose purpose)
		{
			if (purpose == ActivationPurpose.Read)
			{
				++_readCount;
			}
			else
			{
				++_writeCount;
			}
		}

		public virtual int WriteCount()
		{
			return _writeCount;
		}

		public virtual int ReadCount()
		{
			return _readCount;
		}

		public static Db4objects.Db4o.Tests.Common.TA.MockActivator ActivatorFor(IActivatable
			 obj)
		{
			Db4objects.Db4o.Tests.Common.TA.MockActivator activator = new Db4objects.Db4o.Tests.Common.TA.MockActivator
				();
			obj.Bind(activator);
			return activator;
		}
	}
}
