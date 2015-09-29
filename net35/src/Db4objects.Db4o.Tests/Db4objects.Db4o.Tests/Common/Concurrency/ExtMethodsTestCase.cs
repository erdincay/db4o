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
#if !SILVERLIGHT
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Concurrency;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ExtMethodsTestCase : Db4oClientServerTestCase
	{
		public static void Main(string[] args)
		{
			new ExtMethodsTestCase().RunConcurrency();
		}

		public virtual void Conc(IExtObjectContainer oc)
		{
			ExtMethodsTestCase em = new ExtMethodsTestCase();
			oc.Store(em);
			Assert.IsFalse(oc.IsClosed());
			Assert.IsTrue(oc.IsActive(em));
			Assert.IsTrue(oc.IsStored(em));
			oc.Deactivate(em, 1);
			Assert.IsTrue(!oc.IsActive(em));
			oc.Activate(em, 1);
			Assert.IsTrue(oc.IsActive(em));
			long id = oc.GetID(em);
			Assert.IsTrue(oc.IsCached(id));
			oc.Purge(em);
			Assert.IsFalse(oc.IsCached(id));
			Assert.IsFalse(oc.IsStored(em));
			Assert.IsFalse(oc.IsActive(em));
			oc.Bind(em, id);
			Assert.IsTrue(oc.IsCached(id));
			Assert.IsTrue(oc.IsStored(em));
			Assert.IsTrue(oc.IsActive(em));
			ExtMethodsTestCase em2 = (ExtMethodsTestCase)oc.GetByID(id);
			Assert.AreSame(em, em2);
			// Purge all and try again
			oc.Purge();
			Assert.IsTrue(oc.IsCached(id));
			Assert.IsTrue(oc.IsStored(em));
			Assert.IsTrue(oc.IsActive(em));
			em2 = (ExtMethodsTestCase)oc.GetByID(id);
			Assert.AreSame(em, em2);
			oc.Delete(em2);
			oc.Commit();
			Assert.IsFalse(oc.IsCached(id));
			Assert.IsFalse(oc.IsStored(em2));
			Assert.IsFalse(oc.IsActive(em2));
			// Null checks
			Assert.IsFalse(oc.IsStored(null));
			Assert.IsFalse(oc.IsActive(null));
			Assert.IsFalse(oc.IsCached(0));
		}
	}
}
#endif // !SILVERLIGHT
