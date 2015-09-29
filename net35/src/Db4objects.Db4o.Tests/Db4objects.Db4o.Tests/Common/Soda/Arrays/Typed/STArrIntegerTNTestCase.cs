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
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Soda.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed
{
	public class STArrIntegerTNTestCase : SodaBaseTestCase
	{
		public int[][][] intArr;

		public STArrIntegerTNTestCase()
		{
		}

		public STArrIntegerTNTestCase(int[][][] arr)
		{
			intArr = arr;
		}

		public override object[] CreateData()
		{
			Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase[] arr = new 
				Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase[5];
			arr[0] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				();
			int[][][] content = new int[][][] {  };
			arr[1] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				(content);
			content = new int[][][] { new int[][] { new int[3], new int[3] } };
			content[0][0][1] = 0;
			content[0][1][0] = 0;
			arr[2] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				(content);
			content = new int[][][] { new int[][] { new int[3], new int[3] } };
			content[0][0][0] = 1;
			content[0][1][0] = 17;
			content[0][1][1] = int.MaxValue - 1;
			arr[3] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				(content);
			content = new int[][][] { new int[][] { new int[2], new int[2] } };
			content[0][0][0] = 3;
			content[0][0][1] = 17;
			content[0][1][0] = 25;
			content[0][1][1] = int.MaxValue - 2;
			arr[4] = new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				(content);
			object[] ret = new object[arr.Length];
			System.Array.Copy(arr, 0, ret, 0, arr.Length);
			return ret;
		}

		public virtual void TestDefaultContainsOne()
		{
			IQuery q = NewQuery();
			int[][][] content = new int[][][] { new int[][] { new int[1] } };
			content[0][0][0] = 17;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				(content));
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDefaultContainsTwo()
		{
			IQuery q = NewQuery();
			int[][][] content = new int[][][] { new int[][] { new int[1] }, new int[][] { new 
				int[1] } };
			content[0][0][0] = 17;
			content[1][0][0] = 25;
			q.Constrain(new Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				(content));
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendOne()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				));
			q.Descend("intArr").Constrain(17);
			Expect(q, new int[] { 3, 4 });
		}

		public virtual void TestDescendTwo()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(17);
			qElements.Constrain(25);
			Expect(q, new int[] { 4 });
		}

		public virtual void TestDescendSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}

		public virtual void TestDescendNotSmaller()
		{
			IQuery q = NewQuery();
			q.Constrain(typeof(Db4objects.Db4o.Tests.Common.Soda.Arrays.Typed.STArrIntegerTNTestCase
				));
			IQuery qElements = q.Descend("intArr");
			qElements.Constrain(3).Smaller();
			Expect(q, new int[] { 2, 3 });
		}
	}
}
