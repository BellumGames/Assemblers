using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel; 

namespace ConvertMicroBlabla
{
    public partial class Form1 : Form
    {
        Dictionary<string, string> SbusDictionary;
        Dictionary<string, string> DbusDictionary;
        Dictionary<string, string> AluDictionary;
        Dictionary<string, string> RbusDictionary;
        Dictionary<string, string> OtherDictionary;
        Dictionary<string, string> MemOpDictionary;
        Dictionary<string, string> SuccesorDictionary;
        Dictionary<string, string> TFDictionary;
        Dictionary<string, string> IndexDictionary;
        Dictionary<string, string> MicroAdresaDeSaltDictionary;
        List<string> Instr;
        public Form1()
        {
            InitializeComponent();
            SbusDictionary = new Dictionary<string, string>();
            DbusDictionary = new Dictionary<string, string>();
            AluDictionary = new Dictionary<string, string>();
            RbusDictionary = new Dictionary<string, string>();
            OtherDictionary = new Dictionary<string, string>();
            SuccesorDictionary = new Dictionary<string, string>();
            IndexDictionary = new Dictionary<string, string>();
            MemOpDictionary = new Dictionary<string, string>();
            TFDictionary = new Dictionary<string, string>();
            MicroAdresaDeSaltDictionary = new Dictionary<string, string>();
            Instr = new List<string>();
            GenerateDictionary();
        }

        private void GenerateDictionary()
        {
            GenerateSBus();
            GenerateDBus();
            GenerateAlu();
            GenerateRbus();
            GenerateOther();
            GenerateSuccesor();
            GenerateIndex();
            GenerateMemOP();
            GenerateTF();
            GenerateMicroAdresa();

        }

