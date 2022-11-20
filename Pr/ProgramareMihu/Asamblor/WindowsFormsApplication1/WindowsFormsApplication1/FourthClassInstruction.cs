using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class FourthClassInstruction
    {
       public string Opcode { get; set; }
    
   public FourthClassInstruction()
       {
           this.Opcode = "111";
       }
   public FourthClassInstruction(string opcode)
    {
        this.Opcode = opcode;
    }
    }
    
}
