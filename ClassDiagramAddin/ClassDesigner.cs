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

            ILayout algorithm = new TreeLayout();
            foreach(var figure in algorithm.GetFigures(cls))
            {
                mhdEditor.View.Drawing.Add(figure);
            }
        }	 
        private MonoHotDraw.SteticComponent mhdEditor;          
    }   
}
