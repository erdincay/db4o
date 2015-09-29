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
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using OMControlLibrary;

namespace OMControlLibrary.Design
{
	public class OMETabStripDesigner : ParentControlDesigner
	{
		#region Fields

		IComponentChangeService changeService;

		#endregion

		#region Initialize & Dispose

		public override void Initialize(System.ComponentModel.IComponent component)
		{
			base.Initialize(component);

			//Design services
			changeService = (IComponentChangeService)GetService(typeof(IComponentChangeService));

			//Bind design events
			changeService.ComponentRemoving += new ComponentEventHandler(OnRemoving);

			Verbs.Add(new DesignerVerb("Add TabStrip", new EventHandler(OnAddTabStrip)));
			Verbs.Add(new DesignerVerb("Remove TabStrip", new EventHandler(OnRemoveTabStrip)));
		}

		protected override void Dispose(bool disposing)
		{
			changeService.ComponentRemoving -= new ComponentEventHandler(OnRemoving);

			base.Dispose(disposing);
		}

		#endregion

		#region Private Methods

		private void OnRemoving(object sender, ComponentEventArgs e)
		{
			IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));

			//Removing a button
			if (e.Component is OMETabStripItem)
			{
				OMETabStripItem itm = e.Component as OMETabStripItem;
				if (Control.Items.Contains(itm))
				{
					changeService.OnComponentChanging(Control, null);
					Control.RemoveTab(itm);
					changeService.OnComponentChanged(Control, null, null, null);
					return;
				}
			}

			if (e.Component is OMETabStrip)
			{
				for (int i = Control.Items.Count - 1; i >= 0; i--)
				{
					OMETabStripItem itm = Control.Items[i];
					changeService.OnComponentChanging(Control, null);
					Control.RemoveTab(itm);
					host.DestroyComponent(itm);
					changeService.OnComponentChanged(Control, null, null, null);
				}
			}
		}

		private void OnAddTabStrip(object sender, EventArgs e)
		{
			IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
			DesignerTransaction transaction = host.CreateTransaction("Add TabStrip");
			OMETabStripItem itm = (OMETabStripItem)host.CreateComponent(typeof(OMETabStripItem));
			changeService.OnComponentChanging(Control, null);
			Control.AddTab(itm);
			int indx = Control.Items.IndexOf(itm) + 1;
			itm.Title = "TabStrip Page " + indx.ToString();
			Control.SelectItem(itm);
			changeService.OnComponentChanged(Control, null, null, null);
			transaction.Commit();
		}

		private void OnRemoveTabStrip(object sender, EventArgs e)
		{
			IDesignerHost host = (IDesignerHost)GetService(typeof(IDesignerHost));
			DesignerTransaction transaction = host.CreateTransaction("Remove Button");
			changeService.OnComponentChanging(Control, null);
			OMETabStripItem itm = Control.Items[Control.Items.Count - 1];
			Control.UnSelectItem(itm);
			Control.Items.Remove(itm);
			changeService.OnComponentChanged(Control, null, null, null);
			transaction.Commit();
		}

		#endregion

		#region Overrides

		/// <summary>
		/// Check HitTest on <see cref="OMETabStrip"/> control and
		/// let the user click on close and menu buttons.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		protected override bool GetHitTest(Point point)
		{
			HitTestResult result = Control.HitTest(point);
			if (result == HitTestResult.CloseButton || result == HitTestResult.MenuGlyph)
				return true;

			return false;
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);

			properties.Remove("DockPadding");
			properties.Remove("DrawGrid");
			properties.Remove("Margin");
			properties.Remove("Padding");
			properties.Remove("BorderStyle");
			properties.Remove("ForeColor");
			properties.Remove("BackColor");
			properties.Remove("BackgroundImage");
			properties.Remove("BackgroundImageLayout");
			properties.Remove("GridSize");
			properties.Remove("ImeMode");
		}

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == 0x201)
			{
				Point pt = Control.PointToClient(Cursor.Position);
				OMETabStripItem itm = Control.GetTabItemByPoint(pt);
				if (itm != null)
				{
					Control.SelectedItem = itm;
					ArrayList selection = new ArrayList();
					selection.Add(itm);
					ISelectionService selectionService = (ISelectionService)GetService(typeof(ISelectionService));
					selectionService.SetSelectedComponents(selection);
				}
			}

			base.WndProc(ref msg);
		}

		public override ICollection AssociatedComponents
		{
			get
			{
				return Control.Items;
			}
		}

		public new virtual OMETabStrip Control
		{
			get
			{
				return base.Control as OMETabStrip;
			}
		}

		#endregion
	}
}
