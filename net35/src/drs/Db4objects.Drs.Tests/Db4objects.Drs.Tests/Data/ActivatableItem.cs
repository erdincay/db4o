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

namespace Db4objects.Drs.Tests.Data
{
	/// <exclude></exclude>
	public class ActivatableItem : IActivatable
	{
		private string name;

		[System.NonSerialized]
		private IActivator _activator;

		public ActivatableItem()
		{
		}

		public ActivatableItem(string name)
		{
			this.name = name;
		}

		public virtual void Activate(ActivationPurpose purpose)
		{
			if (_activator != null)
			{
				_activator.Activate(purpose);
			}
		}

		public virtual void Bind(IActivator activator)
		{
			_activator = activator;
		}

		public virtual object Name()
		{
			Activate(ActivationPurpose.Read);
			return name;
		}

		public virtual string GetName()
		{
			Activate(ActivationPurpose.Read);
			return name;
		}

		public virtual void SetName(string name)
		{
			Activate(ActivationPurpose.Write);
			this.name = name;
		}
	}
}
