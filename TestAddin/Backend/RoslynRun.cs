using System;
using System.Linq;
using System.Windows;
using System.IO;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;


using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Backend
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    /// 


    public class RoslynRun
    {
        private static IEnumerable<FieldNode> GetFieldNodes(FieldDeclarationSyntax fd)
        {
            // For each variable in the field
            foreach (var variable in fd.Declaration.Variables)
            {
				FieldNode field 	= 	new FieldNode();
                field.Modifier 		= 	fd.Modifiers.ToString();
                field.Name 			= 	variable.Identifier.ToString();
                field.ReturnType 	= 	fd.Declaration.Type.ToString();

                yield return field;
            }
        }
        private static MethodNode GetMethodNode(MethodDeclarationSyntax method)
        {
            MethodNode methodnode 	= new MethodNode();
            methodnode.ReturnType 	= method.ReturnType.ToString();
            methodnode.Modifier 	= method.Modifiers.ToString();
            methodnode.Name 		= method.Identifier.ToString();

            foreach (var parameter in method.ParameterList.Parameters)
            {
                Parameter p = new Parameter();
                p.name = parameter.Identifier.ToString();
                p.type = parameter.Type.ToString();
                methodnode.Parameters.Add(p);
            }
            return methodnode;
        }
		private static PropertyNode GetPropertyNode(PropertyDeclarationSyntax property)
		{
			PropertyNode propertynode 	= new PropertyNode();
			propertynode.Name 			= property.Identifier.ToString();
			propertynode.Modifier 		= property.Modifiers.ToString();
			propertynode.ReturnType 	= property.Type.ToString();
			return propertynode;
		}
        private static ClassNode GetClassNode(ClassDeclarationSyntax EachClass)
        {
            ClassNode classnode = new ClassNode();
            classnode.Name = EachClass.Identifier.ToString();

            // For each member in that class
            foreach (var member in EachClass.Members)
            {
                if (member is FieldDeclarationSyntax)
                {
                    FieldDeclarationSyntax fd = member as FieldDeclarationSyntax;

                    foreach (var field in GetFieldNodes(fd))
                    {
                        classnode.Fields.Add(field);
                    }
                }
                else if (member is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax method = member as MethodDeclarationSyntax;
                    classnode.Methods.Add(GetMethodNode(method));
                }
				else if (member is PropertyDeclarationSyntax)
				{
					PropertyDeclarationSyntax property = member as PropertyDeclarationSyntax;
					classnode.Properties.Add(GetPropertyNode(property));
				}
				else if (member is EventDeclarationSyntax)
				{
					//TODO
				}
            }

            if (EachClass.BaseList != null)
            {
				var res = model.GetDeclaredSymbol(EachClass);
				var baseType = res.BaseType.ToString();
                if(!baseType.Equals("object")){
                    classnode.Links.Add(baseType);
                }
                foreach(var item in res.Interfaces)
                {
                    classnode.Links.Add(item.ToString());
                }
            }
            return classnode;
        }
        private static StructNode GetStructNode(StructDeclarationSyntax EachStruct)
        {
            StructNode structnode = new StructNode();
			structnode.Name = EachStruct.Identifier.ToString();
            // For each member in that class
            foreach (var member in EachStruct.Members)
            {
                if (member is FieldDeclarationSyntax)
                {
                    FieldDeclarationSyntax fd = member as FieldDeclarationSyntax;

                    foreach (var field in GetFieldNodes(fd))
                    {
                        structnode.Fields.Add(field);
                    }
                }
                else if (member is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax method = member as MethodDeclarationSyntax;
                    structnode.Methods.Add(GetMethodNode(method));
                }
            }
            if (EachStruct.BaseList != null)
            {
                foreach (var baseType in EachStruct.BaseList.Types)
                {
                    structnode.Links.Add(baseType.ToString());
                }
            }
            return structnode;
        }
        private static InterfaceNode GetInterfaceNode(InterfaceDeclarationSyntax EachInterface)
        {
            InterfaceNode interfacenode = new InterfaceNode();
			interfacenode.Name = EachInterface.Identifier.ToString();
            foreach (var member in EachInterface.Members)
            {
                MethodDeclarationSyntax method = member as MethodDeclarationSyntax;
                interfacenode.Methods.Add(GetMethodNode(method));
            }
            //if (EachInterface.BaseList != null)
            //{
            //    foreach (var baseType in EachInterface.BaseList.Types)
            //    {
            //        interfacenode.Links.Add(baseType.ToString());
            //    }
            //}
            if (EachInterface.BaseList != null)
            {
                var res = model.GetDeclaredSymbol(EachInterface);

                foreach(var item in res.Interfaces)
                {
                    interfacenode.Links.Add(item.ToString());
                }
            }
            return interfacenode;
        }

        private static EnumNode GetEnumNode(EnumDeclarationSyntax EachEnum)
        {
            EnumNode enumnode = new EnumNode();
            enumnode.Name = EachEnum.Identifier.ToString();
            foreach (var member in EachEnum.Members)
            {
                //Console.WriteLine(member.Identifier.ToString());
                //enumnode.AddMember(member.Identifier.ToString());
                //enumnode.Fields.Add(member.Identifier.ToString());
            }


            return enumnode;
        }
		public static UMLClass ParseFiles(Compilation compilation)
        {

			RoslynRun.compilation = compilation;
            //List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            //foreach (var file in solution.AllFiles)
            //{
            //    syntaxTrees.Add(CSharpSyntaxTree.ParseText(file.OriginalText));
            //}
			
            //var references = new MetadataReference[]
            //{
            //        MetadataReference.CreateFromAssembly(typeof(object).Assembly),
            //        MetadataReference.CreateFromAssembly(typeof(System.IO.File).Assembly),
            //        MetadataReference.CreateFromAssembly(typeof(System.String).Assembly),
            //        MetadataReference.CreateFromAssembly(typeof(System.Linq.Enumerable).Assembly),
            //};

            //var compilation = CSharpCompilation.Create("temporary",
            //                                             syntaxTrees,
            //                                            references);
            //var diagnostics = compilation.GetDiagnostics();

			foreach (var item in compilation.GetDiagnostics())
            {
                if (item.Severity == DiagnosticSeverity.Error)
                {
                    Console.WriteLine("Code has compile time errors. Kindly fix them");
					Console.WriteLine(item.GetMessage());
					Console.WriteLine(item.ToString());
					//Console.WriteLine(item.Descriptor.ToString());
					Console.WriteLine(item.Location.ToString());
					//Console.WriteLine(item.
                    //return new UMLClass();
                }
            }
			Console.WriteLine(".\n.\n.\n.");
			var syntaxTrees = compilation.SyntaxTrees;
			foreach(var syntaxTree in syntaxTrees)
			{
				models.Add(compilation.GetSemanticModel(syntaxTree));
			}
            UMLClass uml = new UMLClass();
            foreach (var st in syntaxTrees)
            {
				var AllClasses = st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
				model = compilation.GetSemanticModel(st);

                //For each class in file
                foreach (var EachClass in AllClasses)
                {
					ClassNode c = GetClassNode(EachClass);
					c.Namespace = model.GetDeclaredSymbol(EachClass).ToString();
                    Console.Write(c.Namespace);
                    foreach(var item in c.Links){
                        Console.Write(item + " ");
                    }
                    Console.WriteLine();
					//c.Namespace = model.GetTypeInfo(EachClass).Type.ContainingNamespace.ToString();
                    uml.ClassNodes.Add(c);
                }

                //For each struct in file
                var AllStructs = st.GetRoot().DescendantNodes().OfType<StructDeclarationSyntax>();
                foreach (var EachStruct in AllStructs)
                {
                    uml.StructNodes.Add(GetStructNode(EachStruct));
                }

                // For reach interface in file
                var AllInterfaces = st.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>();
                foreach (var EachInterface in AllInterfaces)
                {
                    InterfaceNode inf = GetInterfaceNode(EachInterface);
                    inf.Namespace = model.GetDeclaredSymbol(EachInterface).ToString();
                    uml.InterfaceNodes.Add(inf);

                }

                //For each enum in file
                var AllEnums = st.GetRoot().DescendantNodes().OfType<EnumDeclarationSyntax>();
                foreach (var EachEnum in AllEnums)
                {
                    uml.EnumNodes.Add(GetEnumNode(EachEnum));
                }
            }
			Console.WriteLine(".\n.\n.\n.");
            return uml;
        }


		private static SemanticModel model;
		private static Compilation compilation;
		private static List<SemanticModel> models = new List<SemanticModel>();
        //public UMLClass AnalyzeCode(Solution solution)
        //{
        //    // string path = @"D:\gsoc related\Roslyn\RoslynExperiments\RoslynExperiments\TestCode";
        //    //string path = @"D:\gsoc related\Roslyn\RoslynExperiments\RoslynExperiments.sln";
            
        //    // var workspace = MSBuildWorkspace.Create().OpenSolutionAsync(path);
        //    //string[] files = Directory.GetFiles(path);
        //    //List<string> fileContent = new List<string>();
        //    //foreach (string fileName in files)
        //    //{
        //    //    //if (Path.GetExtension(fileName) == ".cs")
        //    //    //{
        //    //    //    string readText = File.ReadAllText(fileName);
        //    //    //    //   Console.WriteLine("Examining file " + fileName + "\n");
        //    //    //    fileContent.Add(readText);
        //    //    //    // ParseFiles(readText);
        //    //    //    // Console.Write("\n\n\n\n");
        //    //    //}
        //    //}
        //     return ParseFiles(solution);
        //    //return ParseFiles(fileContent);
        //  //  Console.ReadLine();
        //}
    }

}