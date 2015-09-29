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
namespace Db4objects.Db4o.SilverlightTestHost
{
	partial class WebBrowerHost
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.silverlightTestHost = new System.Windows.Forms.WebBrowser();
			this.SuspendLayout();
			// 
			// silverlightTestHost
			// 
			this.silverlightTestHost.Dock = System.Windows.Forms.DockStyle.Fill;
			this.silverlightTestHost.Location = new System.Drawing.Point(0, 0);
			this.silverlightTestHost.MinimumSize = new System.Drawing.Size(20, 20);
			this.silverlightTestHost.Name = "silverlightTestHost";
			this.silverlightTestHost.Size = new System.Drawing.Size(575, 525);
			this.silverlightTestHost.TabIndex = 0;
			// 
			// WebBrowerHost
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(575, 525);
			this.Controls.Add(this.silverlightTestHost);
			this.Name = "WebBrowerHost";
			this.Text = "WebBrowerHost";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.WebBrowser silverlightTestHost;
	}
}