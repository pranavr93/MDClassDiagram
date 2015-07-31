// MonoDevelop ClassDesigner
//
// Authors:
//	Manuel Cerón <ceronman@gmail.com>
//
// Copyright (C) 2009 Manuel Cerón
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using Gtk;
using Gdk;
using MonoHotDraw.Figures;
using MonoHotDraw.Handles;
using MonoHotDraw.Util;
using MonoHotDraw.Locators;
using MonoDevelop.Core;
using Backend;


//using System;
//using System.Collections.Generic;
using System.IO;

//using Gtk;
//using Gdk;

//using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using Mono.TextEditor;
using System.ComponentModel;


//using System;

using Mono.Addins;

//using MonoDevelop.Ide.Gui;
//using MonoDevelop.Ide.Gui.Pads;

//using MonoDevelop.Projects;
using MonoDevelop.Ide.Gui.Components;


namespace Figures {

    public abstract class TypeFigure: VStackFigure {

		public TypeFigure(): base() {
			Spacing = 30.0;
			Header = new TypeHeaderFigure();
            members = new VStackFigure();
			Add(Header);

			expandHandle = new ToggleButtonHandle(this, new AbsoluteLocator(10, 20));
			expandHandle.Toggled += delegate(object sender, ToggleEventArgs e) {
				if (e.Active) {
					Add(members);
				}
				else {
					Remove(members);
				}
			};
			expandHandle.Active = false;
			expandHandle.FillColor = new Cairo.Color(0,0,0.0,0.0);
			CreateGroups();
		}
		public TypeFigure(Node node): this() {

			Header.Name = node.Name;
			Header.Namespace = node.Namespace;
			Header.Type = node.Type.ToString();
			foreach(var field in node.Fields){
                
                //string iconid = MonoDevelop.Ide.Gui.Stock.Field;
                //Xwt.Drawing.Image image = MonoDevelop.Ide.ImageService.GetIcon(iconid,IconSize.Menu);
              //  Xwt.Drawing.BitmapImage img = image.ToBitmap();
               // img.
                //ImageService;
                //Xwt.Drawing.Image.
                            
                //ImageService.Get
                //if(iconz == null)
                //    Console.WriteLine("pixbuff nullllll");
                //Pixbuf icon = iconz.Pixbuf;
                Pixbuf icon = null;   
                AddField(icon,field.ReturnType, field.Name);
                //Pixbuf icon = MonoDevelop.Ide.ImageService.Get(MonoDevelop.Ide.Gui.Stock.Property, IconSize.Menu);
               // MonoDevelop.Ide.ImageService.GetIcon(
                //MonoDevelop.Ide.ImageService.G

               



			}
			foreach(var method in node.Methods){
				Pixbuf icon = null;
				AddMethod(icon,method.ReturnType,method.Name);
			}
            foreach(var evnt in node.Events){
                Pixbuf icon = null;
                AddEvent(icon, evnt.ReturnType,evnt.Name);
            }
            foreach(var property in node.Properties){
                Pixbuf icon = null;
                AddProperty(icon, property.ReturnType, property.Name);
            }
		}


		public override void BasicDrawSelected (Cairo.Context context) {
			RectangleD rect = DisplayBox;
			rect.OffsetDot5();
			context.LineWidth = 2.0;
			context.Rectangle(GdkCairoHelper.CairoRectangle(rect));
			context.Stroke();
		}

		public override void BasicDraw (Cairo.Context context) {
			RectangleD rect = DisplayBox;
			rect.OffsetDot5();
			context.LineWidth = 1.0;
			context.Rectangle(GdkCairoHelper.CairoRectangle(rect));
			//context.Color = new Cairo.Color(1.0, 1.0, 0.7, 0.3);
            context.Color = new Cairo.Color(0.879, 0.88, 0.90, 1.0);
			context.FillPreserve();
			context.Color = new Cairo.Color(0.0, 0.0, 0.0, 1.0);
			context.Stroke();

			base.BasicDraw(context);
		}

		public override bool ContainsPoint (double x, double y) {
			return DisplayBox.Contains(x, y);
		}

		public override RectangleD DisplayBox {
			get {
				RectangleD rect = base.DisplayBox;
				rect.X -= 20;
				rect.Y -= 10;
				rect.Width += 40;
				rect.Height += 30;
				return rect;
			}
			set {
				base.DisplayBox = value;
			}
		}

		public override IEnumerable<IHandle> HandlesEnumerator {
			get {
				yield return expandHandle;
				foreach (IHandle handle in base.HandlesEnumerator)
					yield return handle;
			}
            //get {
            //    if (_handles == null) {
            //        InstantiateHandles ();
            //    }

            //    foreach (IHandle handle in _handles) {
            //        yield return handle;
            //    }
            //}
		}
        private void InstantiateHandles () {
            _handles = new List <IHandle> ();
            _handles.Add (new SouthEastHandle (this));
            _handles.Add (new SouthWestHandle (this));
            _handles.Add (new NorthWestHandle (this));
            _handles.Add (new NorthEastHandle (this));
            _handles.Add (new NorthHandle (this));
            _handles.Add (new EastHandle (this));
            _handles.Add (new SouthHandle (this));
            _handles.Add (new WestHandle (this));
        }

		// FIXME: Use an IType member instead
		public string Name {
			get { return Header.Name; }
		}
		public string Namespace{
			get { return Header.Namespace; }
		}

		public void AddField(Pixbuf icon, string type, string name) {
			fields.AddMember(icon, type, name);
		}

		public void AddMethod(Pixbuf icon, string retvalue, string name) {
			methods.AddMember(icon, retvalue, name);
		}

		public void AddProperty(Pixbuf icon, string type, string name) {
			properties.AddMember(icon, type, name);
		}

		public void AddEvent(Pixbuf icon, string type, string name) {
			events.AddMember(icon, type, name);
		}

		protected virtual void AddMemberGroup(VStackFigure group) {
			members.Add(group);
		}

		protected TypeHeaderFigure Header { get; set; }

		protected virtual void CreateGroups() {
			fields = new TypeMemberGroupFigure(GettextCatalog.GetString("Fields"));
			properties = new TypeMemberGroupFigure(GettextCatalog.GetString("Properties"));
			methods = new TypeMemberGroupFigure(GettextCatalog.GetString("Methods"));
			events = new TypeMemberGroupFigure(GettextCatalog.GetString("Events"));

			AddMemberGroup(fields);
			AddMemberGroup(properties);
			AddMemberGroup(methods);
			AddMemberGroup(events);
		}

        protected override void OnFigureChanged (FigureEventArgs e) {
            base.OnFigureChanged(e);

        }

		protected TypeMemberGroupFigure fields;
		protected TypeMemberGroupFigure properties;
		protected TypeMemberGroupFigure methods;
		protected TypeMemberGroupFigure events;
        private List <IHandle> _handles;

        private VStackFigure members;
		private ToggleButtonHandle expandHandle;
	}
}
