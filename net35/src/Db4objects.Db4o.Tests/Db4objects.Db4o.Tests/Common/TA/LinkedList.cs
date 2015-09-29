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
namespace Db4objects.Db4o.Tests.Common.TA
{
	/// <exclude></exclude>
	public class LinkedList
	{
		public static Db4objects.Db4o.Tests.Common.TA.LinkedList NewList(int depth)
		{
			if (depth == 0)
			{
				return null;
			}
			Db4objects.Db4o.Tests.Common.TA.LinkedList head = new Db4objects.Db4o.Tests.Common.TA.LinkedList
				(depth);
			head.next = NewList(depth - 1);
			return head;
		}

		public Db4objects.Db4o.Tests.Common.TA.LinkedList next;

		public int value;

		public LinkedList(int v)
		{
			value = v;
		}

		public virtual Db4objects.Db4o.Tests.Common.TA.LinkedList NextN(int depth)
		{
			Db4objects.Db4o.Tests.Common.TA.LinkedList node = this;
			for (int i = 0; i < depth; ++i)
			{
				node = node.next;
			}
			return node;
		}

		public override bool Equals(object other)
		{
			Db4objects.Db4o.Tests.Common.TA.LinkedList otherList = (Db4objects.Db4o.Tests.Common.TA.LinkedList
				)other;
			if (value != otherList.value)
			{
				return false;
			}
			if (next == otherList.next)
			{
				return true;
			}
			if (otherList.next == null)
			{
				return false;
			}
			return next.Equals(otherList.next);
		}
	}
}
