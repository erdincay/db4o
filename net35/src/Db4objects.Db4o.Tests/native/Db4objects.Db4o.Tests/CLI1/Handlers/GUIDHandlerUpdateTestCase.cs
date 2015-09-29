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
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    internal class GUIDHandlerUpdateTestCase : ValueTypeHandlerUpdateTestCaseBase<Guid>
    {
		protected override string TypeName()
		{
			return base.TypeName() + IndexedInfo();
		}

    	protected override Guid[] GetData()
    	{
    		return Data;
    	}

        protected override bool DefragmentInReadWriteMode()
        {
            return true;
        }
		
		private static readonly Guid[] Data = new Guid[] {
            Guid.Empty,
            new Guid("a17b9ce4-e7f7-464c-a7d8-598a229f6a0c"),
            new Guid("c2e5944a-b630-4cad-b49e-8e261a08c14c"),
            new Guid("7e255e48-5320-4b1f-86da-2d680aaed914"),
            new Guid("9d33da58-44ae-44c3-b719-4c006be0cb44"),
        };

		protected override void ConfigureForTest(IConfiguration config)
		{
			base.ConfigureForTest(config);
			config.ObjectClass(typeof(Item)).ObjectField("Typed").Indexed(IndexedOnCurrentVersion);
		}

		protected override void ConfigureForStore(IConfiguration config)
		{
			base.ConfigureForStore(config);
			config.ObjectClass(typeof(Item)).ObjectField("Typed").Indexed(IndexedOnOldVersion);
		}
		
    	protected virtual bool IndexedOnCurrentVersion
    	{
    		get { return false; }
    	}

		protected virtual bool IndexedOnOldVersion
    	{
			get { return false; }
		}

    	protected static bool IsEmbeeded(IExtObjectContainer objectContainer)
    	{
#if !CF && !SILVERLIGHT
            Db4objects.Db4o.CS.Internal.ClientObjectContainer client = objectContainer as Db4objects.Db4o.CS.Internal.ClientObjectContainer;
    		return client == null;
#else
    	    return true;
#endif
    	}

    	private string IndexedInfo()
		{
			return String.Format("_{0}-{1}_", IndexedInfo(IndexedOnOldVersion), IndexedInfo(IndexedOnCurrentVersion));
		}

		private string IndexedInfo(bool indexed)
		{
			return indexed ? "INDEXED" : "NONINDEXED";
		}
	}
}
