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
using Db4oUnit;
using Db4objects.Db4o.Tests.Common.TA.TA;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.TA.TA
{
	public class TADateArrayTestCase : TAItemTestCaseBase
	{
		public static readonly DateTime[] data = new DateTime[] { new DateTime(0), new DateTime
			(1), new DateTime(1191972104500L) };

		public static void Main(string[] args)
		{
			new TADateArrayTestCase().RunAll();
		}

		/// <exception cref="System.Exception"></exception>
		protected override void AssertItemValue(object obj)
		{
			TADateArrayItem item = (TADateArrayItem)obj;
			for (int i = 0; i < data.Length; i++)
			{
				Assert.AreEqual(data[i], item.GetTyped()[i]);
				Assert.AreEqual(data[i], (DateTime)item.GetUntyped()[i]);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected override object CreateItem()
		{
			TADateArrayItem item = new TADateArrayItem();
			item._typed = new DateTime[data.Length];
			item._untyped = new object[data.Length];
			System.Array.Copy(data, 0, item._typed, 0, data.Length);
			System.Array.Copy(data, 0, item._untyped, 0, data.Length);
			return item;
		}
	}
}
