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
using System.Drawing;
using System.Windows.Forms;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of UIStyle.
	/// </summary>
	public class UIStyle
	{
        public static readonly Color Db4oGrey = Color.FromArgb(0xFF, 255, 255, 255);

        public static readonly Color Db4oRed = Color.FromArgb(0xFF, 187, 30, 30);
    
		public static readonly Color BackColor = Color.FromArgb(0xFF, 0x83, 0x83, 0x83);
		
		public static readonly Color TextColor = Color.White;
		
		public static void Apply(Control control)
		{
			control.BackColor = UIStyle.BackColor;
			control.ForeColor = UIStyle.TextColor;
		}
		
		public static void ApplyConsoleStyle(Control control)
		{
			control.BackColor = UIStyle.BackColor;
            control.ForeColor = Color.Black;
		}
		
		public static void ApplyButtonStyle(Control control)
		{
			control.ForeColor = UIStyle.Db4oGrey;
			control.BackColor = UIStyle.Db4oRed;
		}
		
		private UIStyle()
		{
		}
	}
}
