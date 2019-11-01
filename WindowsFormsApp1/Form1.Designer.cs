namespace WindowsFormsApp1
{
    partial class Form1
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
            this.groupBoxInput = new System.Windows.Forms.GroupBox();
            this.browseFileName = new System.Windows.Forms.TextBox();
            this.browseFileBtn = new System.Windows.Forms.Button();
            this.groupBoxInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxInput
            // 
            this.groupBoxInput.Controls.Add(this.browseFileName);
            this.groupBoxInput.Controls.Add(this.browseFileBtn);
            this.groupBoxInput.Location = new System.Drawing.Point(13, 13);
            this.groupBoxInput.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxInput.Name = "groupBoxInput";
            this.groupBoxInput.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxInput.Size = new System.Drawing.Size(814, 69);
            this.groupBoxInput.TabIndex = 2;
            this.groupBoxInput.TabStop = false;
            this.groupBoxInput.Text = "Import";
            // 
            // browseFileName
            // 
            this.browseFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.browseFileName.Location = new System.Drawing.Point(22, 23);
            this.browseFileName.Margin = new System.Windows.Forms.Padding(4);
            this.browseFileName.Name = "browseFileName";
            this.browseFileName.ReadOnly = true;
            this.browseFileName.Size = new System.Drawing.Size(662, 22);
            this.browseFileName.TabIndex = 0;
            this.browseFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // browseFileBtn
            // 
            this.browseFileBtn.Location = new System.Drawing.Point(706, 19);
            this.browseFileBtn.Margin = new System.Windows.Forms.Padding(4);
            this.browseFileBtn.Name = "browseFileBtn";
            this.browseFileBtn.Size = new System.Drawing.Size(100, 28);
            this.browseFileBtn.TabIndex = 0;
            this.browseFileBtn.Text = "Browse...";
            this.browseFileBtn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 111);
            this.Controls.Add(this.groupBoxInput);
            this.Name = "Form1";
            this.Text = "ML Classsification Tool";
            this.groupBoxInput.ResumeLayout(false);
            this.groupBoxInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxInput;
        private System.Windows.Forms.TextBox browseFileName;
        private System.Windows.Forms.Button browseFileBtn;
    }
}

