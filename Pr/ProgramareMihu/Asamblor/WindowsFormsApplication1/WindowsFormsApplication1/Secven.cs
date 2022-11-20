using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Secven : Form
    {
        
        public Secven(List<String> AsmInstrList,List<String> MachineCodList)
        {
            InitializeComponent();
            List<String> RegisterList = GenerateRegisterList();
            PopulateAsmListBox(AsmInstrList);
            PopulateRegisterListBox(RegisterList);
            PopulateMachineCodListBox(MachineCodList);
            
        }

        

        private List<String> GenerateRegisterList()
        {
           List<String> RegisterList = new List<String>();
            for(int i=0;i<16;i++)
            {
                if (i < 10)
                {
                    RegisterList.Add("R" + i + "           " + "0");
                }
                else
                {
                    RegisterList.Add("R" + i + "         " + "0");
                }
            }
            return RegisterList;
        }

        private void PopulateAsmListBox(List<String> AsmInstrList)
        {
            listboxAsmInstr.DataSource = AsmInstrList;
        }

        private void PopulateRegisterListBox(List<string> RegisterList)
        {
            listBoxRegister.DataSource = RegisterList;
        }
        private void PopulateMachineCodListBox(List<string> MachineCodList)
        {
            listboxMachineCode.DataSource = MachineCodList;
        }
    }
}
