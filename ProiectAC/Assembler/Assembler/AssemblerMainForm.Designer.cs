namespace Assembler
{
    partial class AssemblerMainForm
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
            this.ParseFileButton = new System.Windows.Forms.Button();
            this.ASMFileLabel = new System.Windows.Forms.Label();
            this.ASMFileTextBox = new System.Windows.Forms.TextBox();
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.OutputTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ParseFileButton
            // 
            this.ParseFileButton.Enabled = false;
            this.ParseFileButton.Location = new System.Drawing.Point(25, 106);
            this.ParseFileButton.Name = "ParseFileButton";
            this.ParseFileButton.Size = new System.Drawing.Size(118, 38);
            this.ParseFileButton.TabIndex = 0;
            this.ParseFileButton.Text = "Parse ASM file";
            this.ParseFileButton.UseVisualStyleBackColor = true;
            this.ParseFileButton.Click += new System.EventHandler(this.ParseFileButton_Click);
            // 
            // ASMFileLabel
            // 
            this.ASMFileLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ASMFileLabel.AutoSize = true;
            this.ASMFileLabel.Location = new System.Drawing.Point(22, 24);
            this.ASMFileLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ASMFileLabel.Name = "ASMFileLabel";
            this.ASMFileLabel.Size = new System.Drawing.Size(98, 17);
            this.ASMFileLabel.TabIndex = 31;
            this.ASMFileLabel.Text = "ASM file name";
            // 
            // ASMFileTextBox
            // 
            this.ASMFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ASMFileTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ASMFileTextBox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ASMFileTextBox.Location = new System.Drawing.Point(25, 45);
            this.ASMFileTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ASMFileTextBox.Name = "ASMFileTextBox";
            this.ASMFileTextBox.Size = new System.Drawing.Size(494, 22);
            this.ASMFileTextBox.TabIndex = 29;
            this.ASMFileTextBox.Text = "Select the file which contains the asm code";
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.OpenFileButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenFileButton.Location = new System.Drawing.Point(536, 45);
            this.OpenFileButton.Margin = new System.Windows.Forms.Padding(4);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(31, 28);
            this.OpenFileButton.TabIndex = 30;
            this.OpenFileButton.Text = "...";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Location = new System.Drawing.Point(25, 171);
            this.OutputTextBox.Multiline = true;
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OutputTextBox.Size = new System.Drawing.Size(542, 265);
            this.OutputTextBox.TabIndex = 32;
            // 
            // AssemblerMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 533);
            this.Controls.Add(this.OutputTextBox);
            this.Controls.Add(this.ASMFileLabel);
            this.Controls.Add(this.ASMFileTextBox);
            this.Controls.Add(this.OpenFileButton);
            this.Controls.Add(this.ParseFileButton);
            this.Name = "AssemblerMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Assembler for didactical processor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ParseFileButton;
        private System.Windows.Forms.Label ASMFileLabel;
        private System.Windows.Forms.TextBox ASMFileTextBox;
        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.TextBox OutputTextBox;
    }
}

