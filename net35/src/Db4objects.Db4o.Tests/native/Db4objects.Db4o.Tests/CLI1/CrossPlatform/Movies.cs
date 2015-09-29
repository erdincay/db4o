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
using System;
using System.Text;

namespace Db4objects.Db4o.Tests.CLI1.CrossPlatform
{
	internal class Movies
	{
#if !CF
		private readonly String[][] _notes;
		private int _counter;

		public Movies()
		{
			_notes = new String[3][];
		}

		public void Add(string movie, string genre)
		{
			if (_counter < _notes.Length)
			{
				_notes[_counter] = new string[] { movie, genre };
				_counter++;
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < _counter; i++)
			{
				sb.AppendFormat("{0}/{1}\r\n", _notes[i][0], _notes[i][1]);
			}

			return sb.ToString();
		}
#endif
	}
}
