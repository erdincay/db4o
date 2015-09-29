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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using System.Reflection;
using OManager.BusinessLayer.UIHelper;
using OMControlLibrary.Common;
using OManager.BusinessLayer.Login;
using Microsoft.VisualStudio.CommandBars;
using OME.Logging.Common;
using Constants = OMControlLibrary.Common.Constants;


namespace OMControlLibrary
{
	/// <summary>
	/// Using this user control, user can login to ObjectManager Enterprise.
	/// </summary>

	[ComVisible(true)]
	public partial class Login: ILoadData
	{
		#region Member Variables

		//Private static variables
	    private static Window loginToolWindow;
		private static CommandBarControl m_cmdBarCtrlCreateDemoDb;
		internal static CommandBarControl m_cmdBarCtrlConnect;
		internal static CommandBarControl m_cmdBarCtrlBackup;
		internal static CommandBarButton m_cmdBarBtnConnect;
		private static Assembly m_AddIn_Assembly;
		//Private variables
		private IList<RecentQueries> m_recentConnections;

		//Constants

        private const string IMAGE_DISCONNECT = "OMAddin.Images.DB_DISCONNECT2_a.GIF";
        private const string IMAGE_DISCONNECT_MASKED = "OMAddin.Images.DB_DISCONNECT2_b.BMP";

		private const string OPEN_FILE_DIALOG_FILTER = "db4o Database Files(*.yap, *.db4o)|*.yap;*.db4o|All Files(*.*)|*.*";
		private const string STRING_SERVER = "server:";
		private const string STRING_COLON = ":";
		private const char CHAR_COLON = ':';

		static Window queryBuilderToolWindow;
	

		#endregion

	
		public Login()
		{
			InitializeComponent();
		
		}

