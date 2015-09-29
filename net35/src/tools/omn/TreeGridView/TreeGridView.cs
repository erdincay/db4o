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
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing.Design;

using OME.Logging.Common;
using OME.Logging.Tracing;

namespace OME.AdvancedDataGridView
{
	/// <summary>
	/// Summary description for TreeGridView.
	/// </summary>
    [System.ComponentModel.DesignerCategory("code"),
    Designer(typeof(System.Windows.Forms.Design.ControlDesigner)),
	ComplexBindingProperties(),
    Docking(DockingBehavior.Ask)]
	public class TreeGridView:DataGridView
	{
        #region Member Variables
        private TreeGridNode _root;
        private TreeGridColumn _expandableColumn;
        internal ImageList _imageList;
        private bool _inExpandCollapse = false;
        internal bool _inExpandCollapseMouseCapture = false;
        private Control hideScrollBarControl;
        private bool _showLines = true;
        private bool _virtualNodes = false;

        public ContextMenuStrip m_deleteContextMenuStrip;
        private const string CONTEXTMENU_CAPTION_SETTONULL = "Set to Null";
        private const string CONTEXTMENU_NAME_DELETE = "MenuDelete";

        public event EventHandler<ContextItemClickedEventArg> OnContextMenuItemClicked;
        public event EventHandler<ContextItemClickedEventArg> OnContextMenuOpening;
        TreeGridView.HitTestInfo hitTestInfo;

        #endregion

        #region Constructor
        public TreeGridView()
		{
            this.SetStyle(ControlStyles.CacheText |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.Opaque, true);

			// Control when edit occurs because edit mode shouldn't start when expanding/collapsing
			this.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.RowTemplate = new TreeGridNode() as DataGridViewRow;
			// This sample does not support adding or deleting rows by the user.
			this.AllowUserToAddRows = false;
			this.AllowUserToDeleteRows = false;
			this._root = new TreeGridNode(this);
			this._root.IsRoot = true;
            
           
            SetDefaultProperties();
			// Ensures that all rows are added unshared by listening to the CollectionChanged event.
			base.Rows.CollectionChanged += delegate(object sender, System.ComponentModel.CollectionChangeEventArgs e){};
        }
       
        private void SetDefaultProperties()
        {
            this.BackgroundColor = Color.White;
            this.RowHeadersVisible = false;
            this.AutoGenerateColumns = false;
            this.AllowUserToResizeRows = false;
            this.AllowUserToAddRows = false;
            this.AllowUserToOrderColumns = false;
            this.AllowDrop = true;
            this.MultiSelect = false;
            this.GridColor = SystemColors.ControlDark;
            this.ScrollBars = ScrollBars.Both;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AllowUserToDeleteRows = false;
            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            this.Visible = true;
            this.AutoGenerateColumns = false;
            this.EnableHeadersVisualStyles = false;
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            DataGridViewCellStyle cellstyle = new DataGridViewCellStyle();
            cellstyle.BackColor = Color.FromArgb(218, 232, 241);
        }
        
        #endregion

        #region Keyboard F2 to begin edit support
        protected override void OnKeyDown(KeyEventArgs e)
		{
			// Cause edit mode to begin since edit mode is disabled to support 
			// expanding/collapsing 
			base.OnKeyDown(e);
			if (!e.Handled)
			{
				if (e.KeyCode == Keys.F2 && this.CurrentCellAddress.X > -1 && this.CurrentCellAddress.Y >-1)
				{
					if (!this.CurrentCell.Displayed)
					{
						this.FirstDisplayedScrollingRowIndex = this.CurrentCellAddress.Y;
					}
					else
					{
						// TODO:calculate if the cell is partially offscreen and if so scroll into view
					}
					//this.SelectionMode = DataGridViewSelectionMode.CellSelect;
					this.BeginEdit(true);
				}
				else if (e.KeyCode == Keys.Enter && !this.IsCurrentCellInEditMode)
				{
					this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
					this.CurrentCell.OwningRow.Selected = true;
				}
			}
        }
        #endregion

        #region Shadow and hide DGV properties