        private void GenerateMicroAdresa()
        {
            MicroAdresaDeSaltDictionary.Add("0", "00000000");
            MicroAdresaDeSaltDictionary.Add("IFCH", "00000001");
            MicroAdresaDeSaltDictionary.Add("B1", "00000010");
            MicroAdresaDeSaltDictionary.Add("B2", "00000011");
            MicroAdresaDeSaltDictionary.Add("B3", "00000100");
            MicroAdresaDeSaltDictionary.Add("B4", "00000101");
            MicroAdresaDeSaltDictionary.Add("IM_S", "00000110");
            MicroAdresaDeSaltDictionary.Add("RD_S", "00001000");
            MicroAdresaDeSaltDictionary.Add("RI_S", "00001010");
            MicroAdresaDeSaltDictionary.Add("RX_S", "00001100");
            MicroAdresaDeSaltDictionary.Add("IM_D", "00001111");
            MicroAdresaDeSaltDictionary.Add("RD_D", "00010001");
            MicroAdresaDeSaltDictionary.Add("RI_D", "00010011");
            MicroAdresaDeSaltDictionary.Add("RX_D", "00010101");
            MicroAdresaDeSaltDictionary.Add("C1", "00010111");
            MicroAdresaDeSaltDictionary.Add("C2", "00011000");
            MicroAdresaDeSaltDictionary.Add("MOV", "00011001");
            MicroAdresaDeSaltDictionary.Add("ADD", "00011011");
            MicroAdresaDeSaltDictionary.Add("SUB", "00011101");
            MicroAdresaDeSaltDictionary.Add("CMP", "00011111");
            MicroAdresaDeSaltDictionary.Add("AND", "00100001");
            MicroAdresaDeSaltDictionary.Add("OR", "00100011");
            MicroAdresaDeSaltDictionary.Add("XOR", "00100101");
            MicroAdresaDeSaltDictionary.Add("CLR", "00100111");
            MicroAdresaDeSaltDictionary.Add("NEG", "00101001");
            MicroAdresaDeSaltDictionary.Add("INC", "00101011");
            MicroAdresaDeSaltDictionary.Add("DEC", "00101101");
            MicroAdresaDeSaltDictionary.Add("ASL", "00101111");
            MicroAdresaDeSaltDictionary.Add("ASR", "00110001");
            MicroAdresaDeSaltDictionary.Add("LSR", "00110011");
            MicroAdresaDeSaltDictionary.Add("ROL", "00110101");
            MicroAdresaDeSaltDictionary.Add("ROR", "00110111");
            MicroAdresaDeSaltDictionary.Add("RLC", "00111001");
            MicroAdresaDeSaltDictionary.Add("RRC", "00110011");
            MicroAdresaDeSaltDictionary.Add("JMP", "00111101");
            MicroAdresaDeSaltDictionary.Add("PUSH RG", "00111111");
            MicroAdresaDeSaltDictionary.Add("POP RG",  "01000001");
            MicroAdresaDeSaltDictionary.Add("CALL", "01000011");
            MicroAdresaDeSaltDictionary.Add("BR", "01000111");
            MicroAdresaDeSaltDictionary.Add("BNE", "01001001");
            MicroAdresaDeSaltDictionary.Add("BEQ", "01001011");
            MicroAdresaDeSaltDictionary.Add("BPL", "01001101");
            MicroAdresaDeSaltDictionary.Add("BMI", "01001111");
            MicroAdresaDeSaltDictionary.Add("BCS", "01010001");
            MicroAdresaDeSaltDictionary.Add("BCC", "01010011");
            MicroAdresaDeSaltDictionary.Add("BVS", "01010101");
            MicroAdresaDeSaltDictionary.Add("BVC", "01010111");
            MicroAdresaDeSaltDictionary.Add("CLC", "01011001");
            MicroAdresaDeSaltDictionary.Add("CLV", "01011101");
            MicroAdresaDeSaltDictionary.Add("CLZ", "01100001");
            MicroAdresaDeSaltDictionary.Add("CLS", "01100101");
            MicroAdresaDeSaltDictionary.Add("CCC", "01101001");
            MicroAdresaDeSaltDictionary.Add("SEC", "01101101");
            MicroAdresaDeSaltDictionary.Add("SEV", "01110001");
            MicroAdresaDeSaltDictionary.Add("SEZ", "01110101");
            MicroAdresaDeSaltDictionary.Add("SES", "01111001");
            MicroAdresaDeSaltDictionary.Add("SCC", "01111101");
            MicroAdresaDeSaltDictionary.Add("NOP", "10000001");
            MicroAdresaDeSaltDictionary.Add("RET", "10000101");
            MicroAdresaDeSaltDictionary.Add("HALT", "10001001");
            MicroAdresaDeSaltDictionary.Add("WAIT", "10001101");
            MicroAdresaDeSaltDictionary.Add("PUSH PC", "10010001");
            MicroAdresaDeSaltDictionary.Add("POP PC", "10010101");
            MicroAdresaDeSaltDictionary.Add("PUSH FLAG", "10011001");
            MicroAdresaDeSaltDictionary.Add("POP FLAG", "10011101");
            MicroAdresaDeSaltDictionary.Add("RETI", "10100001");
            MicroAdresaDeSaltDictionary.Add("INT", "10100101");

        }

        private void GenerateTF()
        {
            TFDictionary.Add("F", "0");
            TFDictionary.Add("T", "1");
        }

        private void GenerateMemOP()
        {
            MemOpDictionary.Add("NOP", "00");
            MemOpDictionary.Add("IFCH", "01");
            MemOpDictionary.Add("READ", "10");
            MemOpDictionary.Add("WRITE", "11");
        }

        private void GenerateIndex()
        {
            IndexDictionary.Add("INDEX0", "000");
            IndexDictionary.Add("INDEX1", "001");
            IndexDictionary.Add("INDEX2", "010");
            IndexDictionary.Add("INDEX3", "011");
            IndexDictionary.Add("INDEX4", "100");
            IndexDictionary.Add("INDEX5", "101");
            IndexDictionary.Add("INDEX6", "110");
            IndexDictionary.Add("INDEX7", "111");
        }

