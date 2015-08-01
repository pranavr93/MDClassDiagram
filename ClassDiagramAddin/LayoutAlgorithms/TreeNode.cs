
using System.Collections.Generic;

using MonoDevelop.Core;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using MonoHotDraw;
using Figures;
using Backend;
using System.IO;
using System;
using System.Linq;
using System.Windows;

namespace ClassDiagramAddin{
    public class TreeNode
    {
        public TreeNode(string name)
        {
            this.Name = name;
            this.children = new List<TreeNode>();
        }
        public void AddChild(TreeNode node)
        {
            this.children.Add(node);
        }

        TreeNode parent;
        public List<TreeNode> children;
        public string Name{get;set;}
        public Node Node {get;set;}
        public TreeNode Parent{get;set;}
    }
}
