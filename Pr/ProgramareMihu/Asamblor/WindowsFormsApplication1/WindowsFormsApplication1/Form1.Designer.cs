namespace WindowsFormsApplication1
{
    partial class InitializeProcessor
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
            this.theDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenMicroInstruction = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // theDialog
            // 
            this.theDialog.DefaultExt = "asm";
            this.theDialog.FileName = "openFileDialog1";
            this.theDialog.Filter = "Asm Files|*.asm";
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(38, 36);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open Asm";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(38, 66);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 1;
            this.btnConvert.Text = "Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "asm";
            this.openFileDialog1.FileName = "openMicroInstrDialog";
            this.openFileDialog1.Filter = "Asm Files|*.asm";
            // 
            // btnOpenMicroInstruction
            // 
            this.btnOpenMicroInstruction.Location = new System.Drawing.Point(137, 36);
            this.btnOpenMicroInstruction.Name = "btnOpenMicroInstruction";
            this.btnOpenMicroInstruction.Size = new System.Drawing.Size(75, 23);
            this.btnOpenMicroInstruction.TabIndex = 2;
            this.btnOpenMicroInstruction.Text = "Open Asm";
            this.btnOpenMicroInstruction.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(237, 126);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // InitializeProcessor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 405);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnOpenMicroInstruction);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnOpen);
            this.Name = "InitializeProcessor";
            this.Text = "Initialize Processor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog theDialog;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOpenMicroInstruction;
        private System.Windows.Forms.Button btnStart;
    }
}

