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
using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Concurrency;
using Db4objects.Db4o.Tests.Common.Persistent;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
	public class ByteArrayTestCase : Db4oClientServerTestCase
	{
		internal const int Iterations = 15;

		internal const int Instances = 2;

		internal const int ArrayLength = 1024 * 512;

		public static void Main(string[] args)
		{
			new ByteArrayTestCase().RunConcurrency();
		}

		#if !CF
		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(SerializableByteArrayHolder)).Translate(new TSerializable
				());
		}
		#endif // !CF

		protected override void Store()
		{
			for (int i = 0; i < Instances; ++i)
			{
				Store(new ByteArrayHolder(CreateByteArray()));
				Store(new SerializableByteArrayHolder(CreateByteArray()));
			}
		}

		public virtual void ConcByteArrayHolder(IExtObjectContainer oc)
		{
			TimeQueryLoop(oc, "raw byte array", typeof(ByteArrayHolder));
		}

		public virtual void ConcSerializableByteArrayHolder(IExtObjectContainer oc)
		{
			TimeQueryLoop(oc, "TSerializable", typeof(SerializableByteArrayHolder));
		}

		private void TimeQueryLoop(IExtObjectContainer oc, string label, Type clazz)
		{
			for (int i = 0; i < Iterations; ++i)
			{
				IQuery query = oc.Query();
				query.Constrain(clazz);
				IObjectSet os = query.Execute();
				Assert.AreEqual(Instances, os.Count);
				while (os.HasNext())
				{
					Assert.AreEqual(ArrayLength, ((IIByteArrayHolder)os.Next()).GetBytes().Length);
				}
			}
		}

		internal virtual byte[] CreateByteArray()
		{
			byte[] bytes = new byte[ArrayLength];
			for (int i = 0; i < bytes.Length; ++i)
			{
				bytes[i] = (byte)(i % 256);
			}
			return bytes;
		}
	}
}
#endif // !SILVERLIGHT