        private void GenerateSuccesor()
        {
            SuccesorDictionary.Add("STEP", "00000");
            SuccesorDictionary.Add("JUMPI", "00001");
            SuccesorDictionary.Add("IF INT JUMPI ELSE STEP", "00010");
            SuccesorDictionary.Add("IF ACLOW JUMPI", "00011");
            SuccesorDictionary.Add("IF CIL JUMPI ELSE STEP", "00100");
            SuccesorDictionary.Add("IF C JUMPI ELSE STEP", "00101");
            SuccesorDictionary.Add("IF Z JUMPI ELSE STEP", "00110");
            SuccesorDictionary.Add("IF S JUMPI ELSE STEP", "00111");
            SuccesorDictionary.Add("IF V JUMPI ELSE STEP", "01000");
            SuccesorDictionary.Add("IF CLASA B2 JUMPI ELSE STEP", "01001");
            SuccesorDictionary.Add("IF CLASA B3 JUMPI ELSE STEP", "01010");
            SuccesorDictionary.Add("IF CLASA B4 JUMPI ELSE STEP", "01011");
        }

        private void GenerateOther()
        {
            OtherDictionary.Add("NONE", "00000");
            OtherDictionary.Add("(Cin,PdCOND)", "00001");
            OtherDictionary.Add("PdCOND", "00010");
            OtherDictionary.Add("Cin", "00011");
            OtherDictionary.Add("+2SP", "00100");
            OtherDictionary.Add("-2SP", "00101");
            OtherDictionary.Add("+2PC", "00110");
            OtherDictionary.Add("A(1)BVI", "00111");
            OtherDictionary.Add("A(0)BVI", "01000");
            OtherDictionary.Add("A(1)BE0", "01001");
            OtherDictionary.Add("A(1)BE1", "01010");
            OtherDictionary.Add("(A(0)BI,A(0)BE)", "01011");
            OtherDictionary.Add("A(0)BPO", "01100");
            OtherDictionary.Add("A(1)C", "01101");
            OtherDictionary.Add("A(1)V", "01110");
            OtherDictionary.Add("A(1)Z", "01111");
            OtherDictionary.Add("A(1)S", "10000");
            OtherDictionary.Add("A(1)(CVZS)", "10001");
            OtherDictionary.Add("A(0)C", "10010");
            OtherDictionary.Add("A(0)V", "10011");
            OtherDictionary.Add("A(0)Z", "10100");
            OtherDictionary.Add("A(0)S", "10101");
            OtherDictionary.Add("A(0)(CVZS)", "10110");
        }

        private void GenerateRbus()
        {
            RbusDictionary.Add("NONE", "0000");
            RbusDictionary.Add("PmFLAG", "0001");
            RbusDictionary.Add("PmRG", "0010");
            RbusDictionary.Add("PmSP", "0011");
            RbusDictionary.Add("PmT", "0100");
            RbusDictionary.Add("PmPC", "0101");
            RbusDictionary.Add("PmIVR", "0110");
            RbusDictionary.Add("PmADR", "0111");
            RbusDictionary.Add("PmMDR", "1000");
        }

        private void GenerateAlu()
        {
            AluDictionary.Add("SBUS", "00000");
            AluDictionary.Add("!SBUS", "00001");
            AluDictionary.Add("DBUS", "00010");
            AluDictionary.Add("!DBUS", "00011");
            AluDictionary.Add("SUM", "00100");
            AluDictionary.Add("AND", "00101");
            AluDictionary.Add("OR", "00110");
            AluDictionary.Add("XOR", "00111");
            AluDictionary.Add("INV", "01000");
            AluDictionary.Add("ASL", "01001");
            AluDictionary.Add("ASR", "01010");
            AluDictionary.Add("LSR", "01011");
            AluDictionary.Add("ROL", "01100");
            AluDictionary.Add("ROR", "01101");
            AluDictionary.Add("SUB", "01110");
            AluDictionary.Add("NONE", "01111");
        }

