// MonoDevelop ClassDesigner
//
// Authors:
using System;
using MonoHotDraw;
using MonoHotDraw.Figures;

namespace Figures {

	public class InheritanceConnectionFigure: LineConnection {
	
		public InheritanceConnectionFigure(): base() {
			EndTerminal = new TriangleArrowLineTerminal();
		}
		
		public InheritanceConnectionFigure(IFigure fig1, IFigure fig2): base(fig1, fig2) {
            // First parameter : width  Second parameter: Height of the arrow
			EndTerminal = new TriangleArrowLineTerminal(7,10);
		}
		
		public override bool CanConnectEnd (IFigure figure) {
			
			//if (figure is ClassFigure) {
			//	if (!figure.Includes(StartFigure)) {					
			//		return true;
			//	}
			//}
			return false;
		}
		
		public override bool CanConnectStart (IFigure figure) {
			//if (figure is ClassFigure) {
			//	if (!figure.Includes(EndFigure)) {
			//		return true;
			//	}
			//}
			return false;
		}
	}
}
