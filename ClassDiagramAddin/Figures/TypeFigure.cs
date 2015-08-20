// MonoDevelop ClassDesigner
//
// Authors:
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

		public TypeFigure(string type): base() {
			Spacing = 10.0;
            Header = new TypeHeaderFigure(GetPixBuf(type));
            members = new VStackFigure();
			Add(Header);

			
           
     

		}

        private Pixbuf GetPixBuf(string type)
        {
            Xwt.Drawing.Image image = MonoDevelop.Ide.ImageService.GetIcon(type,IconSize.Menu);
            return MonoDevelop.Components.GtkUtil.ToPixbuf(image);
        }
		public TypeFigure(Node node, string type): this(type) {
			Header.Name = node.Name;
			Header.Namespace = node.Namespace;
			Header.Type = node.Type.ToString();

            // Opensthe corresponding file
            openClassHandle = new ToggleButtonHandle(this,new AbsoluteLocator(this.DisplayBox.Width, 10));
            openClassHandle.Toggled += delegate(object sender, ToggleEventArgs e) {
                if (e.Active) {
                    //openClassHandle.Active = false;;
                    MonoDevelop.Ide.IdeApp.Workbench.OpenDocument(new FilePath(node.FilePath),IdeApp.ProjectOperations.CurrentSelectedProject,true);
                }
            };
            openClassHandle.Active = false;
            openClassHandle.FillColor = new Cairo.Color(0,0,0.0,0.0);

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

			foreach(var field in node.Fields){
                Pixbuf icon = GetPixBuf(MonoDevelop.Ide.Gui.Stock.Field);
                AddField(icon,field.ReturnType, field.Name);
			}
			foreach(var method in node.Methods){
                Pixbuf icon = GetPixBuf(MonoDevelop.Ide.Gui.Stock.Method);
				AddMethod(icon,method.ReturnType,method.Name);
			}
            foreach(var evnt in node.Events){
                Pixbuf icon = GetPixBuf(MonoDevelop.Ide.Gui.Stock.Event);
                AddEvent(icon, evnt.ReturnType,evnt.Name);
            }
            foreach(var property in node.Properties){
                Pixbuf icon = GetPixBuf(MonoDevelop.Ide.Gui.Stock.Property);
                AddProperty(icon, property.ReturnType, property.Name);
            }
            if(node.Fields.Count == 0)
                members.Remove(fields);
            if(node.Methods.Count == 0)
                members.Remove(methods);
            if(node.Properties.Count == 0)
                members.Remove(properties);
            if(node.Events.Count == 0)
                members.Remove(events);
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
            context.SetSourceColor(this.color);

			context.FillPreserve();

            //Uncomment below to give border colour
            context.SetSourceColor(new Cairo.Color(0.0, 0.0, 0.0, 1.0));
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
                yield return openClassHandle;
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

            //Spacing for the above will create gap between Field title and each field
            //fields.Spacing = 30;
            //properties.Spacing = 30;
            //methods.Spacing = 30;
            //events.Spacing = 30;

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
        private ToggleButtonHandle openClassHandle;
        public Cairo.Color color {get;set;}
        //private string NodeType;
	}
}