        private void GenerateSBus()
        {
            SbusDictionary.Add("NONE", "0000");
            SbusDictionary.Add("PdFLAGs", "0001");
            SbusDictionary.Add("PdRGs", "0010");
            SbusDictionary.Add("PdSPs", "0011");
            SbusDictionary.Add("PdTs", "0100");
            SbusDictionary.Add("!PdTs", "0101");
            SbusDictionary.Add("PdPCs", "0110");
            SbusDictionary.Add("PdIVRs", "0111");
            SbusDictionary.Add("PdADRs", "1000");
            SbusDictionary.Add("PdMDRs", "1001");
            SbusDictionary.Add("PdIRs", "1010");
            SbusDictionary.Add("Pd0s", "1011");
            SbusDictionary.Add("Pd1s", "1100");
        }

        private void GenerateDBus()
        {
            DbusDictionary.Add("NONE",   "0000");
            DbusDictionary.Add("PdFLAGd", "0001");
            DbusDictionary.Add("PdRGd", "0010");
            DbusDictionary.Add("PdSPd", "0011");
            DbusDictionary.Add("PdTd", "0100");
            DbusDictionary.Add("PdPCd", "0101");
            DbusDictionary.Add("PdIVRd", "0110");
            DbusDictionary.Add("PdADRd", "0111");
            DbusDictionary.Add("PdMDRd", "1000");
            DbusDictionary.Add("!PdMDRd", "1001");
            DbusDictionary.Add("PdIRd", "1010");
            DbusDictionary.Add("Pd0d", "1011");
            DbusDictionary.Add("Pd1d", "1100");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Range range;

            string str;
            int rCnt = 0;
            int cCnt = 0;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open("E:\\Dropbox\\Proiect AC\\Test.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            range = xlWorkSheet.UsedRange;

            for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
            {
                string temporaryString = string.Empty;
                string temporaryfull = string.Empty;
                temporaryString = (string)(range.Cells[rCnt, 2] as Excel.Range).Value2;
                temporaryfull += SbusDictionary[temporaryString];
                temporaryString = (string)(range.Cells[rCnt, 3] as Excel.Range).Value2;
                temporaryfull += DbusDictionary[temporaryString];
                temporaryString = (string)(range.Cells[rCnt, 4] as Excel.Range).Value2;
                temporaryfull += AluDictionary[temporaryString];
                temporaryString = (string)(range.Cells[rCnt, 5] as Excel.Range).Value2;
                temporaryfull += RbusDictionary[temporaryString];
                temporaryString = (string)(range.Cells[rCnt, 6] as Excel.Range).Value2;
                temporaryfull += OtherDictionary[temporaryString];
                temporaryString = (string)(range.Cells[rCnt, 7] as Excel.Range).Value2;
                temporaryfull += MemOpDictionary[temporaryString];
               temporaryString = (string)(range.Cells[rCnt, 8] as Excel.Range).Value2;
                temporaryfull += SuccesorDictionary[temporaryString];
                temporaryString = (string)(range.Cells[rCnt, 9] as Excel.Range).Value2;
                temporaryfull += TFDictionary[temporaryString];
                temporaryString = (string)(range.Cells[rCnt, 10] as Excel.Range).Value2;
                temporaryfull += IndexDictionary[temporaryString];
                temporaryString = (string)(range.Cells[rCnt, 11] as Excel.Range).Value2;
                temporaryfull += MicroAdresaDeSaltDictionary[temporaryString];
                Instr.Add(temporaryfull.PadLeft(64,'0'));
            }

            xlWorkBook.Close(true, null, null);
            xlApp.Quit();

            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);

            MessageBox.Show( "conversion finished","info");

            writeBinaryFile("microcodBinary", Instr);
            
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Unable to release the Object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
        private void writeBinaryFile(String filename, List<string> instructions ){

            String FilePath = "E:\\Dropbox\\Proiect AC\\ProgramareMihu\\iQuest\\ConvertMicroBlabla\\" + filename + ".bin";
            if (!File.Exists(FilePath))
            {
                FileStream fs = new FileStream(FilePath, FileMode.CreateNew);
                BinaryWriter binaryfile = new System.IO.BinaryWriter(fs);
                foreach (string s in Instr) 
                    binaryfile.Write(s + Environment.NewLine);

                binaryfile.Close();
            }
            else
            {
                MessageBox.Show("File already exists!");
            }
        }
    }
}
