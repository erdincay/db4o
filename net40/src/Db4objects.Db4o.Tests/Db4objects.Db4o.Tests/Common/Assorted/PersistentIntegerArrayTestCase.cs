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
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	/// <exclude></exclude>
	public class PersistentIntegerArrayTestCase : AbstractDb4oTestCase, IOptOutMultiSession
		, IOptOutDefragSolo
	{
		public static void Main(string[] arguments)
		{
			new PersistentIntegerArrayTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			int[] original = new int[] { 10, 99, 77 };
			PersistentIntegerArray arr = new PersistentIntegerArray(SlotChangeFactory.IdSystem
				, null, original);
			arr.Write(SystemTrans());
			int id = arr.GetID();
			Reopen();
			arr = new PersistentIntegerArray(SlotChangeFactory.IdSystem, null, id);
			arr.Read(SystemTrans());
			int[] copy = arr.Array();
			ArrayAssert.AreEqual(original, copy);
		}
	}
}
