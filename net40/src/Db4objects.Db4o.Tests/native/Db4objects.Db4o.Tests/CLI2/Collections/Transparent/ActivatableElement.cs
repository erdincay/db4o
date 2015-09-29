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
using Db4objects.Db4o.Activation;
using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	[Serializable]
	public class ActivatableElement : AbstractCollectionElement, IActivatable
	{
		public ActivatableElement(string name) : base(name)
		{
		}

		public void Bind(IActivator activator)
		{
			if (activator == _activator)
			{
				return;
			}

			if (activator != null && _activator != null)
			{
				throw new InvalidOperationException();
			}

			_activator = activator;
		}

		public void Activate(ActivationPurpose purpose)
		{
			if (_activator == null) return;
			_activator.Activate(purpose);
		}

		public override bool Equals(object obj)
		{
			ActivatableElement other = obj as ActivatableElement;
			if (other == null || other.GetType() != typeof(ActivatableElement))
			{
				return false;
			}
			
			Activate(ActivationPurpose.Read);

			return Name.Equals(other.Name);
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	
		public override string ToString()
		{
			Activate(ActivationPurpose.Read);
			return "ActivatableElement " + Name;
		}

		protected override void ReadFieldAccess()
		{
			Activate(ActivationPurpose.Read);
		}

		[NonSerialized]
		private IActivator _activator;
	}
}
