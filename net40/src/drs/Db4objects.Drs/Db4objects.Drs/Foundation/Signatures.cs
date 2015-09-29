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
using System.Collections.Generic;
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Foundation
{
	public class Signatures
	{
		private readonly IDictionary<Signature, long> _loidBySignature = new Dictionary<Signature
			, long>();

		private readonly IDictionary<long, Signature> _signatureByLoid = new Dictionary<long
			, Signature>();

		public virtual void Add(Signature signature, long signatureLoid)
		{
			_loidBySignature[signature] = signatureLoid;
			_signatureByLoid[signatureLoid] = signature;
		}

		public virtual long LoidForSignature(Signature signature)
		{
			return _loidBySignature[signature];
		}

		public virtual Signature SignatureForLoid(long signatureLoid)
		{
			return _signatureByLoid[signatureLoid];
		}
	}
}
