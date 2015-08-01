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
    public class BruteLayout: ILayout
    {
        public BruteLayout()
        {
            figures = new List<TypeFigure>();
            NodeMapping = new Dictionary<string, Node>();
            AllNodes = new List<Node>();
        }
        public IEnumerable<IFigure> GetFigures(UMLClass cls)
        {
            // Add all entities
            foreach(var classnode in cls.ClassNodes){
                figures.Add(new ClassFigure(classnode));
                AllNodes.Add(classnode);
                NodeMapping.Add(classnode.Namespace,classnode);
            }
            foreach(var interfacenode in cls.InterfaceNodes){
                figures.Add(new InterfaceFigure(interfacenode));
                AllNodes.Add(interfacenode);
                NodeMapping.Add(interfacenode.Namespace,interfacenode);
            }
            foreach(var structnode in cls.StructNodes){
                figures.Add(new StructFigure(structnode));
                AllNodes.Add(structnode);
                NodeMapping.Add(structnode.Namespace,structnode);
            }
            foreach(var enumnode in cls.EnumNodes){
                figures.Add(new EnumFigure(enumnode));
                AllNodes.Add(enumnode);
                NodeMapping.Add(enumnode.Namespace,enumnode);
            }

            // Iterate over links of all entities and draw links.
            foreach (var node in AllNodes) {
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
            double x = 50.0;
            double y = 50.0;


            foreach (TypeFigure figure in figures)  {
                figure.MoveTo(x, y);

                yield return figure;
                x += figure.DisplayBox.Width + 50.0;
                if (x > 1000.0) {
                    x = 50.0;
                    y += figure.DisplayBox.Height + 100.0;
                }
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
        Dictionary<string,Node> NodeMapping;
        List<Node> AllNodes;
    }

}

