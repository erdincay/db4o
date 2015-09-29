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
using Db4objects.Db4o.Tests.Common.Soda.Classes.Untypedhierarchy;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Untypedhierarchy
{
	public class STRUH2
	{
		public object parent;

		public object h3;

		public string foo2;

		public STRUH2()
		{
		}

		public STRUH2(STRUH3 a3)
		{
			h3 = a3;
			a3.parent = this;
		}

		public STRUH2(string str)
		{
			foo2 = str;
		}

		public STRUH2(STRUH3 a3, string str)
		{
			h3 = a3;
			a3.parent = this;
			foo2 = str;
		}
	}
}
