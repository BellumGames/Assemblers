using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
   public  class FirstClassInstruction
    {
       public string Opcode { get; set; }
       public string Mas { get; set; }
       public string Rs { get; set; }
       public string Mad { get; set; }
       public string Rd { get; set; }
    public FirstClassInstruction()
       {
           this.Opcode = "0";
       }
   public FirstClassInstruction(string opcode,string mas,string rs,string mad,string rd)
    {
        this.Opcode = opcode;
        this.Mas = mas;
        this.Rs = rs;
        this.Mad = mad;
        this.Rd = rd;
    }

  }
}
