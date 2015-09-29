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
using OManager.BusinessLayer.Config;

namespace OMControlLibrary
{
	public partial class SearchPathDialog :  Form, INotifyPropertyChanged
	{
		public SearchPathDialog()
		{
			InitializeComponent();

			InitializePaths();
			
			InitializeDatabinding();

			StartPosition = FormStartPosition.CenterParent;
		}

		private void InitializePaths()
		{
			foreach (string path in _searchPath.Paths)
			{
				listSearchPath.Items.Add(path);
			}

			if (listSearchPath.Items.Count > 0)
			{
				listSearchPath.SelectedIndex = 0;
			}
		}

		private void InitializeDatabinding()
		{
			btnRemovePath.DataBindings.Add("Enabled", this, LIST_EMPTY__CHECK_PROPERTY_NAME, true, DataSourceUpdateMode.Never);
		}

		public bool IsSearchPathListEmpty
		{
			get { return listSearchPath.Items.Count > 0 && listSearchPath.SelectedItem != null; }	
		}

		private void btnAddPath_Click(object sender, EventArgs e)
		{
			TryAddSearchPath((string) listSearchPath.SelectedItem);
		}

		private void TryAddSearchPath(string selectedPath)
		{
			using (FolderBrowserDialog browser = new FolderBrowserDialog())
			{
				browser.Description = "Select search folder";
				if (!string.IsNullOrEmpty(selectedPath))
				{
					browser.SelectedPath = selectedPath;
				}
				if (browser.ShowDialog() == DialogResult.OK)
				{
					if (_searchPath.Add(browser.SelectedPath))
					{
						listSearchPath.Items.Add(browser.SelectedPath);
					}
				}
			}
		}

		private void btnRemovePath_Click(object sender, EventArgs e)
		{
			DeleteSearchPath();
		}

		private void DeleteSearchPath()
		{
			int selectedIndex = listSearchPath.SelectedIndex;
			if (selectedIndex < 0)
			{
				return;	
			}

			_searchPath.Remove((string)listSearchPath.SelectedItem);
			listSearchPath.Items.Remove(listSearchPath.SelectedItem);
			listSearchPath.SelectedIndex = Math.Min(selectedIndex, listSearchPath.Items.Count-1);
		}

		private void listSearchPath_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Delete:
					DeleteSearchPath();
					break;

				case Keys.Insert:
					TryAddSearchPath((string) listSearchPath.SelectedItem);
					break;
			}
		}

		private void listSearchPath_SelectedIndexChanged(object sender, EventArgs e)
		{
			RaisePropertyChangedEvent(LIST_EMPTY__CHECK_PROPERTY_NAME);
		}

		private void RaisePropertyChangedEvent(string propertyName)
		{
			if (null != PropertyChanged)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private readonly ISearchPath _searchPath = Config.Instance.AssemblySearchPath;

		private const string LIST_EMPTY__CHECK_PROPERTY_NAME = "IsSearchPathListEmpty";
	}
}
