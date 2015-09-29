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
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of OutputView.
	/// </summary>
	public class OutputView : DockContent
	{
		OutputViewControl _console;
		
		public OutputView(MainForm main)
		{
			CloseButton = false;
			DockAreas = (
					DockAreas.Float |
					DockAreas.DockBottom |
					DockAreas.DockTop |
					DockAreas.DockLeft |
					DockAreas.DockRight);
			ShowHint = DockState.DockBottom;
			Text = "Output";
			
			_console = new OutputViewControl();
			_console.MainForm = main;
			_console.Dock = DockStyle.Fill;
			Controls.Add(_console);
		}
		
		public void AppendText(string text)
		{
			_console.AppendText(text);
		}
		
		public void WriteLine(string line)
		{
			_console.WriteLine(line);
		}
	}
}