        // This sample does not support databinding
		[Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
		public new object DataSource
		{
			get { return null; }
			set { throw new NotSupportedException("The TreeGridView does not support databinding"); }
		}

		[Browsable(false),
	    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
		public new object DataMember
		{
			get { return null; }
			set { throw new NotSupportedException("The TreeGridView does not support databinding"); }
		}

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRowCollection Rows
        {
            get { return base.Rows; }
        }

        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new bool VirtualMode
        {
            get { return false; }
            set { throw new NotSupportedException("The TreeGridView does not support virtual mode"); }
        }

        // none of the rows/nodes created use the row template, so it is hidden.
        [Browsable(false),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        EditorBrowsable(EditorBrowsableState.Never)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate; }
            set { base.RowTemplate = value; }
        }

        #endregion

        #region Public methods
        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(DataGridViewRow row)
        {
            return row as TreeGridNode;
        }

        [Description("Returns the TreeGridNode for the given DataGridViewRow")]
        public TreeGridNode GetNodeForRow(int index)
        {
            return GetNodeForRow(base.Rows[index]);
        }
        #endregion

        #region Public properties
        [Category("Data"),
		Description("The collection of root nodes in the treelist."),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public TreeGridNodeCollection Nodes
		{
			get
			{
				return this._root.Nodes;
			}
		}

		public new TreeGridNode CurrentRow
		{
			get
			{
				return base.CurrentRow as TreeGridNode;
			}
		}

        [DefaultValue(false),
        Description("Causes nodes to always show as expandable. Use the NodeExpanding event to add nodes.")]
        public bool VirtualNodes
        {
            get { return _virtualNodes; }
            set { _virtualNodes = value; }
        }
	
		public TreeGridNode CurrentNode
		{
			get
			{
				return this.CurrentRow;
			}
		}

        [DefaultValue(true)]
        public bool ShowLines
        {
            get { return this._showLines; }
            set { 
                if (value != this._showLines) {
                    this._showLines = value;
                    this.Invalidate();
                } 
            }
        }
	
		public ImageList ImageList
		{
			get { return this._imageList; }
			set { 
				this._imageList = value; 
				//TODO: should we invalidate cell styles when setting the image list?
			
			}
		}

        public new int RowCount
        {
            get { return this.Nodes.Count; }
            set
            {
                for (int i = 0; i < value; i++)
                    this.Nodes.Add(new TreeGridNode());

            }
        }
        #endregion

        #region Site nodes and collapse/expand support
        protected override void OnRowsAdded(DataGridViewRowsAddedEventArgs e)
        {
            base.OnRowsAdded(e);
            // Notify the row when it is added to the base grid 
            int count = e.RowCount - 1;
            TreeGridNode row;
            while (count >= 0)
            {
                row = base.Rows[e.RowIndex + count] as TreeGridNode;
                if (row != null)
                {
                    row.Sited();
                }
                count--;
            }
        }

		internal protected void UnSiteAll()
		{
			this.UnSiteNode(this._root);
		}

		internal protected virtual void UnSiteNode(TreeGridNode node)
		{
            if (node.IsSited || node.IsRoot)
			{
				// remove child rows first
				foreach (TreeGridNode childNode in node.Nodes)
				{
					this.UnSiteNode(childNode);
				}

				// now remove this row except for the root
				if (!node.IsRoot)
				{
					base.Rows.Remove(node);
					// Row isn't sited in the grid anymore after remove. Note that we cannot
					// Use the RowRemoved event since we cannot map from the row index to
					// the index of the expandable row/node.
					node.UnSited();
				}
			}
		}

		internal protected virtual bool CollapseNode(TreeGridNode node)
		{
			if (node.IsExpanded)
			{
				CollapsingEventArgs exp = new CollapsingEventArgs(node);
				this.OnNodeCollapsing(exp);

				if (!exp.Cancel)
				{
					this.LockVerticalScrollBarUpdate(true);
                    this.SuspendLayout();
                    _inExpandCollapse = true;
                    node.IsExpanded = false;

					foreach (TreeGridNode childNode in node.Nodes)
					{
						Debug.Assert(childNode.RowIndex != -1, "Row is NOT in the grid.");
						this.UnSiteNode(childNode);
					}

					CollapsedEventArgs exped = new CollapsedEventArgs(node);
					this.OnNodeCollapsed(exped);
					//TODO: Convert this to a specific NodeCell property
                    _inExpandCollapse = false;
                    this.LockVerticalScrollBarUpdate(false);
                    this.ResumeLayout(true);
                    this.InvalidateCell(node.Cells[0]);

				}

				return !exp.Cancel;
			}
			else
			{
				// row isn't expanded, so we didn't do anything.				
				return false;
			}
		}

		internal protected virtual void SiteNode(TreeGridNode node)
		{
			//TODO: Raise exception if parent node is not the root or is not sited.
			int rowIndex = -1;
			TreeGridNode currentRow;
			node._grid = this;

			if (node.Parent != null && node.Parent.IsRoot == false)
			{
				// row is a child
				Debug.Assert(node.Parent != null && node.Parent.IsExpanded == true);

				if (node.Index > 0)
				{
					currentRow = node.Parent.Nodes[node.Index - 1];
				}
				else
				{
					currentRow = node.Parent;
				}
			}
			else
			{
				// row is being added to the root
				if (node.Index > 0)
				{
					currentRow = node.Parent.Nodes[node.Index - 1];
				}
				else
				{
					currentRow = null;
				}

			}

			if (currentRow != null)
			{
				while (currentRow.Level >= node.Level)
				{
					if (currentRow.RowIndex < base.Rows.Count - 1)
					{
						currentRow = base.Rows[currentRow.RowIndex + 1] as TreeGridNode;
						Debug.Assert(currentRow != null);
					}
					else
						// no more rows, site this node at the end.
						break;

				}
				if (currentRow == node.Parent)
					rowIndex = currentRow.RowIndex + 1;
				else if (currentRow.Level < node.Level)
					rowIndex = currentRow.RowIndex;
				else
					rowIndex = currentRow.RowIndex + 1;
			}
			else
				rowIndex = 0;


			Debug.Assert(rowIndex != -1);
			this.SiteNode(node, rowIndex);

			Debug.Assert(node.IsSited);
			if (node.IsExpanded)
			{
				// add all child rows to display
				foreach (TreeGridNode childNode in node.Nodes)
				{
					//TODO: could use the more efficient SiteRow with index.
					this.SiteNode(childNode);
				}
			}
		}

		internal protected virtual void SiteNode(TreeGridNode node, int index)
		{
			if (index < base.Rows.Count)
			{
				base.Rows.Insert(index, node);
			}
			else
			{
				// for the last item.
				base.Rows.Add(node);
			}
		}

		internal protected virtual bool ExpandNode(TreeGridNode node)
		{
            if (!node.IsExpanded || this._virtualNodes)
			{
				ExpandingEventArgs exp = new ExpandingEventArgs(node);
				this.OnNodeExpanding(exp);

				if (!exp.Cancel)
				{
					this.LockVerticalScrollBarUpdate(true);
                    this.SuspendLayout();
                    _inExpandCollapse = true;
                    node.IsExpanded = true;

					//TODO Convert this to a InsertRange
					foreach (TreeGridNode childNode in node.Nodes)
					{
						Debug.Assert(childNode.RowIndex == -1, "Row is already in the grid.");

						this.SiteNode(childNode);
					}

					ExpandedEventArgs exped = new ExpandedEventArgs(node);
					this.OnNodeExpanded(exped);
					//TODO: Convert this to a specific NodeCell property
                    _inExpandCollapse = false;
                    this.LockVerticalScrollBarUpdate(false);
                    this.ResumeLayout(true);
                    this.InvalidateCell(node.Cells[0]);
				}

				return !exp.Cancel;
			}
			else
			{
				// row is already expanded, so we didn't do anything.
				return false;
			}
        }


        protected override void OnCellMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            base.OnCellMouseDoubleClick(e);
            if (e.Button == MouseButtons.Left)
            {
                this.BeginEdit(true);
            }
        }
        //protected override void OnColumnHeaderMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        //{
        //    base.OnColumnHeaderMouseDoubleClick(e);
        //    return;
        //}

        //protected override void OnRowHeaderMouseDoubleClick(DataGridViewCellMouseEventArgs e)
        //{
        //    base.OnRowHeaderMouseDoubleClick(e);
        //    return;
        //}
        //protected override void OnMouseDoubleClick(MouseEventArgs e)
        //{
        //    base.OnMouseDoubleClick(e);
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        this.BeginEdit(true);
        //    }
        //}

        protected override void OnMouseUp(MouseEventArgs e)
        {
            // used to keep extra mouse moves from selecting more rows when collapsing
            try
            {
                base.OnMouseUp(e);
                this._inExpandCollapseMouseCapture = false;
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }


        }
        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    // while we are expanding and collapsing a node mouse moves are
        //    // supressed to keep selections from being messed up.
        //    //if (!this._inExpandCollapseMouseCapture)
        //    //    base.OnMouseMove(e);

        //    //try
        //    //{
        //    //    if (this.Rows.Count > 1)
        //    //    {
        //    //        DataGridView.HitTestInfo hitTestInfo = this.HitTest(e.X, e.Y);

        //    //        if (e.Button == MouseButtons.Left)
        //    //        {
        //    //            if (hitTestInfo.RowIndex != -1)
        //    //            {
        //    //                DataGridViewRow row = this.Rows[hitTestInfo.RowIndex];
        //    //                this.DoDragDrop(row, DragDropEffects.Copy);
        //    //            }
        //    //        }
        //    //    }
        //    //}
        //    //catch (Exception oEx)
        //    //{
        //    //    LoggingHelper.ShowMessage(oEx);
        //    //}
        //}
        #endregion

        #region Collapse/Expand events
        public event ExpandingEventHandler NodeExpanding;
        public event ExpandedEventHandler NodeExpanded;
        public event CollapsingEventHandler NodeCollapsing;
        public event CollapsedEventHandler NodeCollapsed;

        protected virtual void OnNodeExpanding(ExpandingEventArgs e)
        {
            if (this.NodeExpanding != null)
            {
                NodeExpanding(this, e);
            }
        }
        protected virtual void OnNodeExpanded(ExpandedEventArgs e)
        {
            if (this.NodeExpanded != null)
            {
                NodeExpanded(this, e);
            }
        }
        protected virtual void OnNodeCollapsing(CollapsingEventArgs e)
        {
            if (this.NodeCollapsing != null)
            {
                NodeCollapsing(this, e);
            }

        }
        protected virtual void OnNodeCollapsed(CollapsedEventArgs e)
        {
            if (this.NodeCollapsed != null)
            {
                NodeCollapsed(this, e);
            }
        }
        #endregion

        #region Helper methods
        protected override void Dispose(bool disposing)
        {
            //this._disposing = true;
            base.Dispose(Disposing);
            this.UnSiteAll();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // this control is used to temporarly hide the vertical scroll bar
            hideScrollBarControl = new Control();
            hideScrollBarControl.Visible = false;
            hideScrollBarControl.Enabled = false;
            hideScrollBarControl.TabStop = false;
            // control is disposed automatically when the grid is disposed
            this.Controls.Add(hideScrollBarControl);
        }

        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
            // ensure full row select
            base.OnRowEnter(e);
            if (this.SelectionMode == DataGridViewSelectionMode.CellSelect ||
                (this.SelectionMode == DataGridViewSelectionMode.FullRowSelect &&
                base.Rows[e.RowIndex].Selected == false))
            {
                this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                base.Rows[e.RowIndex].Selected = true;
            }
        }
        
