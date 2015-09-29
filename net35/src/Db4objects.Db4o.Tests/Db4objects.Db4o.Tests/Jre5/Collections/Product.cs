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

namespace Db4objects.Db4o.Tests.Jre5.Collections
{
	public class Product : ActivatableImpl
	{
		private string _code;

		private string _description;

		public Product(string code, string description)
		{
			_code = code;
			_description = description;
		}

		public virtual string Code()
		{
			Activate(ActivationPurpose.Read);
			return _code;
		}

		public virtual string Description()
		{
			Activate(ActivationPurpose.Read);
			return _description;
		}

		public override bool Equals(object p)
		{
			Activate(ActivationPurpose.Read);
			if (p == null)
			{
				return false;
			}
			if (p.GetType() != this.GetType())
			{
				return false;
			}
			Db4objects.Db4o.Tests.Jre5.Collections.Product rhs = (Db4objects.Db4o.Tests.Jre5.Collections.Product
				)p;
			return rhs._code == _code;
		}
	}
}
