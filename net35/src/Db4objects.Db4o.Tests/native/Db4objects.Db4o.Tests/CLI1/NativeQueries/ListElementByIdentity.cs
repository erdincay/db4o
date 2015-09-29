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
using System.Collections.Generic;

namespace Db4objects.Db4o.Tests.CLI1.NativeQueries
{
	public class ListElementByIdentity : AbstractDb4oTestCase, IOptOutMultiSession
	{
		public IList<LebiElement> _list;

		override protected void Store()
		{
			StoreElement("1");
			StoreElement("2");
			StoreElement("3");
			StoreElement("4");
		}

		public void Test()
		{
			LebiElement elem = (LebiElement)Db().QueryByExample(new LebiElement("23"))[0];


            IList<ListElementByIdentity> res = Db().Query((System.Predicate<ListElementByIdentity>)delegate(ListElementByIdentity lebi)
			{
				return lebi._list.Contains(elem);
			});

			Assert.AreEqual(1, res.Count);
			Assert.AreEqual("23", res[0]._list[3]._name);

		}

		private void StoreElement(string prefix)
		{
			ListElementByIdentity lebi = new ListElementByIdentity();
			lebi.CreateListElements(prefix);
			Store(lebi);
		}

		private void CreateListElements(string prefix)
		{
			_list = new List<LebiElement>();
			_list.Add(new LebiElement(prefix + "0"));
			_list.Add(new LebiElement(prefix + "1"));
			_list.Add(new LebiElement(prefix + "2"));
			_list.Add(new LebiElement(prefix + "3"));
		}
	}

	public class LebiElement
	{
		public string _name;

		public LebiElement(string name)
		{
			_name = name;
		}
	}
}
