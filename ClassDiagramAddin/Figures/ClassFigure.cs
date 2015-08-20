using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using MonoHotDraw.Figures;
using MonoDevelop.Core;
using Backend;


namespace Figures {

	public class ClassFigure: TypeFigure {
        static string type = MonoDevelop.Ide.Gui.Stock.Class;
        public ClassFigure(ClassNode classnode): base(classnode,type) {
            base.color = new Cairo.Color(0.879, 0.88, 0.90, 1.0);
		}

	}
}
