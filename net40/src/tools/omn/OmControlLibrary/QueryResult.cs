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
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using OManager.BusinessLayer.Common;
using OManager.BusinessLayer.QueryManager;
using OManager.BusinessLayer.UIHelper;
using OManager.DataLayer.QueryParser;
using OManager.DataLayer.Reflection;
using OMControlLibrary.Common;
using OME.AdvancedDataGridView;
using OME.Logging.Common;
using OME.Logging.Tracing;
using Constants=OMControlLibrary.Common.Constants;
using Thread=System.Threading.Thread;

namespace OMControlLibrary
{
	[ComVisible(true)]
	public partial class QueryResult : ViewBase
	{
		private const string COLUMN_NUMBER = "No.";
		private const string CONST_DOT = ".";
		private const char CONST_SPACE = ' ';
		private const string CONST_TAB_CAPTION = "Object ";
		private const string CONST_TRUE = "true";
		private const string CONST_FALSE = "false";
		private const string CONST_COMMA = ",";
		
		#region Member Variables

		private string strstoreValue;
		private string strstoreTreeValue;
		internal readonly string ClassName = Helper.BaseClass;

		private dbDataGridView masterView;
		private OMETabStrip detailsTabs;

		internal OMQuery omQuery;
		internal long[] objectid;
		private List<long> lstObjIdLong;

		private const int m_pagingStartIndex = 0;
		private int m_pageCount = 1;

		private readonly WindowVisibilityEvents windowsVisEvents;
		private readonly WindowEvents _windowsEvents;
		private static WindowVisibilityEvents _events;

		#endregion

		public void Setobjectid(long[] objectIds)
		{
			objectid = objectIds;
			LoadData();
		}

