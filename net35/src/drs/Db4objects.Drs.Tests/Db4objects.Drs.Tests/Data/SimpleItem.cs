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
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests.Data
{
	public class SimpleItem
	{
		private string value;

		private Db4objects.Drs.Tests.Data.SimpleItem child;

		private SimpleListHolder parent;

		public SimpleItem()
		{
		}

		public SimpleItem(SimpleListHolder parent_, Db4objects.Drs.Tests.Data.SimpleItem 
			child_, string value_)
		{
			parent = parent_;
			value = value_;
			child = child_;
		}

		public SimpleItem(string value_) : this(null, null, value_)
		{
		}

		public SimpleItem(Db4objects.Drs.Tests.Data.SimpleItem child, string value_) : this
			(null, child, value_)
		{
		}

		public SimpleItem(SimpleListHolder parent_, string value_) : this(parent_, null, 
			value_)
		{
		}

		public virtual string GetValue()
		{
			return value;
		}

		public virtual void SetValue(string value_)
		{
			value = value_;
		}

		public virtual Db4objects.Drs.Tests.Data.SimpleItem GetChild()
		{
			return GetChild(0);
		}

		public virtual Db4objects.Drs.Tests.Data.SimpleItem GetChild(int level)
		{
			Db4objects.Drs.Tests.Data.SimpleItem tbr = child;
			while (--level > 0 && tbr != null)
			{
				tbr = tbr.child;
			}
			return tbr;
		}

		public virtual void SetChild(Db4objects.Drs.Tests.Data.SimpleItem child_)
		{
			child = child_;
		}

		public virtual SimpleListHolder GetParent()
		{
			return parent;
		}

		public virtual void SetParent(SimpleListHolder parent_)
		{
			parent = parent_;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(Db4objects.Drs.Tests.Data.SimpleItem))
			{
				return false;
			}
			Db4objects.Drs.Tests.Data.SimpleItem rhs = (Db4objects.Drs.Tests.Data.SimpleItem)
				obj;
			return rhs.GetValue().Equals(GetValue());
		}

		public override string ToString()
		{
			string childString;
			if (child != null)
			{
				childString = child != this ? child.ToString() : "this";
			}
			else
			{
				childString = "null";
			}
			return value + "[" + childString + "]";
		}
	}
}
