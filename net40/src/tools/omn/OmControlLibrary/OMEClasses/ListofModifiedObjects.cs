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
using OManager.BusinessLayer.UIHelper;
using OMControlLibrary.Common;
using System.Windows.Forms;
using OME.Logging.Common;

namespace OMControlLibrary
{
	public class ListofModifiedObjects
	{

		private static Hashtable hash;

		public static Hashtable Instance
		{
			get
			{
				if (hash == null)
				{
					hash = new Hashtable();
				}
				return hash;
			}
		}

		public static void AddDatagrid(string strClassName, dbDataGridView dbDataGridViewQueryResult)
		{
			if (Instance.ContainsKey(strClassName))
			{
				Instance.Remove(strClassName);
				Instance.Add(strClassName, dbDataGridViewQueryResult);
			}
			else
			{
				Instance.Add(strClassName, dbDataGridViewQueryResult);
			}
		}
		
		public static DialogResult SaveBeforeWindowHiding(string Caption, dbDataGridView db)
		{
			DialogResult result = DialogResult.Cancel;

		    bool checkforValueChanged = false;
			try
			{
				foreach (DataGridViewRow row in db.Rows)
				{
					if (Convert.ToBoolean(row.Cells[Constants.QUERY_GRID_ISEDITED_HIDDEN].Value))
					{
						checkforValueChanged = true;
						break;
					}
				}

				if (!checkforValueChanged)
					return result;
				
				result = MessageBox.Show("'" + Caption + "' contains some modified objects. Do you want to save changes?",
				                         Helper.GetResourceString(Constants.PRODUCT_CAPTION), MessageBoxButtons.YesNo,
				                         MessageBoxIcon.Question);

				if (result == DialogResult.Yes)
				{
					foreach (DataGridViewRow row in db.Rows)
					{
                        if (Convert.ToBoolean(row.Cells[Constants.QUERY_GRID_ISEDITED_HIDDEN].Value))
                        {

                            int hierarchyLevel = dbInteraction.GetDepth(row.Tag);

                            Helper.DbInteraction.SaveCollection(row.Tag, hierarchyLevel);
                        }
					}
				}
				else
				{
					foreach (DataGridViewRow row in db.Rows)
					{
						if (Convert.ToBoolean(row.Cells[Constants.QUERY_GRID_ISEDITED_HIDDEN].Value))
						{
							dbInteraction.RefreshObject(row.Tag, 1);
						}
					}
				}
			}
			catch (Exception ex)
			{
				LoggingHelper.ShowMessage(ex);
			}

			return result;
		}

        //public static int CalculateLevel(object obj)
        //{
        //    return dbInteraction.GetDepth(obj);
        //}
	}
}