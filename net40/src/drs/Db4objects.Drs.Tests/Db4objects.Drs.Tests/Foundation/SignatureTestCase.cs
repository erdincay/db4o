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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Encoding;
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Tests.Foundation
{
	public class SignatureTestCase : ITestCase
	{
		public virtual void Test()
		{
			StatefulBuffer writer = new StatefulBuffer(null, 300);
			string stringRepresentation = SignatureGenerator.GenerateSignature();
			new LatinStringIO().Write(writer, stringRepresentation);
			Signature signature = new Signature(writer.GetWrittenBytes());
			Assert.AreEqual(stringRepresentation, signature.ToString());
		}
	}
}
