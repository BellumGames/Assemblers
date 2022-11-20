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

namespace ProiectAC
{
    public partial class MainProgram : Form
    {
        struct label
        {
            public string name;
            public int address;
        };

        struct procedure
        {
            public string name;
            public int address;
        };

        // declarations
        #region declaratii

        label[] labels = new label[50];
        procedure[] procedures = new procedure[50];

        static byte[] RAM = new byte[65536];
        static UInt16 IR = 0;
        static UInt16 FLAG = 0;
        static UInt16 IVR = 0;
        static UInt16 T = 0;
        static UInt16 SP = 65534;
        static UInt16 MDR = 0;
        static UInt16 ADR = 0;
        static UInt16 PC = 0;
        static UInt16 PCMax = 0; // PC Maxim, in functie de cate instructiuni sunt in fisier
        static UInt16[] R = new UInt16[16];
        static UInt16 reg = 0;
        static UInt16 g = 0;
        static UInt16 clasa;
        static UInt16 IndexValoare;

        static UInt64[] MPM = new UInt64[200];
        static UInt64 MIR = 0;
        static UInt16 MAR = 0;
        static UInt16 SBUS = 0;
        static UInt16 DBUS = 0;
        static UInt16 RBUS = 0;
        static UInt16 Carry = 0;

        static int Step = 0;

        static UInt16 bit = 0;

        static string registru;
        static string valoareImediata;
        static string valImediataBinar;
        static string IM = "00";
        static string AD = "01";
        static string AI = "10";
        static string AX = "11";

        static int contor;
        static int adresaSalt;
        static int modAdresare;
        static int offset;
        static string valIndex;
        static string valIndexSursa;
        static string valIndexDest;
        static string offsetString;

        Dictionary<string, string> b1 = new Dictionary<string, string>();
        Dictionary<string, string> b2 = new Dictionary<string, string>();
        Dictionary<string, string> b3 = new Dictionary<string, string>();
        Dictionary<string, string> b4 = new Dictionary<string, string>();
        Dictionary<string, string> registers = new Dictionary<string, string>() {
            {"R0","0000"},{"R1","0001"},{"R2","0010"},{"R3","0011"},{"R4","0100"},{"R5","0101"},{"R6","0110"},{"R7","0111"},
            {"R8","1000"},{"R9","1001"},{"R10","1010"},{"R11","1011"},{"R12","1100"},{"R13","1101"},{"R14","1110"},{"R15","1111"}
        };
        Dictionary<string, string> registreIndex = new Dictionary<string, string>();
        Dictionary<UInt16, byte> mem = new Dictionary<UInt16, byte>();

        List<string> prelines = new List<string>();
        List<string> instructions = new List<string>();

        List<string> microcod = new List<string>();

        string str, firstStr, opcode, numeOpcode = null;
        string codif;
        int nrb1 = 0;
        int nrb2 = 0;
        int nrb3 = 0;
        int nrb4 = 0;
        int nbLabels = 0;
        int nbProcedures = 0;
        int ct;

        static bool isb1 = false;
        static bool isb2 = false;
        static bool isb3 = false;
        static bool isb4 = false;
        static bool isValImm = false;
        static bool isIndexSrc = false;
        static bool isIndexDest = false;
        static bool isError = false;
        static bool isJMP = false;
        static bool isCALL = false;
        static bool isPUSHRi = false;
        static bool isPOPRi = false;
        bool asmFileOpened = false;
        bool microCodeOpened = false;
        bool compiledOk;
        static bool isStep;
        static bool isINT;

        Memory memory;

        #endregion

        //initialize component and call CitireCodificare()
        public MainProgram()
        {
            InitializeComponent();
            CitireCodificari();
        }

        //read instructions' opcode
        private void CitireCodificari()
        {
            StreamReader codificareFile = new StreamReader("codifinput.txt");
            char[] separators = { ' ' };
            string[] values = null;
            ct = 1;
            firstStr = codificareFile.ReadLine();
            if (firstStr != null)
            {
                values = firstStr.Split(separators);
                nrb1 = Convert.ToInt16(values[0]);
                nrb2 = Convert.ToInt16(values[1]);
                nrb3 = Convert.ToInt16(values[2]);
                nrb4 = Convert.ToInt16(values[3]);
            }
            while ((str = codificareFile.ReadLine()) != null)
            {
                values = str.Split(separators);
                if (ct <= nrb1)
                {
                    numeOpcode = values[0];
                    opcode = values[1];
                    b1.Add(values[0], values[1]);
                }
                else
                    if (ct <= (nrb1 + nrb2))
                {
                    numeOpcode = values[0];
                    opcode = values[1];
                    b2.Add(values[0], values[1]);
                }
                else
                        if (ct <= (nrb1 + nrb2 + nrb3))
                {
                    numeOpcode = values[0];
                    opcode = values[1];
                    b3.Add(numeOpcode, opcode);
                }
                else
                {
                    if (values[1] == "PC" || values[1] == "FLAG")
                    {
                        numeOpcode = values[0] + " " + values[1];
                        opcode = values[2];
                        b4.Add(numeOpcode, opcode);
                    }
                    else
                    {
                        numeOpcode = values[0];
                        opcode = values[1];
                        b4.Add(numeOpcode, opcode);
                    }
                }
                ct++;
            }
            foreach (KeyValuePair<UInt16, byte> line in mem)
            {
                mem.Remove(line.Key);
            }
            codificareFile.Close();
        }

        //open asm file and write instructions in list box
        private void openasmBtn_Click(object sender, EventArgs e)
        {
            contor = 1;
            instrBox.Text = "";

            StreamReader sr = new StreamReader("input.asm");
            while ((str = sr.ReadLine()) != null)
            {
                instrBox.Items.Add(contor + ". " + str);
                prelines.Add(str);
                contor++;
            }

            PCMax -= 2;
            asmFileOpened = true;
            sr.Close();
        }

        //Assembler
        #region Assembler
        //precompile and compile asm file
        private void assembleBtn_Click(object sender, EventArgs e)
        {
            if (asmFileOpened)
            {
                preCompile(prelines);
                compile(instructions);
                if (compiledOk)
                {
                    MessageBox.Show("Compile process completed with succes!");
                    loadInstructionsIntoRAM();
                }
                else
                {
                    MessageBox.Show("Correct the mistakes and open again the asm file!");
                    prelines.Clear();
                    instructions.Clear();
                    assembleBox.Items.Clear();
                    instrBox.Items.Clear();
                }
            }
            else
                MessageBox.Show("You opened no asm file! Open a file and try again!!");
        }

        //remove whitespaces and save labels 
        private void preCompile(List<string> prelines)
        {
            PCMax = 0;
            for (int i = 0; i < prelines.Count; i++)
            {
                //eliminam comentariile
                int k = prelines[i].IndexOf(";");
                if (k != -1)
                    prelines[i] = prelines[i].Remove(k);
                prelines[i] = prelines[i].ToUpper();
                prelines[i] = prelines[i].Trim(); // remove whitespace from the beginning or ending of the string


                if (prelines[i].Contains(".DATA") == false && prelines[i].Contains(".CODE") == false && prelines[i].Contains("END") == false)
                {
                    //salvam etichetele si proc si calc adresele lor
                    if (prelines[i].Contains("ET") == true && prelines[i].Contains(":") == true)
                    {
                        //daca avem eticheta
                        k = prelines[i].IndexOf(":");
                        if (k != -1)
                            prelines[i] = prelines[i].Remove(k);

                        labels[nbLabels].name = prelines[i];
                        labels[nbLabels++].address = PCMax;
                    }
                    else
                        if (prelines[i].Contains("PROC") == true)
                    {
                        //daca avem procedura
                        k = prelines[i].IndexOf(" ");
                        if (k != -1)
                        {
                            if (prelines[i].Substring(k + 1) != "ENDP")
                            {
                                prelines[i] = prelines[i].Substring(0, k);
                                procedures[nbProcedures].name = prelines[i];
                                procedures[nbProcedures].address = PCMax;
                                nbProcedures++;
                            }
                        }
                    }
                    else
                    {
                        if (prelines[i].Contains("START:") == true)
                        {
                            k = prelines[i].IndexOf(":");
                            if (k != -1)
                                prelines[i] = prelines[i].Remove(k);

                            labels[nbLabels].name = prelines[i];
                            labels[nbLabels++].address = PCMax;

                        }
                        else
                        {
                            //daca nu e eticheta sau procedura sau endp pe rand
                            PCMax += 2;
                            instructions.Add(prelines[i]);

                            isb1 = isb2 = isb3 = isb4 = false;
                            isJMP = isCALL = false;
                            opcode = checkInstrType(prelines[i]);

                            if (isb1)
                            {
                                string operandSursa = prelines[i].Substring(prelines[i].IndexOf(",") + 1);
                                checkMA(operandSursa);

                                if (modAdresare == 0 || modAdresare == 3)
                                    PCMax += 2;
                                string s1 = prelines[i].Substring(0, prelines[i].IndexOf(","));
                                string operandDestinatie = s1.Substring(s1.IndexOf(" ") + 1);
                                checkMA(operandDestinatie);
                                if (modAdresare == 3)
                                    PCMax += 2;
                            }

                            if (isb2)
                            {
                                checkMA(prelines[i].Substring(prelines[i].IndexOf(" ") + 1));
                                if (isJMP || isCALL)
                                    PCMax += 2;
                                if (modAdresare == 3)
                                    PCMax += 2;
                            }
                        }
                    }
                }
            }
        }

