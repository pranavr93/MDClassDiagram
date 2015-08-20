// MonoDevelop ClassDesigner
using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using MonoHotDraw.Figures;
using MonoDevelop.Core;
using Backend;

namespace Figures 
{
	public class InterfaceFigure: TypeFigure 
    {
        static string type = MonoDevelop.Ide.Gui.Stock.Interface;
		public InterfaceFigure(InterfaceNode interfacenode): base(interfacenode, type) 
        {
            base.color = new Cairo.Color(1.0, 1.0, 0.7, 0.3);
		}

	}
}
