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
using MonoHotDraw.Figures;

namespace ClassDiagramAddin{
    public interface ILayout
    {
        IEnumerable<IFigure> GetFigures(UMLClass cls);
    }

}
