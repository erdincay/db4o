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
using EnvDTE;
using EnvDTE80;

namespace OMAddin
{
	class OutputWindow
	{
		public static void Initialize(DTE2 applicationObject)
		{
			if (_instance == null)
			{
				_instance = new OutputWindow(applicationObject);
			}
		}

		public static OutputWindowPane Pane
		{
			get
			{
				return _instance._outputWindowPane;
			}
		}

		private OutputWindow(DTE2 applicationObject)
		{
			if (null == _outputWindowPane)
			{
				EnvDTE.OutputWindow outputWindow = (EnvDTE.OutputWindow)applicationObject.Windows.Item(Constants.vsWindowKindOutput).Object;
				_outputWindowPane = outputWindow.OutputWindowPanes.Add("OMN Output");
			}
		}

		private readonly OutputWindowPane _outputWindowPane;
		private static OutputWindow _instance;
	}
}
