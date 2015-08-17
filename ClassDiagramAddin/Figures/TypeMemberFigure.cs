using System;
using Cairo;
using Gdk;
using MonoHotDraw.Figures;
using MonoHotDraw.Util;

namespace Figures {

    public class TypeMemberFigure: HStackFigure {

        public TypeMemberFigure(Pixbuf icon,string retvalue, string name): base()
        {
            _icon = new PixbufFigure(icon);

            _retvalue = new SimpleTextFigure(retvalue);
            _name = new SimpleTextFigure(name);

            _name.Padding = 0.0;
            _name.FontSize = 10;
            _retvalue.Padding = 0.0;
            _retvalue.FontSize = 10;
            _retvalue.FontColor = new Cairo.Color(0, 0, 1.0);

            Alignment = HStackAlignment.Bottom;

            Add(_icon);
            Add(_retvalue);
            Add(_name);
        }

        private SimpleTextFigure _retvalue;
        private SimpleTextFigure _name;
        private PixbufFigure _icon;
       // private Xwt.Drawing.Image _icon;
    }
}