		public override void SetLiterals()
		{
			try
			{
				labelFile.Text = Helper.GetResourceString(Common.Constants.LOGIN_RECENTCONNECTION_TEXT);
				labelHost.Text = Helper.GetResourceString(Common.Constants.LOGIN_HOST_TEXT);
				labelPort.Text = Helper.GetResourceString(Common.Constants.LOGIN_PORT_TEXT);
				labelUserName.Text = Helper.GetResourceString(Common.Constants.LOGIN_USERNAME_TEXT);
				labelPassword.Text = Helper.GetResourceString(Common.Constants.LOGIN_PASSWORD_TEXT);
				labelType.Text = Helper.GetResourceString(Common.Constants.LOGIN_TYPE_TEXT);
				labelNewConnection.Text = Helper.GetResourceString(Common.Constants.LOGIN_NEWCONNECTION_TEXT);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		public static void CreateLoginToolWindow(CommandBarControl cmdBarCtrl,
			CommandBarButton cmdBarBtn, Assembly addIn_Assembly,
			CommandBarControl cmdBarCtrlBackup, CommandBarControl dbCreateDemoDbControl)
		{
			try
			{
				m_AddIn_Assembly = addIn_Assembly;
				m_cmdBarCtrlConnect = cmdBarCtrl;
				m_cmdBarBtnConnect = cmdBarBtn;
				m_cmdBarCtrlBackup = cmdBarCtrlBackup;
				m_cmdBarCtrlCreateDemoDb = dbCreateDemoDbControl;

                loginToolWindow = CreateToolWindow(Common.Constants.CLASS_NAME_LOGIN, Common.Constants.LOGIN, NewFormattedGuid());

                if (loginToolWindow.AutoHides)
				{
                    loginToolWindow.AutoHides = false;
				}
                loginToolWindow.Visible = true;
                loginToolWindow.Width = 425;
                loginToolWindow.Height = 170;
				Helper.CheckIfLoginWindowIsVisible = true;
				
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static string NewFormattedGuid()
		{
			return Guid.NewGuid().ToString(Helper.GetResourceString(Common.Constants.GUID_FORMATTER_STRING));
		}
	
		public static void CreateQueryBuilderToolWindow()
		{
			try
			{
				string caption = Helper.GetResourceString(Common.Constants.QUERY_BUILDER_CAPTION);
				queryBuilderToolWindow = CreateToolWindow(Common.Constants.CLASS_NAME_QUERYBUILDER, caption, Common.Constants.GUID_QUERYBUILDER );
				
				if (queryBuilderToolWindow.AutoHides)
				{
					queryBuilderToolWindow.AutoHides = false;
				}
				queryBuilderToolWindow.IsFloating = false;
				queryBuilderToolWindow.Linkable = false;
				queryBuilderToolWindow.Visible = true;
				
			}
			catch (Exception e)
			{
				LoggingHelper.HandleException(e); 
			}
		}

		public void LoadAppropriatedata()
		{
			ClearPanelControls();
			ShowAppropriatePanel(true);
			m_recentConnections = dbInteraction.FetchRecentQueries(false);
			if (m_recentConnections != null)
			{
				PopulateConnections(m_recentConnections);
				m_cmdBarCtrlBackup.Enabled = false;
				m_cmdBarCtrlCreateDemoDb.Enabled = true;
			}
		}

		private void ClearPanelControls()
		{
			toolTipForTextBox.RemoveAll();
			comboBoxFilePath.Items.Clear();
			textBoxConnection.Clear();
			textBoxHost.Clear();
			textBoxPassword.Clear();
			textBoxPort.Clear();
			textBoxPassword.Clear();
			chkReadOnly.Checked = false;
		}
		private void ShowAppropriatePanel(bool param)
		{
			panelLocal.Visible = param;
			panelRemote.Visible = !param;
			radioButtonLocal.Checked = param;
			radioButtonRemote.Checked = !param;
		}
		private void AfterSuccessfullyConnected()
		{
			try
			{
				m_cmdBarCtrlConnect.Caption = Common.Constants.TOOLBAR_DISCONNECT;
				m_cmdBarBtnConnect.Caption = Common.Constants.TOOLBAR_DISCONNECT;
				m_cmdBarBtnConnect.TooltipText = Common.Constants.TOOLBAR_DISCONNECT;
				

				if (radioButtonLocal.Checked)
				{
					m_cmdBarCtrlBackup.Enabled = true;
					m_cmdBarCtrlCreateDemoDb.Enabled = false;
				}
				Helper.SetPicture(m_AddIn_Assembly, (CommandBarButton)m_cmdBarCtrlConnect.Control, IMAGE_DISCONNECT, IMAGE_DISCONNECT_MASKED);
				Helper.SetPicture(m_AddIn_Assembly, (CommandBarButton)m_cmdBarBtnConnect.Control, IMAGE_DISCONNECT, IMAGE_DISCONNECT_MASKED);
				
#if !NET_4_0
                ((CommandBarButton)m_cmdBarCtrlConnect).State = MsoButtonState.msoButtonDown;
				m_cmdBarBtnConnect.State = MsoButtonState.msoButtonDown;
#endif

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		private void PopulateConnections(IList<RecentQueries> listConnections)
		{
			try
			{
				if (listConnections != null && listConnections.Count > 0)
				{

					CompareTimestamps comparator = new CompareTimestamps();
					((List< RecentQueries >)listConnections).Sort(comparator);

					comboBoxFilePath.Items.Clear();
					comboBoxFilePath.Items.Add(Helper.GetResourceString(Common.Constants.COMBOBOX_DEFAULT_TEXT));
					foreach (RecentQueries recentQuery in listConnections)
					{
						if (recentQuery.ConnParam.Host == null)
							comboBoxFilePath.Items.Add(new ComboItem(recentQuery.ConnParam.Connection, recentQuery.ConnParam.ConnectionReadOnly));
						else
						{
							comboBoxFilePath.Items.Add(recentQuery.ConnParam.Connection);
							textBoxHost.Text = recentQuery.ConnParam.Host;
							textBoxPort.Text = recentQuery.ConnParam.Port.ToString();
							textBoxUserName.Text = recentQuery.ConnParam.UserName;
							textBoxPassword.Focus();
						}
					}
					if (comboBoxFilePath.Items.Count > 1)
					{
						comboBoxFilePath.SelectedIndex = 1;
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
	private void Login_Load(object sender, EventArgs e)
		{
			try
			{
			
				LoadAppropriatedata();
				SetLiterals();
				
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

	private void radioButton_Click(object sender, EventArgs e)
		{
			try
			{

				ClearPanelControls();
				if (radioButtonLocal.Checked)
				{
					PopulateConnections(dbInteraction.FetchRecentQueries(false));
					panelLocal.Visible = true;
					panelRemote.Visible = false;
				}
				else
				{
					
					PopulateConnections(dbInteraction.FetchRecentQueries(true));
					panelLocal.Visible = false;
					panelRemote.Visible = true;
					
					
					m_cmdBarCtrlBackup.Enabled = false;
					m_cmdBarCtrlCreateDemoDb.Enabled = true;
				}
				
				
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		private void buttonBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				
				openFileDialog.Filter = OPEN_FILE_DIALOG_FILTER;
				openFileDialog.Title = Helper.GetResourceString(Common.Constants.LOGIN_OPEN_FILE_DIALOG_CAPTION);
				if (openFileDialog.ShowDialog() != DialogResult.Cancel)
				{
					textBoxConnection.Text = openFileDialog.FileName;
					toolTipForTextBox.SetToolTip(textBoxConnection, textBoxConnection.Text);
					if (comboBoxFilePath.Items.Contains(textBoxConnection.Text))
						comboBoxFilePath.SelectedItem = textBoxConnection.Text;
					buttonConnect.Focus();
				}

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		private void buttonConnect_Click(object sender, EventArgs e)
		{
			try
			{
				ConnParams conparam = null;
				if (radioButtonLocal.Checked)
				{
					if (!(Validations.ValidateLocalLoginParams(ref comboBoxFilePath, ref textBoxConnection)))
						return;
					conparam = new ConnParams(textBoxConnection.Text.Trim(), chkReadOnly.Checked);

				}
				else
				{
					if (!(Validations.ValidateRemoteLoginParams(ref comboBoxFilePath, ref textBoxHost, ref textBoxPort,
						                                        ref textBoxUserName, ref textBoxPassword)))
						return;

					string connection = STRING_SERVER + textBoxHost.Text.Trim() + STRING_COLON + textBoxPort.Text.Trim() + STRING_COLON +
					                    textBoxUserName.Text.Trim();
					conparam = new ConnParams(connection, textBoxHost.Text.Trim(), textBoxUserName.Text.Trim(),
					                          textBoxPassword.Text.Trim(), Convert.ToInt32(textBoxPort.Text.Trim()));

				}
				
				 RecentQueries currRecentQueries = new RecentQueries(conparam);
				 RecentQueries tempRecentQueries = currRecentQueries.ChkIfRecentConnIsInDb();
				 if (tempRecentQueries != null)
				 {
				 	currRecentQueries = tempRecentQueries;
				 	currRecentQueries.ConnParam.ConnectionReadOnly = chkReadOnly.Checked;      
				 }
				string  exceptionString = dbInteraction.ConnectoToDB(currRecentQueries);

				if (exceptionString == string.Empty)
				{
					dbInteraction.SetCurrentRecentConnection(currRecentQueries);
					dbInteraction.SaveRecentConnection(currRecentQueries);
					AfterSuccessfullyConnected();

					loginToolWindow.Close(vsSaveChanges.vsSaveChangesNo);
					Helper.CheckIfLoginWindowIsVisible = false;
					ObjectBrowserToolWin.CreateObjectBrowserToolWindow();
					ObjectBrowserToolWin.ObjBrowserWindow.Visible = true;

					PropertyPaneToolWin.CreatePropertiesPaneToolWindow(true);
					PropertyPaneToolWin.PropWindow.Visible = true;

					CreateQueryBuilderToolWindow();

				}
				else
				{
					dbInteraction.CloseCurrDb(); 
					textBoxConnection.Clear();
					MessageBox.Show(exceptionString,
					                Helper.GetResourceString(Common.Constants.PRODUCT_CAPTION),
					                MessageBoxButtons.OK,
					                MessageBoxIcon.Error);
					return;
				}

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void comboBoxFilePath_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (radioButtonRemote.Checked)
				{
					if (!comboBoxFilePath.Text.Equals(Helper.GetResourceString(Common.Constants.COMBOBOX_DEFAULT_TEXT)))
					{
						string[] strRemote = comboBoxFilePath.Text.Split(CHAR_COLON);
						textBoxHost.Text = strRemote[1];
						textBoxPort.Text = strRemote[2];
						textBoxUserName.Text = strRemote[3];
						textBoxPassword.Focus();
						toolTipForTextBox.SetToolTip(comboBoxFilePath, comboBoxFilePath.SelectedItem.ToString());
					}
					else
					{
						textBoxHost.Clear();
						textBoxPort.Clear();
						textBoxUserName.Clear();
						textBoxPassword.Clear();
					}
				}
				else
				{
					if (!comboBoxFilePath.Text.Equals(Helper.GetResourceString(Common.Constants.COMBOBOX_DEFAULT_TEXT)))
					{
						ComboItem comboItem = comboBoxFilePath.SelectedItem as ComboItem;
						textBoxConnection.Text = comboItem.ToString();
						chkReadOnly.Checked = comboItem.ReadonlyParam;
						toolTipForTextBox.SetToolTip(comboBoxFilePath, comboBoxFilePath.SelectedItem.ToString());
					}
					else
					{
						textBoxConnection.Clear();
						chkReadOnly.Checked = false;
					}
					
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void comboBoxFilePath_DropdownItemSelected(object sender, ToolTipComboBox.DropdownItemSelectedEventArgs e)
		{
			try
			{
				if (e.SelectedItem < 0 || e.Scrolled) toolTipForTextBox.Hide(comboBoxFilePath);
				else
					toolTipForTextBox.Show(comboBoxFilePath.Items[e.SelectedItem].ToString(), comboBoxFilePath, e.Bounds.Location.X + Cursor.Size.Width, e.Bounds.Location.Y + Cursor.Size.Height);
			}
			catch (Exception ex)
			{
				LoggingHelper.HandleException(ex);
			}
		}
	
		private void buttonCancel_Click(object sender, EventArgs e)
		{
			try
			{
				textBoxPort.Clear();
				textBoxHost.Clear();
				textBoxConnection.Clear();
				textBoxPassword.Clear();
				textBoxUserName.Clear();

                loginToolWindow.Close(vsSaveChanges.vsSaveChangesNo);
				Helper.CheckIfLoginWindowIsVisible = false;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
		
		private void textBoxPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				char c = e.KeyChar;

				//Allow only numeric charaters in filter textbox.
				if (!Helper.IsNumeric(c.ToString()))
					e.Handled = true;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void textBoxPort_TextChanged(object sender, EventArgs e)
		{
			int result;
			if (!Int32.TryParse(textBoxPort.Text.Trim(), out result))
			{
				textBoxPort.Text = string.Empty;
			}
		}

		private void textBoxPort_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Modifiers == Keys.Control)
				e.Handled = true;
		}

		class ComboItem
		{
			private string m_Name;
			private bool m_Value;

			public ComboItem(string name, bool in_value)
			{
				m_Name = name;
				m_Value = in_value;
			}

			public bool ReadonlyParam
			{
				get { return m_Value; }
			}

			public override string ToString()
			{
				return m_Name;
			}

		}
	}
}
