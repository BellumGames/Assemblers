namespace WindowsFormsApplication1
{
    partial class Secven
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listboxAsmInstr = new System.Windows.Forms.ListBox();
            this.listBoxRegister = new System.Windows.Forms.ListBox();
            this.listboxMachineCode = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listboxAsmInstr
            // 
            this.listboxAsmInstr.FormattingEnabled = true;
            this.listboxAsmInstr.Location = new System.Drawing.Point(12, 12);
            this.listboxAsmInstr.Name = "listboxAsmInstr";
            this.listboxAsmInstr.Size = new System.Drawing.Size(253, 212);
            this.listboxAsmInstr.TabIndex = 0;
            // 
            // listBoxRegister
            // 
            this.listBoxRegister.FormattingEnabled = true;
            this.listBoxRegister.Location = new System.Drawing.Point(557, 12);
            this.listBoxRegister.Name = "listBoxRegister";
            this.listBoxRegister.Size = new System.Drawing.Size(278, 212);
            this.listBoxRegister.TabIndex = 1;
            // 
            // listboxMachineCode
            // 
            this.listboxMachineCode.FormattingEnabled = true;
            this.listboxMachineCode.Location = new System.Drawing.Point(284, 12);
            this.listboxMachineCode.Name = "listboxMachineCode";
            this.listboxMachineCode.Size = new System.Drawing.Size(255, 212);
            this.listboxMachineCode.TabIndex = 2;
            // 
            // Secven
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 481);
            this.Controls.Add(this.listboxMachineCode);
            this.Controls.Add(this.listBoxRegister);
            this.Controls.Add(this.listboxAsmInstr);
            this.Name = "Secven";
            this.Text = "Secven";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listboxAsmInstr;
        private System.Windows.Forms.ListBox listBoxRegister;
        private System.Windows.Forms.ListBox listboxMachineCode;
    }
}