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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1.Aliases
{
    /// <summary>
	/// </summary>
	public class TypeAliasesTestCase : BaseAliasesTestCase
	{
		public void TestTypeAlias()
		{
		    Db().Store(new Person1("Homer Simpson"));
			Db().Store(new Person1("John Cleese"));

			Reopen();
			Fixture().ConfigureAtRuntime(new AddAliasAction());
            Reopen();
			AssertAliasedData(Db());
		}

		private sealed class AddAliasAction : IRuntimeConfigureAction
		{
			public void Apply(IConfiguration config)
			{
				config.AddAlias(
					// Person1 instances should be read as Person2 objects
					new TypeAlias(
					GetTypeName(typeof(Person1)),
					GetTypeName(typeof(Person2))));
			}
	
		    private static string GetTypeName(Type type)
		    {
		        return ReflectPlatform.FullyQualifiedName(type);
		    }
		}
	}
}
