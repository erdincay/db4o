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
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Querying;

namespace Db4objects.Db4o.Tests.Common.Querying
{
	public class CascadeDeleteFalse : AbstractDb4oTestCase
	{
		public class CascadeDeleteFalseHelper
		{
		}

		public CascadeDeleteFalse.CascadeDeleteFalseHelper h1;

		public CascadeDeleteFalse.CascadeDeleteFalseHelper h2;

		public CascadeDeleteFalse.CascadeDeleteFalseHelper h3;

		protected override void Configure(IConfiguration conf)
		{
			conf.ObjectClass(this).CascadeOnDelete(true);
			conf.ObjectClass(this).ObjectField("h3").CascadeOnDelete(false);
		}

		protected override void Store()
		{
			CascadeDeleteFalse cdf = new CascadeDeleteFalse();
			cdf.h1 = new CascadeDeleteFalse.CascadeDeleteFalseHelper();
			cdf.h2 = new CascadeDeleteFalse.CascadeDeleteFalseHelper();
			cdf.h3 = new CascadeDeleteFalse.CascadeDeleteFalseHelper();
			Db().Store(cdf);
		}

		public virtual void Test()
		{
			CheckHelperCount(3);
			CascadeDeleteFalse cdf = (CascadeDeleteFalse)((CascadeDeleteFalse)RetrieveOnlyInstance
				(GetType()));
			Db().Delete(cdf);
			CheckHelperCount(1);
		}

		private void CheckHelperCount(int count)
		{
			Assert.AreEqual(count, CountOccurences(typeof(CascadeDeleteFalse.CascadeDeleteFalseHelper
				)));
		}

		public static void Main(string[] args)
		{
			new CascadeDeleteFalse().RunSolo();
		}
	}
}
