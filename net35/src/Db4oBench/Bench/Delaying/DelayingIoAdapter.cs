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
using Db4objects.Db4o.Bench.Delaying;
using Db4objects.Db4o.Bench.Timing;
using Db4objects.Db4o.IO;

namespace Db4objects.Db4o.Bench.Delaying
{
	[System.ObsoleteAttribute(@"use -equivalent instead.")]
	public class DelayingIoAdapter : VanillaIoAdapter
	{
		private static Delays _delays = new Delays(0, 0, 0, 0);

		private TicksTiming _timing;

		public DelayingIoAdapter(IoAdapter delegateAdapter) : this(delegateAdapter, _delays
			)
		{
		}

		public DelayingIoAdapter(IoAdapter delegateAdapter, Delays delays) : base(delegateAdapter
			)
		{
			_delays = delays;
			_timing = new TicksTiming();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public DelayingIoAdapter(IoAdapter delegateAdapter, string path, bool lockFile, long
			 initialLength) : this(delegateAdapter, path, lockFile, initialLength, _delays)
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public DelayingIoAdapter(IoAdapter delegateAdapter, string path, bool lockFile, long
			 initialLength, Delays delays) : this(delegateAdapter.Open(path, lockFile, initialLength
			, false), delays)
		{
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override IoAdapter Open(string path, bool lockFile, long initialLength, bool
			 readOnly)
		{
			return new Db4objects.Db4o.Bench.Delaying.DelayingIoAdapter(_delegate, path, lockFile
				, initialLength);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override int Read(byte[] bytes, int length)
		{
			Delay(_delays.values[Delays.Read]);
			return _delegate.Read(bytes, length);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Seek(long pos)
		{
			Delay(_delays.values[Delays.Seek]);
			_delegate.Seek(pos);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Sync()
		{
			Delay(_delays.values[Delays.Sync]);
			_delegate.Sync();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public override void Write(byte[] buffer, int length)
		{
			Delay(_delays.values[Delays.Write]);
			_delegate.Write(buffer, length);
		}

		private void Delay(long time)
		{
			_timing.WaitTicks(time);
		}
	}
}
