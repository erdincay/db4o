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
using System.Runtime.InteropServices;
using WeifenLuo.WinFormsUI.Docking;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of WebBrowserView.
	/// </summary>
	public class WebBrowserView : DockContent, IOleClientSite, IDocHostUIHandler
	{
		private readonly WebBrowser _webBrowser;
				
		public WebBrowserView()
		{			
			CloseButton = false;
			DockAreas = DockAreas.Document;
			
			WebBrowserViewControl control = new WebBrowserViewControl();
			control.Dock = DockStyle.Fill;
			_webBrowser = control.WebBrowser;
			_webBrowser.DocumentTitleChanged += OnDocumentTitleChanged;
			
			Controls.Add(control);
		}

		private void OnDocumentTitleChanged(object sender, EventArgs e)
		{
			Text = _webBrowser.Document.Url.Segments[_webBrowser.Document.Url.Segments.Length - 1];
		}

		/// <summary>
		/// object that will be exposed inside the WebBrowser through
		/// the window.external property.
		/// </summary>
		public object External
		{
			get
			{
				return _webBrowser.ObjectForScripting;
			}
			
			set
			{
				_webBrowser.ObjectForScripting = value;
			}
		}
		
		public void Navigate(string url)
		{			
			_webBrowser.Navigate(url);
		}
		
		#region IOleClientSite implementation
		void IOleClientSite.SaveObject()
		{			
		}
		
		void IOleClientSite.GetMoniker(uint dwAssign, uint dwWhichMoniker, ref object ppmk)
		{			
		}
		
		void IOleClientSite.GetContainer(ref object ppContainer)
		{
			ppContainer = this;			
		}
		
		void IOleClientSite.ShowObject()
		{			
		}
		
		void IOleClientSite.OnShowWindow(bool fShow)
		{			
		}
		
		void IOleClientSite.RequestNewObjectLayout()
		{			
		}
		#endregion
		
		#region IDocHostUIHandler implementation
		uint IDocHostUIHandler.ShowContextMenu(uint dwID, ref tagPOINT ppt,
		                     object pcmdtReserved,
		                     object pdispReserved)
		{
			return 0;
		}
		
		void IDocHostUIHandler.GetHostInfo(ref DOCHOSTUIINFO pInfo)
		{
			pInfo.dwFlags |= (uint)(DOCHOSTUIFLAG.DOCHOSTUIFLAG_NO3DBORDER |
			                  DOCHOSTUIFLAG.DOCHOSTUIFLAG_DISABLE_SCRIPT_INACTIVE);
			
		}
		
		void IDocHostUIHandler.ShowUI(uint dwID, ref object pActiveObject, ref object pCommandTarget, ref object pFrame, ref object pDoc)
		{			
		}
		
		void IDocHostUIHandler.HideUI()
		{			
		}
		
		void IDocHostUIHandler.UpdateUI()
		{			
		}
		
		void IDocHostUIHandler.EnableModeless(int fEnable)
		{			
		}
		
		void IDocHostUIHandler.OnDocWindowActivate(int fActivate)
		{			
		}
		
		void IDocHostUIHandler.OnFrameWindowActivate(int fActivate)
		{			
		}
		
		void IDocHostUIHandler.ResizeBorder(ref tagRECT prcBorder, int pUIWindow, int fRameWindow)
		{			
		}
				
		uint IDocHostUIHandler.TranslateAccelerator(ref tagMSG lpMsg, ref Guid pguidCmdGroup, uint nCmdID)
		{			
			// let CTRL+A, CTRL+C to go through
			const int VK_CONTROL = 0x11;
			if (GetAsyncKeyState(VK_CONTROL) < 0)
			{
				switch (lpMsg.wParam &= 0xFF)
				{
					case 'A':
					case 'C':
						return 1;
				}
			}
			return 0;
		}
		
		void IDocHostUIHandler.GetOptionKeyPath(ref string pchKey, uint dw)
		{			
		}
		
		object IDocHostUIHandler.GetDropTarget(ref object pDropTarget)
		{		
			return pDropTarget;
		}		
		
		void IDocHostUIHandler.GetExternal(out object ppDispatch)
		{			
			ppDispatch = _webBrowser.ObjectForScripting;
		}		
		
		uint IDocHostUIHandler.TranslateUrl(uint dwTranslate, string pchURLIn, ref string ppchURLOut)
		{			
			return 0;
		}
		
		IDataObject IDocHostUIHandler.FilterDataObject(IDataObject pDO)
		{			
			return pDO;
		}
		#endregion
		
		[DllImport("User32.dll")]
		static extern short GetAsyncKeyState(int key);
	}
}
