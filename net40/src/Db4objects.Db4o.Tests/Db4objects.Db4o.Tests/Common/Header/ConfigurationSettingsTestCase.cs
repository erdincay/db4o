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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Tests.Common.Header
{
	public class ConfigurationSettingsTestCase : AbstractDb4oTestCase, IOptOutMultiSession
	{
		/// <exception cref="System.Exception"></exception>
		[System.ObsoleteAttribute]
		public virtual void TestChangingUuidSettings()
		{
			Fixture().Config().GenerateUUIDs(ConfigScope.Globally);
			Reopen();
			Assert.AreEqual(ConfigScope.Globally, GenerateUUIDs());
			Db().Configure().GenerateUUIDs(ConfigScope.Disabled);
			Assert.AreEqual(ConfigScope.Disabled, GenerateUUIDs());
			Fixture().Config().GenerateUUIDs(ConfigScope.Globally);
			Reopen();
			Assert.AreEqual(ConfigScope.Globally, GenerateUUIDs());
		}

		private ConfigScope GenerateUUIDs()
		{
			return ((LocalObjectContainer)Db()).Config().GenerateUUIDs();
		}
	}
}
