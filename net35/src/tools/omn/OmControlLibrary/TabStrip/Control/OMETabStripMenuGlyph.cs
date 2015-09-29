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
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OMControlLibrary
{
	internal class OMETabStripMenuGlyph
	{
		#region Fields

		private Rectangle glyphRect = Rectangle.Empty;
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
			get { return glyphRect; }
			set { glyphRect = value; }
		}

		#endregion

		#region Ctor

		internal OMETabStripMenuGlyph(ToolStripProfessionalRenderer renderer)
		{
			this.renderer = renderer;
		}

		#endregion

		#region Methods

		public void DrawGlyph(Graphics g)
		{
			if (isMouseOver)
			{
				Color fill = renderer.ColorTable.ButtonSelectedHighlight; //Color.FromArgb(35, SystemColors.Highlight);
				g.FillRectangle(new SolidBrush(fill), glyphRect);
				Rectangle borderRect = glyphRect;

				borderRect.Width--;
				borderRect.Height--;

				g.DrawRectangle(SystemPens.Highlight, borderRect);
			}

			SmoothingMode bak = g.SmoothingMode;

			g.SmoothingMode = SmoothingMode.Default;

			using (Pen pen = new Pen(Color.Black))
			{
				pen.Width = 2;

				g.DrawLine(pen, new Point(glyphRect.Left + (glyphRect.Width / 3) - 2, glyphRect.Height / 2 - 1),
					new Point(glyphRect.Right - (glyphRect.Width / 3), glyphRect.Height / 2 - 1));
			}

			g.FillPolygon(Brushes.Black, new Point[]{
                new Point(glyphRect.Left + (glyphRect.Width / 3)-2, glyphRect.Height / 2+2),
                new Point(glyphRect.Right - (glyphRect.Width / 3), glyphRect.Height / 2+2),
                new Point(glyphRect.Left + glyphRect.Width / 2-1,glyphRect.Bottom-4)});

			g.SmoothingMode = bak;
		}

		#endregion
	}
}
