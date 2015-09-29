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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using OManager.BusinessLayer.Login;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.Connection;
using OManager.DataLayer.PropertyTable;
using OManager.DataLayer.Reflection;
using OMControlLibrary.Common;
using OME.Logging.Common;
using OME.Logging.Tracing;
using Constants=OMControlLibrary.Common.Constants;

namespace OMControlLibrary
{
	public partial class PropertiesTab : ViewBase
	{
		#region Member Variables

		readonly dbDataGridView dbGridViewProperties;

		private bool m_showObjectPropertiesTab;
		private bool m_showClassProperties;

		private static PropertiesTab instance;
	    object m_selectedObject;

		#endregion

		#region Properties

		public void SelectDefaultTab()
		{
			tabStripProperties.SelectedItem = tabStripProperties.Items[0];
		}

		public static PropertiesTab Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new PropertiesTab();
				}
				return instance;
			}
		}

		public bool ShowObjectPropertiesTab
		{
			get { return m_showObjectPropertiesTab; }
			set
			{
				m_showObjectPropertiesTab = value;
				tabItemObjectProperties.Visible = m_showObjectPropertiesTab;
			}
		}

		public bool ShowClassProperties
		{
			get { return m_showClassProperties; }
			set
			{
				m_showClassProperties = value;
				tabItemClassProperties.Visible =
					m_showClassProperties;
			}
		}

		#endregion

		#region Constructor
		public PropertiesTab()
		{
			InitializeComponent();
			tabStripProperties.HideMenuGlyph = true;
			dbGridViewProperties = new dbDataGridView();
			dbGridViewProperties.Dock = DockStyle.Fill;

			tabStripProperties.AlwaysShowMenuGlyph = false;
		}

		#endregion

		#region Private Methods

		public void DisplayDatabaseProperties()
		{
			try
			{
				OMETrace.WriteFunctionStart();

				dbGridViewProperties.Size = panelDatabaseProperties.Size;
				dbGridViewProperties.Rows.Clear();
				dbGridViewProperties.Columns.Clear();

				dbGridViewProperties.PopulateDisplayGrid(Constants.VIEW_DBPROPERTIES, null);

				dbGridViewProperties.Rows.Add(1);
				if (Helper.DbInteraction.GetTotalDbSize() == -1)
				{
					dbGridViewProperties.Rows[0].Cells[0].Value = "NA for Client/Server";
				}
				else
				{
					dbGridViewProperties.Rows[0].Cells[0].Value = Helper.DbInteraction.GetTotalDbSize() + " bytes";
				}

				dbGridViewProperties.Rows[0].Cells[1].Value = Helper.DbInteraction.NoOfClassesInDb().ToString();
				if (Helper.DbInteraction.GetFreeSizeOfDb() == -1)
				{
					dbGridViewProperties.Rows[0].Cells[2].Value = "NA for Client/Server";
				}
				else
				{
					dbGridViewProperties.Rows[0].Cells[2].Value = Helper.DbInteraction.GetFreeSizeOfDb() + " bytes";
				}

				if (!panelDatabaseProperties.Controls.Contains(dbGridViewProperties))
					panelDatabaseProperties.Controls.Add(dbGridViewProperties);

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void DisplayClassProperties()
		{
			try
			{
				OMETrace.WriteFunctionStart();

				if (Helper.ClassName != null)
				{
					if (dbInteraction.GetCurrentRecentConnection() != null)
						if (dbInteraction.GetCurrentRecentConnection().ConnParam != null)
						{
							buttonSaveIndex.Enabled = dbInteraction.GetCurrentRecentConnection().ConnParam.Host == null;

							labelNoOfObjects.Text = "Number of objects : " + dbInteraction.NoOfObjectsforAClass(Helper.ClassName);
							dbGridViewProperties.Size = Size;
							dbGridViewProperties.Rows.Clear();
							dbGridViewProperties.Columns.Clear();

							ArrayList fieldPropertiesList = GetFieldsForAllClass();
							dbGridViewProperties.ReadOnly = false;
							dbGridViewProperties.PopulateDisplayGrid(Constants.VIEW_CLASSPOPERTY, fieldPropertiesList);

							//Enable Disable IsIndexed Checkboxes
							foreach (DataGridViewRow row in dbGridViewProperties.Rows)
							{
                                IType type = row.Cells["Type"].Value as IType;
                                if (type.IsEditable)
								{
									row.Cells[2].ReadOnly = false;
								}
								else 
									row.Cells[2].ReadOnly = true;
							}

							if (!panelForClassPropTable.Controls.Contains(dbGridViewProperties))
								panelForClassPropTable.Controls.Add(dbGridViewProperties);
							dbGridViewProperties.Dock = DockStyle.Fill;
						}
				}
				else
					buttonSaveIndex.Enabled = false;

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static ArrayList GetFieldsForAllClass()
		{
			ClassPropertiesTable classPropTable = null;
			try
			{
				classPropTable = dbInteraction.GetClassProperties(Helper.ClassName);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return classPropTable.FieldEntries;
		}

		private void DisplayObjectProperties()
		{
			try
			{
				OMETrace.WriteFunctionStart();

				if (m_selectedObject != null)
				{
					ArrayList objectProperties = new ArrayList();
					ObjectPropertiesTable objTable = dbInteraction.GetObjectProperties(m_selectedObject);
					objectProperties.Add(objTable);

					dbGridViewProperties.Rows.Clear();
					dbGridViewProperties.Columns.Clear();
					dbGridViewProperties.Size = panelDatabaseProperties.Size;

					dbGridViewProperties.PopulateDisplayGrid(Constants.VIEW_OBJECTPROPERTIES, objectProperties);

					if (!panelObjectProperties.Controls.Contains(dbGridViewProperties))
						panelObjectProperties.Controls.Add(dbGridViewProperties);

					dbGridViewProperties.Refresh();
				}

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Event Handlers
		private void tabControlProperties_Resize(object sender, EventArgs e)
		{
			try
			{
				if (dbGridViewProperties != null)
					dbGridViewProperties.Size = Size;
				if (panelDataGrid != null)
				{
					panelDataGrid.Height = Height - tableLayoutPanelClassProp.Height;
					panelDataGrid.Width = Width;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

		}

		private void PropertiesTab_Load(object sender, EventArgs e)
		{
			try
			{
				CheckForIllegalCrossThreadCalls = false;
				dbGridViewProperties.Dock = DockStyle.Fill;

				LoadProperties();

				
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void LoadProperties()
		{
			tabItemObjectProperties.Visible = instance.m_showObjectPropertiesTab;
			if (!tabItemClassProperties.Visible)
				tabItemClassProperties.Visible = instance.m_showClassProperties;

			if (Helper.Tab_index.Equals(0))
			{
				DisplayDatabaseProperties();
			}
			else if (Helper.Tab_index.Equals(1))
			{
				DisplayClassProperties();
				buttonSaveIndex.Enabled = !dbInteraction.GetCurrentRecentConnection().ConnParam.ConnectionReadOnly;  
			}
			else if (Helper.Tab_index.Equals(2))
			{
				DisplayObjectProperties();
			}

			tabStripProperties.SelectedItem = tabStripProperties.Items[Helper.Tab_index];

			instance = this;
		}

		

		private void tabStripProperties_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e)
		{
			try
			{
				if (e.Item != null && e.ChangeType == OMETabStripItemChangeTypes.SelectionChanged)
					RefreshPropertiesTab(m_selectedObject);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Public Methods
		public void RefreshPropertiesTab(object selectedObject)
		{
			try
			{
				m_selectedObject = selectedObject;

				if (tabItemDatabaseProperties.Visible &&
					tabStripProperties.SelectedItem.Equals(tabItemDatabaseProperties))
				{
					DisplayDatabaseProperties();
				}
				else if (tabItemClassProperties.Visible &&
					tabStripProperties.SelectedItem.Equals(tabItemClassProperties))
				{
					DisplayClassProperties();
					buttonSaveIndex.Enabled = !dbInteraction.GetCurrentRecentConnection().ConnParam.ConnectionReadOnly;  
				}
				else if (tabItemObjectProperties.Visible &&
					tabStripProperties.SelectedItem.Equals(tabItemObjectProperties))
				{
					DisplayObjectProperties();
				}
				else
					tabStripProperties.SelectedItem = tabStripProperties.Items[0];

				Helper.Tab_index = Convert.ToInt32(tabStripProperties.SelectedItem.Tag.ToString());
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		#endregion



		private void SaveIndex()
		{
			try
			{
				SaveIndexClass saveIndexInstance = new SaveIndexClass(Helper.ClassName);
				
				foreach (DataGridViewRow row in dbGridViewProperties.Rows)
				{
					bool boolValue = Convert.ToBoolean(row.Cells[2].Value);
					saveIndexInstance.Fieldname.Add(row.Cells[0].Value.ToString());
					saveIndexInstance.Indexed.Add(boolValue);
				}

				CloseQueryResultToolWindows();
				
				ConnParams conparam = dbInteraction.GetCurrentRecentConnection().ConnParam;
				dbInteraction.CloseCurrDb();
				saveIndexInstance.SaveIndex();

				RecentQueries currRecentConnection = new RecentQueries(conparam);
				Db4oClient.conn = conparam;  
				RecentQueries tempRc = currRecentConnection.ChkIfRecentConnIsInDb();
				if (tempRc != null)
					currRecentConnection = tempRc;
				currRecentConnection.Timestamp = DateTime.Now;
				dbInteraction.ConnectoToDB(currRecentConnection);
				dbInteraction.SetCurrentRecentConnection(currRecentConnection);

				//Only if following line is added the index is saved.
				OMQuery omQuery = new OMQuery(saveIndexInstance.Classname , DateTime.Now);
				long[] objectid = dbInteraction.ExecuteQueryResults(omQuery);

				if (ObjectBrowser.Instance.ToolStripButtonAssemblyView.Checked)
					ObjectBrowser.Instance.DbtreeviewObject.FindNSelectNode(ObjectBrowser.Instance.DbAssemblyTreeView.Nodes[0], saveIndexInstance.Classname, ObjectBrowser.Instance.DbAssemblyTreeView);
				else
					ObjectBrowser.Instance.DbtreeviewObject.FindNSelectNode(ObjectBrowser.Instance.DbtreeviewObject.Nodes[0], saveIndexInstance.Classname, ObjectBrowser.Instance.DbtreeviewObject);

				tabStripProperties.SelectedItem = tabItemClassProperties;
				MessageBox.Show("Index Saved Successfully!", Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.OK);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void CloseQueryResultToolWindows()
		{
			foreach (Window entry in GetAllPluginWindows())
			{
				switch(entry.Caption  )
				{
					case Constants.QUERYBUILDER:
					case Constants.DB4OPROPERTIES :
					case Constants.DB4OBROWSER:
						break;
					default:
						if (entry.Visible )
						{
							entry.Close(vsSaveChanges.vsSaveChangesNo);
						}
						break;
				}
					
			}
		}


		private void buttonSaveIndex_Click(object sender, EventArgs e)
		{
			try
			{
				if (ListofModifiedObjects.Instance.Count > 0)
				{
					DialogResult dialogRes = MessageBox.Show("This will close all the Query result windows. Do you want to continue?", Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dialogRes == DialogResult.Yes)
					{
						SaveIndex();

					}
				}
				else
				{
					SaveIndex();
				}

			}
			catch (Exception e1)
			{
				LoggingHelper.ShowMessage(e1);
			}
		}




	}
}