		private void LockVerticalScrollBarUpdate(bool lockUpdate/*, bool delayed*/)
		{
            // Temporarly hide/show the vertical scroll bar by changing its parent
            if (!this._inExpandCollapse)
            {
                if (lockUpdate)
                {
                    this.VerticalScrollBar.Parent = hideScrollBarControl;
                }
                else
                {
                    this.VerticalScrollBar.Parent = this;
                }
            }
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (typeof(TreeGridColumn).IsAssignableFrom(e.Column.GetType()))
            {
                if (_expandableColumn == null)
                {
                    // identify the expanding column.			
                    _expandableColumn = (TreeGridColumn)e.Column;
                }
            }

            // Expandable Grid doesn't support sorting. This is just a limitation of the sample.
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;

            base.OnColumnAdded(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            try
            {
                base.OnMouseDown(e);
                hitTestInfo = this.HitTest(e.X, e.Y);
                DataGridViewHitTestType hitTestType = hitTestInfo.Type;

                if (this.Rows.Count < 1)
                {
                    if (this.ContextMenuStrip != null)
                        this.ContextMenuStrip = null;

                    return;
                }

                //If user clicks on blank space below grid, then remove the context menu
                if (hitTestInfo.Type == DataGridViewHitTestType.None || hitTestInfo.ColumnIndex!=1  ||
                     hitTestInfo.RowIndex == -1)
                {
                    this.ContextMenuStrip = null;
                    return;
                }

                if (e.Button == MouseButtons.Right)
                {
                    this.Rows[hitTestInfo.RowIndex].Selected = true;


                    if (hitTestType == DataGridViewHitTestType.Cell)
                    {
						
                        if (this.ContextMenuStrip == null)
                            BuildContextMenu();

                        this.ContextMenuStrip = m_deleteContextMenuStrip;
                    }
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);   
            }
        }

        private static class Win32Helper
        {
            public const int WM_SYSKEYDOWN = 0x0104,
                             WM_KEYDOWN = 0x0100,
                             WM_SETREDRAW = 0x000B;

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern IntPtr SendMessage(System.Runtime.InteropServices.HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern IntPtr SendMessage(System.Runtime.InteropServices.HandleRef hWnd, int msg, int wParam, int lParam);

            [System.Runtime.InteropServices.DllImport("USER32.DLL", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            public static extern bool PostMessage(System.Runtime.InteropServices.HandleRef hwnd, int msg, IntPtr wparam, IntPtr lparam);

        }
        #endregion

        #region Context Menu

        public void BuildContextMenu()
        {
            ToolStripMenuItem objMainMenu;

            try
            {
                m_deleteContextMenuStrip = new ContextMenuStrip();

                objMainMenu = new ToolStripMenuItem(CONTEXTMENU_CAPTION_SETTONULL);
                objMainMenu.Name = CONTEXTMENU_NAME_DELETE;
                objMainMenu.Tag = CONTEXTMENU_NAME_DELETE;

                m_deleteContextMenuStrip.Items.Add(objMainMenu);

                m_deleteContextMenuStrip.ItemClicked += m_deleteContextMenuStrip_ItemClicked;
                m_deleteContextMenuStrip.Opening += m_deleteContextMenuStrip_Opening;
            }
            catch(Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }
        }

        void m_deleteContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

            ContextItemClickedEventArg arg = new ContextItemClickedEventArg();
            arg.CancelEventArguments = e;

            DataGridViewHitTestType hitTestType = hitTestInfo.Type;

            if (hitTestType == DataGridViewHitTestType.Cell)
            {
                arg.Data = this;//.Rows[hitTestInfo.RowIndex];
            }


            if (OnContextMenuOpening != null)
            {
                OnContextMenuOpening(sender, arg);
            }
        }

        void m_deleteContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
             try
            {
                ToolStripItem tsItem = e.ClickedItem;

                if (tsItem.Tag != null)
                {
                    ContextItemClickedEventArg arg = new ContextItemClickedEventArg();
                    arg.Item = e.ClickedItem;

                    
                    DataGridViewHitTestType hitTestType = hitTestInfo.Type;

                    if (hitTestType == DataGridViewHitTestType.Cell)
                    {
                        arg.Data = this;//.Rows[hitTestInfo.RowIndex];
                    }

                    

                    if (OnContextMenuItemClicked != null)
                        OnContextMenuItemClicked(sender, arg);
                }
            }
            catch (Exception oEx)
            {
                LoggingHelper.ShowMessage(oEx);
            }

        }

        #endregion

    }

    /// <summary>
    /// DBContextItemClickedEventArg : For handling the contextmenu click
    /// </summary>
    public class ContextItemClickedEventArg : System.EventArgs
    {
        private object m_data = null;
        private CancelEventArgs m_cancelEventArguments  = null;
        private object m_item = null;

        public object Item
        {
            get { return m_item; }
            set { m_item = value; }
        }

        public CancelEventArgs CancelEventArguments
        {
            get { return m_cancelEventArguments; }
            set { m_cancelEventArguments = value; }
        }

        public object Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public ContextItemClickedEventArg(object data)
        {
            m_data = data;
        }

        public ContextItemClickedEventArg()
        {

        }
    }

}
