using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public abstract class Node
    {
		public string Namespace {get;set;}
		public string Name {get;set;}
		public Type Type {get;set;}
        public string FilePath {get;set;}
		public List<MethodNode> Methods;
		public List<FieldNode> Fields;
		public List<PropertyNode> Properties;
		public List<EventNode> Events;
		//public List<Node> AllNodes;

		public List<string> Links;
        public List<string> Implementations;


        public Node(string name) : this(){
			this.Name = name;
		}
		public Node(){
			this.Name = "Type Name";
			this.Methods = new List<MethodNode>();
			this.Fields = new List<FieldNode>();
			this.Properties = new List<PropertyNode>();
			this.Events = new List<EventNode>();
			this.Links = new List<string>();
            this.Implementations = new List<string>();
		}

    }
}
