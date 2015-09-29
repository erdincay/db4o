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
using System.Text;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Drs.Foundation
{
	public class Signature
	{
		public readonly byte[] bytes;

		public Signature(byte[] bytes)
		{
			this.bytes = bytes;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (!(obj is Db4objects.Drs.Foundation.Signature))
			{
				return false;
			}
			Db4objects.Drs.Foundation.Signature other = (Db4objects.Drs.Foundation.Signature)
				obj;
			return Arrays4.Equals(bytes, other.bytes);
		}

		public override int GetHashCode()
		{
			int hc = 0;
			for (int i = 0; i < bytes.Length; i++)
			{
				hc <<= 2;
				hc = hc ^ bytes[i];
			}
			return hc;
		}

		public override string ToString()
		{
			return ToString(bytes);
		}

		public static string ToString(byte[] bytes)
		{
			return BytesToString(bytes);
		}

		private static string BytesToString(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				char c = (char)bytes[i];
				sb.Append(c);
			}
			return sb.ToString();
		}

		public virtual string AsString()
		{
			return BytesToString(bytes);
		}
	}
}
