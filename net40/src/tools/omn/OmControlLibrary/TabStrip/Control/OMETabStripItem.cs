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
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using OMControlLibrary.Design;

namespace OMControlLibrary
{
	[Designer(typeof(OMETabStripItemDesigner))]
	[ToolboxItem(false)]
	[DefaultProperty("Title")]
	[DefaultEvent("Changed")]
	public class OMETabStripItem : Panel
	{
		#region Events

		public event EventHandler Changed;

		#endregion

		#region Fields

		//private DrawItemState drawState = DrawItemState.None;
		private RectangleF stripRect = Rectangle.Empty;
		private Image image = null;
		private bool canClose = true;
		private bool selected = false;
		private bool visible = true;
		private bool isDrawn = false;
		private string title = string.Empty;

		#endregion

		#region Props

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size Size
		{
			get { return base.Size; }
			set { base.Size = value; }
		}

		[DefaultValue(true)]
		public new bool Visible
		{
			get { return visible; }
			set
			{
				if (visible == value)
					return;

				visible = value;
				OnChanged();
			}
		}

		internal RectangleF StripRect
		{
			get { return stripRect; }
			set { stripRect = value; }
		}

		[Browsable(false)]
		[DefaultValue(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public bool IsDrawn
		{
			get { return isDrawn; }
			set
			{
				if (isDrawn == value)
					return;

				isDrawn = value;
			}
		}

		/// <summary>
		/// Image of <see cref="OMETabStripItem"/> which will be displayed
		/// on menu items.
		/// </summary>
		[DefaultValue(null)]
		public Image Image
		{
			get { return image; }
			set { image = value; }
		}

		[DefaultValue(true)]
		public bool CanClose
		{
			get { return canClose; }
			set { canClose = value; }
		}

		[DefaultValue("Name")]
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				if (title == value)
					return;

				title = value;
				OnChanged();
			}
		}

		/// <summary>
		/// Gets and sets a value indicating if the page is selected.
		/// </summary>
		[DefaultValue(false)]
		[Browsable(false)]
		public bool Selected
		{
			get { return selected; }
			set
			{
				if (selected == value)
					return;

				selected = value;
			}
		}

		[Browsable(false)]
		public string Caption
		{
			get { return Title; }
		}

		#endregion

		#region Ctor

		public OMETabStripItem()
			: this(string.Empty, null)
		{
		}

		public OMETabStripItem(Control displayControl)
			: this(string.Empty, displayControl)
		{
		}

		public OMETabStripItem(string caption, Control displayControl)
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ContainerControl, true);

			selected = false;
			Visible = true;

			UpdateText(caption, displayControl);

			//Add to controls
			if (displayControl != null)
				Controls.Add(displayControl);
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Handles proper disposition of the tab page control.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				if (image != null)
					image.Dispose();
			}
		}

		#endregion

		#region ShouldSerialize

		public bool ShouldSerializeIsDrawn()
		{
			return false;
		}

		public bool ShouldSerializeDock()
		{
			return false;
		}

		public bool ShouldSerializeControls()
		{
			return Controls != null && Controls.Count > 0;
		}

		public bool ShouldSerializeVisible()
		{
			return true;
		}

		#endregion

		#region Methods

		private void UpdateText(string caption, Control displayControl)
		{
			if (displayControl != null && displayControl is ICaptionSupport)
			{
				ICaptionSupport capControl = displayControl as ICaptionSupport;
				Title = capControl.Caption;
			}
			else if (caption.Length <= 0 && displayControl != null)
			{
				Title = displayControl.Text;
			}
			else if (caption != null)
			{
				Title = caption;
			}
			else
			{
				Title = string.Empty;
			}
		}

		public void Assign(OMETabStripItem item)
		{
			Visible = item.Visible;
			Text = item.Text;
			CanClose = item.CanClose;
			Tag = item.Tag;
		}

		protected internal virtual void OnChanged()
		{
			if (Changed != null)
				Changed(this, EventArgs.Empty);
		}

		/// <summary>
		/// Return a string representation of page.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Caption;
		}

		#endregion
	}
}
