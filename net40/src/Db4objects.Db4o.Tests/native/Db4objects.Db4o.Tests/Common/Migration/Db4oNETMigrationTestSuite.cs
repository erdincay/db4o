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
using System.Collections;
using Db4objects.Db4o.Tests.CLI1.Handlers;
using System.IO;
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;
using Db4objects.Db4o.Tests.CLI2.Handlers;

namespace Db4objects.Db4o.Tests.Common.Migration
{
#if !CF && !SILVERLIGHT
    class Db4oNETMigrationTestSuite : Db4oMigrationTestSuite
    {
        //override protected string[] Libraries()
        //{
        //    return new string[] { AssemblyPathFor("7.9") };
        //}

    	private string AssemblyPathFor(string version)
    	{
    		return WorkspaceServices.WorkspacePath("db4o.archives/net-2.0/" + version + "/Db4objects.Db4o.dll");
    	}

    	protected override Type[] TestCases()
        {
            if (!Directory.Exists(Db4oLibrarian.LibraryPath()))
            {
                TestPlatform.GetStdErr().WriteLine("DISABLED: " + GetType());
                return new Type[] { };
            }

            ArrayList list = new ArrayList();
            list.AddRange(base.TestCases());

            Type[] netTypes = new Type[] {
                typeof(SimplestPossibleHandlerUpdateTestCase),
                typeof(GenericListVersionUpdateTestCase),
                typeof(GenericDictionaryVersionUpdateTestCase),
                typeof(DateTimeHandlerUpdateTestCase),
				typeof(DateTimeOffsetHandlerUpdateTestCase),
                typeof(IndexedDateTimeUpdateTestCase),
                typeof(DecimalHandlerUpdateTestCase),
				typeof(EnumHandlerUpdateTestCase),
                typeof(GUIDHandlerUpdateTestCase),
                typeof(GUIDHandlerUpdateIndexedOnCurrentVersionTestCase),
                typeof(GUIDHandlerUpdateIndexedOnPreviousVersionsTestCase),
                typeof(GUIDHandlerUpdateIndexedOnPreviousButNotOnCurrentVersionTestCase),
                typeof(HashtableUpdateTestCase),
                typeof(NestedStructHandlerUpdateTestCase),
                typeof(SByteHandlerUpdateTestCase),
                typeof(StructHandlerUpdateTestCase),
                typeof(UIntHandlerUpdateTestCase),
                typeof(ULongHandlerUpdateTestCase),
                typeof(UShortHandlerUpdateTestCase),
				typeof(HandlerVersionWhenSeekingToFieldTestCase),
            };

            list.AddRange(netTypes);
        	return (Type[]) list.ToArray(typeof(Type));
        }
    }
#endif
}
