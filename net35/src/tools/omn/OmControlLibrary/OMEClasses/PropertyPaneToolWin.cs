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
using EnvDTE;
using OMControlLibrary.Common;
using OME.Logging.Common;

namespace OMControlLibrary
{
	public class PropertyPaneToolWin
	{
		private static Window propWindow;

		public static Window PropWindow
		{
			get
			{
				if (propWindow == null)
				{
					CreatePropertiesPaneToolWindow(false);
					propWindow.Visible = false;
				}
				return propWindow;
			}
			set
			{
				propWindow = value;
			}
		}

		public static void CreatePropertiesPaneToolWindow(bool DbDetails)
		{
			try
			{
				const string className = Common.Constants.CLASS_NAME_PROPERTIES;
				const string guidpos = Common.Constants.GUID_PROPERTIES;

				string caption = Helper.GetResourceString(Common.Constants.PROPERTIES_TAB_DATABASE_CAPTION);
				PropWindow = ViewBase.CreateToolWindow(className, caption, guidpos);
				Helper.SetTabPicture(PropWindow, Common.Constants.DB4OICON);
				
				if (PropWindow.AutoHides)
				{
					PropWindow.AutoHides = false;
				}
				PropWindow.Visible = true;
			}

			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}

		
	}

	public class ObjectBrowserToolWin
	{
		private static Window objBrowserWindow;

		public static Window ObjBrowserWindow
		{
			get
			{
				if (objBrowserWindow == null)
				{
					CreateObjectBrowserToolWindow();
					ObjBrowserWindow.Visible = false;
				}
				return objBrowserWindow;
			}
			set
			{
				objBrowserWindow = value;
			}
		}
		public static void CreateObjectBrowserToolWindow()
		{
			try
			{
					objBrowserWindow = ViewBase.CreateToolWindow(
											Common.Constants.CLASS_NAME_OBJECTBROWSER,
											Helper.GetResourceString(Common.Constants.DB4O_BROWSER_CAPTION),
											Common.Constants.GUID_OBJECTBROWSER);

                    Helper.SetTabPicture(objBrowserWindow, Common.Constants.DB4OICON);
					//objBrowserWindow.SetTabPicture(Helper.GetResourceImage(Common.Constants.DB4OICON));
				
				if (objBrowserWindow.AutoHides)
				{
					objBrowserWindow.AutoHides = false;
				}
				objBrowserWindow.Visible = true;
			}
			catch (Exception oEx)
			{
				LoggingHelper.ShowMessage(oEx);
			}
		}
	}
}
