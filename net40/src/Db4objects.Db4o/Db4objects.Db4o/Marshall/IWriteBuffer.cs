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
namespace Db4objects.Db4o.Marshall
{
	/// <summary>a buffer interface with write methods.</summary>
	/// <remarks>a buffer interface with write methods.</remarks>
	public interface IWriteBuffer
	{
		/// <summary>writes a single byte to the buffer.</summary>
		/// <remarks>writes a single byte to the buffer.</remarks>
		/// <param name="b">the byte</param>
		void WriteByte(byte b);

		/// <summary>writes an array of bytes to the buffer</summary>
		/// <param name="bytes">the byte array</param>
		void WriteBytes(byte[] bytes);

		/// <summary>writes an int to the buffer.</summary>
		/// <remarks>writes an int to the buffer.</remarks>
		/// <param name="i">the int</param>
		void WriteInt(int i);

		/// <summary>writes a long to the buffer</summary>
		/// <param name="l">the long</param>
		void WriteLong(long l);
	}
}
