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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.Header;

namespace Db4objects.Db4o.Tests.Common.Header
{
	public class IdentityTestCase : AbstractDb4oTestCase
	{
		public static void Main(string[] arguments)
		{
			new IdentityTestCase().RunAll();
		}

		public virtual void TestIdentitySignatureIsNotNull()
		{
			Db4oDatabase identity = Db().Identity();
			Assert.IsNotNull(identity.GetSignature());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestIdentityPreserved()
		{
			Db4oDatabase ident = Db().Identity();
			Reopen();
			Db4oDatabase ident2 = Db().Identity();
			Assert.IsNotNull(ident);
			Assert.AreEqual(ident, ident2);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestGenerateIdentity()
		{
			if (IsMultiSession())
			{
				return;
			}
			byte[] oldSignature = Db().Identity().GetSignature();
			GenerateNewIdentity();
			Reopen();
			ArrayAssert.AreNotEqual(oldSignature, Db().Identity().GetSignature());
		}

		private void GenerateNewIdentity()
		{
			((LocalObjectContainer)Db()).GenerateNewIdentity();
		}
	}
}
