using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
   public class ThirdClassInstruction
    {
       public string Opcode { get; set; }
       public string OffSet { get; set; }

   public ThirdClassInstruction()
       {
           this.Opcode = "110";
       }
   public ThirdClassInstruction(string opcode,string offset)
    {
        this.Opcode = opcode;
        this.OffSet = offset;
    }
    }
}