        //compile asm file(instructions) and write codification in binary file
        private void compile(List<string> instructions)
        {
            StreamWriter fisIesire = new StreamWriter("output.txt");

            string fout = "codificare.bin";
            FileStream fs = new FileStream(fout, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            PCMax = 0;
            compiledOk = true;

            foreach (string instr in instructions)
            {
                isb1 = isb2 = isb3 = isb4 = false;
                isError = false;
                isValImm = isIndexDest = isIndexSrc = false;
                isJMP = isCALL = isPUSHRi = isPOPRi = false;
                codif = valoareImediata = valImediataBinar = valIndex = valIndexSursa = valIndexDest = null;

                opcode = checkInstrType(instr);

                if (opcode != null)
                {
                    codif = opcode;
                }
                else
                {
                    compiledOk = false;
                    MessageBox.Show("Invalid instruction: " + instr);
                    break;
                }

                #region clasa B1
                //clasa B1
                if (isb1)
                {
                    string operandSursa = instr.Substring(instr.IndexOf(",") + 1);
                    checkMA(operandSursa); // mod adresare operand sursa

                    switch (modAdresare)
                    {
                        case 0:
                            {
                                if (operandSursa.Contains("ET") == true || operandSursa.Contains("START") == true)
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Invalid instruction: " + instr);
                                }
                                else
                                {
                                    codif += IM;
                                    isValImm = true;
                                    valImediataBinar = Convert_Binary(valoareImediata, 16);
                                    codif += "0000";
                                }
                                break;
                            }
                        case 1:
                            {
                                bool regGasit = false;
                                codif += AD;
                                foreach (KeyValuePair<string, string> r in registers)
                                {
                                    if (r.Key == registru)
                                    {
                                        regGasit = true;
                                        codif += r.Value;
                                    }
                                }
                                if (!regGasit)
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Invalid source register in instruction: " + instr);
                                }
                                break;
                            }
                        case 2:
                            {
                                bool regGasit = false;
                                codif += AI;
                                foreach (KeyValuePair<string, string> r in registers)
                                {
                                    if (r.Key == registru)
                                    {
                                        regGasit = true;
                                        codif += r.Value;
                                    }
                                }
                                if (!regGasit)
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Invalid source register in instruction: " + instr);
                                }
                                break;
                            }
                        case 3:
                            {
                                bool regGasit = false;
                                codif += AX;
                                foreach (KeyValuePair<string, string> r in registers)
                                {
                                    if (r.Key == registru)
                                    {
                                        regGasit = true;
                                        codif += r.Value;
                                    }
                                }
                                if (!regGasit)
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Invalid source register in instruction: " + instr);
                                }
                                if (Convert.ToInt32(valIndex) >= -32767 && Convert.ToInt32(valIndex) <= 32767)
                                {
                                    isIndexSrc = true;
                                    valIndexSursa = Convert_Binary(valIndex, 16);
                                }
                                else
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Index not in range (-32767,+32767) in instruction: " + instr);
                                }
                                break;
                            }
                        default:
                            {
                                compiledOk = false;
                                MessageBox.Show("Invalid instruction: " + instr);
                                break;
                            }
                    }

                    string s1 = instr.Substring(0, instr.IndexOf(","));
                    string operandDestinatie = s1.Substring(s1.IndexOf(" ") + 1);
                    checkMA(operandDestinatie); //mod adresare operand destinatie

