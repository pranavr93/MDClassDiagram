using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class MethodNode
    {
        public string Name;
        public string Modifier;
        public string ReturnType;
        public List<Parameter> Parameters;
        public MethodNode(string Name, string Modifier, string ReturnType){
            this.Name = Name;
            this.Modifier = Modifier;
            this.ReturnType = ReturnType;
            Parameters = new List<Parameter>();
        }
        public MethodNode()
        {
            Parameters = new List<Parameter>();
        }

    }
    public class Parameter
    {
        public string name;
        public string type;
    }
}
