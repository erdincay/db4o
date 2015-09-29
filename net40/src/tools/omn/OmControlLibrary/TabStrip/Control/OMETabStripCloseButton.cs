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

namespace OMControlLibrary
{
	internal class OMETabStripCloseButton
	{
		#region Fields

		private Rectangle crossRect = Rectangle.Empty;
		private bool isMouseOver = false;
		private ToolStripProfessionalRenderer renderer;

		#endregion

		#region Props

		public bool IsMouseOver
		{
			get { return isMouseOver; }
			set { isMouseOver = value; }
		}

		public Rectangle Bounds
		{
			get { return crossRect; }
			set { crossRect = value; }
		}

		#endregion

		#region Ctor

		internal OMETabStripCloseButton(ToolStripProfessionalRenderer renderer)
		{
			this.renderer = renderer;
		}

		#endregion

		#region Methods

		public void DrawCross(Graphics g)
		{
			if (isMouseOver)
			{
				Color fill = renderer.ColorTable.ButtonSelectedHighlight;

				g.FillRectangle(new SolidBrush(fill), crossRect);

				Rectangle borderRect = crossRect;

				borderRect.Width--;
				borderRect.Height--;

				g.DrawRectangle(SystemPens.Highlight, borderRect);
			}

			using (Pen pen = new Pen(Color.Black, 1.6f))
			{
				g.DrawLine(pen, crossRect.Left + 3, crossRect.Top + 3,
					crossRect.Right - 5, crossRect.Bottom - 4);

				g.DrawLine(pen, crossRect.Right - 5, crossRect.Top + 3,
					crossRect.Left + 3, crossRect.Bottom - 4);
			}
		}

		#endregion
	}
}
