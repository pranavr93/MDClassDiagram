// MonoDevelop ClassDesigner
//
using System;
using System.Collections.Generic;
using Cairo;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;
using MonoHotDraw.Handles;
using MonoHotDraw.Locators;
using Gdk;


namespace Figures {
	
    public class TypeHeaderFigure: VStackFigure {
		
        public TypeHeaderFigure(Pixbuf icon): base() {
			namespaceFigure = new SimpleTextFigure("Namespace");
			namespaceFigure.Padding = 0;
            namespaceFigure.FontSize = 9;
			
			typeFigure = new SimpleTextFigure("Type");
			typeFigure.Padding = 0;
            typeFigure.FontSize = 7;

			nameFigure = new SimpleTextFigure("Name");
			nameFigure.Padding = 0;
            nameFigure.FontSize = 12;



			Spacing = 3.0;
            _icon = new PixbufFigure(icon);
            //Add(new TypeMemberFigure(icon,"",typeFigure.Text));
            Add(_icon);
			//Add(typeFigure);
			Add(namespaceFigure);
			Add(nameFigure);
		}
		
		public string Name {
			get {
				return nameFigure.Text;
			}
			set {
				nameFigure.Text = value;
			}
		}
		
		public string Namespace {
			get {
				return namespaceFigure.Text;
			}
			set {
				namespaceFigure.Text = value;
			}
		}
		
		public string Type {
			get {
				return typeFigure.Text;
			}
			set {
   				typeFigure.Text = value;
			}
		}
        public PixbufFigure Icon{
            get {
                return _icon;
            }
            set {
                _icon = value;
            }
        
        }
		private SimpleTextFigure namespaceFigure;
		private SimpleTextFigure typeFigure;
		private SimpleTextFigure nameFigure;
        private PixbufFigure _icon;
	}
}
