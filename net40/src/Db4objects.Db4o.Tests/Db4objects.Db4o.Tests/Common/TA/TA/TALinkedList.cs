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

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	/// <exclude></exclude>
	public class TALinkedList : ActivatableImpl
	{
		public static Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList NewList(int depth)
		{
			if (depth == 0)
			{
				return null;
			}
			Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList head = new Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList
				(depth);
			head.next = NewList(depth - 1);
			return head;
		}

		public virtual Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList NextN(int depth)
		{
			Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList node = this;
			for (int i = 0; i < depth; ++i)
			{
				node = node.Next();
			}
			return node;
		}

		public Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList next;

		public int value;

		public TALinkedList(int v)
		{
			value = v;
		}

		public virtual int Value()
		{
			Activate(ActivationPurpose.Read);
			return value;
		}

		public virtual Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList Next()
		{
			Activate(ActivationPurpose.Read);
			return next;
		}

		public override bool Equals(object other)
		{
			Activate(ActivationPurpose.Read);
			Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList otherList = (Db4objects.Db4o.Tests.Common.TA.TA.TALinkedList
				)other;
			if (value != otherList.Value())
			{
				return false;
			}
			if (next == otherList.Next())
			{
				return true;
			}
			if (otherList.Next() == null)
			{
				return false;
			}
			return next.Equals(otherList.Next());
		}
	}
}
