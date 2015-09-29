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
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OMControlLibrary.Design
{
	public class OMETabStripItemDesigner : ParentControlDesigner
	{
		#region Fields

		OMETabStripItem TabStrip;

		#endregion

		#region Init & Dispose

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			TabStrip = component as OMETabStripItem;
		}

		#endregion

		#region Overrides

		protected override void PreFilterProperties(System.Collections.IDictionary properties)
		{
			base.PreFilterProperties(properties);

			properties.Remove("Dock");
			properties.Remove("AutoScroll");
			properties.Remove("AutoScrollMargin");
			properties.Remove("AutoScrollMinSize");
			properties.Remove("DockPadding");
			properties.Remove("DrawGrid");
			properties.Remove("Font");
			properties.Remove("Padding");
			properties.Remove("MinimumSize");
			properties.Remove("MaximumSize");
			properties.Remove("Margin");
			properties.Remove("ForeColor");
			properties.Remove("BackColor");
			properties.Remove("BackgroundImage");
			properties.Remove("BackgroundImageLayout");
			properties.Remove("RightToLeft");
			properties.Remove("GridSize");
			properties.Remove("ImeMode");
			properties.Remove("BorderStyle");
			properties.Remove("AutoSize");
			properties.Remove("AutoSizeMode");
			properties.Remove("Location");
		}

		public override SelectionRules SelectionRules
		{
			get
			{
				return 0;
			}
		}

		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			return (parentDesigner.Component is OMETabStrip);
		}

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			if (TabStrip != null)
			{
				using (Pen p = new Pen(SystemColors.ControlDark))
				{
					p.DashStyle = DashStyle.Dash;
					pe.Graphics.DrawRectangle(p, 0, 0, TabStrip.Width - 1, TabStrip.Height - 1);
				}
			}
		}

		#endregion
	}
}