		private void LoadData()
		{

			masterView.Rows.Clear();
			detailsTabs.Items.Clear();
			detailsTabs.SelectedItem = null;

			Hashtable hAttributes = new Hashtable();
			try
			{
				omQuery = (OMQuery) Helper.OMResultedQuery[ClassName];
				if (omQuery != null)
				{
					hAttributes = omQuery.AttributeList;
				}
				PagingData pagingData = new PagingData(m_pagingStartIndex);

				if (objectid != null)
				{
					lstObjIdLong = new List<long>(objectid);
					pagingData.ObjectId = lstObjIdLong;

					const int pageNumber = m_pagingStartIndex + 1;
					lblPageCount.Text = pagingData.GetPageCount().ToString();
					txtCurrentPage.Text = pageNumber.ToString();
					labelNoOfObjects.Text = pagingData.ObjectId.Count.ToString();
					if (lstObjIdLong.Count > 0)
					{
						List<Hashtable> hashListResult = dbInteraction.ReturnQueryResults(pagingData, true, omQuery.BaseClass,
						                                                                  omQuery.AttributeList);
						masterView.SetDataGridColumnHeader(hashListResult, ClassName, omQuery.AttributeList);
						masterView.SetDatagridRows(hashListResult, ClassName, hAttributes, 1);
						ListofModifiedObjects.AddDatagrid(ClassName, masterView);
					}

					if (pagingData.ObjectId.Count <= PagingData.PAGE_SIZE)
					{
						btnPrevious.Enabled = false;
						btnFirst.Enabled = false;
						btnNext.Enabled = false;
						btnLast.Enabled = false;
					}
					else
					{
						btnPrevious.Enabled = false;
						btnFirst.Enabled = false;
						btnNext.Enabled = true;
						btnLast.Enabled = true;
					}

				}
				ApplyReadonlyCondition(dbInteraction.GetCurrentRecentConnection().ConnParam.ConnectionReadOnly);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

	

		#region Constructor

		public QueryResult()
		{
			strstoreTreeValue = string.Empty;
			strstoreValue = string.Empty;
			try
			{
				SetStyle(ControlStyles.CacheText | ControlStyles.OptimizedDoubleBuffer, true);

				InitializeComponent();
				Events events = ApplicationObject.Events;
				_windowsEvents = events.get_WindowEvents(null);
				_windowsEvents.WindowActivated += _windowsEvents_WindowActivated;
				


				Events2 eventsSource = (Events2)ApplicationObject.Events;
				_events = eventsSource.get_WindowVisibilityEvents(Helper.QueryResultToolWindow);
				_events.WindowHiding += WindowHiding;
				InitializeResultDataGridView();
				InitializeTabControl();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		void WindowHiding(Window Window)
		{
			if (Window.Object is QueryResult )
			{
				Helper.SaveDataIfRequired();
			      
			}
		}
	



		#endregion

		#region WindowEvents

			
			private void ApplyReadonlyCondition(bool setValue)
			{
				masterView.ReadOnly = setValue;
				btnSave.Enabled = !setValue;
				btnDelete.Enabled = !setValue;
				buttonSaveResult.Enabled=!setValue;
			}

		private static void _windowsEvents_WindowActivated(Window GotFocus, Window LostFocus)
		{
			if (GotFocus.Object is QueryResult)
			{
				PropertiesTab.Instance.ShowObjectPropertiesTab = false;
				SelectTreeNodeInObjBrowser(GotFocus.Caption);
			}
		}




		private static void SelectTreeNodeInObjBrowser(string winCaptionArg)
		{
			string winCaption = winCaptionArg;
			foreach (DictionaryEntry entry in Helper.HashClassGUID)
			{
				string caption = Helper.GetCaption(entry.Key.ToString());

				if (winCaption != caption)
					continue;

				dbTreeView view = CurrentViewMode();
				ObjectBrowser.Instance.DbtreeviewObject.FindNSelectNode(view.Nodes[0], entry.Key.ToString(), view);
			}
		}

		private static dbTreeView CurrentViewMode()
		{
			return ObjectBrowser.Instance.ToolStripButtonAssemblyView.Checked
								? ObjectBrowser.Instance.DbAssemblyTreeView
								: ObjectBrowser.Instance.DbtreeviewObject;
		}

	

	   

	    #endregion

		#region Query Result Events

		private void QueryResult_Load(object sender, EventArgs e)
		{
			try
			{

				OMETrace.WriteFunctionStart();
				CheckForIllegalCrossThreadCalls = false;
				SetLiterals();
				OMETrace.WriteFunctionEnd();

			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region DataGridView Events

		private void masterView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				OMETrace.WriteFunctionStart();

				EnsureDetailViewItemNotSelected();

				DataGridViewCell cell = masterView[e.ColumnIndex, e.RowIndex];
				object currObj = cell.OwningRow.Tag;
				string fieldName = masterView.Columns[e.ColumnIndex].HeaderText;
				object value = cell.Value;

			    IType type = (IType) cell.Tag;
			    if (Validations.ValidateDataType(type, value))
				{
					if (strstoreValue != value.ToString())
					{
						if (currObj != null)
						{
							UpdateMasterViewObjectEditedStatus(masterView.Rows[e.RowIndex], true);
							dbInteraction.EditObject(currObj, fieldName, value.ToString());

                            masterView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = type.Cast(cell.Value);
							//TODO: Why double check the condition bellow here? 
							if (strstoreValue != value.ToString())
							{
								cell.Style.ForeColor = Color.Red;
								cell.Style.SelectionForeColor = Color.Red;

								detailsTabs.SelectedItem.Name = CONST_TRUE;

								UpdateDataTreeView(cell.OwningRow.Tag, cell.OwningRow);
							}
						}

						btnSave.Enabled = true;
					}
					cell.OwningColumn.SortMode = sortStore;

					OMETrace.WriteFunctionEnd();
				}
				else
				{
                    masterView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = type.Cast(strstoreValue);
					strstoreValue = string.Empty;
					cell.Style.ForeColor = Color.Black;
					cell.Style.SelectionForeColor = Color.White;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void EnsureDetailViewItemNotSelected()
		{
			TreeGridView treeview = TreeViewFor(detailsTabs.SelectedItem);
			if (treeview.SelectedRows.Count > 0)
				treeview.SelectedRows[0].Selected = false;
		}

		private static void UpdateMasterViewObjectEditedStatus(DataGridViewRow row, bool edited)
		{
			row.Cells[Constants.QUERY_GRID_ISEDITED_HIDDEN].Value = edited;
		}

		private DataGridViewColumnSortMode sortStore;
		private void masterView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			try
			{
				DataGridViewCell cell = ((DataGridView) sender).CurrentCell;

			    string fieldName = cell.OwningColumn.HeaderText;
                if(fieldName.Contains( "."))
                {
                    string[] splitString = fieldName.Split('.');
                    fieldName = splitString[splitString.Length - 1];
                }
                IType fieldType = dbInteraction.GetFieldType(cell.OwningColumn.Tag.ToString(), fieldName);
                if (!fieldType.IsEditable)
				{
					e.Cancel = true;
					return;
				}

				btnSave.Enabled = true;
				EnsureDetailViewItemNotSelected();

				if (cell.Value != null)
					strstoreValue = cell.Value.ToString();

				sortStore = cell.OwningColumn.SortMode;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void masterView_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				if (masterView.SelectedRows.Count > 0 && detailsTabs.SelectedItem != null)
				{
					object selectedObject = masterView.SelectedRows[0].Tag;
					if (selectedObject != null && selectedObject.Equals(ReferencedObjectFor(detailsTabs.SelectedItem)))
					{
						PropertiesTab.Instance.ShowObjectPropertiesTab = true;
						PropertiesTab.Instance.RefreshPropertiesTab(selectedObject);
						return;
					}
				}

				if (masterView.SelectedRows.Count <= 0)
					return;

				DataGridViewRow row = masterView.SelectedRows[0];
				if (null == row)
					return;

				if (row.Tag != null)
				{
					OMETabStripItem foundTab = FindDetailsTabForObjectIndex(DetailsTabCaptionFor(row));
					if (null != foundTab)
					{
						detailsTabs.SelectedItem = foundTab;
						return;
					}

					TreeGridView treeview = dbInteraction.GetObjectHierarchy(row.Tag, ClassName);

					OMETabStripItem tabPage = new OMETabStripItem(DetailsTabCaptionFor(row), treeview);
					tabPage.Name = tabPage.Title;
					detailsTabs.AddTab(tabPage);

					RegisterTreeviewEvents(treeview);
					// This check helps in avoding recusrrion.
					if (masterView.SortOrder == SortOrder.None)
					{
						detailsTabs.SelectedItem = tabPage;
					}
					
				}
				else
					row.Selected = false;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static object ReferencedObjectFor(OMETabStripItem item)
		{
			return TreeViewFor(item).Nodes[0].Tag;
		}

		private static string DetailsTabCaptionFor(DataGridViewRow row)
		{
			return DetailsTabCaptionFor((int) row.Cells[COLUMN_NUMBER].Value);
		}

		private static string DetailsTabCaptionFor(int index)
		{
			return CONST_TAB_CAPTION + index;
		}

		private OMETabStripItem FindDetailsTabForObjectIndex(string caption)
		{
			foreach (OMETabStripItem tp in detailsTabs.Items)
			{
				if (tp.Caption.Equals(caption))
				{
					return tp;
				}
			}
			return null;
		}

		private void RegisterTreeviewEvents(TreeGridView treeview)
		{
			treeview.NodeExpanded += treeview_NodeExpanded;
			treeview.CellBeginEdit += treeview_CellBeginEdit;
			treeview.CellEndEdit += treeview_CellEndEdit;
			treeview.OnContextMenuItemClicked += treeview_OnContextMenuItemClicked;
			treeview.OnContextMenuOpening += treeview_OnContextMenuOpening;
			treeview.Click += treeview_Click;
			treeview.Columns[0].Width = (treeview.Width - 2)/3;
			treeview.Columns[1].Width = (treeview.Width - 2)/3;
			treeview.Columns[2].Width = (treeview.Width - 2)/3;
		}

		private void treeview_Click(object sender, EventArgs e)
		{
			TreeGridView treeview = (TreeGridView) sender;
			CheckForObjectPropertiesTab(treeview.Nodes[0].Tag);
		}

		private void treeview_OnContextMenuItemClicked(object sender, ContextItemClickedEventArg e)
		{
		    try
		    {
		        TreeGridView treeview = (TreeGridView) e.Data;
		        if (treeview.SelectedRows.Count <= 0)
		            return;

		        treeview.ContextMenuStrip.Dispose();
		        DialogResult dialogRes =
		            MessageBox.Show("This will set the value to null in the database. Do you want to continue?",
		                            Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.YesNo,
		                            MessageBoxIcon.Question);

		        if (dialogRes != DialogResult.Yes)
		            return;

		        TreeGridNode node = (TreeGridNode) treeview.SelectedCells[0].OwningRow;

		        long id = dbInteraction.GetLocalID(treeview.Nodes[0].Tag);

		       
		            dbInteraction.SetFieldToNull(
		                ParentObjectFor(node),
		                CommonValues.UndecorateFieldName(node.Cells[0].Value.ToString()));

	           object obj = null;
				if (id != 0)
				{
					obj = dbInteraction.GetObjById(id);
				}
				else
				{
					MessageBox.Show("This object is already Null.", Helper.GetResourceString(Constants.PRODUCT_CAPTION),
					                MessageBoxButtons.OK, MessageBoxIcon.Information);
				    return;
				}
                
				if (obj != null)
				{
					dbInteraction.RefreshObject(obj, DepthFor(node));

					UpdateResultTable(treeview.SelectedCells[0].OwningRow.Cells[0],
					                  "null",
					                  (TreeGridNode) treeview.SelectedCells[0].OwningRow.Cells[0].OwningRow,
					                  (OMETabStripItem) treeview.Parent, true);

					treeview = dbInteraction.GetObjectHierarchy(obj, FieldTypeNameFor(treeview.Nodes[0]));
					detailsTabs.SelectedItem.Controls.Clear();
					detailsTabs.SelectedItem.Controls.Add(treeview);
					RegisterTreeviewEvents(treeview);
				}
				else //delete tab as teh obj is deleted and delete it from db grid view
				{
					int index = OffsetInCurrentPageFor(ObjectIndexInMasterViewFor((OMETabStripItem) treeview.Parent));
					detailsTabs.RemoveTab(detailsTabs.SelectedItem);
					masterView.Rows.RemoveAt(index - 1);

					if( detailsTabs.SelectedItem !=null )
                        detailsTabs.SelectedItem .Controls.Clear();
					lstObjIdLong.Remove(id);

					int m_pageCount = CurrentPageNumber();
					int startIndex = (CurrentPageNumber()*PagingData.PAGE_SIZE) - PagingData.PAGE_SIZE;
					int endIndex = startIndex + PagingData.PAGE_SIZE;
					labelNoOfObjects.Text = lstObjIdLong.Count.ToString();

					PagingData pgData = new PagingData(startIndex, endIndex);
					pgData.ObjectId = lstObjIdLong;
					if (lstObjIdLong.Count > 0)
					{
						List<Hashtable> hashListResult = dbInteraction.ReturnQueryResults(pgData, false, omQuery.BaseClass,
						                                                                  omQuery.AttributeList);
						Hashtable hAttributes = null;

						if (omQuery != null)
						{
							hAttributes = omQuery.AttributeList;
						}
						masterView.SetDatagridRows(hashListResult, ClassName, hAttributes, 1);
					}

					treeview = dbInteraction.GetObjectHierarchy(ReferencedObjectFor(detailsTabs.SelectedItem), ClassName);

				    if (detailsTabs.SelectedItem != null) detailsTabs.SelectedItem.Controls.Add(treeview);
				    RegisterTreeviewEvents(treeview);

					int delIndex = ObjectIndexInMasterViewFor(detailsTabs.SelectedItem);
					UpdateObjectDetailTablCaptions(delIndex);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static bool IsObjectInMasterViewEdited(DataGridViewRow row)
		{
			return row != null 
				? Convert.ToBoolean(row.Cells[Constants.QUERY_GRID_ISEDITED_HIDDEN].Value)
				: false;
		}

		private static int DepthFor(TreeGridNode node)
		{
			int depth = 0;
			if (node.Parent == null) 
				return depth;

			while (node.Parent.Tag != null)
			{
				depth++;
				node = node.Parent;
			}

			return depth;
		}

		private static object ParentObjectFor(TreeGridNode node)
		{
			return node.Parent.Tag;
		}

		private static void treeview_OnContextMenuOpening(object sender, ContextItemClickedEventArg e)
		{
			try
			{
				TreeGridView treeview = (TreeGridView) e.Data;
				treeview.EndEdit();

				CancelEventArgs args = e.CancelEventArguments;
				if(dbInteraction.GetCurrentRecentConnection().ConnParam.ConnectionReadOnly)
				{
					args.Cancel = true;
					return;
				}
				if (treeview.SelectedRows.Count > 0)
				{
					DataGridViewRow selectedRow = treeview.SelectedCells[0].OwningRow;
					TreeGridNode treeGridNode = selectedRow as TreeGridNode;

					args.Cancel = !IsSetToNullOperationValidFor(FieldTypeForObjectInRow(selectedRow), treeGridNode.Parent.Tag);
				}
				else
				{
					args.Cancel = true;
				}
				
			}
			catch (Exception ex)
			{
				LoggingHelper.ShowMessage(ex);
			}
		}

		private static bool IsSetToNullOperationValidFor(IType targetFieldType, object containingObject)
		{
          return (targetFieldType.HasIdentity || targetFieldType.IsNullable) && (containingObject != null);
		}

	    private static string FieldTypeNameFor(DataGridViewRow fieldRow)
		{
			return FieldTypeForObjectInRow(fieldRow).FullName;
		}

		private static IType FieldTypeForObjectInRow(DataGridViewRow fieldRow)
		{
		   return (IType) fieldRow.Cells[2].Tag;
		}

	    #endregion

		#region TreeView Events

		private static void treeview_NodeExpanded(object sender, ExpandedEventArgs e)
		{
			try
			{
				if (e.Node.Nodes.Count == 0)
					return;

				if (e.Node.Nodes[0].Cells[0].Value.ToString() != BusinessConstants.DB4OBJECTS_DUMMY)
					return;

				e.Node.Nodes.RemoveAt(0);
				TreeGridView tree = e.Node.DataGridView as TreeGridView;
				Helper.DbInteraction.ExpandTreeNode(e.Node, ((tree.Parent)).Name == CONST_TRUE);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void treeview_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			OMETrace.WriteFunctionStart();

			if (masterView.SelectedRows.Count > 0)
				masterView.SelectedRows[0].Selected = false;

			DataGridViewCell cell = ((TreeGridView) sender).CurrentCell;

			if (Validations.ValidateDataType(FieldTypeForObjectInRow(cell.OwningRow), cell.Value))
			{
				if (strstoreTreeValue != cell.Value.ToString())
				{
					ArrayList hierarchy = new ArrayList();
					List<int> offset = new List<int>();
					List<string> nameList = new List<string>();
					List<IType> typeList = new List<IType>();
					try
					{
						TreeGridNode currNode = (TreeGridNode) cell.OwningRow;

						hierarchy.Add(currNode.Tag);
						nameList.Add((string) currNode.Cells[0].Value);
						offset.Add(-2);
						typeList.Add(FieldTypeForObjectInRow(currNode));

						TreeGridNode node = currNode;
						while (node.Parent.Tag != null)
						{
							hierarchy.Add(node.Parent.Tag);
							typeList.Add(FieldTypeForObjectInRow(node.Parent));
							int level = -1;

							if (dbInteraction.IsArray(node.Parent.Tag) || dbInteraction.IsCollection(node.Parent.Tag))
							{
								string name = (string) node.Parent.Cells[0].Value;
								level = node.Parent.Nodes.IndexOf(node);
								int index = name.IndexOf(CONST_SPACE);
								name = name.Substring(0, index);
								nameList.Add(name);
							}
							else
							{
								nameList.Add((string) node.Parent.Cells[0].Value);
							}

							offset.Add(level);

							node = node.Parent;
						}
						hierarchy.Reverse();
						nameList.Reverse();
						typeList.Reverse();
						offset.Reverse();

						object newValue = ValueType(typeList).Cast(cell.Value);
						if (!RenderHierarchy.TryUpdateValueType(currNode, newValue))
						{
							Helper.DbInteraction.UpdateCollection(hierarchy, offset, nameList, typeList, newValue);
						}

						OMETabStripItem pg = (OMETabStripItem)((TreeGridView)sender).Parent;
						pg.Name = CONST_TRUE;
						UpdateDeepestFieldChanged(pg, hierarchy.Count);

						UpdateResultTable(cell, cell.Value, currNode, pg, false);
						cell.OwningRow.Selected = true;
					}
					catch (Exception ex)
					{
						LoggingHelper.ShowMessage(ex);
					}
				}
			}
			else
			{
				cell.Style.ForeColor = Color.Black;
				cell.Style.SelectionForeColor = Color.White;
				cell.Value = strstoreTreeValue;
				strstoreTreeValue = string.Empty;
			}

			OMETrace.WriteFunctionEnd();
		}

		private OMETabStripItem FindDetailsTabFor(object candidate)
		{
			foreach(OMETabStripItem current in detailsTabs.Controls)
			{
				if (ReferencedObjectFor(current) == candidate)
				{
					return current;
				}
			}

			return null;
		}

		private int UpdateDethForObject(object obj)
		{
			OMETabStripItem detailsPage = FindDetailsTabFor(obj);
			return UpdateDethFor(detailsPage);
		}

		private static int UpdateDethFor(OMETabStripItem detailsTab)
		{
			return detailsTab.Tag != null ?  (int) detailsTab.Tag : 0;
		}

		private static void UpdateDeepestFieldChanged(Control detailsTab, int depth)
		{
			if (detailsTab.Tag == null)
			{
				detailsTab.Tag = depth;
				return;
			}

			int currentDepth = (int) detailsTab.Tag;
			if (depth > currentDepth)
			{
				detailsTab.Tag = depth;
			}
		}

		private void UpdateResultTable(DataGridViewCell cell, object editValue, TreeGridNode currNode, OMETabStripItem pg, bool updateToNull)
		{
			try
			{
				int pageIndex = OffsetInCurrentPageFor(ObjectIndexInMasterViewFor(pg));
				string columnName;
				if (currNode.Parent.Cells[1].Value != null && omQuery.AttributeList.Count > 0)
				{
					columnName = GetFullPath(currNode);
				}
				else
				{
					columnName = currNode.Cells[0].Value.ToString();
				}
				//This fix is applied cause (G) also contains "(" and therefore next line will not fail
				//If (G) is removed from the columnname.

				if (columnName.Contains("(G)"))
				{
					int index1 = columnName.IndexOf("(G)");
					columnName = columnName.Remove(index1, 3);
				}

				if (columnName.Contains("("))
				{
					int index = columnName.IndexOf('(');
					columnName = columnName.Remove(index - 1, columnName.Length - index + 1);
				}

				foreach (DataGridViewColumn col in masterView.Columns)
				{
					if (col.HeaderText.Equals(columnName))
					{
						masterView.Rows[pageIndex - 1].Cells[columnName].Value = editValue.ToString();
						break;
					}
				}

				masterView.Rows[pageIndex - 1].Cells[1].Selected = true;

				if (!updateToNull)
				{
					UpdateMasterViewObjectEditedStatus(masterView.Rows[pageIndex - 1], true);
					buttonSaveResult.Enabled = true;
					cell.Style.ForeColor = Color.Red;
					cell.Style.SelectionForeColor = Color.Red;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void treeview_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
		{
			try
			{
			    DataGridViewCell cell = ((TreeGridView) sender).CurrentCell;
                if (cell.Value == null || cell.Value.ToString()  == "null")
                {
                    e.Cancel = true;
                    return;
                }

			    buttonSaveResult.Enabled = true;
				if (masterView.SelectedRows.Count > 0)
					masterView.SelectedRows[0].Selected = false;

				//DataGridViewCell cell = ((TreeGridView) sender).CurrentCell;
				strstoreTreeValue = cell.Value != null ? cell.Value.ToString() : string.Empty;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Buttons Events

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				OMETrace.WriteFunctionStart();

				foreach (DataGridViewRow row in masterView.Rows)
				{
					if (IsObjectInMasterViewEdited(row))
					{
						UpdateMasterViewObjectEditedStatus(row, false);
						dbInteraction.SaveObjects(row.Tag);
					}
				}
				int startindex = (int) masterView.Rows[masterView.Rows.Count - 1].Cells[1].Value;
				int endindex = startindex + PagingData.PAGE_SIZE;
				if (endindex > lstObjIdLong.Count && startindex < lstObjIdLong.Count)
				{
					endindex = lstObjIdLong.Count;
				}

				PagingData pagingData = new PagingData(startindex, endindex);
				pagingData.ObjectId = lstObjIdLong;
				dbInteraction.ReturnQueryResults(pagingData, true, omQuery.BaseClass, omQuery.AttributeList);

				MakeAllElementsInGridBlack(masterView);
				btnSave.Enabled = false;
				buttonSaveResult.Enabled = false;

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
				OMETrace.WriteFunctionStart();
				
				if (masterView.SelectedRows.Count > 0)
				{
					DataGridViewRow row = masterView.SelectedRows[0];
                    long deletedId = dbInteraction.GetLocalID(row.Tag);
                    int objectIndex = ObjectIndexInMasterViewFor(detailsTabs.SelectedItem);
					if (deletedId > 0)
					{
						const string strShowMessage = "Do you want to delete this object ?";
						DialogResult dialogRes = MessageBox.Show(strShowMessage,
						                                         Helper.GetResourceString(Constants.PRODUCT_CAPTION),
						                                         MessageBoxButtons.OKCancel,
						                                         MessageBoxIcon.Question);



						if (dialogRes == DialogResult.OK)
						{
							dbInteraction.DeleteObject(row.Tag);

							RemoveObjectFromDetailsView(row.Tag);

							UpdateObjectDetailTablCaptions(objectIndex);

							lstObjIdLong.Remove(deletedId);

							const int pageNumber = m_pagingStartIndex + 1;

							PagingData pagData = PagingData.StartingAtPage(pageNumber);
							pagData.ObjectId = lstObjIdLong;

							lblPageCount.Text = pagData.GetPageCount().ToString();
							txtCurrentPage.Text = pageNumber.ToString();
							labelNoOfObjects.Text = pagData.ObjectId.Count.ToString();

							masterView.Rows.Clear();
							if (lstObjIdLong.Count > 0)
							{
								List<Hashtable> hashListResult = dbInteraction.ReturnQueryResults(pagData, true,
								                                                                  omQuery.BaseClass,
								                                                                  omQuery.AttributeList);
								Hashtable hAttributes = (omQuery != null) ? omQuery.AttributeList : null;
								masterView.SetDatagridRows(hashListResult, ClassName, hAttributes, pagData.StartIndex + 1);

								SetSelectedObjectInMasterView(Math.Min(objectIndex, lstObjIdLong.Count));
							}
						}
					}
					else
					{
						MessageBox.Show(
							"Object is already deleted. This window will get closed now. Please fire the query again for refreshed results.",
							Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.OK,
										 MessageBoxIcon.Information);
						Window queryResult = GetWindow(Helper.GetCaption(ClassName));
						queryResult.Close(vsSaveChanges.vsSaveChangesYes);
					}
                   
                       
				    buttonSaveResult.Enabled = false;
				}
				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void UpdateObjectDetailTablCaptions(int startIndex)
		{
			foreach (OMETabStripItem tp in detailsTabs.Items)
			{
				
				int tabIndex = ObjectIndexInMasterViewFor(tp);
				if (tabIndex > startIndex)
				{
					tp.Title = DetailsTabCaptionFor(tabIndex - 1);
				}
			}
		}

		private void RemoveObjectFromDetailsView(object obj)
		{
			OMETabStripItem found = FindDetailsTabFor(obj);
			if (found != null)
			{
				detailsTabs.RemoveTab(found);
			}
		}

		private void buttonSaveResult_Click(object sender, EventArgs e)
		{
			try
			{
				OMETrace.WriteFunctionStart();
				SaveDetailsViewChangedObjects(detailsTabs);

				int startindex = (int) masterView.Rows[masterView.Rows.Count - 1].Cells[1].Value;
				int endindex = startindex + PagingData.PAGE_SIZE;
				if (endindex > lstObjIdLong.Count && startindex < lstObjIdLong.Count)
				{
					endindex = lstObjIdLong.Count;
				}
				PagingData pagingData = new PagingData(startindex, endindex);
				pagingData.ObjectId = lstObjIdLong;
				dbInteraction.ReturnQueryResults(pagingData, false, omQuery.BaseClass, omQuery.AttributeList);
				btnSave.Enabled = false;
				buttonSaveResult.Enabled = false;
				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void SaveDetailsViewChangedObjects(OMETabStrip detailTabs)
		{
			foreach (OMETabStripItem detailsPage in detailTabs.Items)
			{
				if (detailsPage.Name.Equals(CONST_TRUE))
				{
					Helper.DbInteraction.SaveCollection(ReferencedObjectFor(detailsPage), UpdateDethFor(detailsPage));

					PaintBlack(TreeViewFor(detailsPage));
					detailsPage.Name = CONST_FALSE;

					int pageIndex = OffsetInCurrentPageFor(ObjectIndexInMasterViewFor(detailsPage));
					
					UpdateMasterViewObjectEditedStatus(masterView.Rows[pageIndex - 1], false);
				}
			}
		}

		private static TreeGridView TreeViewFor(OMETabStripItem pg)
		{
			return pg != null ? (TreeGridView) pg.Controls[0] : null;
		}

		private void btnPrevious_Click(object sender, EventArgs e)
		{
			try
			{
				m_pageCount--;
				if (m_pageCount <= 0)
					m_pageCount = m_pagingStartIndex + 1;

				txtCurrentPage.Text = m_pageCount.ToString();
				KeyEventArgs keyArgs = new KeyEventArgs(Keys.Enter);
				txtObjectNumber_KeyDown(txtCurrentPage, keyArgs);

				if (m_pageCount == 1)
				{
					btnPrevious.Enabled = false;
					btnFirst.Enabled = false;
					btnNext.Enabled = true;
					btnLast.Enabled = true;
				}
				else
				{
					btnPrevious.Enabled = true;
					btnNext.Enabled = true;
					btnLast.Enabled = true;
					btnFirst.Enabled = true;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void btnLast_Click(object sender, EventArgs e)
		{
			try
			{
				m_pageCount = PageCount();
				txtCurrentPage.Text = lblPageCount.Text;

				KeyEventArgs keyArgs = new KeyEventArgs(Keys.Enter);
				txtObjectNumber_KeyDown(txtCurrentPage, keyArgs);

				if (m_pageCount == PageCount())
				{
					btnPrevious.Enabled = true;
					btnLast.Enabled = false;
					btnFirst.Enabled = true;
					btnNext.Enabled = false;
				}
				else
				{
					btnPrevious.Enabled = true;
					btnNext.Enabled = true;
					btnLast.Enabled = true;
					btnFirst.Enabled = true;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			try
			{
				m_pageCount++;

				txtCurrentPage.Text = m_pageCount.ToString();
				KeyEventArgs keyArgs = new KeyEventArgs(Keys.Enter);
				txtObjectNumber_KeyDown(txtCurrentPage, keyArgs);

				if (m_pageCount >= PageCount())
				{
					btnPrevious.Enabled = true;
					btnFirst.Enabled = true;
					btnLast.Enabled = false;
					btnNext.Enabled = false;
				}
				else
				{
					btnPrevious.Enabled = true;
					btnNext.Enabled = true;
					btnLast.Enabled = true;
					btnFirst.Enabled = true;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}


		private void RefreshPaging(DataGridView db)
		{
			try
			{
				if (HasChangedData(db))
				{
					DialogResult dialogRes = MessageBox.Show("Do you want to save modified objects on this page, else they will be discarded.",
					                                         Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.YesNo,
					                                         MessageBoxIcon.Question);

					if (dialogRes == DialogResult.Yes)
					{
						buttonSaveResult_Click(buttonSaveResult, null);
					}
					else
					{
						foreach (DataGridViewRow row in db.Rows)
						{
							if (IsObjectInMasterViewEdited(row))
							{
								dbInteraction.RefreshObject(row.Tag, UpdateDethForObject(row.Tag));
								UpdateDataTreeView(row.Tag, row);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LoggingHelper.ShowMessage(ex);
			}
		}

		private static bool HasChangedData(DataGridView db)
		{
			foreach (DataGridViewRow row in db.Rows)
			{
				if (IsObjectInMasterViewEdited(row))
				{
					return true;
				}
			}
			return false;
		}

		private void btnFirst_Click(object sender, EventArgs e)
		{
			try
			{
				m_pageCount = m_pagingStartIndex + 1;
				txtCurrentPage.Text = m_pageCount.ToString();

				KeyEventArgs keyArgs = new KeyEventArgs(Keys.Enter);
				txtObjectNumber_KeyDown(txtCurrentPage, keyArgs);

				if (m_pageCount == 1)
				{
					btnPrevious.Enabled = false;
					btnFirst.Enabled = false;
					btnLast.Enabled = true;
					btnNext.Enabled = true;
				}
				else
				{
					btnPrevious.Enabled = true;
					btnNext.Enabled = true;
					btnLast.Enabled = true;
					btnFirst.Enabled = true;
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void txtObjectNumber_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				Hashtable hAttributes = null;
				PagingData pagingData;
				
				if (e.Modifiers == Keys.Control)
				{
					e.Handled = true;
				}

				if (e.KeyCode == Keys.Enter)
				{
					if (masterView.SortedColumn != null)
						masterView.SortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;

					RefreshPaging(masterView);

					if (CurrentPageNumber() > PageCount())
						txtCurrentPage.Text = lblPageCount.Text;
					else if (CurrentPageNumber() == 0)
						txtCurrentPage.Text = m_pagingStartIndex.ToString();


					if (!string.IsNullOrEmpty(txtCurrentPage.Text.Trim()) && CurrentPageNumber() <= PageCount())
					{
						m_pageCount = CurrentPageNumber();
						pagingData = PagingData.StartingAtPage(m_pageCount);
						pagingData.ObjectId = lstObjIdLong;
						if (lstObjIdLong.Count > 0)
						{
							List<Hashtable> hashListResult = dbInteraction.ReturnQueryResults(pagingData, true, omQuery.BaseClass, omQuery.AttributeList);
							if (omQuery != null)
							{
								hAttributes = omQuery.AttributeList;
							}

							masterView.SetDatagridRows(hashListResult, ClassName, hAttributes, pagingData.StartIndex + 1);

							ListofModifiedObjects.AddDatagrid(ClassName, masterView);
						}

						int totalPages = PageCount();

						if (m_pageCount == 1 && totalPages == 1)
						{
							btnPrevious.Enabled = false;
							btnFirst.Enabled = false;
							btnNext.Enabled = false;
							btnLast.Enabled = false;
							btnNext.Enabled = false;
							return;
						}

						if (m_pageCount >= totalPages)
						{
							btnPrevious.Enabled = true;
							btnFirst.Enabled = true;
							btnLast.Enabled = false;
							btnNext.Enabled = false;
						}
						else if (m_pageCount < totalPages && m_pageCount > 1)
						{
							btnPrevious.Enabled = true;
							btnNext.Enabled = true;
							btnLast.Enabled = true;
							btnFirst.Enabled = true;
						}
						else if (m_pageCount <= 1)
						{
							btnPrevious.Enabled = false;
							btnFirst.Enabled = false;
							btnLast.Enabled = true;
							btnNext.Enabled = true;
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void txtObjectNumber_KeyPress(object sender, KeyPressEventArgs e)
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

		#endregion

		#region Other Events

		private void tabControlObjHierarchy_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e)
		{
			FinishPendingEdits();

			if (IsEmptySelectionChange(e))
				return;

			OMETabStripItem item = e.Item;
			if (string.IsNullOrEmpty(item.Title))
				return;

			try
			{
				int objectIndex = ObjectIndexInMasterViewFor(item);
				EnsureCurrentPageIs(PageNumberFor(objectIndex));

				//This check helps avaoiding recurssion.
				if (masterView.SortOrder == SortOrder.None)
				{
					SetSelectedObjectInMasterView(objectIndex);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void SetSelectedObjectInMasterView(int objectIndex)
		{
			EnsureCurrentPageIs(PageNumberFor(objectIndex));
			DataGridViewRow row = masterView.Rows[OffsetInCurrentPageFor(objectIndex) - 1];
			row.Selected = true;
			row.Cells[1].Selected = true;
		}

		private void EnsureCurrentPageIs(int pageNumber)
		{
			if (CurrentPageNumber() != pageNumber)
			{
				SetCurrentPageTo(pageNumber);
			}
		}

		private void SetCurrentPageTo(int pageNumber)
		{
			txtCurrentPage.Text = pageNumber.ToString();
			txtObjectNumber_KeyDown(txtCurrentPage, new KeyEventArgs(Keys.Enter));
		}

		private int PageCount()
		{
			return Convert.ToInt32(lblPageCount.Text);
		}

		private int CurrentPageNumber()
		{
			return Convert.ToInt32(txtCurrentPage.Text);
		}

		private static int PageNumberFor(int masterObjectIndex)
		{
			double pageNumber = (double) masterObjectIndex / PagingData.PAGE_SIZE;
			return Math.Max(Convert.ToInt32(Math.Ceiling(pageNumber)), 1);
		}

		private static int OffsetInCurrentPageFor(int masterViewObjectIndex)
		{
			int offset = masterViewObjectIndex % PagingData.PAGE_SIZE;
			return offset == 0 ? PagingData.PAGE_SIZE : offset;
		}

		private static int ObjectIndexInMasterViewFor(OMETabStripItem item)
		{
			return Convert.ToInt32(item.Title.Split(CONST_SPACE)[1]);
		}

		private static bool IsEmptySelectionChange(TabStripItemChangedEventArgs e)
		{
			return e.Item == null || (e.ChangeType != OMETabStripItemChangeTypes.SelectionChanged);
		}

		private void FinishPendingEdits()
		{
			masterView.EndEdit();
			
			TreeGridView treeview = TreeViewFor(detailsTabs.SelectedItem);
			if (treeview != null)
				treeview.EndEdit();
		}

		private void panelResultGridOptions_SizeChanged(object sender, EventArgs e)
		{
			if (panelResultGridOptions.Width <= panelLeft.Width + panelRight.Width)
			{
				panelResultGridOptions.MinimumSize = new Size(panelLeft.Width + panelRight.Width, Height);
			}
		}

		private void txtObjectNumber_TextChanged(object sender, EventArgs e)
		{
			try
			{
				int result;
				if (!Int32.TryParse(txtCurrentPage.Text.Trim(), out result))
				{
					if (result <= 0)
					{
						int pageNo = result + 1;
						txtCurrentPage.Text = pageNo.ToString();
					}
					else
						txtCurrentPage.Text = result.ToString();

					txtCurrentPage.SelectAll();
				}

				if (string.IsNullOrEmpty(txtCurrentPage.Text) || txtCurrentPage.Text == "0")
				{
					const int pageNumber = m_pagingStartIndex + 1;
					txtCurrentPage.Text = pageNumber.ToString();
					txtCurrentPage.SelectAll();
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Public and Internal Helper Methods

		public override void SetLiterals()
		{
			base.SetLiterals();

			try
			{
				buttonSaveResult.Text = Helper.GetResourceString(Constants.BUTTON_SAVE_CAPTION);
				btnSave.Text = Helper.GetResourceString(Constants.BUTTON_SAVE_CAPTION);
				btnDelete.Text = Helper.GetResourceString(Constants.BUTTON_DELETE_CAPTION);
				lblFechedObjects.Text = Helper.GetResourceString(Constants.LABEL_OBJECTS_NO);
				lblof.Text = Helper.GetResourceString(Constants.LABEL_OF);
				toolTipPagging.SetToolTip(btnFirst, Helper.GetResourceString(Constants.TOOLTIP_PAGE_FIRST));
				toolTipPagging.SetToolTip(btnPrevious, Helper.GetResourceString(Constants.TOOLTIP_PAGE_PREV));
				toolTipPagging.SetToolTip(btnNext, Helper.GetResourceString(Constants.TOOLTIP_PAGE_NEXT));
				toolTipPagging.SetToolTip(btnLast, Helper.GetResourceString(Constants.TOOLTIP_PAGE_LAST));
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		internal void ClearResult()
		{
			try
			{
				masterView.Rows.Clear();
				masterView.Columns.Clear();

				detailsTabs.Items.Clear();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		internal static void CheckColor(TreeGridNode node)
		{
			try
			{
				if (node.Cells[1].Style.ForeColor == Color.Red)
				{
					node.Cells[1].Style.ForeColor = Color.Black;
					node.Cells[1].Style.SelectionForeColor = Color.White;
				}
				if (node.HasChildren)
				{
					foreach (TreeGridNode currnode in node.Nodes)
						CheckColor(currnode);
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		internal static void PaintBlack(TreeGridView treeobj)
		{
			try
			{
				TreeGridNode node = treeobj.Nodes[0];
				CheckColor(node);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		#endregion

		#region Private Helper Methods

		private void InitializeTabControl()
		{
			detailsTabs = new OMETabStrip();

			detailsTabs.Dock = DockStyle.Fill;
			detailsTabs.Show();
			tableLayoutPanelResult.Controls.Add(detailsTabs, 0, 0);
			detailsTabs.TabStripItemSelectionChanged += tabControlObjHierarchy_TabStripItemSelectionChanged;
			detailsTabs.Click += tabControlObjHierarchy_Click;
			detailsTabs.TabStripItemClosing += tabControlObjHierarchy_TabStripItemClosing;
		}

		private void tabControlObjHierarchy_Click(object sender, EventArgs e)
		{
			OMETabStripItem item = ((OMETabStrip) sender).SelectedItem;
			if (item != null)
				CheckForObjectPropertiesTab(ReferencedObjectFor(item));
		}

		private void CheckForObjectPropertiesTab(object SelectedObject)
		{
			try
			{
				if (detailsTabs.Items.Count >= 1)
				{
					PropertiesTab propertiesTab = PropertiesTab.Instance;
					if (propertiesTab.ShowObjectPropertiesTab == false)
					{
						propertiesTab.ShowObjectPropertiesTab = true;
						propertiesTab.RefreshPropertiesTab(SelectedObject);
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private void tabControlObjHierarchy_TabStripItemClosing(TabStripItemClosingEventArgs e)
		{
			if (detailsTabs.Controls.Count == 1)
				e.Cancel = true;
		}

		private void InitializeResultDataGridView()
		{
			try
			{
				masterView = new dbDataGridView();
				masterView.Size = Size;
				masterView.ReadOnly = false;
				masterView.EditMode = DataGridViewEditMode.EditOnF2;
				masterView.ScrollBars = ScrollBars.Both;
				masterView.AllowUserToOrderColumns = true;
				masterView.AllowUserToResizeColumns = true;
				masterView.AllowDrop = true;
				masterView.Dock = DockStyle.Fill;
				masterView.SelectionChanged += masterView_SelectionChanged;
				masterView.CellEndEdit += masterView_CellEndEdit;
				masterView.CellBeginEdit += masterView_CellBeginEdit;
				masterView.Click += masterView_Click;
				tableLayoutPanelResultGrid.Controls.Add(masterView, 0, 0);
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
        
		private void masterView_Click(object sender, EventArgs e)
		{
			masterView_SelectionChanged(sender, e);
		}

		private void UpdateDataTreeView(object objUpdated, DataGridViewRow row)
		{
			try
			{
				OMETrace.WriteFunctionStart();

				if (objUpdated != null)
				{
					OMETabStripItem detailsTab = FindDetailsTabForObjectIndex(DetailsTabCaptionFor(row));
					if (detailsTab != null)
					{
						TreeGridView treeview = dbInteraction.GetObjectHierarchy(objUpdated, ClassName);

						detailsTab.Controls.Clear();
						detailsTab.Controls.Add(treeview);
						RegisterTreeviewEvents(treeview);
						detailsTab.Name = CONST_TRUE;
					}
				}

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		private static void MakeAllElementsInGridBlack(DataGridView gridView)
		{
			try
			{
				foreach (DataGridViewRow row in gridView.Rows)
				{
					foreach (DataGridViewCell cell in row.Cells)
					{
						if (cell.Style.ForeColor == Color.Red)
						{
							cell.Style.ForeColor = Color.Black;
							cell.Style.SelectionForeColor = Color.White;
						}
					}
				}
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}


		private static string GetFullPath(TreeGridNode treenode)
		{
			StringBuilder fullpath = new StringBuilder(string.Empty);
			TreeGridNode treenodeParent;
			List<string> stringParent = new List<string>();
			string parentName=string.Empty ;
		    string fieldName=string.Empty ;
			string fillpathString = string.Empty;

			try
			{
				OMETrace.WriteFunctionStart();

				treenodeParent = treenode.Parent;
                while (treenodeParent != null)
                {
                    parentName = treenodeParent.Cells[0].Value.ToString();
                    if (parentName.Contains("Object ID"))
                    {
                        int index = parentName.IndexOf("Object ID");
                        fieldName = parentName.Remove(index - 1).Trim();
                    }

                    //reaches the root
                    if (treenodeParent.Cells[0].Value.ToString().IndexOf(CONST_COMMA) != -1)
                    {
                        //Set the base class name for selected field
                        Helper.BaseClass = parentName;
                        fieldName = treenodeParent.Cells[0].Value.ToString().Split(CONST_COMMA.ToCharArray())[0];
                    }
                    string[] completename = fieldName.Split('.');
                    fieldName = completename[completename.Length - 1];
                    stringParent.Add(fieldName);
                    if (treenodeParent.Parent.RowIndex != -1)
                    {
                        treenodeParent = treenodeParent.Parent;
                    }
                    else
                        break;
                }

			    for (int i = stringParent.Count; i > 0; i--)
				{
					string parent = stringParent[i - 1] + ".";
					fullpath.Append(parent);
				}

				fillpathString = fullpath.Append(treenode.Cells[0].Value.ToString()).ToString();

				OMETrace.WriteFunctionEnd();
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}

			return fillpathString;
		}

		private static IType ValueType(IList<IType> types)
		{
			return types[types.Count - 1];
		}

		#endregion
	}
}