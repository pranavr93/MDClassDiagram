using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class StructNode : Node
    {

		public StructNode(string structname) : base(structname)
        {
			this.Type = Type.Struct;
        }
		public StructNode() :base()
        {
			this.Type = Type.Struct;
        }

    }
}
