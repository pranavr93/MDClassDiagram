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

namespace FirstAddin{
	public class ClassDesigner : AbstractViewContent
	{
		public override void Load(FileOpenInformation fileOpenInformation)
		{
		}
		public override Gtk.Widget Control {
			get {
				
				return mhdEditor;
			}
		}
        public void BruteLayout(List<Node> AllNodes)
        {
            //Algorithm 1 for layout
            double x = 50.0;
            double y = 50.0;

            foreach (TypeFigure figure in figures)  {
                mhdEditor.View.Drawing.Add(figure);
                figure.MoveTo(x, y);
                x += figure.DisplayBox.Width + 50.0;
                if (x > 1000.0) {
                    x = 50.0;
                    y += figure.DisplayBox.Height + 100.0;
                }
            }
            // Iterate over links of all entities and draw links.
            foreach (var node in AllNodes) {
                TypeFigure subclass = GetFigure(node.Namespace);
                //Console.WriteLine(node.Namespace + " count " + node.Links.Count);
                foreach(var link in node.Links){
                    //Console.WriteLine(node.Namespace + " " + link);
                    TypeFigure superclass = GetFigure(link);
                    if (subclass != null && superclass != null) {
                        InheritanceConnectionFigure connection = new InheritanceConnectionFigure(subclass, superclass);
                        mhdEditor.View.Drawing.Add(connection);
                    }
                }
            }
        }
        Dictionary<string,TreeNode> map = new Dictionary<string, TreeNode>();
        private TreeNode GetTreeNode(string name)
        {
            if(map.ContainsKey(name))
                return map[name];
            else
                return null;
        }
        public void TreeBasedLayout(UMLClass cls)
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

                }
            }
        }
		public ClassDesigner(UMLClass cls)
		{
			
			ContentName = GettextCatalog.GetString ("Class Diagram");
			IsViewOnly = true;

			mhdEditor = new MonoHotDraw.SteticComponent();
			mhdEditor.ShowAll();
			figures = new List<TypeFigure>();
			List<Node> AllNodes = new List<Node>();
			// Add all entities
			foreach(var classnode in cls.ClassNodes){
				figures.Add(new ClassFigure(classnode));
				AllNodes.Add(classnode);
			}
			foreach(var interfacenode in cls.InterfaceNodes){
				figures.Add(new InterfaceFigure(interfacenode));
				AllNodes.Add(interfacenode);
			}
			foreach(var structnode in cls.StructNodes){
				figures.Add(new StructFigure(structnode));
				AllNodes.Add(structnode);
			}
			foreach(var enumnode in cls.EnumNodes){
				figures.Add(new EnumFigure(enumnode));
				AllNodes.Add(enumnode);
			}

            BruteLayout(AllNodes);
            TreeBasedLayout(cls);

		}
		private TypeFigure GetFigure(string name) {
			foreach (TypeFigure figure in figures) {
				if (figure.Namespace == name)
					return figure;
			}
            //Console.WriteLine("\n\n");
			return null;
		}
		private MonoHotDraw.SteticComponent mhdEditor;	
		private List<TypeFigure> figures;
	}

	
}
