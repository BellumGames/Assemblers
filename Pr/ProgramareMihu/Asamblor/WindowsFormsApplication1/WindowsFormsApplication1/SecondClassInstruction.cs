using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
   public class SecondClassInstruction
    {
       
       public string Opcode { get; set; }
       public string Mad { get; set; }
       public string Rd { get; set; }
       public SecondClassInstruction()
       {
           this.Opcode = "101";
       }
   public SecondClassInstruction(string opcode,string mad,string rd)
    {
        this.Opcode = opcode;
        this.Mad = mad;
        this.Rd = rd;
    }
    }
}
