using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
	public class PropertyNode
	{
		public string Name{get;set;}
		public string Modifier{get;set;}
		public string ReturnType{get;set;}
		public PropertyNode()
		{

		}
		public PropertyNode(string Name)
		{
			this.Name = Name;
		}
	}
}
