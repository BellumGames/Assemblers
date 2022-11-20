using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace WindowsFormsApplication1
{
    public partial class InitializeProcessor : Form
    {
        Dictionary<string,string> MasDictionary;
        Dictionary<string,string> MadDictionary;
        Dictionary<string,string> RegisterDictionary;
        Dictionary<string,string> FirstClassInstrDictionary;
        Dictionary<string,string> SecondClassInstrDictionary;
        Dictionary<string,string> ThirdClassInstrDictionary;
        Dictionary<string,string> FourthClassInstrDictionary;
        Dictionary<string, string> LabelDictionary;
        FirstClassInstruction FirstClassInstruction;
        SecondClassInstruction SecondClassInstruction;
        ThirdClassInstruction ThirdClassInstruction;
        FourthClassInstruction FourthClassInstruction;
        private List<string> InstrList;
        private List<string> LineInstruction;
        private List<string> MachineCodList;
        private string possibleExtra;
        private string FullInstruction;
        ushort FullInstructionBinary;
        private string GlobalInstruction;
        private int PC;
        public InitializeProcessor()
        {
            InitializeComponent();
            InstrList = new List<string>();
            LineInstruction = new List<string>();
            MachineCodList = new List<string>();
            GenerateDictionary();
            PopulateDictionary();
            FullInstruction = "";
            GlobalInstruction = "";
            FullInstructionBinary = 0;
            LabelDictionary = new Dictionary<string, string>();
            PC = 0;
            
        }
        private void GenerateDictionary()
         {
            MasDictionary = new Dictionary<string, string>();
            MadDictionary = new Dictionary<string, string>();
            RegisterDictionary = new Dictionary<string, string>();
            GenerateClassDictionary();
        }
        private void GenerateClassDictionary()
        {
            FirstClassInstrDictionary = new Dictionary<string, string>();
            SecondClassInstrDictionary = new Dictionary<string, string>();
            ThirdClassInstrDictionary = new Dictionary<string, string>();
            FourthClassInstrDictionary = new Dictionary<string, string>();
        }
        private void  PopulateDictionary ()
        {
            PopulateMasDictionary();
            PopulateMadDictionary();
            PopulateRegisterDictionary();
            PopulateFirstClassInstrDictionary();
            PopulateSecondClassInstrDictionary();
            PopulareThirdClassInstrDictionary();
            PopulateFourthClassInstrDictionary();

        }
        private void PopulateMasDictionary()
        {
            MasDictionary.Add("AM", "00");
            MasDictionary.Add("AD", "01");
            MasDictionary.Add("AI", "10");
            MasDictionary.Add("AX", "11");
        }
        private void PopulateMadDictionary()
        {
            RegisterDictionary.Add("R0", "0000");
            RegisterDictionary.Add("R1", "0001");
            RegisterDictionary.Add("R2", "0010");
            RegisterDictionary.Add("R3", "0011");
            RegisterDictionary.Add("R4", "0100");
            RegisterDictionary.Add("R5", "0101");
            RegisterDictionary.Add("R6", "0110");
            RegisterDictionary.Add("R7", "0111");
            RegisterDictionary.Add("R8", "1000");
            RegisterDictionary.Add("R9", "1001"); 
            RegisterDictionary.Add("R10", "1010");
            RegisterDictionary.Add("R11", "1011");
            RegisterDictionary.Add("R12", "1100");
            RegisterDictionary.Add("R13", "1101");
            RegisterDictionary.Add("R14", "1110");
            RegisterDictionary.Add("R15", "1111");
           
        }
        private void PopulateRegisterDictionary()
        {
            MadDictionary.Add("AM", "00");
            MadDictionary.Add("AD", "01");
            MadDictionary.Add("AI", "10");
            MadDictionary.Add("AX", "11");
        }
        private void PopulateFirstClassInstrDictionary()
        {
            FirstClassInstrDictionary.Add("mov", "000");
            FirstClassInstrDictionary.Add("add", "001");
            FirstClassInstrDictionary.Add("sub", "010");
            FirstClassInstrDictionary.Add("cmp", "011");
            FirstClassInstrDictionary.Add("and", "100");
            FirstClassInstrDictionary.Add("or", " 101");
            FirstClassInstrDictionary.Add("xor", "110");
        }
        private void PopulateSecondClassInstrDictionary()
        {
            SecondClassInstrDictionary.Add("clr", "0000000");
            SecondClassInstrDictionary.Add("neg", "0000001");
            SecondClassInstrDictionary.Add("inc", "0000010");
            SecondClassInstrDictionary.Add("dec", "0000011");
            SecondClassInstrDictionary.Add("asl", "0000100");
            SecondClassInstrDictionary.Add("asr", "0000101");
            SecondClassInstrDictionary.Add("lsr", "0000110");
            SecondClassInstrDictionary.Add("rol", "0000111");
            SecondClassInstrDictionary.Add("ror", "0001000");
            SecondClassInstrDictionary.Add("rlc", "0001001");
            SecondClassInstrDictionary.Add("rrc", "0001010");
            SecondClassInstrDictionary.Add("jmp", "0001011");
            SecondClassInstrDictionary.Add("call", "0001100");
            SecondClassInstrDictionary.Add("push", "0001101");
            SecondClassInstrDictionary.Add("pop", "0001110");
        }
        private void PopulareThirdClassInstrDictionary()
        {
            ThirdClassInstrDictionary.Add("br", "00000");
            ThirdClassInstrDictionary.Add("bne", "00001");
            ThirdClassInstrDictionary.Add("beq", "00010");
            ThirdClassInstrDictionary.Add("bpl", "00011");
            ThirdClassInstrDictionary.Add("bmi", "00100");
            ThirdClassInstrDictionary.Add("bcs", "00101");
            ThirdClassInstrDictionary.Add("bcc", "00110");
            ThirdClassInstrDictionary.Add("bvs", "00111");
            ThirdClassInstrDictionary.Add("bvc", "01000");
        }
        private void PopulateFourthClassInstrDictionary()
        {
            FourthClassInstrDictionary.Add("clc","0000000000000");
            FourthClassInstrDictionary.Add("clv","0000000000001");
            FourthClassInstrDictionary.Add("clz", "0000000000010");
            FourthClassInstrDictionary.Add("cls", "0000000000011");
            FourthClassInstrDictionary.Add("ccc", "0000000000100");
            FourthClassInstrDictionary.Add("sec", "0000000000101");
            FourthClassInstrDictionary.Add("sev", "0000000000110");
            FourthClassInstrDictionary.Add("sez", "0000000000111");
            FourthClassInstrDictionary.Add("ses", "0000000001000");
            FourthClassInstrDictionary.Add("scc", "0000000001001");
            FourthClassInstrDictionary.Add("nop", "0000000001010");
            FourthClassInstrDictionary.Add("ret", "0000000001011");
            FourthClassInstrDictionary.Add("reti","0000000001100");
            FourthClassInstrDictionary.Add("halt","0000000001101");
            FourthClassInstrDictionary.Add("wait","0000000001110");
            FourthClassInstrDictionary.Add("push pc","0000000001111");
            FourthClassInstrDictionary.Add("pop pc", "0000000010000");
            FourthClassInstrDictionary.Add("push flag", "0000000010001");
            FourthClassInstrDictionary.Add("pop flag", "0000000010010");
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            PC = 0;
            Stream myStream = null;
            theDialog.InitialDirectory = @"D:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (myStream = theDialog.OpenFile())
                    {
                       
                        StreamReader reader = new StreamReader(myStream);
                        while (!reader.EndOfStream)
                        {
                            string readedInstr = reader.ReadLine();
                            
                            if(readedInstr.Contains(":"))
                            {
                                LabelDictionary.Add(readedInstr.Substring(0, readedInstr.IndexOf(":")), PC.ToString());
                                InstrList.Add(readedInstr.Substring(readedInstr.IndexOf(":") + 2, readedInstr.Length - readedInstr.IndexOf(":") - 2));
                            }
                            else
                            {
                                InstrList.Add(readedInstr);
                            }
                            PC += 2;
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            GlobalInstruction = "";
            PC = 0;
            ushort Instr=0;
            foreach(string Instruction in InstrList)
            {
                DecodeInstruction(Instruction);
                PC += 2;
                Instr += FullInstructionBinary;
            }
         //   Instr = Convert.ToByte(GlobalInstruction);
            String FilePath = theDialog.FileName.Substring(0, theDialog.FileName.IndexOf(".asm")) + ".obj";
            FilePath = Path.GetFullPath(FilePath);
            if (!File.Exists(FilePath))
            {
                FileStream fs = new FileStream(FilePath, FileMode.CreateNew);
                BinaryWriter binaryfile = new System.IO.BinaryWriter(fs);
               // StreamWriter file = new System.IO.StreamWriter(FilePath);
                binaryfile.Write(Instr);
                binaryfile.Close();
                MessageBox.Show("gata");

            }
            else
            {
                MessageBox.Show("File already exists!");
            }
            int b;
        }
        private void DecodeInstruction(string Instruction)
        {

           possibleExtra = ""; 
           LineInstruction = SplitWhitespace(Instruction);
            foreach(var lineinstr in LineInstruction)
            {
               int instructionclass = FindClass(lineinstr);
               if (instructionclass == 1)
               {
                   FirstClassInstruction = new FirstClassInstruction();
                   DecodeFirstInstruction();

               }
               if(instructionclass == 2 )
               {
                   SecondClassInstruction = new SecondClassInstruction();
                   DecodeSecondInstruction();
               }
               if (instructionclass == 3)
               {
                   ThirdClassInstruction = new ThirdClassInstruction();
                   DecodeThirdInstruction();
               }
               if (instructionclass == 4)
               {
                   FourthClassInstruction = new FourthClassInstruction();
                   DecodeFourthInstruction();
               }
            }
           
        }
        private void DecodeFirstInstruction()
        {
            string InstructionOpcode=LineInstruction[0].ToLower();
            FirstClassInstruction.Opcode += FirstClassInstrDictionary[InstructionOpcode];
            GetMasAndRsFromInstruction(LineInstruction[2]);
            GetMadAndRdFromInstruction(LineInstruction[1]);
            FullInstruction = FirstClassInstruction.Opcode + FirstClassInstruction.Mas + FirstClassInstruction.Rs + FirstClassInstruction.Mad + FirstClassInstruction.Rd;
            FullInstructionBinary = Convert.ToUInt16(FullInstruction,2);
            MachineCodList.Add(FullInstruction);
            if (possibleExtra != "")

            {
                ExtendExtra(16);
                FullInstruction = Environment.NewLine + possibleExtra;
                FullInstructionBinary += Convert.ToUInt16(possibleExtra,2);
            }
            MachineCodList.Add(FullInstruction);
     
        }
        private void DecodeSecondInstruction()
        {
            string InstructionOpcode = LineInstruction[0].ToLower();
            SecondClassInstruction.Opcode += SecondClassInstrDictionary[InstructionOpcode];
            GetMadAndRdFromSecondInstruction(LineInstruction[1]);
            FullInstruction = SecondClassInstruction.Opcode + SecondClassInstruction.Mad + SecondClassInstruction.Rd;
            FullInstructionBinary = Convert.ToUInt16(FullInstruction,2);
            MachineCodList.Add(FullInstruction);
            if (possibleExtra != "")
            {
                ExtendExtra(16);
                FullInstruction = Environment.NewLine + possibleExtra;
                FullInstructionBinary += Convert.ToUInt16(possibleExtra,2);
            }
            MachineCodList.Add(FullInstruction);
        }
        private void DecodeThirdInstruction()
        {
            string InstructionOpcode = LineInstruction[0].ToLower();
            ThirdClassInstruction.Opcode += ThirdClassInstrDictionary[InstructionOpcode];
            string offset = LabelDictionary[LineInstruction[1]];
            offset = (PC- int.Parse(offset)).ToString();
            offset = Convert.ToString(int.Parse(offset), 2);
            offset=ExtendOffset(offset,8);
            MachineCodList.Add(FullInstruction);
            FullInstructionBinary = Convert.ToUInt16(FullInstruction,2);

        }
        private void DecodeFourthInstruction()
        {
            string InstructionOpcode = LineInstruction[0].ToLower();
            FourthClassInstruction.Opcode += FourthClassInstrDictionary[InstructionOpcode];
            FullInstruction = FourthClassInstruction.Opcode;
            FullInstructionBinary = Convert.ToUInt16(FullInstruction,2);
            MachineCodList.Add(FullInstruction);
        }
        private string ExtendOffset(string offset,int nr)
        {
            while (offset.Length != nr)
            {
                offset = "0" + offset;
            }
            return offset;
        }
        private void ExtendExtra(int nr)
        {
            while (possibleExtra.Length != nr)
            {
                possibleExtra = "0" + possibleExtra;
            }
        }
        private void  GetMasAndRsFromInstruction(string Instruction)
        {
           
        if(!(Instruction.Contains('R')))
            {
                FirstClassInstruction.Mas= MasDictionary["AM"];
                FirstClassInstruction.Rs = "0000";
                if(Instruction.Contains("H"))
                {
                    Instruction = int.Parse(Instruction, System.Globalization.NumberStyles.HexNumber).ToString();
                }
                possibleExtra=Convert.ToString(int.Parse(Instruction), 2);
                int a;
           
            }
       else
            {
                if(!(Instruction.Contains("(")))
                {
                    FirstClassInstruction.Mas=MasDictionary["AD"];
                    FirstClassInstruction.Rs = RegisterDictionary[Instruction];
                }
                else
                {
                    if(Instruction.Substring(0,1)=="(" && Instruction.Substring(Instruction.Length-1,1)==")")
                    {
                        FirstClassInstruction.Mas= MasDictionary["AI"];
                    }
                    else
                    {
                        FirstClassInstruction.Mas = MasDictionary["AX"];
                        possibleExtra = Convert.ToString(int.Parse(Instruction.Substring(0, Instruction.IndexOf("("))), 2);
                    }
                    FirstClassInstruction.Rs = RegisterDictionary[Instruction.Substring(Instruction.IndexOf("(") + 1, Instruction.IndexOf(")") - Instruction.IndexOf("(") - 1)];
                }
            }
       }
        private void GetMadAndRdFromInstruction(string Instruction)
        {
            if (!(Instruction.Contains('R')))
            {
                FirstClassInstruction.Mad = MadDictionary["AM"];
                FirstClassInstruction.Rd = "0000";
                if (Instruction.Contains("H"))
                {
                    Instruction = int.Parse(Instruction, System.Globalization.NumberStyles.HexNumber).ToString();
                }
                possibleExtra = Convert.ToString(int.Parse(Instruction), 2);
            }
            else
            {
                if (!(Instruction.Contains("(")))
                {
                    FirstClassInstruction.Mad = MadDictionary["AD"];
                    FirstClassInstruction.Rd = RegisterDictionary[Instruction];
                }
                else
                {
                    if (Instruction.Substring(0, 1) == "(" && Instruction.Substring(Instruction.Length - 1, 1) == ")")
                    {
                        FirstClassInstruction.Mad = MadDictionary["AI"];
                    }
                    else
                    {
                        FirstClassInstruction.Mad = MadDictionary["AX"];
                        possibleExtra = Convert.ToString(int.Parse(Instruction.Substring(0, Instruction.IndexOf("("))), 2);
                    }
                    FirstClassInstruction.Rd = RegisterDictionary[Instruction.Substring(Instruction.IndexOf("(") + 1, Instruction.IndexOf(")") - Instruction.IndexOf("(") - 1)];
                }
            }
        }
        private void GetMadAndRdFromSecondInstruction(string Instruction)
        {
            if (!(Instruction.Contains('R')))
            {
                SecondClassInstruction.Mad = MadDictionary["AM"];
                SecondClassInstruction.Rd = "0000";
                if (LabelDictionary.ContainsKey(Instruction))
                {
                    possibleExtra = Convert.ToString(int.Parse(LabelDictionary[Instruction]) - PC, 2);
                }
                else
                {
                    if (Instruction.Contains("H"))
                    {
                        Instruction = Convert.ToInt32(Instruction.Remove(Instruction.Length - 1, 1), 16).ToString();
                    }
                    possibleExtra = Convert.ToString(int.Parse(Instruction), 2);
                }
            }
            else
            {
                if (!(Instruction.Contains("(")))
                {
                    SecondClassInstruction.Mad = MadDictionary["AD"];
                    SecondClassInstruction.Rd = RegisterDictionary[Instruction];
                }
                else
                {
                    if (Instruction.Substring(0, 1) == "(" && Instruction.Substring(Instruction.Length - 1, 1) == ")")
                    {
                        SecondClassInstruction.Mad = MadDictionary["AI"];
                    }
                    else
                    {
                        SecondClassInstruction.Mad = MadDictionary["AX"];
                        possibleExtra = Convert.ToString(int.Parse(Instruction.Substring(0, Instruction.IndexOf("("))), 2);
                    }
                    SecondClassInstruction.Rd = RegisterDictionary[Instruction.Substring(Instruction.IndexOf("(") + 1, Instruction.IndexOf(")") - Instruction.IndexOf("(") - 1)];
                }
            }
        }
        private  List<string> SplitWhitespace(string input)
        {
           string testinput = input.ToLower();
           List<string> forced=new List<string>();
           forced.Add(testinput);
           if (!FourthClassInstrDictionary.ContainsKey(testinput))
               return input.Split(new char[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
           else
               return forced;
        }
        private int FindClass(string Instruction)
        {
            Instruction = Instruction.ToLower();
            if (CheckForFirstClass(Instruction))
                return 1;
            if (CheckForSecondClass(Instruction))
                return 2;
            if (CheckForThirdClass(Instruction))
                return 3;
            if (CheckForFourthClass(Instruction))
                return 4;
            return 0;
        }

        private bool CheckForFirstClass(string Instruction)
        {
            return (FirstClassInstrDictionary.ContainsKey(Instruction));
        }
        private bool CheckForSecondClass(string Instruction)
        {
            return (SecondClassInstrDictionary.ContainsKey(Instruction));
        }
        private bool CheckForThirdClass(string Instruction)
        {
            return (ThirdClassInstrDictionary.ContainsKey(Instruction));
        }
        private bool CheckForFourthClass(string Instruction)
        {
            return (FourthClassInstrDictionary.ContainsKey(Instruction));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Secven SecventiatorForm = new Secven(InstrList, MachineCodList);
            this.Hide();
            SecventiatorForm.ShowDialog();
        }

        
    }
}
