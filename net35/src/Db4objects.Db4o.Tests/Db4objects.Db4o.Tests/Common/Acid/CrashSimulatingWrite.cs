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
using System.IO;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Internal.Transactionlog;
using Sharpen.IO;

namespace Db4objects.Db4o.Tests.Common.Acid
{
	public class CrashSimulatingWrite
	{
		internal int _index;

		internal byte[] _data;

		internal long _offset;

		internal int _length;

		internal byte[] _lockFileData;

		internal byte[] _logFileData;

		public CrashSimulatingWrite(int index, byte[] data, long offset, int length, byte
			[] lockFileData, byte[] logFileData)
		{
			_index = index;
			_data = data;
			_offset = offset;
			_length = length;
			_lockFileData = lockFileData;
			_logFileData = logFileData;
		}

		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Write(string path, RandomAccessFile raf, bool writeTrash)
		{
			if (_offset == 0)
			{
				writeTrash = false;
			}
			raf.Seek(_offset);
			raf.Write(BytesToWrite(_data, writeTrash), 0, _length);
			Write(FileBasedTransactionLogHandler.LockFileName(path), _lockFileData, writeTrash
				);
			Write(FileBasedTransactionLogHandler.LogFileName(path), _logFileData, writeTrash);
		}

		public override string ToString()
		{
			return string.Empty + _index + " A:(" + _offset + ") L:(" + _length + ")";
		}

		private void Write(string fileName, byte[] bytes, bool writeTrash)
		{
			if (bytes == null)
			{
				return;
			}
			try
			{
				RandomAccessFile raf = new RandomAccessFile(fileName, "rw");
				raf.Write(BytesToWrite(bytes, writeTrash));
				raf.Close();
			}
			catch (IOException e)
			{
				throw new Db4oException(e);
			}
		}

		private byte[] BytesToWrite(byte[] bytes, bool writeTrash)
		{
			if (!writeTrash)
			{
				return bytes;
			}
			byte[] trash = new byte[bytes.Length];
			for (int i = 0; i < trash.Length; i++)
			{
				trash[i] = (byte)(i + 100);
			}
			return trash;
		}
	}
}
