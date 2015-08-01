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
        public TreeLayout()
        {
            figures = new List<TypeFigure>();
            NodeMapping = new Dictionary<string, Node>();
            AllNodes = new List<Node>();
            TreeNodes = new List<TreeNode>();
            map = new Dictionary<string, TreeNode>();
            Roots = new List<TreeNode>();
            connections = new List<InheritanceConnectionFigure>();
        }
        private TreeNode GetTreeNode(string name)
        {
            if(map.ContainsKey(name))
                return map[name];
            else
                return null;
        }
        public IEnumerable<IFigure> GetFigures(UMLClass cls)
        {
            // Map every class node.
            // Namespace - Treenode
            foreach(var cnode in cls.ClassNodes)
            {
                TreeNode tn = new TreeNode(cnode.Namespace);
                tn.Node = cnode;
                TreeNodes.Add(tn);
                map.Add(tn.Name,tn);
            }

            //Create all parent child links
            foreach(var cnode in cls.ClassNodes)
            {
                foreach(var parent in cnode.Links)
                {
                    var parentnode = GetTreeNode(parent);
                    var childnode = GetTreeNode(cnode.Namespace);

                    parentnode.AddChild(childnode);
                    childnode.Parent = parentnode;
                }
            }
            // Add all roots to a list
            foreach(TreeNode tn in TreeNodes)
            {
                var temp = tn;
                while(temp.Parent !=null){
                    temp = temp.Parent;
                }
                if(!Roots.Contains(temp))
                {
                    Roots.Add(temp);
                }
            }
            foreach(var root in Roots)
            {
                Console.WriteLine(root.Name.ToString());
            }

            double x = 50;
            double y = 50;
            List<ClassFigure> Singles = new List<ClassFigure>();
            foreach(var root in Roots)
            {
                if(root.children.Count == 0){
                    Singles.Add(new ClassFigure((ClassNode)root.Node));
                    continue;
                }
                Queue<TreeNode> queue1 = new Queue<TreeNode>();
                Queue<TreeNode> queue2 = new Queue<TreeNode>();
                queue1.Enqueue(root);
                int level = 0;
                x = 50;
                var root_figure = new ClassFigure((ClassNode)root.Node);
                root_figure.MoveTo(x,y);
                figures.Add(root_figure);
                while(queue1.Count != 0 || queue2.Count != 0){
                    while(queue1.Count !=0){
                        var parent = queue1.Dequeue();
                        foreach(var child in parent.children){
                            queue2.Enqueue(child);
                        }
                    }
                    level = level + 1;
                    y += 200.0;
                    x = 50;
                    foreach(var node in queue2){
                        var figure = new ClassFigure((ClassNode)node.Node);
                        figure.MoveTo(x,y);
                        x += figure.DisplayBox.Width + 50.0;
                        figures.Add(figure);
                        //connections.Add(new InheritanceConnectionFigure(figure, node.P
                    }
                    if(queue2.Count == 0) continue;
                    while(queue2.Count !=0){
                        var parent = queue2.Dequeue();
                        foreach(var child in parent.children){
                            queue1.Enqueue(child);
                        }
                    }
                    level = level + 1;
                    y += 200.0;
                    x = 50;
                    foreach(var node in queue2){
                        var figure = new ClassFigure((ClassNode)node.Node);
                        figure.MoveTo(x,y);
                        x += figure.DisplayBox.Width + 50.0;
                        figures.Add(figure);
                    }

                }
            }

            foreach(var node in cls.ClassNodes){
                TypeFigure subclass = GetFigure(node.Namespace);
                foreach(var link in node.Links){
                    TypeFigure superclass = GetFigure(link);
                    if (subclass != null && superclass != null) {
                        InheritanceConnectionFigure connection = new InheritanceConnectionFigure(subclass, superclass);
                        //connection.EndHandle.Owner.
                        yield return connection;
                    }
                }
            }
            foreach(var item in figures){
                yield return item;
            }
            // Draw single nodes in a row
            x = 50;
            foreach(var item in Singles){
                item.MoveTo(x,y);
                x += item.DisplayBox.Width + 50.0;
                yield return item;
            }
        }
        private TypeFigure GetFigure(string name) {
            foreach (TypeFigure figure in figures) {
                if (figure.Namespace == name)
                    return figure;
            }
            return null;
        }
        private List<TypeFigure> figures;
        private List<InheritanceConnectionFigure> connections;
        Dictionary<string,Node> NodeMapping;
        List<Node> AllNodes;
        List<TreeNode> TreeNodes;
        List<TreeNode> Roots;
        Dictionary<string,TreeNode> map;
    }
}