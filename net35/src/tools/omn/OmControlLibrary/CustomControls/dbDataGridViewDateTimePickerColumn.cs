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
using System.Text;
using System.Windows.Forms;
using System.Collections;

/*******************************************************************************
Copyright Notice: Applied Identity, Inc.
Created By: Hardew Singh
Creation Date: 14/07/2006
Module Name: AIDataGridViewDateTimePickerColumn
File Description: This class is a custom DataGridViewColumn
                  for displaying DateTimePicker in cell

Modified By			:  Prabhakar Ahinave
Modification Date	:  02/06/2007
Description			:  Added LdapAttribute property.
*******************************************************************************/

namespace OMControlLibrary.Common
{
	//This class is used to display datetime picker control when is in edit
	//mode. It displayed with check box. we can also associate some text with
	//this column while datetimepicker check box is unchecked.
	//Cells of this column are AIDataGridViewDateTimePickerCell.
	public class dbDataGridViewDateTimePickerColumn : DataGridViewColumn
	{

		#region Member Variables

		string m_DefaultCellValue = string.Empty;
		ArrayList m_Rules;

		#endregion Member Variables

		#region Contructor

		public dbDataGridViewDateTimePickerColumn() : base(new dbDataGridViewDateTimePickerCell())
		{
			m_Rules = new ArrayList();
		}

		#endregion Contructor

		#region Properties

		/// <summary>
		/// Get/Set default value for the cell.
		/// If datetimepicker check box is unchecked this value will be displayed.
		/// </summary>
		public string DefaultCellValue
		{
			get { return m_DefaultCellValue; }
			set { m_DefaultCellValue = value; }
		}

		//Interface: IAIControls.Rules
		/// <summary>
		/// Get/Set rules with this column
		/// </summary>
		public ArrayList Rules
		{
			get { return m_Rules; }
			set { m_Rules = value; }
		}

		#endregion Properties

		#region Override methods

		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				// Ensure that the cell used for the template is a AIDataGridViewDateTimePickerCell.
				if (value != null &&
					!value.GetType().IsAssignableFrom(typeof(dbDataGridViewDateTimePickerCell)))
				{
					//TODO: Hardew: Exception message need to change it as per the project standard.
					throw new InvalidCastException("Must be a AIDataGridViewDateTimePickerCell");
				}
				base.CellTemplate = value;
			}
		}

		#endregion Override methods

	}
}
