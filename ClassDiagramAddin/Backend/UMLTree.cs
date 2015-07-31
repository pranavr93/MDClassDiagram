using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class UMLTree
    {
        private Dictionary<String, List<String> > Graph;
        private UMLClass diagram;
        private Dictionary<String, Node> Mapping;
        public UMLTree(UMLClass diagram)
        {
            this.diagram = diagram;
            Graph = new Dictionary<string,List<string>>();
            Mapping = new Dictionary<string,Node>();

            foreach (var classnode in diagram.ClassNodes){
				if(!Graph.ContainsKey(classnode.Name)){
					Graph.Add(classnode.Name, new List<String>());
					Mapping.Add(classnode.Name, classnode);
				}                 
            }               

            foreach (var interfacenode in diagram.InterfaceNodes){
				if(!Graph.ContainsKey(interfacenode.Name)){
					Graph.Add(interfacenode.Name, new List<String>());
					Mapping.Add(interfacenode.Name, interfacenode);
				}	                
            }
                

            foreach (var structnode in diagram.StructNodes){
				if (!Graph.ContainsKey (structnode.Name)) {
					Graph.Add(structnode.Name, new List<String>());
					Mapping.Add(structnode.Name, structnode);
				}               
			}              

            foreach (var enumnode in diagram.EnumNodes){
				if (!Graph.ContainsKey (enumnode.Name)) {
					Graph.Add(enumnode.Name, new List<String>());
					Mapping.Add(enumnode.Name, enumnode);
				}                
            }           

        }
        private void AddRelationship(Node from, string to)
        {
			Graph[from.Name].Add(to);
        }
        public void BuildGraph()
        {
            foreach(var node in diagram.ClassNodes)
            {
                foreach(var link in node.Links)
                {
                    //We do not add a link if the 'to' node is not present in the project
                    if(Graph.ContainsKey(link))
                    {
                        this.AddRelationship(node, link );
                    }
                }
            }
        }
        public Node GetNode(string name)
        {
            if (Mapping.ContainsKey(name))
                return Mapping[name];
            return null;
        }
    }
}
