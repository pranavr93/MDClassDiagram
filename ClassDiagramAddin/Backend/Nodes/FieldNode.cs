using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class FieldNode
    {
        public string Name;
        public string Modifier;
        public string ReturnType;
		public FieldNode()
        {
			
        }
		public FieldNode(string Name, string Modifier, string ReturnType){
            this.Name = Name;
            this.Modifier = Modifier;
            this.ReturnType = ReturnType;
        }
    }
}
