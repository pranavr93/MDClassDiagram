﻿using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using MonoHotDraw.Figures;
using MonoDevelop.Core;
using Backend;

namespace Figures 
{

	public class StructFigure: TypeFigure 
    {
        static string type = MonoDevelop.Ide.Gui.Stock.Struct;
		public StructFigure(StructNode structnode): base(structnode, type) 
        {
            base.color = new Cairo.Color(0.879, 0.88, 0.90, 1.0);
		}
	}
}
