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
		private TypeFigure GetFigure(string name) {
            //Console.WriteLine("In func: " + name); 
			foreach (TypeFigure figure in figures) {
                //Console.WriteLine("Try : " + figure.Namespace);
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
