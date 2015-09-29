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
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace OMControlLibrary.BaseClasses
{
	[ToolboxItem(false)]
	public class BaseStyledPanel : ContainerControl
	{
		#region Fields

		private static ToolStripProfessionalRenderer renderer;

		#endregion

		#region Events

		public event EventHandler ThemeChanged;

		#endregion

		#region Ctor

		static BaseStyledPanel()
		{
			renderer = new ToolStripProfessionalRenderer();
		}

		public BaseStyledPanel()
		{
			// Set painting style for better performance.
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.UserPaint, true);
		}

		#endregion

		#region Methods

		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			UpdateRenderer();
			Invalidate();
		}

		protected virtual void OnThemeChanged(EventArgs e)
		{
			if (ThemeChanged != null)
				ThemeChanged(this, e);
		}

		private void UpdateRenderer()
		{
			if (!UseThemes)
			{
				renderer.ColorTable.UseSystemColors = true;
			}
			else
			{
				renderer.ColorTable.UseSystemColors = false;
			}
		}

		#endregion

		#region Props

		[Browsable(false)]
		public ToolStripProfessionalRenderer ToolStripRenderer
		{
			get { return renderer; }
		}

		[DefaultValue(true)]
		[Browsable(false)]
		public bool UseThemes
		{
			get
			{
				return VisualStyleRenderer.IsSupported && VisualStyleInformation.IsSupportedByOS && Application.RenderWithVisualStyles;
			}
		}

		#endregion
	}
}