                    switch (modAdresare)
                    {
                        case 1:
                            {
                                bool regGasit = false;
                                codif += AD;
                                foreach (KeyValuePair<string, string> r in registers)
                                {
                                    if (r.Key == registru)
                                    {
                                        codif += r.Value;
                                        regGasit = true;
                                    }
                                }
                                if (!regGasit)
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Invalid destination register in instruction: " + instr);
                                }
                                break;
                            }
                        case 2:
                            {
                                bool regGasit = false;
                                codif += AI;
                                foreach (KeyValuePair<string, string> r in registers)
                                {
                                    if (r.Key == registru)
                                    {
                                        codif += r.Value;
                                        regGasit = true;
                                    }
                                }
                                if (!regGasit)
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Invalid destination register in instruction: " + instr);
                                }
                                break;
                            }
                        case 3:
                            {
                                bool regGasit = false;
                                codif += AX;
                                foreach (KeyValuePair<string, string> r in registers)
                                {
                                    if (r.Key == registru)
                                    {
                                        codif += r.Value;
                                        regGasit = true;
                                    }
                                }
                                if (!regGasit)
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Invalid destination register in instruction: " + instr);
                                }
                                if (Convert.ToInt32(valIndex) >= -32767 && Convert.ToInt32(valIndex) <= 32767)
                                {
                                    isIndexDest = true;
                                    valIndexDest = Convert_Binary(valIndex, 16);
                                }
                                else
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Index not in range (-32767,+32767) in instruction: " + instr);
                                }
                                break;
                            }
                        default:
                            {
                                compiledOk = false;
                                MessageBox.Show("Invalid instruction: " + instr);
                                break;
                            }
                    }
                    if (compiledOk)
                    {
                        fisIesire.WriteLine(codif);
                        bw.Write(Convert.ToInt16(codif, 2));
                        // assembleBox.Items.Add(codif);
                        if (isValImm)
                        {
                            fisIesire.WriteLine(valImediataBinar);
                            bw.Write(Convert.ToInt16(valImediataBinar, 2));
                            PCMax += 2;
                        }
                        if (isIndexSrc)
                        {
                            fisIesire.WriteLine(valIndexSursa);
                            bw.Write(Convert.ToInt16(valIndexSursa, 2));
                            PCMax += 2;
                        }
                        if (isIndexDest)
                        {
                            fisIesire.WriteLine(valIndexDest);
                            bw.Write(Convert.ToInt16(valIndexDest, 2));
                            PCMax += 2;
                        }
                    }
                    else
                        break;
                }
                #endregion

                #region clasa B2
                //clasa B2
                if (isb2)
                {
                    checkMA(instr.Substring(instr.IndexOf(" ") + 1)); //mod adresare destinatie

                    switch (modAdresare)
                    {
                        case 0:
                            {
                                if (isJMP || isCALL)
                                {
                                    codif += IM;
                                    isValImm = true;
                                    valImediataBinar = Convert_Binary(valoareImediata, 16);
                                    codif += "0000";
                                }
                                else
                                {
                                    compiledOk = false;
                                    MessageBox.Show("Immediate value not allowed in B2 instruction: " + instr);
                                }
                                break;
                            }
                        case 1:
                            {
                                if (!isJMP && !isCALL)
                                {
                                    codif += AD;
                                    bool regGasit = false;
                                    foreach (KeyValuePair<string, string> r in registers)
                                    {
                                        if (r.Key == registru)
                                        {
                                            codif += r.Value;
                                            regGasit = true;
                                        }
                                    }
                                    if (!regGasit)
                                    {
                                        compiledOk = false;
                                        MessageBox.Show("Invalid destination register in instruction: " + instr);
                                    }
                                }
                                else
                                {
                                    compiledOk = false;
                                    MessageBox.Show("JMP or CALL incorrect: " + instr);
                                }
                                break;
                            }
                        case 2:
                            {
                                if (!isPUSHRi && !isPOPRi)
                                {
                                    bool regGasit = false;
                                    codif += AI;
                                    foreach (KeyValuePair<string, string> r in registers)
                                    {
                                        if (r.Key == registru)
                                        {
                                            codif += r.Value;
                                            regGasit = true;
                                        }
                                    }
                                    if (!regGasit)
                                    {
                                        compiledOk = false;
                                        MessageBox.Show("Invalid destination register in instruction: " + instr);
                                    }
                                }
                                else
                                {
                                    compiledOk = false;
                                    MessageBox.Show("PUSH Ri or POP Ri incorrect: " + instr);
                                }
                                break;
                            }
                        case 3:
                            {
                                if (!isPUSHRi && !isPOPRi)
                                {
                                    bool regGasit = false;
                                    codif += AX;
                                    foreach (KeyValuePair<string, string> r in registers)
                                    {
                                        if (r.Key == registru)
                                        {
                                            codif += r.Value;
                                            regGasit = true;
                                        }
                                    }
                                    if (!regGasit)
                                    {
                                        compiledOk = false;
                                        MessageBox.Show("Invalid destination register in instruction: " + instr);
                                    }
                                    if (Convert.ToInt32(valIndex) >= -32767 && Convert.ToInt32(valIndex) <= 32767)
                                    {
                                        isIndexDest = true;
                                        valIndexDest = Convert_Binary(valIndex, 16);
                                    }
                                    else
                                    {
                                        compiledOk = false;
                                        MessageBox.Show("Index not in range (-32767,+32767) in instruction: " + instr);
                                    }
                                }
                                else
                                {
                                    compiledOk = false;
                                    MessageBox.Show("PUSH Ri or POP Ri incorrect: " + instr);
                                }
                                break;
                            }
                        default:
                            {
                                compiledOk = false;
                                MessageBox.Show("Invalid instruction: " + instr);
                                break;
                            }
                    }
                    if (compiledOk)
                    {
                        fisIesire.WriteLine(codif);
                        bw.Write(Convert.ToInt16(codif, 2));
                        // assembleBox.Items.Add(codif);
                        if (isValImm)
                        {
                            fisIesire.WriteLine(valImediataBinar);
                            bw.Write(Convert.ToInt16(valImediataBinar, 2));
                            PCMax += 2;
                        }
                        if (isIndexDest)
                        {
                            fisIesire.WriteLine(valIndexDest);
                            bw.Write(Convert.ToInt16(valIndexDest, 2));
                            PCMax += 2;
                        }
                    }
                    else
                        break;
                }
                #endregion

                #region clasa B3
                //clasa B3
                if (isb3)
                {
                    checkMA(instr.Substring(instr.IndexOf(" ") + 1)); //mod adresare

                    if (modAdresare == 0)
                    {
                        int val = Convert.ToInt16(valoareImediata);
                        offset = val - (PCMax + 2);
                        if ((offset >= -127) && (offset <= 127))
                        {
                            offsetString = Convert_Binary(offset.ToString(), 8);
                        }
                        else
                        {
                            compiledOk = false;
                            MessageBox.Show("Jump address to high. It is not in range (-127,127)! on instruction: " + instr);
                        }
                    }
                    else
                    {
                        compiledOk = false;
                        MessageBox.Show("Invalid B3 instruction: " + instr);
                    }
                    if (compiledOk)
                    {
                        codif += offsetString;
                        fisIesire.WriteLine(codif);
                        bw.Write(Convert.ToInt16(codif, 2));
                        // assembleBox.Items.Add(codif);
                    }
                    else
                        break;
                }
                #endregion

                #region clasa B4
                if (isb4)
                {
                    fisIesire.WriteLine(codif);
                    bw.Write(Convert.ToInt16(codif, 2));
                    // assembleBox.Items.Add(codif);
                }
                #endregion

                if (compiledOk)
                {
                    fisIesire.Flush();
                    PCMax += 2;
                }
                else
                    break;
            }
            fisIesire.Close();
            bw.Close();
        }

        //check what instruction type has current instruction processed
        private string checkInstrType(string instr)
        {
            foreach (KeyValuePair<string, string> t4 in b4) // clasa b4?
            {
                if (t4.Key == instr)
                {
                    isb4 = true;
                    return t4.Value; // return opcode-ul instructiunii
                }
            }

            char[] separators = { ' ' };
            string[] values = instr.Split(separators);

            foreach (KeyValuePair<string, string> t1 in b1) //clasa b1?
            {
                if (t1.Key == values[0])
                {
                    isb1 = true;
                    return t1.Value;
                }
            }

            foreach (KeyValuePair<string, string> t2 in b2) //clasa b2?
            {
                if (t2.Key == values[0])
                {
                    isb2 = true;
                    if (values[0] == "JMP")
                        isJMP = true;
                    if (values[0] == "CALL")
                        isCALL = true;
                    if (values[0] == "PUSH")
                        isPUSHRi = true;
                    if (values[0] == "POP")
                        isPOPRi = true;
                    return t2.Value;
                }
            }
            foreach (KeyValuePair<string, string> t3 in b3) // clasa b3?
            {
                if (t3.Key == values[0])
                {
                    isb3 = true;
                    return t3.Value;
                }
            }
            return null;
        }

        //check the address mode of the current instruction
        private void checkMA(string s)
        {
            if ((s.Length >= 2 && s.Substring(0, 2) == "ET") || (s.Length >= 5 && s.Substring(0, 5) == "START"))
            {
                modAdresare = 0;
                foreach (label l in labels)
                {
                    if (l.name == s)
                    {
                        valoareImediata = l.address.ToString();
                    }
                }
            }
            else
            {
                if (s.IndexOf("R") != -1) //adresare AD,AI,AX
                {
                    if (s.IndexOf(")") == -1)
                    {//adresare AD
                        modAdresare = 1;
                        registru = s;
                    }
                    else
                    {//AI sau AX
                        if (s.IndexOf("(") == 0)
                        {//AI
                            modAdresare = 2;
                            s = s.Remove(s.IndexOf(")"));
                            registru = s.Substring(1);
                        }
                        else
                        {//AX
                            modAdresare = 3;
                            valIndex = s.Substring(0, s.IndexOf("("));
                            s = s.Substring(s.IndexOf("(") + 1);
                            registru = s.Substring(0, s.IndexOf(")"));
                        }
                    }
                }
                else
                {//AM
                    modAdresare = 0;
                    valoareImediata = s;
                }
            }
        }

        //convert a string into binary of nr bits 
        //returns a string containing the bits
        private string Convert_Binary(string st, int nr)
        {
            Int64 adresa = Convert.ToInt64(st);
            Int64 n = adresa;
            Int64 reg = 0;
            string str = "";
            string str_reg = "";

            if (n < 0)
            {
                n = (-1) * n - 1;
            }
            for (int i = 0; i < nr; i++)
            {
                reg = n % 2;
                if (adresa < 0)
                {
                    if (reg == 0)
                        reg = 1;
                    else
                    {
                        if (reg == 1)
                            reg = 0;
                    }
                }
                n = n / 2;
                str += Convert.ToString(reg);
            }

            for (int i = 0; i < nr; i++)
            {
                str_reg += str.Substring(nr - 1 - i, 1);
            }
            return str_reg;
        }

        //load instructions into RAM MEMORY after compilation
        private void loadInstructionsIntoRAM()
        {
            string fin = "codificare.bin";
            FileStream fs = new FileStream(fin, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            int counter = 0;
            int pos = 0;
            byte length = (byte)br.BaseStream.Length;
            while (pos < length)
            {
                RAM[counter] = br.ReadByte();
                mem.Add((UInt16)counter, RAM[counter]);
                pos += sizeof(byte);
                counter++;
            }

            br.Close();
        }
        #endregion

        //Load Microcode
        #region Microcode

        //create and open microcode
        private void openmicrocodeBtn_Click(object sender, EventArgs e)
        {
            loadMicroCodeText();
            createMicroCodeBinaryFile();
            openMicroCode();
            contor = -1;
        }

        //read txt file with microcode and create binary file
        private void createMicroCodeBinaryFile()
        {
            string fout = "microproprogram_emulare.bin";
            FileStream fs1 = new FileStream(fout, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs1);

            StreamReader sr = new StreamReader("microprogram_emulare.txt");

            while ((str = sr.ReadLine()) != null)
            {
                bw.Write(Convert.ToInt64(str, 2));
            }

            sr.Close();
            bw.Close();
        }

        //open microcode binary file and load the microcode into MPM
        private void openMicroCode()
        {
            string fin = "microproprogram_emulare.bin";
            FileStream fs2 = new FileStream(fin, FileMode.Open);
            BinaryReader bw = new BinaryReader(fs2);

            int counter = 0;
            int pos = 0;
            Int64 length = (Int64)bw.BaseStream.Length;
            while (pos < length)
            {
                MPM[counter] = bw.ReadUInt64();
                pos += sizeof(UInt64);
                counter++;
            }

            microCodeOpened = true;
            MessageBox.Show("Microcode file opened with succes!");
            bw.Close();
        }

        private void loadMicroCodeText()
        {
            StreamReader sr = new StreamReader("microprogram_emulare_text.txt");

            while ((str = sr.ReadLine()) != null)
            {
                microcod.Add(str);
            }
            listBox1.DataSource = microcod;
            sr.Close();
        }
        #endregion

        //Sequencer
        #region Sequencer

        //run simulator - one single step
        private void runSimulatorStep()
        {
            Step++;
            if (PC <= PCMax)
            {
                switch (Step)
                {
                    case 1:
                        {
                            MIR = MPM[MAR];
                            listBox1.SetSelected(MAR, true);
                            DecodifSBUS();
                        }
                        break;
                    case 2:
                        DecodifDBUS();
                        break;
                    case 3:
                        DecodifALU();
                        break;
                    case 4:
                        DecodifDestRBUS();
                        break;
                    case 5:
                        DecodifOtherOp();
                        break;
                    case 6:
                        TestIfchReadWrite();
                        break;
                    case 7:
                        {
                            g = TestG();
                            if (g == 1) // MAR = ADRESA_SALT + INDEX
                            {
                                UInt16 Index;
                                // selectie index
                                #region SelectieIndex
                                switch ((MIR & 0x000000000000E00) >> 9)
                                {
                                    case 0x0: //INDEX0 
                                        MAR = (UInt16)(MIR & 0x0000000000FF);
                                        break;
                                    case 0x1: //INDEX1
                                        UInt16 cl = getCl();
                                        switch (cl)
                                        {
                                            case 0:
                                                MAR = (UInt16)(MIR & 0x00000000000000FF);
                                                break;
                                            case 1:
                                                MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + 0x1);
                                                break;
                                            case 2:
                                                MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + 0x3);
                                                break;
                                            case 3:
                                                MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + 0x2);
                                                break;
                                        }
                                        break;
                                    case 0x2: //INDEX2
                                        IndexValoare = (UInt16)((IR & 0x0C00) >> 9);
                                        MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                                        break;
                                    case 0x3: //INDEX3
                                        IndexValoare = (UInt16)((IR & 0x0030) >> 3);
                                        MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                                        break;
                                    case 0x4: //INDEX4
                                        IndexValoare = (UInt16)((IR & 0x7000) >> 11);
                                        MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                                        break;
                                    case 0x5: //INDEX5
                                        IndexValoare = (UInt16)((IR & 0x07C0) >> 5);
                                        MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                                        break;
                                    case 0x6: //INDEX6
                                        IndexValoare = (UInt16)((IR & 0x1F00) >> 7);
                                        MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                                        break;
                                    case 0x7: //INDEX7
                                        IndexValoare = (UInt16)((IR & 0x001F) << 1);
                                        MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                                        break;
                                    default:
                                        break;
                                }
                                #endregion
                            }
                            else
                            { // MAR = MAR + 1
                                MAR = (UInt16)(MAR + 0x1);
                            }
                            //MARtext.Text = Convert_Binary(MAR.ToString(), 16);
                            SBUS = 0;
                            DBUS = 0;
                            RBUS = 0;
                            Step = 0;
                            resetGraphics();
                        }
                        break;
                    default:
                        break;
                }
            }
            else
                MessageBox.Show("The program was successfully simulated!");
        }

        //run sumulator to the end
        private void runSimulator()
        {
            while (PC <= PCMax)
            {
                MIR = MPM[MAR];
                listBox1.SetSelected(MAR, true);
                //MIRtext.Text = Convert_Binary(MIR.ToString(), 64);
                DecodifSBUS();
                DecodifDBUS();
                DecodifALU();
                DecodifDestRBUS();
                DecodifOtherOp();

                g = TestG();
                if (g == 1) // MAR = ADRESA_SALT + INDEX
                {
                    UInt16 Index;
                    // selectie index
                    #region SelectieIndex
                    switch ((MIR & 0x000000000000E00) >> 9)
                    {
                        case 0x0: //INDEX0 
                            MAR = (UInt16)(MIR & 0x0000000000FF);
                            break;
                        case 0x1: //INDEX1
                            int cl = getCl();
                            switch (cl)
                            {
                                case 0:
                                    MAR = (UInt16)(MIR & 0x00000000000000FF);
                                    break;
                                case 1:
                                    MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + 0x1);
                                    break;
                                case 2:
                                    MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + 0x3);
                                    break;
                                case 3:
                                    MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + 0x2);
                                    break;
                            }
                            break;
                        case 0x2: //INDEX2
                            IndexValoare = (UInt16)((IR & 0x0C00) >> 9);
                            MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                            break;
                        case 0x3: //INDEX3
                            IndexValoare = (UInt16)((IR & 0x0030) >> 3);
                            MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                            break;
                        case 0x4: //INDEX4
                            IndexValoare = (UInt16)((IR & 0x7000) >> 11);
                            MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                            break;
                        case 0x5: //INDEX5
                            IndexValoare = (UInt16)((IR & 0x07C0) >> 5);
                            MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                            break;
                        case 0x6: //INDEX6
                            IndexValoare = (UInt16)((IR & 0x1F00) >> 7);
                            MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                            break;
                        case 0x7: //INDEX7
                            IndexValoare = (UInt16)((IR & 0x001F) << 1);
                            MAR = (UInt16)((UInt16)(MIR & 0x00000000000000FF) + IndexValoare);
                            break;
                        default:
                            break;
                    }
                    #endregion
                }
                else
                { // MAR = MAR + 1
                    MAR = (UInt16)(MAR + 0x1);
                }
                //MARtext.Text = Convert_Binary(MAR.ToString(), 16);

                TestIfchReadWrite();

                SBUS = 0;
                DBUS = 0;
                RBUS = 0;
                resetGraphics();
            }
            MessageBox.Show("The program was successfully simulated!");
        }

        #region Decodificatoare

        //decodificator camp Sursa SBUS
        private void DecodifSBUS()
        {
            UInt16 campSbus = (UInt16)((MIR & 515396075520) >> 35);
            switch (campSbus)
            {
                case 0x1: //PdIR[Off]s
                    PdIROffs();
                    SBUS = (UInt16)(IR & 0x00FF);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0x2: //PdFLAGs		
                    PdFLAGs();
                    SBUS = (UInt16)(FLAG);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0x3: //PdSPs
                    PdSPs();
                    SBUS = (UInt16)(SP);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0x4: //PdTs
                    PdTs();
                    SBUS = (UInt16)(T);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0x5: //PdnTs	
                    PdnTs();
                    UInt16 t = T;
                    SBUS = (UInt16)(~t);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0x6: //PdPCs	
                    PdPCs();
                    SBUS = (UInt16)(PC);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0x7: //PdIVRs	
                    PdIVRs();
                    SBUS = (UInt16)(IVR);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0x8: //PdADRs		
                    PdADRs();
                    SBUS = (UInt16)(ADR);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0x9: //PdMDRs	
                    PdMDRs();
                    SBUS = (UInt16)(MDR);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0xA: //PdRGs		
                    int nr_reg = (IR & 0x03C0) >> 6; //preiau reg din camp RS din IR
                    SBUS = (UInt16)(R[nr_reg]);
                    #region registrii
                    switch (nr_reg)
                    {
                        case 0x1:
                            PdRg1();
                            break;
                        case 0x2:
                            PdRg2();
                            break;
                        case 0x3:
                            PdRg3();
                            break;
                        case 0x4:
                            PdRg4();
                            break;
                        case 0x5:
                            PdRg5();
                            break;
                        case 0x6:
                            PdRg6();
                            break;
                        case 0x7:
                            PdRg7();
                            break;
                        case 0x8:
                            PdRg8();
                            break;
                        case 0x9:
                            PdRg9();
                            break;
                        case 0xA:
                            PdRg10();
                            break;
                        case 0xB:
                            PdRg11();
                            break;
                        case 0xC:
                            PdRg12();
                            break;
                        case 0xD:
                            PdRg13();
                            break;
                        case 0xE:
                            PdRg14();
                            break;
                        case 0xF:
                            PdRg15();
                            break;
                        default:
                            PdRg0();
                            break;
                    }
                    #endregion
                    PdRgs();
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0xB: //Pd0s
                    Pd0s();
                    SBUS = 0;
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0xC: //Pd-1s
                    Pdm1s();
                    Int16 v = -1;
                    SBUS = (UInt16)v;
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                case 0xD: //Pd1s
                    Pd1s();
                    SBUS = (UInt16)(1);
                    SBUSlabel.Text = Convert_Binary(SBUS.ToString(), 16);
                    break;

                default:
                    break;
            }
        }

        //decodificator camp Sursa DBUS
        private void DecodifDBUS()
        {
            UInt16 campDbus = (UInt16)((MIR & 0x0000000780000000) >> 31);
            switch (campDbus)
            {
                case 0x1: //PdIR[Off]d
                    PdIROffd();
                    DBUS = (UInt16)(IR & 0x00FF);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0x2: //PdFLAGd
                    PdFLAGd();
                    DBUS = (UInt16)(FLAG);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0x3: //PdSPd
                    PdSPd();
                    DBUS = (UInt16)(SP);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0x4: //PdTd
                    PdTd();
                    DBUS = (UInt16)(T);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0x5: //PdnTd
                    PdnTd();
                    UInt16 t = T;
                    DBUS = (UInt16)(~t);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0x6: //PdPCd
                    PdPCd();
                    DBUS = (UInt16)(PC);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0x7: //PdIVRd
                    PdIVRd();
                    DBUS = (UInt16)(IVR);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0x8: //PdADRd
                    PdADRd();
                    DBUS = (UInt16)(ADR);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0x9: //PdMDRd
                    PdMDRd();
                    DBUS = (UInt16)(MDR);
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0xA: //PdRGd
                    int nr_reg = IR & 0x000F; //preiau reg din camp RS din IR
                    DBUS = Convert.ToUInt16(R[nr_reg]);
                    #region registrii
                    switch (nr_reg)
                    {
                        case 0x1:
                            PdRg1();
                            break;
                        case 0x2:
                            PdRg2();
                            break;
                        case 0x3:
                            PdRg3();
                            break;
                        case 0x4:
                            PdRg4();
                            break;
                        case 0x5:
                            PdRg5();
                            break;
                        case 0x6:
                            PdRg6();
                            break;
                        case 0x7:
                            PdRg7();
                            break;
                        case 0x8:
                            PdRg8();
                            break;
                        case 0x9:
                            PdRg9();
                            break;
                        case 0xA:
                            PdRg10();
                            break;
                        case 0xB:
                            PdRg11();
                            break;
                        case 0xC:
                            PdRg12();
                            break;
                        case 0xD:
                            PdRg13();
                            break;
                        case 0xE:
                            PdRg14();
                            break;
                        case 0xF:
                            PdRg15();
                            break;
                        default:
                            PdRg0();
                            break;
                    }
                    #endregion
                    PdRgd();
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                case 0xB: //Pd0d
                    Pd0d();
                    DBUS = 0;
                    DBUSlabel.Text = Convert_Binary(DBUS.ToString(), 16);
                    break;

                default: //none
                    break;
            }
        }

        //decodificator camp Operatie ALU
        private void DecodifALU()
        {
            UInt16 campALU = (UInt16)((MIR & 0x0000000078000000) >> 27);
            switch (campALU)
            {
                case 0x1: //SUM
                    #region sum
                    if ((((UInt16)(MIR & 0x0000000007800000)) >> 18) == 2)
                    { //CIN activat atunci mai adun si val 1
                        try
                        {
                            ActivateCIN();
                            ALUlabel.Text = "SUM";
                            Alu();
                            RBUS = (UInt16)(SBUS + DBUS + 0x1);
                            RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                        }
                        catch (OverflowException) // daca apare overflow
                        {
                            FLAG = (UInt16)(FLAG | 0X0001); // setez bit overflow V
                            //FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                        }
                    }
                    else
                    { //adunare
                        try
                        {
                            ALUlabel.Text = "SUM";
                            Alu();
                            RBUS = (UInt16)(SBUS + DBUS);
                            RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                        }
                        catch (OverflowException)
                        {
                            FLAG = (UInt16)(FLAG | 0X0001); // setez bit V
                            //FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                        }
                    }
                    #endregion
                    break;
                case 0x2: //AND
                    ALUlabel.Text = "AND";
                    Alu();
                    RBUS = (UInt16)(SBUS & DBUS);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0x3: //OR
                    ALUlabel.Text = "OR";
                    Alu();
                    RBUS = (UInt16)(SBUS | DBUS);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0x4: //XOR
                    ALUlabel.Text = "XOR";
                    Alu();
                    RBUS = (UInt16)(SBUS ^ DBUS);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0x5: //ASL
                    ALUlabel.Text = "ASL";
                    Alu();
                    Carry = (UInt16)((DBUS & 0x8000) >> 15);
                    RBUS = (UInt16)(DBUS << 1);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0x6: //ASR
                    ALUlabel.Text = "ASR";
                    Alu();
                    Carry = (UInt16)(DBUS & 0x0001);
                    Int16 t = (Int16)((Int16)DBUS >> 1);
                    RBUS = (UInt16)t;
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0x7: //LSR
                    ALUlabel.Text = "LSR";
                    Alu();
                    Carry = (UInt16)(DBUS & 0x0001);
                    RBUS = (UInt16)(DBUS >> 1);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0x8: //ROL
                    ALUlabel.Text = "ROL";
                    Alu();
                    Carry = (UInt16)((DBUS & 0x8000) >> 15);
                    RBUS = (UInt16)((DBUS << 1) + Carry);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0x9: //ROR 
                    ALUlabel.Text = "ROR";
                    Alu();
                    Carry = (UInt16)(DBUS & 0x0001);
                    bit = (UInt16)(Carry << 15);
                    RBUS = (UInt16)((DBUS >> 1) + bit);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0xA: //RLC
                    ALUlabel.Text = "RLC";
                    Alu();
                    bit = Carry;
                    Carry = (UInt16)((DBUS & 0x8000) >> 15);
                    RBUS = (UInt16)((DBUS << 1) + bit);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0xB: //RRC
                    ALUlabel.Text = "RRC";
                    Alu();
                    bit = Carry;
                    Carry = (UInt16)(DBUS & 0x0001);
                    UInt16 leftBit = (UInt16)(Carry << 15);
                    RBUS = (UInt16)((DBUS >> 1) + (bit << 15));
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                case 0xC: //nDBUS
                    ALUlabel.Text = "nDBUS";
                    Alu();
                    RBUS = (UInt16)(~DBUS);
                    RBUSlabel.Text = Convert_Binary(RBUS.ToString(), 16);
                    break;
                default: //none
                    break;
            }

        }

        //decodificator camp Destinatie RBUS
        private void DecodifDestRBUS()
        {
            UInt16 campRbus = (UInt16)((MIR & 0x0000000007800000) >> 23);
            switch (campRbus)
            {
                case 0x1: //PmFLAG
                    PmFLAG();
                    FLAG = RBUS;
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;

                case 0x2: //PmRG
                    int nr_reg = IR & 0x000F;
                    R[nr_reg] = RBUS;
                    PmRGline.BorderColor = Color.Red;
                    switch (nr_reg)
                    {
                        case 0:
                            R0text.Text = Convert_Binary(R[0].ToString(), 16);
                            R0label.BackColor = Color.Red;
                            break;
                        case 1:
                            R1text.Text = Convert_Binary(R[1].ToString(), 16);
                            R1label.BackColor = Color.Red;
                            break;
                        case 2:
                            R2text.Text = Convert_Binary(R[2].ToString(), 16);
                            R2label.BackColor = Color.Red;
                            break;
                        case 3:
                            R3text.Text = Convert_Binary(R[3].ToString(), 16);
                            R3label.BackColor = Color.Red;
                            break;
                        case 4:
                            R4text.Text = Convert_Binary(R[4].ToString(), 16);
                            R4label.BackColor = Color.Red;
                            break;
                        case 5:
                            R5text.Text = Convert_Binary(R[5].ToString(), 16);
                            R5label.BackColor = Color.Red;
                            break;
                        case 6:
                            R6text.Text = Convert_Binary(R[6].ToString(), 16);
                            R6label.BackColor = Color.Red;
                            break;
                        case 7:
                            R7text.Text = Convert_Binary(R[7].ToString(), 16);
                            R7label.BackColor = Color.Red;
                            break;
                        case 8:
                            R8text.Text = Convert_Binary(R[8].ToString(), 16);
                            R8label.BackColor = Color.Red;
                            break;
                        case 9:
                            R9text.Text = Convert_Binary(R[9].ToString(), 16);
                            R9label.BackColor = Color.Red;
                            break;
                        case 10:
                            R10text.Text = Convert_Binary(R[10].ToString(), 16);
                            R10label.BackColor = Color.Red;
                            break;
                        case 11:
                            R11text.Text = Convert_Binary(R[11].ToString(), 16);
                            R11label.BackColor = Color.Red;
                            break;
                        case 12:
                            R12text.Text = Convert_Binary(R[12].ToString(), 16);
                            R12label.BackColor = Color.Red;
                            break;
                        case 13:
                            R13text.Text = Convert_Binary(R[13].ToString(), 16);
                            R13label.BackColor = Color.Red;
                            break;
                        case 14:
                            R14text.Text = Convert_Binary(R[14].ToString(), 16);
                            R14label.BackColor = Color.Red;
                            break;
                        case 15:
                            R15text.Text = Convert_Binary(R[15].ToString(), 16);
                            R15label.BackColor = Color.Red;
                            break;
                    }
                    break;

                case 0x3: //PmSP
                    PmSP();
                    SP = RBUS;
                    SPtext.Text = Convert_Binary(SP.ToString(), 16);
                    break;

                case 0x4: //PmT
                    PmT();
                    T = RBUS;
                    Ttext.Text = Convert_Binary(T.ToString(), 16);
                    break;

                case 0x5: //PmPC
                    PmPC();
                    PC = RBUS;
                    PCtext.Text = Convert_Binary(PC.ToString(), 16);
                    break;

                case 0x6: //PmIVR
                    PmIVR();
                    IVR = RBUS;
                    IVRtext.Text = Convert_Binary(IVR.ToString(), 16);
                    break;

                case 0x7: //PmADR
                    PmADR();
                    ADR = RBUS;
                    ADRtext.Text = Convert_Binary(ADR.ToString(), 16);
                    break;

                case 0x8: //PmMDR
                    PmMDR();
                    MDR = RBUS;
                    MDRtext.Text = Convert_Binary(MDR.ToString(), 16);
                    break;

                default: //none
                    break;
            }
        }

        //decodificator camp Other Operations
        private void DecodifOtherOp()
        {
            UInt16 campOtherOp = (UInt16)((MIR & 0x00000000007C0000) >> 18);
            switch (campOtherOp)
            {
                case 0x1: //PdCOND
                    if (RBUS == 0) // rezultat 0 => setez bitul Z
                    {
                        FLAG = (UInt16)(FLAG | 0x0004);
                    }
                    if (RBUS >> 15 == 0x1) // setez bit de semn S
                    {
                        FLAG = (UInt16)(FLAG | 0x0002);
                    }
                    PdCond();
                    FLAG = (UInt16)(FLAG | (Carry << 3));
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0x2: //CIN + PdCOND
                    if (RBUS == 0) // rezultat 0 => setez bitul Z
                    {
                        FLAG = (UInt16)(FLAG | 0x0004);
                    }
                    if (RBUS >> 15 == 0x1) // setez bit de semn S
                    {
                        FLAG = (UInt16)(FLAG | 0x0002);
                    }
                    PdCond();
                    FLAG = (UInt16)(FLAG | (Carry << 3));
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0x3: //+2SP
                    SPinc();
                    SP += 2;
                    SPtext.Text = Convert_Binary(SP.ToString(), 16);
                    break;
                case 0x4: //-2SP
                    SPdec();
                    SP -= 2;
                    SPtext.Text = Convert_Binary(SP.ToString(), 16);
                    break;
                case 0x5: //+2PC
                    PCinc();
                    PC += 2;
                    PCtext.Text = Convert_Binary(PC.ToString(), 16);
                    break;
                case 0x6: //A(0)BPO
                    break;
                case 0x7: //A(0)C
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG & 0xFFF7);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0x8: //A(1)C
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG | 0x0008);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0x9: //A(0)V
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG & 0xFFFE);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0xA: //A(1)V
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG | 0x0001);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0xB: //A(0)Z
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG & 0xFFFB);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0xC: //A(1)Z
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG | 0x0004);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0xD: //A(0)S
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG & 0xFFFD);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0xE: //A(1)S
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG | 0x0002);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0xF: //A(0)CVZS
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG & 0xFFF0);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0x10: //A(1)CVZS
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG | 0x000F);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0x11: //A(0)BVI
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG & 0x0FF7F);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                case 0x12: //A(1)BVI
                    FLAGlabel.BackColor = Color.Red;
                    FLAG = (UInt16)(FLAG | 0x0080);
                    FLAGtext.Text = Convert_Binary(FLAG.ToString(), 16);
                    break;
                default: //none
                    break;
            }

        }

        #endregion

        //test global function g to set next MAR
        private UInt16 TestG()
        {
            UInt16 nTF, cond = 0;
            nTF = (UInt16)((MIR & 0x0000000000000100) >> 8);
            switch ((MIR & 0x000000000000F000) >> 12)
            {
                case 1: //INT
                    if (isINT == true)
                        cond = 1;
                    break;
                case 2: //C
                    cond = (UInt16)((FLAG & 0x0008) >> 3);
                    break;
                case 3: //Z
                    cond = (UInt16)((FLAG & 0x0004) >> 2);
                    break;
                case 4: //S
                    cond = (UInt16)((FLAG & 0x0002) >> 1);
                    break;
                case 5: //V
                    cond = (UInt16)(FLAG & 0x0001);
                    break;
                case 6:
                    UInt16 ma = (UInt16)((IR & 0x0030) >> 4);
                    if (ma == 1)
                        cond = 1;
                    break;
                case 0x7://CIL
                    break;
                case 0x8://ACLOW
                    break;
                default: //NONE
                    cond = 1; // return 1 daca nT/F = 0(not nT/F)
                    break;
            }
            return (UInt16)(nTF ^ cond);
        }

        //test if there is a memory operation: Ifch,Read or Write
        private void TestIfchReadWrite()
        {
            UInt16 memOp = (UInt16)((MIR & 0x0000000000030000) >> 16);
            switch (memOp)
            {
                case 0x1: //IFCH
                    contor++;
                    Ifch();
                    IR = (UInt16)((UInt16)(RAM[ADR + 1] << 8) | (UInt16)(RAM[ADR]));
                    IRtext.Text = Convert_Binary(IR.ToString(), 16);
                    break;
                case 0x2: //READ
                    Read();
                    MDR = (UInt16)((UInt16)(RAM[ADR + 1] << 8) | (UInt16)(RAM[ADR]));
                    MDRtext.Text = Convert_Binary(MDR.ToString(), 16);
                    break;
                case 0x3: //WRITE
                    Write();
                    RAM[ADR] = (byte)MDR;
                    RAM[ADR + 1] = (byte)((UInt16)(MDR >> 8));
                    UInt16 ADR_2 = (UInt16)(ADR + 1);

                    if (mem.ContainsKey(ADR))
                    {
                        mem[ADR] = RAM[ADR];
                        mem[ADR_2] = RAM[ADR_2];
                    }
                    else
                    {
                        mem.Add(ADR, RAM[ADR]);
                        mem.Add((UInt16)(ADR + 1), RAM[ADR + 1]);
                    }

                    break;
                default: // no mem op
                    break;
            }
        }

        private UInt16 getCl1()
        {
            UInt16 IR15 = (UInt16)((IR & 0x8000) >> 15);
            UInt16 IR13 = (UInt16)((IR & 0x2000) >> 13);
            UInt16 rez = (UInt16)(IR15 & IR13);
            return rez; // IR15 & IR13
        }

        private UInt16 getCl0()
        {
            UInt16 IR15 = (UInt16)((IR & 0x8000) >> 15);
            UInt16 nIR14 = (UInt16)(((~IR) & 0x4000) >> 14);
            UInt16 rez = (UInt16)(IR15 & nIR14);
            return rez; //IR15 & nIR14
        }

        private UInt16 getCl()
        {
            UInt16 CL1 = (UInt16)(getCl1());
            UInt16 CL0 = (UInt16)(getCl0());
            clasa = (UInt16)((CL1 << 1) | CL0);
            return clasa;
        }

        #endregion

        #region Graphics

        private void PdIROffs()
        {
            IRlabel.BackColor = Color.Red;
            PdIRsline.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdIROffd()
        {
            IRlabel.BackColor = Color.Red;
            PdIRdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void PdFLAGs()
        {
            FLAGlabel.BackColor = Color.Red;
            PdFLAGsline.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdFLAGd()
        {
            FLAGlabel.BackColor = Color.Red;
            PdFLAGdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }


        private void PdSPs()
        {
            SPlabel.BackColor = Color.Red;
            PdSPsline.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdSPd()
        {
            SPlabel.BackColor = Color.Red;
            PdSPdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void PdTs()
        {
            PdTslabel.Text = "PdTs";
            Tlabel.BackColor = Color.Red;
            PdTsline.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdTd()
        {
            PdTdlabel.Text = "PdTd";
            Tlabel.BackColor = Color.Red;
            PdTdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void PdnTs()
        {
            PdTslabel.Text = "PdnTs";
            Tlabel.BackColor = Color.Red;
            PdTsline.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdnTd()
        {
            PdTdlabel.Text = "PdnTd";
            Tlabel.BackColor = Color.Red;
            PdTdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void PdPCs()
        {
            PClabel.BackColor = Color.Red;
            PdPCsline.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdPCd()
        {
            PClabel.BackColor = Color.Red;
            PdPCdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void PdIVRs()
        {
            IVRlabel.BackColor = Color.Red;
            PdIVRsline.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdIVRd()
        {
            IVRlabel.BackColor = Color.Red;
            PdIVRdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void PdADRs()
        {
            ADRlabel.BackColor = Color.Red;
            PdADRsline1.BorderColor = Color.Red;
            PdADRsline2.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdADRd()
        {
            ADRlabel.BackColor = Color.Red;
            PdADRdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void PdMDRs()
        {
            MDRlabel.BackColor = Color.Red;
            PdMDRsline1.BorderColor = Color.Red;
            PdMDRsline2.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdMDRd()
        {
            MDRlabel.BackColor = Color.Red;
            PdMDRdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void PdRg0()
        {
            R0label.BackColor = Color.Red;
        }

        private void PdRg1()
        {
            R1label.BackColor = Color.Red;
        }

        private void PdRg2()
        {
            R2label.BackColor = Color.Red;
        }

        private void PdRg3()
        {
            R3label.BackColor = Color.Red;
        }

        private void PdRg4()
        {
            R4label.BackColor = Color.Red;
        }

        private void PdRg5()
        {
            R5label.BackColor = Color.Red;
        }

        private void PdRg6()
        {
            R6label.BackColor = Color.Red;
        }

        private void PdRg7()
        {
            R7label.BackColor = Color.Red;
        }

        private void PdRg8()
        {
            R8label.BackColor = Color.Red;
        }

        private void PdRg9()
        {
            R9label.BackColor = Color.Red;
        }

        private void PdRg10()
        {
            R10label.BackColor = Color.Red;
        }

        private void PdRg11()
        {
            R11label.BackColor = Color.Red;
        }

        private void PdRg12()
        {
            R12label.BackColor = Color.Red;
        }

        private void PdRg13()
        {
            R13label.BackColor = Color.Red;
        }

        private void PdRg14()
        {
            R14label.BackColor = Color.Red;
        }

        private void PdRg15()
        {
            R15label.BackColor = Color.Red;
        }

        private void PdRgs()
        {
            PdRGsline.BorderColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void PdRgd()
        {
            PdRGdline.BorderColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void Pd0s()
        {
            Pd0sline.BorderColor = Color.Red;
            zerolabel.BackColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void Pd0d()
        {
            Pd0dline.BorderColor = Color.Red;
            zerolabel.BackColor = Color.Red;
            DBUSline.FillColor = Color.Red;
            DBUSlabel.ForeColor = Color.Red;
        }

        private void Pdm1s()
        {
            Pdminussline.BorderColor = Color.Red;
            minusunulabel.BackColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void Pd1s()
        {
            Pd1sline.BorderColor = Color.Red;
            unulabel.BackColor = Color.Red;
            SBUSline.FillColor = Color.Red;
            SBUSlabel.ForeColor = Color.Red;
        }

        private void ActivateCIN()
        {
            CINlabel.BackColor = Color.Red;
            CINline.BorderColor = Color.Red;
        }

        private void Alu()
        {
            ALUline1.BorderColor = Color.Red;
            ALUline2.BorderColor = Color.Red;
            PdAluline.BorderColor = Color.Red;
            RBUSShape1.FillColor = Color.Red;
            RBUSShape2.FillColor = Color.Red;
            RBUSShape3.FillColor = Color.Red;
            RBUSlabel.ForeColor = Color.Red;
        }

        private void PmFLAG()
        {
            PmFLAGline.BorderColor = Color.Red;
            PmFLAGline1.BorderColor = Color.Red;
            FLAGlabel.BackColor = Color.Red;
        }

        private void PmSP()
        {
            PmSPline.BorderColor = Color.Red;
            SPlabel.BackColor = Color.Red;
        }

        private void PmT()
        {
            PmTline.BorderColor = Color.Red;
            Tlabel.BackColor = Color.Red;
        }

        private void PmPC()
        {
            PmPCline.BorderColor = Color.Red;
            PClabel.BackColor = Color.Red;
        }

        private void PmIVR()
        {
            PmIVRline.BorderColor = Color.Red;
            IVRlabel.BackColor = Color.Red;
        }

        private void PmADR()
        {
            PmADRline.BorderColor = Color.Red;
            ADRlabel.BackColor = Color.Red;
        }

        private void PmMDR()
        {
            PmMDRline1.BorderColor = Color.Red;
            PmMDRline2.BorderColor = Color.Red;
            MDRlabel.BackColor = Color.Red;
        }

        private void PdCond()
        {
            PdCondline1.BorderColor = Color.Red;
            PdCondline2.BorderColor = Color.Red;
            PdCondline3.BorderColor = Color.Red;
            PmFLAGline1.BorderColor = Color.Red;
            FLAGlabel.BackColor = Color.Red;
        }

        private void SPinc()
        {
            SPlabel.BackColor = Color.Red;
        }

        private void SPdec()
        {
            SPlabel.BackColor = Color.Red;
        }

        private void PCinc()
        {
            PClabel.BackColor = Color.Red;
        }

        private void Ifch()
        {
            PdADRsline2.BorderColor = Color.Red;
            ADRline1.BorderColor = Color.Red;
            ADRline2.BorderColor = Color.Red;
            DOline1.BorderColor = Color.Red;
            DOline5.BorderColor = Color.Red;
            DOline6.BorderColor = Color.Red;
            IRlabel.BackColor = Color.Red;
        }

        private void Read()
        {
            PdADRsline2.BorderColor = Color.Red;
            ADRline1.BorderColor = Color.Red;
            ADRline2.BorderColor = Color.Red;
            DOline1.BorderColor = Color.Red;
            DOline2.BorderColor = Color.Red;
            DOline3.BorderColor = Color.Red;
            PmMDRline2.BorderColor = Color.Red;
            PmMDRline1.BorderColor = Color.Red;
            MDRlabel.BackColor = Color.Red;
        }

        private void Write()
        {
            PdADRsline2.BorderColor = Color.Red;
            ADRline1.BorderColor = Color.Red;
            ADRline2.BorderColor = Color.Red;
            PdMDRsline2.BorderColor = Color.Red;
            DataInline1.BorderColor = Color.Red;
            DataInline2.BorderColor = Color.Red;
            MemShape.FillColor = Color.Red;
        }
        #endregion

        private void stepBtn_Click(object sender, EventArgs e)
        {
            if (compiledOk && microCodeOpened)
            {
                isStep = true;
                runSimulatorStep();
            }
            else
            {
                MessageBox.Show("Something went wrong! Please reload the process!");
            }
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            if (compiledOk && microCodeOpened)
            {
                if (isStep == false)
                {
                    contor = -1;
                    runSimulator();
                }
                else MessageBox.Show("You cannot run the simulator to the end while running step by step!");
            }
            else
            {
                MessageBox.Show("Something went wrong! Please reload the process!");
            }
        }

        //reinitialize registers and simulator layout
        private void resetSimBtn_Click(object sender, EventArgs e)
        {
            resetRegisters();
            resetGraphics();
            resetMemory();
            contor = -1;
        }

        //reinitialize registers
        private void resetRegisters()
        {
            PC = 0; IR = 0; SP = 65534; ; MAR = 0; ADR = 0;
            FLAG = 0; T = 0; IVR = 0; MDR = 0;
            isStep = false;
            for (int i = 0; i < 16; i++)
                R[i] = 0;
            PCtext.Text = "0000000000000000";
            IRtext.Text = "0000000000000000";
            SPtext.Text = Convert_Binary(SP.ToString(), 16);
            ADRtext.Text = "0000000000000000";
            FLAGtext.Text = "0000000000000000";
            Ttext.Text = "0000000000000000";
            IVRtext.Text = "0000000000000000";
            MDRtext.Text = "0000000000000000";
            R0text.Text = "0000000000000000";
            R1text.Text = "0000000000000000";
            R2text.Text = "0000000000000000";
            R3text.Text = "0000000000000000";
            R4text.Text = "0000000000000000";
            R5text.Text = "0000000000000000";
            R6text.Text = "0000000000000000";
            R7text.Text = "0000000000000000";
            R8text.Text = "0000000000000000";
            R9text.Text = "0000000000000000";
            R10text.Text = "0000000000000000";
            R11text.Text = "0000000000000000";
            R12text.Text = "0000000000000000";
            R13text.Text = "0000000000000000";
            R14text.Text = "0000000000000000";
            R15text.Text = "0000000000000000";

            contor = -1;
        }

        private void MainProgram_Load(object sender, EventArgs e)
        {

        }

        private void resetMemory()
        {
            /*if (mem.Count != 0)
            {
                foreach (KeyValuePair<UInt16, byte> line in mem)
                {
                    mem.Remove(line.Key);
                }
            }*/
        }
        private void resetGraphics()
        {
            MemShape.FillColor = SystemColors.InactiveCaption;
            SBUSline.FillColor = Color.SteelBlue;
            DBUSline.FillColor = Color.SteelBlue;
            RBUSShape1.FillColor = Color.SteelBlue;
            RBUSShape2.FillColor = Color.SteelBlue;
            RBUSShape3.FillColor = Color.SteelBlue;
            PmPCline.BorderColor = Color.RoyalBlue;
            PClabel.BackColor = SystemColors.ActiveCaption;
            PdPCdline.BorderColor = Color.RoyalBlue;
            PdPCsline.BorderColor = Color.RoyalBlue;
            PmIVRline.BorderColor = Color.RoyalBlue;
            IVRlabel.BackColor = SystemColors.ActiveCaption;
            PdIRdline.BorderColor = Color.RoyalBlue;
            PdIVRsline.BorderColor = Color.RoyalBlue;
            PmTline.BorderColor = Color.RoyalBlue;
            Tlabel.BackColor = SystemColors.ActiveCaption;
            PdTsline.BorderColor = Color.RoyalBlue;
            PdTdline.BorderColor = Color.RoyalBlue;
            PmSPline.BorderColor = Color.RoyalBlue;
            SPlabel.BackColor = SystemColors.ActiveCaption;
            PdSPsline.BorderColor = Color.RoyalBlue;
            PdSPdline.BorderColor = Color.RoyalBlue;
            PdFLAGdline.BorderColor = Color.RoyalBlue;
            PdFLAGsline.BorderColor = Color.RoyalBlue;
            FLAGlabel.BackColor = SystemColors.ActiveCaption;
            PmFLAGline.BorderColor = Color.RoyalBlue;
            PmFLAGline1.BorderColor = Color.RoyalBlue;
            PdCondline1.BorderColor = Color.RoyalBlue;
            PdCondline2.BorderColor = Color.RoyalBlue;
            PdCondline3.BorderColor = Color.RoyalBlue;
            PdAluline.BorderColor = Color.RoyalBlue;
            ALUlabel.Text = "NONE";
            ALUline1.BorderColor = Color.DarkOrange;
            ALUline2.BorderColor = Color.DarkOrange;
            CINline.BorderColor = Color.RoyalBlue;
            CINlabel.BackColor = SystemColors.ActiveCaption;
            unulabel.BackColor = SystemColors.ActiveCaption;
            minusunulabel.BackColor = SystemColors.ActiveCaption;
            zerolabel.BackColor = SystemColors.ActiveCaption;
            Pd1sline.BorderColor = Color.RoyalBlue;
            Pdminussline.BorderColor = Color.RoyalBlue;
            Pd0dline.BorderColor = Color.RoyalBlue;
            Pd0sline.BorderColor = Color.RoyalBlue;
            PdRGsline.BorderColor = Color.RoyalBlue;
            PdRGdline.BorderColor = Color.RoyalBlue;
            PmRGline.BorderColor = Color.RoyalBlue;
            R0label.BackColor = SystemColors.ActiveCaption;
            R1label.BackColor = SystemColors.ActiveCaption;
            R2label.BackColor = SystemColors.ActiveCaption;
            R3label.BackColor = SystemColors.ActiveCaption;
            R4label.BackColor = SystemColors.ActiveCaption;
            R5label.BackColor = SystemColors.ActiveCaption;
            R6label.BackColor = SystemColors.ActiveCaption;
            R7label.BackColor = SystemColors.ActiveCaption;
            R8label.BackColor = SystemColors.ActiveCaption;
            R9label.BackColor = SystemColors.ActiveCaption;
            R10label.BackColor = SystemColors.ActiveCaption;
            R11label.BackColor = SystemColors.ActiveCaption;
            R12label.BackColor = SystemColors.ActiveCaption;
            R13label.BackColor = SystemColors.ActiveCaption;
            R14label.BackColor = SystemColors.ActiveCaption;
            R15label.BackColor = SystemColors.ActiveCaption;
            PdTslabel.Text = "PdTs";
            PdIRdline.BorderColor = Color.RoyalBlue;
            PdIRsline.BorderColor = Color.RoyalBlue;
            IRlabel.BackColor = SystemColors.ActiveCaption;
            PdADRsline1.BorderColor = Color.RoyalBlue;
            PdADRsline2.BorderColor = Color.RoyalBlue;
            PdADRdline.BorderColor = Color.RoyalBlue;
            ADRline1.BorderColor = Color.RoyalBlue;
            ADRline2.BorderColor = Color.RoyalBlue;
            PmADRline.BorderColor = Color.RoyalBlue;
            ADRlabel.BackColor = SystemColors.ActiveCaption;
            PdMDRsline1.BorderColor = Color.RoyalBlue;
            PdMDRsline2.BorderColor = Color.RoyalBlue;
            PdMDRdline.BorderColor = Color.RoyalBlue;
            DataInline1.BorderColor = Color.RoyalBlue;
            DataInline2.BorderColor = Color.RoyalBlue;
            MDRlabel.BackColor = SystemColors.ActiveCaption;
            PmMDRline1.BorderColor = Color.RoyalBlue;
            PmMDRline2.BorderColor = Color.RoyalBlue;
            DOline1.BorderColor = Color.RoyalBlue;
            DOline2.BorderColor = Color.RoyalBlue;
            DOline3.BorderColor = Color.RoyalBlue;
            DOline4.BorderColor = Color.RoyalBlue;
            DOline5.BorderColor = Color.RoyalBlue;
            DOline6.BorderColor = Color.RoyalBlue;
            SBUSlabel.ForeColor = Color.RoyalBlue;
            DBUSlabel.ForeColor = Color.RoyalBlue;
            RBUSlabel.ForeColor = Color.RoyalBlue;
        }

        private void memoryBtn_Click(object sender, EventArgs e)
        {
            memory = new Memory();
            memory.Show();

            foreach (KeyValuePair<UInt16, byte> line in mem)   //adaug instructiunile in listbox de memorie
            {
                memory.listBoxMem.Items.Add(line.Key + "     " + Convert_Binary(line.Value.ToString(), 8));
            }
        }

    }
}
