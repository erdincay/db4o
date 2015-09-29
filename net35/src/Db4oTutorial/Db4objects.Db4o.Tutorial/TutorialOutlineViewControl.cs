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
	/// Description of TutorialOutlineViewControl.
	/// </summary>
	public class TutorialOutlineViewControl : UserControl
	{
		private TreeView _tree;
		private PictureBox _logo;

		public TutorialOutlineViewControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			UIStyle.Apply(this);
			UIStyle.Apply(_tree);	
		}
		
		public TreeView TreeView
		{
			get
			{
				return _tree;
			}
		}

		public void SetLogo(string path)
		{
			_logo.Image = Image.FromFile(path);
		}
		
		#region Windows Forms Designer generated code
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent() {
			this._logo = new System.Windows.Forms.PictureBox();
			this._tree = new System.Windows.Forms.TreeView();
			((System.ComponentModel.ISupportInitialize)(this._logo)).BeginInit();
			this.SuspendLayout();
			// 
			// _logo
			// 
			this._logo.BackColor = System.Drawing.Color.White;
			this._logo.Dock = System.Windows.Forms.DockStyle.Top;
			this._logo.Location = new System.Drawing.Point(0, 0);
			this._logo.Name = "_logo";
			this._logo.Size = new System.Drawing.Size(292, 42);
			this._logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this._logo.TabIndex = 0;
			this._logo.TabStop = false;
			// 
			// _tree
			// 
			this._tree.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._tree.Dock = System.Windows.Forms.DockStyle.Fill;
			this._tree.Location = new System.Drawing.Point(0, 42);
			this._tree.Name = "_tree";
			this._tree.Size = new System.Drawing.Size(292, 246);
			this._tree.TabIndex = 1;
			// 
			// TutorialOutlineViewControl
			// 
			this.Controls.Add(this._tree);
			this.Controls.Add(this._logo);
			this.Name = "TutorialOutlineViewControl";
			this.Size = new System.Drawing.Size(292, 288);
			((System.ComponentModel.ISupportInitialize)(this._logo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		
	}
}
