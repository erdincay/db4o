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
using System.Collections.Generic;

namespace OManager.BusinessLayer.UIHelper
{
	public class PagingData
	{
		public static readonly int PAGE_SIZE = 50;
		private const int START_PAGE_INDEX = 1;

		public PagingData(int startIndex, int endIndex)
		{
			m_startIndex = startIndex;
			m_endIndex =endIndex;
		}

		public PagingData(int startIndex) : this(startIndex, PAGE_SIZE)
		{
		}

		int m_startIndex;
		public int StartIndex
		{
			get { return m_startIndex; }
			set { m_startIndex = value; }
		}
		
		int m_endIndex;
		public int EndIndex
		{
			get { return m_endIndex; }
			set { m_endIndex = value; }
		}
		
		IList<long> m_objectId;
		public IList<long> ObjectId
		{
			get { return m_objectId; }
			set { m_objectId = value; }
		}

		public int GetPageCount()
		{
			int objectCount = m_objectId.Count;
			
			double pageCount = objectCount / (double)PAGE_SIZE;
			if (pageCount <= 0)
				pageCount = START_PAGE_INDEX;

			return (int) Math.Ceiling(pageCount);
		}

		public static PagingData StartingAtPage(int startPage)
		{
			int startIndex = NormalizePageIndex(startPage) * PAGE_SIZE;
			return new PagingData(startIndex, startIndex + PAGE_SIZE);
		}

		private static int NormalizePageIndex(int pageIndex)
		{
			return pageIndex - START_PAGE_INDEX;
		}

		public override string ToString()
		{
			return "PagingData(Start : " + m_startIndex + ", End: " + m_endIndex + ")";
		}
	}
}