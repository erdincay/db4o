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
using System.Reflection;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using System.Runtime.InteropServices;
using OMControlLibrary.Common;
using Constants = OMControlLibrary.Common.Constants;

namespace OMControlLibrary
{
	[ComVisibleAttribute(true)]
	public partial class ViewBase : UserControl, IView
	{

		#region Member Variables

		private static DTE2 m_applicationObject;
	
		#endregion

		#region Properties

		public static DTE2 ApplicationObject
		{
			get { return m_applicationObject; }
			set { m_applicationObject = value; }
		}

		#endregion

		#region Constructor
		public ViewBase()
		{
			InitializeComponent();
		}
		#endregion


		#region Virtual Method

		/// <summary>
		/// Set Literals
		/// </summary>
		public virtual void SetLiterals()
		{

		}

		#endregion

		//TODO: Use the window caption as soon as we fix the dependency on Caption being equal to "Closed".
		
		private static readonly IList<Window> _omnWindows = new List<Window>();
		
		public static void ResetToolWindowList()
		{
			lock (_omnWindows)
			{
				_omnWindows.Clear();
			}
		}


		public static IList<Window> GetAllPluginWindows()
		{
			lock (_omnWindows)
			{
				return _omnWindows; 
			}
		}

		internal static Window CreateToolWindow(string toolWindowClass, string caption, string guidpos)
		{
			
			
			Window found = GetWindow(caption);
			if (found != null)
			{
				found.Activate();
				LoadDataForAppropriateClass(found);
				return found;
			}

			string assemblypath = Assembly.GetExecutingAssembly().CodeBase.Remove(0, 8);
			object ctlobj = null;

			Windows2 wins2obj = (Windows2)ApplicationObject.Windows;
			Window window = wins2obj.CreateToolWindow2(
									FindAddin(ApplicationObject.AddIns),
									assemblypath,
									toolWindowClass,
									caption,
									guidpos,
									ref ctlobj);

			_omnWindows.Add(window);
			window.Linkable = true;

			return window;
		}

		public static Window GetWindow(string caption)
		{
			foreach (Window  entry in _omnWindows)
			{
				if (caption == entry.Caption )
				{
					return entry;
				}
			}
			return null;
		}
		

		private static void LoadDataForAppropriateClass(Window win)
		{
			ILoadData loaddata = null;
			switch (win.Caption)
			{
				case Constants.LOGIN:
					loaddata = win.Object as Login;
					break;
				case Constants.DB4OBROWSER:
					loaddata = win.Object as ObjectBrowser;
					break;
				case Constants.QUERYBUILDER:
					loaddata = win.Object as QueryBuilder;
					break;
				default:
					break;
			}
			if (loaddata != null)
				loaddata.LoadAppropriatedata();
		}

		
		public static bool IsOMNWindow(Window window)
		{
			return window != null ? _omnWindows.Contains(window) : false;
		}

		private static AddIn FindAddin(AddIns addins)
		{
			foreach (AddIn addin in addins)
			{
				if (!string.IsNullOrEmpty(addin.Name) && addin.Name.Contains("OMAddin"))
				{
					return addin;
				}
			}

			throw new InvalidOperationException();
		}
	}
}
