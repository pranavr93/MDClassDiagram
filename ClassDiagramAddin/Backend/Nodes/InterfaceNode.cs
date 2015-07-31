using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class InterfaceNode : Node
    {
		public InterfaceNode() : base()
        {
			this.Type = Type.Interface;
        }
		public InterfaceNode(string interfacename) : base(interfacename)
		{
			this.Type = Type.Interface;
		}
    }
}
