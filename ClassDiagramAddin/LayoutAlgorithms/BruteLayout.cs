/*
 * This is a brute layout algorithm which positions class diagrams one 
 * after the other and finally draws the links. Can result in ugly diagrams
 * when lot of link overlaps happen.
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
    public class BruteLayout: ILayout
    {
        public BruteLayout()
        {
            figures = new List<TypeFigure>();
            figureToInterfaces = new Dictionary<string, SimpleTextFigure>();
            AllNodes = new List<Node>();
            implementations = new List<SimpleTextFigure>();
        }
        public IEnumerable<IFigure> GetFigures(UMLClass cls)
        {
            // Add all entities
            foreach(var classnode in cls.ClassNodes){
                figures.Add(new ClassFigure(classnode));
                AllNodes.Add(classnode);
                //NodeMapping.Add(classnode.Namespace,classnode);
            }
            foreach(var interfacenode in cls.InterfaceNodes){
                figures.Add(new InterfaceFigure(interfacenode));
                AllNodes.Add(interfacenode);
                //NodeMapping.Add(interfacenode.Namespace,interfacenode);
            }
            foreach(var structnode in cls.StructNodes){
                figures.Add(new StructFigure(structnode));
                AllNodes.Add(structnode);
                //NodeMapping.Add(structnode.Namespace,structnode);
            }
            foreach(var enumnode in cls.EnumNodes){
                figures.Add(new EnumFigure(enumnode));
                AllNodes.Add(enumnode);
                //NodeMapping.Add(enumnode.Namespace,enumnode);
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
                if(node.Implementations.Count ==0) continue;
                string imps = "";
                foreach(var implementation in node.Implementations)
                {
                    imps+=implementation;
                    imps+="\n";
                }
                SimpleTextFigure interf = new SimpleTextFigure(imps);
                figureToInterfaces.Add(subclass.Namespace,interf);
                implementations.Add(interf);
                InheritanceConnectionFigure connex = new InheritanceConnectionFigure(subclass, interf);
                yield return connex;

            }
            double x = 50.0;
            double y = 50.0;


            foreach (TypeFigure figure in figures)  {
                figure.MoveTo(x, y);
                if(figureToInterfaces.ContainsKey(figure.Namespace)){
                    var simple = figureToInterfaces[figure.Namespace];
                    simple.MoveTo(x,y-50);
                    yield return simple;
                } 
                yield return figure;
                x += figure.DisplayBox.Width + 50.0;
                if (x > 1000.0) {
                    x = 50.0;
                    y += figure.DisplayBox.Height + 100.0;

                }
            }
            //foreach(var figure in implementations)
            //{
            //    figure.MoveTo(x,y);
            //    yield return figure;
            //    x += figure.DisplayBox.Width + 50.0;
            //    if (x > 1000.0) {
            //        x = 50.0;
            //        y += figure.DisplayBox.Height + 100.0;
            //    }
            //}
        }
        private TypeFigure GetFigure(string name) {
            foreach (TypeFigure figure in figures) {
                if (figure.Namespace == name)
                    return figure;
            }
            return null;
        }
        //private SimpleTextFigure GetImp(string name){
        //    foreach(SimpleTextFigure figure in implementations){
        //        if(figure.Text == name){
        //            return figure;
        //        }
        //    }
        //    return null;
        //}
        private List<TypeFigure> figures;
        private List<SimpleTextFigure> implementations;
        Dictionary<string,SimpleTextFigure> figureToInterfaces;
        List<Node> AllNodes;
    }

}

