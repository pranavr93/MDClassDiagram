using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects;
using System;
using MonoDevelop.Ide.TypeSystem;
using Microsoft.CodeAnalysis;
using Mono.TextEditor;
using Backend;
using MonoDevelop.Core;

namespace FirstAddin
{
	class InsertDateHandler : CommandHandler
	{
		protected override void Run ()
		{
			var selectedProject = IdeApp.ProjectOperations.CurrentSelectedProject;
			var temp =TypeSystemService.GetCompilationAsync(selectedProject);
			var res = temp.Result;
			UMLClass cls = RoslynRun.ParseFiles(res);

			ClassDesigner view = new ClassDesigner(cls);
			IdeApp.Workbench.OpenDocument(view,true);
			return ;


			//MonoDevelop.Ide.Gui.Document doc = IdeApp.Workbench.ActiveDocument;
			//string date = DateTime.Now.ToString ();

			//IdeApp.ProjectOperations.CreateProjectFile(
			//	IdeApp.ProjectOperations.CurrentSelectedProject,"hello"); 
			string test = "";
			//Document d = IdeApp.Workbench.NewDocument("Class Diagram","text/plain" , "whats up?");



			ProjectFile pf = selectedProject.AddFile("class_diagram");
			IdeApp.Workbench.OpenDocument(pf.FilePath,selectedProject,true);
			MonoDevelop.Ide.Gui.Document doc = IdeApp.Workbench.ActiveDocument;
			var textEditorData = doc.GetContent<ITextEditorDataProvider> ().GetTextEditorData ();

			//d.SaveAs("class_diagram");
			//d.Select();
			//selectedProject.
			//ProjectFile file = new ProjectFile();
			//file.Name = "Class Diagram File";
			//selectedProject.AddFile(file);

			//selectedProject.AddFile("class diagram");
			//ProjectFile file = new ProjectFile("class diagram");
			//file.


			//var temp =TypeSystemService.GetCompilationAsync(selectedProject);
			//var res = temp.Result;
			//UMLClass cls = RoslynRun.ParseFiles(res);

			foreach(var item in cls.ClassNodes){
				test+=item.Name;
				test+="\n\t";
				foreach(var subitem in item.Fields){
					test+=subitem.Name;
					test+="\n\t";
				}
				test+="\n\n";
			}

			textEditorData.InsertAtCaret (test);
		}
		protected override void Update (CommandInfo info)
		{
			MonoDevelop.Ide.Gui.Document doc = IdeApp.Workbench.ActiveDocument;
			info.Enabled = doc != null && doc.GetContent<ITextEditorDataProvider> () != null;
		}
	}
}