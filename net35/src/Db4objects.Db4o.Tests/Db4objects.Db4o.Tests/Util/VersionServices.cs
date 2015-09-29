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
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Marshall;
using Sharpen.IO;

namespace Db4objects.Db4o.Tests.Util
{
	public class VersionServices
	{
		public const byte Header3040 = 123;

		public const byte Header4657 = 4;

		public const byte Header60 = 100;

		/// <exception cref="System.IO.IOException"></exception>
		public static byte FileHeaderVersion(string testFile)
		{
			RandomAccessFile raf = new RandomAccessFile(testFile, "r");
			byte[] bytes = new byte[1];
			raf.Read(bytes);
			// readByte() doesn't convert to .NET.
			byte db4oHeaderVersion = bytes[0];
			raf.Close();
			return db4oHeaderVersion;
		}

		public static int SlotHandlerVersion(IExtObjectContainer objectContainer, object 
			obj)
		{
			int id = (int)objectContainer.GetID(obj);
			IObjectInfo objectInfo = objectContainer.GetObjectInfo(obj);
			ObjectContainerBase container = (ObjectContainerBase)objectContainer;
			Transaction trans = container.Transaction;
			ByteArrayBuffer buffer = container.ReadBufferById(trans, id);
			UnmarshallingContext context = new UnmarshallingContext(trans, (ObjectReference)objectInfo
				, Const4.Transient, false);
			context.Buffer(buffer);
			context.PersistentObject(obj);
			context.ActivationDepth(new LegacyActivationDepth(0));
			context.Read();
			return context.HandlerVersion();
		}
	}
}
