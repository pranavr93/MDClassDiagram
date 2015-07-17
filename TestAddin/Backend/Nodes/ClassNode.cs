using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class ClassNode : Node
    {
        //public string ClassName;

        //public List<Method> Methods;
        //public List<Field> Fields;
        //public List<String> Links;

		//public Type Type;
        //private bool inProject = true;

		public ClassNode(string ClassName) : base(ClassName)
        {
			this.Type = Type.Class;
        }

        public ClassNode() 
        {
            this.Type = Type.Class;
        }


    }
}
