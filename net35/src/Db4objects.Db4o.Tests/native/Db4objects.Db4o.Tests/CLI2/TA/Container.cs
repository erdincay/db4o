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

namespace Db4objects.Db4o.Tests.CLI2.TA
{
	public class Named : ActivatableImpl
	{
		public string _name;

		public Named(string name)
		{
			_name = name;
		}

		/// <summary>
		/// Activatable based implementation. Activates the
		/// object before field access.
		/// </summary>
		public string Name
		{
			get
			{
				Activate(ActivationPurpose.Read);
				return _name;
			}
		}

		public string PassThroughName
		{
			get { return _name; }
		}
	}

	public class NullableContainer<T> : Named where T : struct
	{
		public T? _value;

		public NullableContainer(string name, T? value) : base(name)
		{
			_value = value;
		}

		/// <summary>
		/// Activatable based implementation. Activates the
		/// object before field access.
		/// </summary>
		public T? Value
		{
			get
			{
				Activate(ActivationPurpose.Read);
				return _value;
			}
		}

		/// <summary>
		/// Bypass activation and access the field directly.
		/// </summary>
		public T? PassThroughValue
		{
			get { return _value; }
		}
	}

	public class Container<T> : Named where T : struct 
	{
		public T _value;

		public Container(string name, T value) : base(name)
		{	
			_value = value;
		}

		/// <summary>
		/// Activatable based implementation. Activates the
		/// object before field access.
		/// </summary>
		public T Value
		{
			get
			{
				Activate(ActivationPurpose.Read);
				return _value;
			}
		}

		/// <summary>
		/// Bypass activation and access the field directly.
		/// </summary>
		public T PassThroughValue
		{
			get { return _value; }
		}
	}
}