using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using MonoHotDraw.Figures;
using MonoDevelop.Core;
using Backend;


namespace Figures {

	public class StructFigure: TypeFigure {

		public StructFigure(StructNode structnode): base(structnode) {
		}

	}
}
