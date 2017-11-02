namespace Corvus._1._0
{
    partial class ConfigWin
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lsbxExt = new System.Windows.Forms.ListBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnIndex = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.fbDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.btnIndex);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtFolder);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(554, 467);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lsbxExt);
            this.panel2.Location = new System.Drawing.Point(24, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(376, 343);
            this.panel2.TabIndex = 5;
            // 
            // lsbxExt
            // 
            this.lsbxExt.FormattingEnabled = true;
            this.lsbxExt.Items.AddRange(new object[] {
            "C",
            "Cs",
            "Csv",
            "Doc",
            "Docx",
            "Epub",
            "Gzip",
            "Html",
            "Java",
            "Jpeg",
            "Jpg",
            "Mp3",
            "Pdf",
            "Png",
            "Ppt",
            "Pptx",
            "Rar",
            "Rtf",
            "Txt",
            "Xlsx",
            "Xml",
            "Zip"});
            this.lsbxExt.Location = new System.Drawing.Point(8, 6);
            this.lsbxExt.Name = "lsbxExt";
            this.lsbxExt.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsbxExt.Size = new System.Drawing.Size(82, 329);
            this.lsbxExt.Sorted = true;
            this.lsbxExt.TabIndex = 7;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(143, 439);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(0, 13);
            this.lblTime.TabIndex = 4;
            // 
            // btnIndex
            // 
            this.btnIndex.Enabled = false;
            this.btnIndex.Location = new System.Drawing.Point(404, 408);
            this.btnIndex.Name = "btnIndex";
            this.btnIndex.Size = new System.Drawing.Size(137, 52);
            this.btnIndex.TabIndex = 2;
            this.btnIndex.Text = "Proceed";
            this.btnIndex.UseVisualStyleBackColor = true;
            this.btnIndex.Click += new System.EventHandler(this.btnIndex_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.Color.DarkGray;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(455, 46);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(86, 36);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFolder.Location = new System.Drawing.Point(2, 12);
            this.txtFolder.Multiline = true;
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(544, 28);
            this.txtFolder.TabIndex = 0;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            // 
            // fbDialog
            // 
            this.fbDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.progressBar1.Location = new System.Drawing.Point(24, 429);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(242, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 6;
            // 
            // ConfigWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(579, 492);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Name = "ConfigWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConfigWin";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigWin_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.FolderBrowserDialog fbDialog;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnIndex;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lsbxExt;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}