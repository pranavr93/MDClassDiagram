/* 
 * Summary :
 * This class implements a tree layout. The idea is to get a clean layout
 * without link overlaps. We first separate out the class diagrams into 
 * separate trees. We then perform level order traversal on each each and 
 * draw them. Single nodes (Class nodes with no inheritance, all structs,
 * enums and interfaces) are then drawn one after the other. Implementations 
 * are indicated through small arrows pointing above the class.
 *
 */ 
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
            figures     = new List<TypeFigure>();
            TreeNodes   = new List<TreeNode>();
            map         = new Dictionary<string, TreeNode>();
            Roots       = new List<TreeNode>();
 
            figureToInterfaces = new Dictionary<string, SimpleTextFigure>();
            Singles = new List<ClassFigure>();
        }
        private TreeNode GetTreeNode(string name){
            if(map.ContainsKey(name))
                return map[name];
            return null;
        }

        /// <summary>
        /// Gets the tree roots.
        /// </summary>
        /// <returns>List of roots</returns>
        /// <param name="cls">Project details</param>
        private List<TreeNode> GetTreeRoots(UMLClass cls){
            // Map every class node.
            // Namespace - Treenode
            foreach(var cnode in cls.ClassNodes){
                TreeNode tn = new TreeNode(cnode.Namespace);
                tn.Node = cnode;
                TreeNodes.Add(tn);
                if(!map.ContainsKey(tn.Name))
                    map.Add(tn.Name,tn);
            }

            //Create all parent child links
            foreach(var cnode in cls.ClassNodes)
            {
                foreach(var parent in cnode.Links)
                {
                    //Console.Write(cnode.Namespace);
                    //Console.WriteLine(parent.ToString());
                    var parentnode = GetTreeNode(parent);
                    var childnode = GetTreeNode(cnode.Namespace);

                    if(parentnode == null){
                        //If inherits class outside project
                        continue;
                    }
                    parentnode.AddChild(childnode);
                    childnode.Parent = parentnode;
                }
            }

            //At this point, we have a forest. Every tree in the 
            //forest wil be drawn separately

            List<TreeNode> Roots = new List<TreeNode>();
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
            return Roots;
        }

        /// <summary>
        /// Returns all the figures to be drawn
        /// </summary>
        /// <returns>The figures.</returns>
        /// <param name="cls">Project details.</param>
        public IEnumerable<IFigure> GetFigures(UMLClass cls)
        {
            // We build trees with the input 
            Roots = GetTreeRoots(cls);

            #region Class Blocks
            double x = X_START;
            double y = Y_START; 
            // For each tree, we perform a level order traversal
            // and draw the tree in that order. This, will make sure
            // that links don't overlap each other. 
            foreach(var root in Roots)
            {
                if(root.children.Count == 0){
                    //figures.Add(new ClassFigure((ClassNode)root.Node));
                    Singles.Add(new ClassFigure((ClassNode)root.Node));
                    continue;
                }
                Queue<TreeNode> queue1 = new Queue<TreeNode>();
                Queue<TreeNode> queue2 = new Queue<TreeNode>();
                queue1.Enqueue(root);
                int level = 0;
                x = X_START;
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

                    x = X_START;
                    y+= Y_INCREMENT;
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
                    x = X_START;
                    y+= Y_INCREMENT;
                    foreach(var node in queue1){
                        var figure = new ClassFigure((ClassNode)node.Node);
                        figure.MoveTo(x,y);
                        x += figure.DisplayBox.Width + 50.0;
                        figures.Add(figure);
                    }

                }
            }
            #endregion Class Blocks

            #region Links
            foreach(var node in cls.ClassNodes){
                // Add all inheritance links
                TypeFigure subclass = GetFigure(node.Namespace);
                foreach(var link in node.Links){
                    TypeFigure superclass = GetFigure(link);
                    if (subclass != null && superclass != null) {
                        InheritanceConnectionFigure connection = new InheritanceConnectionFigure(subclass, superclass);
                        yield return connection;
                    }
                }
                if(node.Implementations.Count ==0) continue;

                // Handle implementations
                string imps = "";
                //Position the text to the center of the box
                string space_string = "";
                int len = subclass.Namespace.Length;

                foreach(var implementation in node.Implementations)
                {
                    for(int i =0;i<(len - implementation.Length) /2;i++) space_string +=" ";
                    
                    imps= imps + space_string + implementation + space_string;
                    imps+="\n";
                    Console.WriteLine(node.Name + " " + implementation);
                }
                SimpleTextFigure interf = new SimpleTextFigure(imps);
                Console.WriteLine(node.Namespace);
                figureToInterfaces.Add(subclass.Namespace,interf);
                InheritanceConnectionFigure connex = new InheritanceConnectionFigure(subclass, interf);
                yield return connex;

            }
            //Positioning the interface names on top of the class figures
            foreach(var item in figures){
                if(figureToInterfaces.ContainsKey(item.Namespace)){
                    var simple = figureToInterfaces[item.Namespace];
                    simple.MoveTo(item.DisplayBox.X,item.DisplayBox.Y-50);
                    yield return simple;
                } 
                yield return item;
            }
            #endregion Links

            #region Single Nodes
            // Draw single nodes in a row
            x = X_START;
            foreach(var item in Singles){
                item.MoveTo(x,y);
                x += item.DisplayBox.Width + SINGLES_GAP;
                if(x>PAGE_WIDTH_LIMIT){
                    x = X_START;
                    y+= Y_INCREMENT;
                }
                if(figureToInterfaces.ContainsKey(item.Namespace)){
                    var simple = figureToInterfaces[item.Namespace];
                    simple.MoveTo(item.DisplayBox.X,item.DisplayBox.Y-50);
                    yield return simple;
                } 
                yield return item;
            }

            // Next, all structures, enum and interfaces are drawn.
            foreach(var node in cls.StructNodes){
                var item = new StructFigure(node);
                item.MoveTo(x,y);
                x += item.DisplayBox.Width + SINGLES_GAP;
                if(x>PAGE_WIDTH_LIMIT){
                    x = X_START;
                    y+= Y_INCREMENT;
                }
                yield return item;
            }
            foreach(var node in cls.EnumNodes){
                var item = new EnumFigure(node);
                item.MoveTo(x,y);
                x += item.DisplayBox.Width + SINGLES_GAP;
                if(x> PAGE_WIDTH_LIMIT){
                    x = X_START;
                    y+= Y_INCREMENT;
                }
                yield return item;
            }
            foreach(var node in cls.InterfaceNodes){
                var item = new InterfaceFigure(node);
                item.MoveTo(x,y);
                x += item.DisplayBox.Width + SINGLES_GAP;
                if(x>PAGE_WIDTH_LIMIT){
                    x = X_START;
                    y+= Y_INCREMENT;
                }
                yield return item;
            }
            #endregion Single Nodes
        }

        /// <summary>
        /// Given full name, gets figure
        /// </summary>
        /// <returns>Figure corresponding to full namespace name.</returns>
        /// <param name="name">Full namespace </param>
        private TypeFigure GetFigure(string name) {
            foreach (TypeFigure figure in figures) {
                if (figure.Namespace == name)
                    return figure;
            }
            foreach(TypeFigure figure in Singles)
            {
                if (figure.Namespace == name)
                    return figure;
            }
            return null;
        }

        private const double X_START          = 50.0;
        private const double Y_START          = 50.0;
        private const double Y_INCREMENT      = 200.0;
        private const double SINGLES_GAP      = 50.0;
        private const double PAGE_WIDTH_LIMIT = 1000.0;

        private List<TypeFigure> figures;

        List<TreeNode> TreeNodes;

        List<TreeNode> Roots;

        Dictionary<string,TreeNode> map;

        Dictionary<string,SimpleTextFigure> figureToInterfaces;
        List<ClassFigure> Singles;
    }

}