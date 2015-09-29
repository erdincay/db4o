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

namespace Db4objects.Db4o.Tests.Common.TA.Collections
{
	public class Page : ActivatableImpl
	{
		public const int Pagesize = 100;

		private object[] _data = new object[Pagesize];

		private int _top = 0;

		private int _pageIndex;

		[System.NonSerialized]
		private bool _dirty = false;

		public Page(int pageIndex)
		{
			_pageIndex = pageIndex;
		}

		public virtual bool Add(object obj)
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			_dirty = true;
			_data[_top++] = obj;
			return true;
		}

		public virtual int Size()
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			return _top;
		}

		public virtual object Get(int indexInPage)
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			//		System.out.println("got from page: " + _pageIndex);
			_dirty = true;
			// just to be safe, we'll mark things as dirty if they are used.
			return _data[indexInPage];
		}

		public virtual bool IsDirty()
		{
			// TA BEGIN
			//		activate();
			// TA END
			return _dirty;
		}

		public virtual void SetDirty(bool dirty)
		{
			// TA BEGIN
			//		activate();
			// TA END
			_dirty = dirty;
		}

		public virtual int GetPageIndex()
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			return _pageIndex;
		}

		public virtual bool AtCapacity()
		{
			return Capacity() == 0;
		}

		public virtual int Capacity()
		{
			// TA BEGIN
			Activate(ActivationPurpose.Read);
			// TA END
			return Db4objects.Db4o.Tests.Common.TA.Collections.Page.Pagesize - Size();
		}
	}
}
