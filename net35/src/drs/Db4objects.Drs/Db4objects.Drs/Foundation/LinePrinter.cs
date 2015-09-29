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
using Db4objects.Drs.Foundation;

namespace Db4objects.Drs.Foundation
{
	public abstract class LinePrinter
	{
		private sealed class _LinePrinter_9 : LinePrinter
		{
			public _LinePrinter_9()
			{
			}

			public override void Println(string str)
			{
			}
		}

		public static readonly LinePrinter NullPrinter = new _LinePrinter_9();

		// do nothing
		public abstract void Println(string str);

		public static LinePrinter ForPrintStream(TextWriter ps)
		{
			return new _LinePrinter_20(ps);
		}

		private sealed class _LinePrinter_20 : LinePrinter
		{
			public _LinePrinter_20(TextWriter ps)
			{
				this.ps = ps;
			}

			public override void Println(string str)
			{
				ps.WriteLine(str);
			}

			private readonly TextWriter ps;
		}
	}
}
