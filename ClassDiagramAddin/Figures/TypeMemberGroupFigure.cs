using System;
using System.Collections.Generic;
using Gdk;
using MonoHotDraw.Figures;
using MonoHotDraw.Handles;
using MonoHotDraw.Locators;
using MonoHotDraw.Util;

namespace Figures {
	
	public class TypeMemberGroupFigure: VStackFigure {
		
		public TypeMemberGroupFigure(string name): base() {
			Spacing = 10;
			Alignment = VStackAlignment.Left;
			
			groupName = new SimpleTextFigure(name);
			groupName.Padding = 0;
			groupName.FontSize = 12;
			groupName.FontColor = new Cairo.Color(0.3, 0.0, 0.0);
            groupName.FontFamily = Pango.Weight.Heavy.ToString();
            //groupName.FontColor  = new Cairo.Color(1.0,1.0,1.0);
            //this.FillColor = new Cairo.Color(1.0,1.0,1.0);
            Add(groupName);
			
			membersStack = new VStackFigure();
			membersStack.Spacing = 8;
			
			expandHandle = new ToggleButtonHandle(this, new AbsoluteLocator(-10, 7.5));
			expandHandle.Toggled += delegate(object sender, ToggleEventArgs e) {
				if (e.Active) {
					Add(membersStack);
				}
				else {
					Remove(membersStack);
				}
			};
           
			expandHandle.Active = true;
			expandHandle.FillColor = new Cairo.Color(0,0,0.0,0.0);
            expandHandle.Height = 10;
            expandHandle.Width = 10;

		}
		
		public void AddMember(Pixbuf icon, string retValue, string name) {
			TypeMemberFigure member = new TypeMemberFigure(icon, retValue, name);
			membersStack.Add(member);
		}
		
		public override IEnumerable<IHandle> HandlesEnumerator {
			get {
				yield return expandHandle;
			}
		}
		
		public override RectangleD InvalidateDisplayBox {
			get {
				RectangleD rect = base.InvalidateDisplayBox;
				rect.Inflate(15, 0);
				return rect;
			}
		}

		
		private SimpleTextFigure groupName;
		private VStackFigure membersStack;
		private ToggleButtonHandle expandHandle;
	}
}
