using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Backend
{
	public class EnumNode : Node
    {
        //public string EnumName;
        public List<String> Members;
        //private bool inProject;
		//public Type Type;
        public EnumNode()
        {
            Members = new List<String>();
			this.Type = Type.Enum;
        }
		public EnumNode(string EnumName) : base(EnumName)
		{
			Members = new List<string>();
			this.Type = Type.Enum;
		}
        public void AddMember(string member)
        {
            Members.Add(member);
        }
    }
}
