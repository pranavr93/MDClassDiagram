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
using MonoHotDraw.Figures;

namespace ClassDiagramAddin{
    public class TreeLayout : ILayout
    {
        private TreeNode GetTreeNode(string name)
        {
            if(map.ContainsKey(name))
                return map[name];
            else
                return null;
        }
        public IEnumerable<IFigure> GetFigures(UMLClass cls)
        {
            foreach(var cnode in cls.ClassNodes)
            {
                TreeNode tn = new TreeNode(cnode.Namespace);
                map.Add(tn.Name,tn);
            }
            foreach(var cnode in cls.ClassNodes)
            {
                foreach(var parent in cnode.Links)
                {
                    var parentnode = GetTreeNode(parent);
                    parentnode.AddChild(cnode.Namespace);
                    var childnode = GetTreeNode(cnode.Namespace);
                    childnode.Parent = parentnode.Name;
                }
            }
            yield return new ClassFigure(new ClassNode());
        }
        private List<TypeFigure> figures;
        Dictionary<string,Node> NodeMapping;
        List<Node> AllNodes;
        Dictionary<string,TreeNode> map = new Dictionary<string, TreeNode>();
    }
}