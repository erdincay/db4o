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

namespace Db4objects.Db4o.Tests.CLI2.Collections.Transparent
{
	[Serializable]
	public class AbstractCollectionElement : ICollectionElement
	{
#if SILVERLIGHT
		public string _name;
#else
		protected string _name;
#endif

		protected AbstractCollectionElement(string name)
		{
			_name = name;	
		}

		public int CompareTo(ICollectionElement other)
		{
			if(Name == null)
			{
				if(other.Name == null)
				{
					return 0;
				}
				
				return -1;
			}
		
			return Name.CompareTo(other.Name);
		}

		public string Name
		{
			get
			{
				ReadFieldAccess();
				return _name;
			}
		}

		protected virtual void ReadFieldAccess()
		{
			// Do nothing
		}
	}
}
