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


namespace OME.AdvancedDataGridView
{
    /// <summary>
    /// This class is associated with dbDataGridViewDateTimePickerColumn.
    /// It is simple DataGridView TextBox Cell but in Edit mode it 
    /// show AIDataGridViewDateTimePickerEditingControl.
    /// </summary>
    public class TreeGridViewDateTimePickerCell : DataGridViewTextBoxCell
    {
        #region Member Variables

        //string m_CellData = string.Empty;

        #endregion Member Variables

        #region Constructor

    	#endregion Constructor

        #region Override methods

        /// <summary>
        /// Initialize Editing Control, control type is AIDataGridViewDateTimePickerEditingControl.
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="initialFormattedValue"></param>
        /// <param name="dataGridViewCellStyle"></param>
        public override void InitializeEditingControl(int rowIndex, object
                initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            try
            {
                base.InitializeEditingControl(rowIndex,
                    initialFormattedValue,
                    dataGridViewCellStyle);

                string typeOfValue = DataGridView.Rows[rowIndex].Cells[2].Value.ToString() ;
                if (typeOfValue == typeof(DateTime).ToString())
                {
                    TreeGridViewDateTimePickerEditingControl ctl = DataGridView.EditingControl as TreeGridViewDateTimePickerEditingControl;

                    if (Value != null && Value != OwningColumn.DefaultCellStyle)
                    {
                        try
                        {
                            ctl.Value = Convert.ToDateTime(Value.ToString());
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            ctl.Value = DateTime.Now;
                        }
                    }
                }
                else if (typeOfValue == typeof(Boolean).ToString())
                {
                    DataGridViewComboBoxEditingControl ctl = DataGridView.EditingControl as DataGridViewComboBoxEditingControl;

                    //setting combox style
                    ctl.DropDownStyle = ComboBoxStyle.DropDownList;
                    ctl.FlatStyle = FlatStyle.Popup;
                    FillBoolColumnValue(ctl);

                    if (Value != null && Value != OwningColumn.DefaultCellStyle)
                    {
                        try
                        {
                            ctl.EditingControlFormattedValue  = Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                            ctl.SelectedItem = ctl.Items[0].ToString();
                        }
                    }
                    ctl.Width = OwningColumn.Width;
                }
            }
            catch (Exception oEx)
            {
                string str = oEx.Message;
                
            }
        }

        public override Type EditType
        {
            get
            {
                string typeOfValue = string.Empty;
            	Type controlType = typeof (DataGridViewTextBoxEditingControl);
                if (DataGridView.Rows[RowIndex].Cells[2].Value != null)
                {
                    typeOfValue = DataGridView.Rows[RowIndex].Cells[2].Value.ToString();
                    if (typeOfValue == typeof (DateTime).ToString())
                    {
                        controlType = typeof (TreeGridViewDateTimePickerEditingControl);
                    }
                    else if (typeOfValue == typeof (Boolean).ToString())
                    {
                        controlType = typeof (DataGridViewComboBoxEditingControl);
                    }
                }
                return controlType;
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that AIDataGridViewDateTimePickerCell contains.
                return typeof(String);
            }
        }

        public void FillBoolColumnValue(DataGridViewComboBoxEditingControl ctrl)
        {
            try
            {
                ctrl.Items.Clear();
                ctrl.Items.AddRange(new object[] { "True" , "False" });
                ctrl.SelectedIndex = 0;
            }
            catch (Exception oEx)
            {
               // LoggingHelper.ShowMessage(oEx);
            }
        }

        #endregion Override methods
    }
}
