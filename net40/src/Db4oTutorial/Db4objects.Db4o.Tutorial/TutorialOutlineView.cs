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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking;

namespace Db4objects.Db4o.Tutorial
{
	/// <summary>
	/// Description of TutorialOutlineView.
	/// </summary>
	public class TutorialOutlineView : DockContent
	{
		private readonly MainForm _main;
		private readonly TreeView _tree;
		private readonly TutorialOutlineViewControl _outline;

		public TutorialOutlineView(MainForm main)
		{
			_main = main;

			Text = "Tutorial Outline";
			DockAreas = (	DockAreas.Float | 
							DockAreas.DockLeft |
			                DockAreas.DockRight);

			ClientSize = new Size(295, 347);
			DockPadding.Bottom = 2;
			DockPadding.Top = 2;
			ShowHint = DockState.DockLeft;
			CloseButton = false;

			_outline = new TutorialOutlineViewControl();

			_outline.Dock = DockStyle.Fill;
			_tree = _outline.TreeView;
			_tree.AfterSelect += _tree_AfterSelect;
			Controls.Add(_outline);
		}

		private void _tree_AfterSelect(object sender, TreeViewEventArgs args)
		{
			_main.NavigateTutorial((string) args.Node.Tag);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			LoadTutorialOutline();
		}

		private delegate void LoadFirstTopicFunction();

		private void LoadTutorialOutline()
		{
			TreeNode current = new TreeNode();
			TreeNode root = current;
			TreeNode currentParent = null;
			Stack nodes = new Stack();

			string path = _main.GetTutorialFilePath("outline.html");
			if (!File.Exists(path))
			{
				return;
			}

			XmlTextReader reader = new XmlTextReader(path);
			while (reader.Read())
			{
				string name = reader.Name;
				switch (name)
				{
					case "body":
						if (reader.IsStartElement())
						{
							SetLogo(ReadLogoPath(reader));
						}
						break;

					case "li":
					{
						reader.ReadStartElement("li");

						string href = reader.GetAttribute("href");

						reader.ReadStartElement("a");
						string description = reader.ReadString();

						current = new TreeNode(description);
						current.Tag = href;

						currentParent.Nodes.Add(current);

						reader.ReadEndElement();
						reader.ReadEndElement();

						break;
					}

					case "ul":
					{
						if (reader.IsStartElement())
						{
							nodes.Push(currentParent);
							currentParent = current;
						}
						else
						{
							currentParent = (TreeNode) nodes.Pop();
						}
						break;
					}
				}
			}

			foreach (TreeNode node in root.Nodes)
			{
				_tree.Nodes.Add(node);
			}
			_tree.ExpandAll();

			BeginInvoke(new LoadFirstTopicFunction(LoadFirstTopic));
		}

		private void SetLogo(string path)
		{
			_outline.SetLogo(_main.CombineTutorialPath(path));
		}

		private string ReadLogoPath(XmlTextReader reader)
		{
			reader.ReadStartElement("body");
			reader.ReadStartElement("a");
			
			string imgSource = reader.GetAttribute("src");
			reader.ReadStartElement("img");

			return imgSource;

		}

		private void LoadFirstTopic()
		{
			_tree.SelectedNode = _tree.Nodes[0];
		}

		private string LoadFile(string fname)
		{
			using (TextReader reader = File.OpenText(fname))
			{
				return reader.ReadToEnd();
			}
		}
	}
}