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
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class MockWriteContext : MockMarshallingContext, IWriteContext
	{
		public MockWriteContext(IObjectContainer objectContainer) : base(objectContainer)
		{
		}

		public virtual void WriteObject(ITypeHandler4 handler, object obj)
		{
			Handlers4.Write(handler, this, obj);
		}

		public virtual void WriteAny(object obj)
		{
			ClassMetadata classMetadata = Container().ClassMetadataForObject(obj);
			WriteInt(classMetadata.GetID());
			Handlers4.Write(classMetadata.TypeHandler(), this, obj);
		}

		public virtual IReservedBuffer Reserve(int length)
		{
			IReservedBuffer reservedBuffer = new _IReservedBuffer_28(this);
			Seek(Offset() + length);
			return reservedBuffer;
		}

		private sealed class _IReservedBuffer_28 : IReservedBuffer
		{
			public _IReservedBuffer_28(MockWriteContext _enclosing)
			{
				this._enclosing = _enclosing;
				this.reservedOffset = this._enclosing.Offset();
			}

			private readonly int reservedOffset;

			public void WriteBytes(byte[] bytes)
			{
				int currentOffset = this._enclosing.Offset();
				this._enclosing.Seek(this.reservedOffset);
				this._enclosing.WriteBytes(bytes);
				this._enclosing.Seek(currentOffset);
			}

			private readonly MockWriteContext _enclosing;
		}
	}
}
