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
using Db4oUnit.Extensions.Util;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class ClassRenameByConfigTestCase : AbstractDb4oTestCase, IOptOutDefragSolo
	{
		public static void Main(string[] args)
		{
			new ClassRenameByConfigTestCase().RunNetworking();
		}

		public class Original
		{
			public string originalName;

			public Original()
			{
			}

			public Original(string name)
			{
				originalName = name;
			}
		}

		public class Changed
		{
			public string changedName;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void Test()
		{
			Store(new ClassRenameByConfigTestCase.Original("original"));
			Db().Commit();
			Assert.AreEqual(1, CountOccurences(typeof(ClassRenameByConfigTestCase.Original)));
			// Rename messages are visible at level 1
			// fixture().config().messageLevel(1);
			IObjectClass oc = Fixture().Config().ObjectClass(typeof(ClassRenameByConfigTestCase.Original
				));
			// allways rename fields first
			oc.ObjectField("originalName").Rename("changedName");
			// we must use ReflectPlatform here as the string must include
			// the assembly name in .net
			oc.Rename(CrossPlatformServices.FullyQualifiedName(typeof(ClassRenameByConfigTestCase.Changed
				)));
			Reopen();
			Assert.AreEqual(0, CountOccurences(typeof(ClassRenameByConfigTestCase.Original)));
			Assert.AreEqual(1, CountOccurences(typeof(ClassRenameByConfigTestCase.Changed)));
			ClassRenameByConfigTestCase.Changed changed = (ClassRenameByConfigTestCase.Changed
				)((ClassRenameByConfigTestCase.Changed)RetrieveOnlyInstance(typeof(ClassRenameByConfigTestCase.Changed
				)));
			Assert.AreEqual("original", changed.changedName);
		}
	}
}
