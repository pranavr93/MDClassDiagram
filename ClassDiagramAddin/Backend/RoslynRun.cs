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
        #region MemberGetters
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
		/// <summary>
        /// /*Gets the property details from the syntax.*/
        /// </summary>
        /// <returns>The property node.</returns>
        /// <param name="property">Property.</param>
        private static PropertyNode GetPropertyNode(PropertyDeclarationSyntax property)
		{
			PropertyNode propertynode 	= new PropertyNode();
			propertynode.Name 			= property.Identifier.ToString();
			propertynode.Modifier 		= property.Modifiers.ToString();
			propertynode.ReturnType 	= property.Type.ToString();

			return propertynode;
		}

        private static EventNode GetEventNode(EventFieldDeclarationSyntax evnt)
        {
            EventNode eventnode = new EventNode();
            eventnode.Name      = evnt.EventKeyword.ToString();
            eventnode.Modifier  = evnt.Modifiers.ToString();
            //Console.WriteLine(eventnode.Name);
            //Console.WriteLine(eventnode.Modifier);
            return eventnode;
        }

        #endregion MemberGetters

        #region TypeGetters
        private static ClassNode GetClassNode(ClassDeclarationSyntax EachClass)
        {
            ClassNode classnode = new ClassNode();
            classnode.Name = EachClass.Identifier.ToString();

            // For each member in that class
            foreach (var member in EachClass.Members)
            {
                //Console.WriteLine(member.ToFullString());
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
                else if (member is EventFieldDeclarationSyntax)
				{
                    //Console.WriteLine("Event found");
                    EventFieldDeclarationSyntax evnt = member as EventFieldDeclarationSyntax; 
                    classnode.Events.Add(GetEventNode(evnt));
				}
            }

            if (EachClass.BaseList != null)
            {
                // We make use of the semantic model as it is required to get the full namespace
                // of the class/interface else there would be problems resolving links in the 
                // class diagram.

				var res = model.GetDeclaredSymbol(EachClass);
				var baseType = res.BaseType.ToString();

                if(!baseType.Equals("object")){
                    // Only one inheritance possible.
                    classnode.Links.Add(baseType);
                }
                foreach(var item in res.Interfaces)
                {
                    //Add all interfaces that the class implements
                    classnode.Implementations.Add(item.ToString());
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
                else if (member is PropertyDeclarationSyntax)
                {
                    PropertyDeclarationSyntax property = member as PropertyDeclarationSyntax;
                    structnode.Properties.Add(GetPropertyNode(property));
                }
                else if (member is EventFieldDeclarationSyntax)
                {
                    //Console.WriteLine("Event found");
                    EventFieldDeclarationSyntax evnt = member as EventFieldDeclarationSyntax; 
                    structnode.Events.Add(GetEventNode(evnt));
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
                if (member is MethodDeclarationSyntax)
                {
                    MethodDeclarationSyntax method = member as MethodDeclarationSyntax;
                    interfacenode.Methods.Add(GetMethodNode(method));
                }
                else if (member is PropertyDeclarationSyntax)
                {
                    PropertyDeclarationSyntax property = member as PropertyDeclarationSyntax;
                    interfacenode.Properties.Add(GetPropertyNode(property));
                }
                else if (member is EventFieldDeclarationSyntax)
                {
                    //Console.WriteLine("Event found");
                    EventFieldDeclarationSyntax evnt = member as EventFieldDeclarationSyntax; 
                    interfacenode.Events.Add(GetEventNode(evnt));
                }
            }
            if (EachInterface.BaseList != null)
            {
                var res = model.GetDeclaredSymbol(EachInterface);
                foreach(var item in res.Interfaces)
                {
                    interfacenode.Implementations.Add(item.ToString());
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
                FieldNode f = new FieldNode();
                f.Name = member.Identifier.ToString();
                enumnode.Fields.Add(f);
                //Console.WriteLine(member.Identifier.ToString());
                //enumnode.AddMember(member.Identifier.ToString());
                //enumnode.Fields.Add(member.Identifier.ToString());
            }


            return enumnode;
        }
		
        #endregion TypeGetters

        public static UMLClass ParseFiles(Compilation compilation)
        {
			foreach (var item in compilation.GetDiagnostics())
            {
                if (item.Severity == DiagnosticSeverity.Error)
                {
                    Console.WriteLine("Code has compile time errors. Kindly fix them");
					Console.WriteLine(item.GetMessage());
					Console.WriteLine(item.ToString());
					Console.WriteLine(item.Location.ToString());

                }
            }
			
            var syntaxTrees = compilation.SyntaxTrees;
			foreach(var syntaxTree in syntaxTrees)
			{
				models.Add(compilation.GetSemanticModel(syntaxTree));
			}

            UMLClass uml = new UMLClass();
            // Each file in the project
            foreach (var st in syntaxTrees)
            {
                // Get semantic model for this file for getting info like class namespaces
				model = compilation.GetSemanticModel(st);

                //For each class in file
                var AllClasses = st.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>();
                foreach (var EachClass in AllClasses)
                {
					ClassNode c = GetClassNode(EachClass);
					c.Namespace = model.GetDeclaredSymbol(EachClass).ToString();
                    c.FilePath = st.FilePath;
                    uml.ClassNodes.Add(c);
                }

                //For each struct in file
                var AllStructs = st.GetRoot().DescendantNodes().OfType<StructDeclarationSyntax>();
                foreach (var EachStruct in AllStructs)
                {
                    StructNode snode = GetStructNode(EachStruct);
                    snode.Namespace = model.GetDeclaredSymbol(EachStruct).ToString();
                    snode.FilePath = st.FilePath;
                    uml.StructNodes.Add(snode);
                }

                // For reach interface in file
                var AllInterfaces = st.GetRoot().DescendantNodes().OfType<InterfaceDeclarationSyntax>();
                foreach (var EachInterface in AllInterfaces)
                {
                    InterfaceNode inf = GetInterfaceNode(EachInterface);
                    inf.Namespace = model.GetDeclaredSymbol(EachInterface).ToString();
                    inf.FilePath = st.FilePath;
                    uml.InterfaceNodes.Add(inf);

                }

                //For each enum in file
                var AllEnums = st.GetRoot().DescendantNodes().OfType<EnumDeclarationSyntax>();
                foreach (var EachEnum in AllEnums)
                {
                    EnumNode enode = GetEnumNode(EachEnum);
                    enode.Namespace = model.GetDeclaredSymbol(EachEnum).ToString();
                    enode.FilePath = st.FilePath;
                    uml.EnumNodes.Add(enode);
                }
            }
			Console.WriteLine(".\n.\n.\n.");
            return uml;
        }


		private static SemanticModel model;

		private static List<SemanticModel> models = new List<SemanticModel>();
 
    }

}