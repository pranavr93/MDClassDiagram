
using System.Collections.Generic;

using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using MonoHotDraw;
using MonoDevelop.ClassDesigner.Figures;
using Backend;
using System.IO;
using System;
using System.Linq;
using System.Windows;

namespace ClassDiagramAddin{
    public class TreeNode
    {
        TreeNode parent;
        List<string> children;
        public string Name{get;set;}
        public string Parent{get;set;}
        public TreeNode(string name)
        {
            this.Name = name;
        }
        public void AddChild(string name)
        {
            this.children.Add(name);
        }
    }
}
