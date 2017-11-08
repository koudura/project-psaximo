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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.configSelector = new System.Windows.Forms.DomainUpDown();
            this.BtnloadConfig = new System.Windows.Forms.Button();
            this.loadSplit = new System.Windows.Forms.Splitter();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtconfigId = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.rbtnLoadChoice = new System.Windows.Forms.RadioButton();
            this.rbtnCreateChoice = new System.Windows.Forms.RadioButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.extensionsBox = new System.Windows.Forms.GroupBox();
            this.Rtfcheck = new System.Windows.Forms.CheckBox();
            this.Pptcheck = new System.Windows.Forms.CheckBox();
            this.Emlcheck = new System.Windows.Forms.CheckBox();
            this.Xlsxcheck = new System.Windows.Forms.CheckBox();
            this.Xlscheck = new System.Windows.Forms.CheckBox();
            this.Pngcheck = new System.Windows.Forms.CheckBox();
            this.Jpgcheck = new System.Windows.Forms.CheckBox();
            this.Mp4check = new System.Windows.Forms.CheckBox();
            this.Mp3check = new System.Windows.Forms.CheckBox();
            this.Javacheck = new System.Windows.Forms.CheckBox();
            this.Vcfcheck = new System.Windows.Forms.CheckBox();
            this.Pdfcheck = new System.Windows.Forms.CheckBox();
            this.DocXcheck = new System.Windows.Forms.CheckBox();
            this.Cscheck = new System.Windows.Forms.CheckBox();
            this.Xmlcheck = new System.Windows.Forms.CheckBox();
            this.Pptxcheck = new System.Windows.Forms.CheckBox();
            this.Doccheck = new System.Windows.Forms.CheckBox();
            this.Csvcheck = new System.Windows.Forms.CheckBox();
            this.Htmlcheck = new System.Windows.Forms.CheckBox();
            this.Txtcheck = new System.Windows.Forms.CheckBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.btnIndex = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.fbDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.indexWorker = new System.ComponentModel.BackgroundWorker();
            this.indexUpdateNote = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.updateWorker = new System.ComponentModel.BackgroundWorker();
            this.OpenConfigWorker = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.extensionsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.btnIndex);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtFolder);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(554, 432);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.progressBar1.Location = new System.Drawing.Point(0, 423);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(550, 5);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Teal;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Location = new System.Drawing.Point(3, 37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(398, 380);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Controls.Add(this.extensionsBox);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(390, 372);
            this.panel3.TabIndex = 8;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox3.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox3.Location = new System.Drawing.Point(3, 260);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(378, 105);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Indexing";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.configSelector);
            this.groupBox2.Controls.Add(this.BtnloadConfig);
            this.groupBox2.Controls.Add(this.loadSplit);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.TxtconfigId);
            this.groupBox2.Controls.Add(this.btnCreate);
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.splitter1);
            this.groupBox2.Controls.Add(this.panel5);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox2.Location = new System.Drawing.Point(161, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(220, 251);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configurations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(8, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select existing configuration.";
            // 
            // configSelector
            // 
            this.configSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.configSelector.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.configSelector.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.configSelector.ForeColor = System.Drawing.SystemColors.Info;
            this.configSelector.Location = new System.Drawing.Point(11, 98);
            this.configSelector.Name = "configSelector";
            this.configSelector.ReadOnly = true;
            this.configSelector.Size = new System.Drawing.Size(200, 20);
            this.configSelector.Sorted = true;
            this.configSelector.TabIndex = 0;
            this.configSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.configSelector.Wrap = true;
            this.configSelector.SelectedItemChanged += new System.EventHandler(this.domainUpDown1_SelectedItemChanged);
            // 
            // BtnloadConfig
            // 
            this.BtnloadConfig.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.BtnloadConfig.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnloadConfig.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnloadConfig.ForeColor = System.Drawing.Color.DarkRed;
            this.BtnloadConfig.Location = new System.Drawing.Point(59, 124);
            this.BtnloadConfig.Name = "BtnloadConfig";
            this.BtnloadConfig.Size = new System.Drawing.Size(94, 29);
            this.BtnloadConfig.TabIndex = 1;
            this.BtnloadConfig.Text = "select";
            this.BtnloadConfig.UseVisualStyleBackColor = false;
            this.BtnloadConfig.Click += new System.EventHandler(this.BtnloadConfig_Click);
            // 
            // loadSplit
            // 
            this.loadSplit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.loadSplit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.loadSplit.Enabled = false;
            this.loadSplit.Location = new System.Drawing.Point(3, 65);
            this.loadSplit.Name = "loadSplit";
            this.loadSplit.Size = new System.Drawing.Size(214, 94);
            this.loadSplit.TabIndex = 10;
            this.loadSplit.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(8, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = " Create new configuration.";
            // 
            // TxtconfigId
            // 
            this.TxtconfigId.AcceptsReturn = true;
            this.TxtconfigId.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.TxtconfigId.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.TxtconfigId.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TxtconfigId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtconfigId.ForeColor = System.Drawing.SystemColors.Info;
            this.TxtconfigId.Location = new System.Drawing.Point(12, 191);
            this.TxtconfigId.MaxLength = 12;
            this.TxtconfigId.Name = "TxtconfigId";
            this.TxtconfigId.Size = new System.Drawing.Size(197, 20);
            this.TxtconfigId.TabIndex = 2;
            this.TxtconfigId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.TxtconfigId, "Configuration Id");
            this.TxtconfigId.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnCreate.Enabled = false;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCreate.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreate.ForeColor = System.Drawing.Color.DarkRed;
            this.btnCreate.Location = new System.Drawing.Point(65, 215);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(94, 29);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "create";
            this.btnCreate.UseVisualStyleBackColor = false;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.rbtnLoadChoice);
            this.panel4.Controls.Add(this.rbtnCreateChoice);
            this.panel4.Location = new System.Drawing.Point(7, 14);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(207, 37);
            this.panel4.TabIndex = 8;
            // 
            // rbtnLoadChoice
            // 
            this.rbtnLoadChoice.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnLoadChoice.AutoSize = true;
            this.rbtnLoadChoice.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.rbtnLoadChoice.FlatAppearance.CheckedBackColor = System.Drawing.Color.Tomato;
            this.rbtnLoadChoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnLoadChoice.ForeColor = System.Drawing.Color.Beige;
            this.rbtnLoadChoice.Location = new System.Drawing.Point(8, 6);
            this.rbtnLoadChoice.Name = "rbtnLoadChoice";
            this.rbtnLoadChoice.Size = new System.Drawing.Size(43, 25);
            this.rbtnLoadChoice.TabIndex = 6;
            this.rbtnLoadChoice.Text = "Load";
            this.rbtnLoadChoice.UseVisualStyleBackColor = true;
            this.rbtnLoadChoice.CheckedChanged += new System.EventHandler(this.rbtnLoadChoice_CheckedChanged);
            // 
            // rbtnCreateChoice
            // 
            this.rbtnCreateChoice.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnCreateChoice.AutoSize = true;
            this.rbtnCreateChoice.Checked = true;
            this.rbtnCreateChoice.FlatAppearance.BorderColor = System.Drawing.Color.SeaGreen;
            this.rbtnCreateChoice.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.rbtnCreateChoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnCreateChoice.ForeColor = System.Drawing.Color.Beige;
            this.rbtnCreateChoice.Location = new System.Drawing.Point(143, 6);
            this.rbtnCreateChoice.Name = "rbtnCreateChoice";
            this.rbtnCreateChoice.Size = new System.Drawing.Size(55, 25);
            this.rbtnCreateChoice.TabIndex = 7;
            this.rbtnCreateChoice.TabStop = true;
            this.rbtnCreateChoice.Text = "Create";
            this.rbtnCreateChoice.UseVisualStyleBackColor = true;
            this.rbtnCreateChoice.CheckedChanged += new System.EventHandler(this.rbtnCreateChoice_CheckedChanged);
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(3, 159);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(214, 89);
            this.splitter1.TabIndex = 9;
            this.splitter1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Location = new System.Drawing.Point(6, 69);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(208, 90);
            this.panel5.TabIndex = 11;
            // 
            // extensionsBox
            // 
            this.extensionsBox.Controls.Add(this.Rtfcheck);
            this.extensionsBox.Controls.Add(this.Pptcheck);
            this.extensionsBox.Controls.Add(this.Emlcheck);
            this.extensionsBox.Controls.Add(this.Xlsxcheck);
            this.extensionsBox.Controls.Add(this.Xlscheck);
            this.extensionsBox.Controls.Add(this.Pngcheck);
            this.extensionsBox.Controls.Add(this.Jpgcheck);
            this.extensionsBox.Controls.Add(this.Mp4check);
            this.extensionsBox.Controls.Add(this.Mp3check);
            this.extensionsBox.Controls.Add(this.Javacheck);
            this.extensionsBox.Controls.Add(this.Vcfcheck);
            this.extensionsBox.Controls.Add(this.Pdfcheck);
            this.extensionsBox.Controls.Add(this.DocXcheck);
            this.extensionsBox.Controls.Add(this.Cscheck);
            this.extensionsBox.Controls.Add(this.Xmlcheck);
            this.extensionsBox.Controls.Add(this.Pptxcheck);
            this.extensionsBox.Controls.Add(this.Doccheck);
            this.extensionsBox.Controls.Add(this.Csvcheck);
            this.extensionsBox.Controls.Add(this.Htmlcheck);
            this.extensionsBox.Controls.Add(this.Txtcheck);
            this.extensionsBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.extensionsBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extensionsBox.ForeColor = System.Drawing.Color.Beige;
            this.extensionsBox.Location = new System.Drawing.Point(3, 3);
            this.extensionsBox.Name = "extensionsBox";
            this.extensionsBox.Size = new System.Drawing.Size(154, 251);
            this.extensionsBox.TabIndex = 0;
            this.extensionsBox.TabStop = false;
            this.extensionsBox.Text = "Extensions";
            // 
            // Rtfcheck
            // 
            this.Rtfcheck.AutoSize = true;
            this.Rtfcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Rtfcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rtfcheck.ForeColor = System.Drawing.Color.Beige;
            this.Rtfcheck.Location = new System.Drawing.Point(90, 226);
            this.Rtfcheck.Name = "Rtfcheck";
            this.Rtfcheck.Size = new System.Drawing.Size(44, 18);
            this.Rtfcheck.TabIndex = 22;
            this.Rtfcheck.Text = "Rtf";
            this.Rtfcheck.UseVisualStyleBackColor = true;
            // 
            // Pptcheck
            // 
            this.Pptcheck.AutoSize = true;
            this.Pptcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Pptcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pptcheck.ForeColor = System.Drawing.Color.Beige;
            this.Pptcheck.Location = new System.Drawing.Point(15, 226);
            this.Pptcheck.Name = "Pptcheck";
            this.Pptcheck.Size = new System.Drawing.Size(44, 18);
            this.Pptcheck.TabIndex = 21;
            this.Pptcheck.Text = "Ppt";
            this.Pptcheck.UseVisualStyleBackColor = true;
            // 
            // Emlcheck
            // 
            this.Emlcheck.AutoSize = true;
            this.Emlcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Emlcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Emlcheck.ForeColor = System.Drawing.Color.Beige;
            this.Emlcheck.Location = new System.Drawing.Point(90, 203);
            this.Emlcheck.Name = "Emlcheck";
            this.Emlcheck.Size = new System.Drawing.Size(44, 18);
            this.Emlcheck.TabIndex = 20;
            this.Emlcheck.Text = "Eml";
            this.Emlcheck.UseVisualStyleBackColor = true;
            // 
            // Xlsxcheck
            // 
            this.Xlsxcheck.AutoSize = true;
            this.Xlsxcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Xlsxcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Xlsxcheck.ForeColor = System.Drawing.Color.Beige;
            this.Xlsxcheck.Location = new System.Drawing.Point(15, 203);
            this.Xlsxcheck.Name = "Xlsxcheck";
            this.Xlsxcheck.Size = new System.Drawing.Size(51, 18);
            this.Xlsxcheck.TabIndex = 19;
            this.Xlsxcheck.Text = "Xlsx";
            this.Xlsxcheck.UseVisualStyleBackColor = true;
            // 
            // Xlscheck
            // 
            this.Xlscheck.AutoSize = true;
            this.Xlscheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Xlscheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Xlscheck.ForeColor = System.Drawing.Color.Beige;
            this.Xlscheck.Location = new System.Drawing.Point(90, 180);
            this.Xlscheck.Name = "Xlscheck";
            this.Xlscheck.Size = new System.Drawing.Size(44, 18);
            this.Xlscheck.TabIndex = 18;
            this.Xlscheck.Text = "Xls";
            this.Xlscheck.UseVisualStyleBackColor = true;
            // 
            // Pngcheck
            // 
            this.Pngcheck.AutoSize = true;
            this.Pngcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Pngcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pngcheck.ForeColor = System.Drawing.Color.Beige;
            this.Pngcheck.Location = new System.Drawing.Point(15, 180);
            this.Pngcheck.Name = "Pngcheck";
            this.Pngcheck.Size = new System.Drawing.Size(44, 18);
            this.Pngcheck.TabIndex = 17;
            this.Pngcheck.Text = "Png";
            this.Pngcheck.UseVisualStyleBackColor = true;
            // 
            // Jpgcheck
            // 
            this.Jpgcheck.AutoSize = true;
            this.Jpgcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Jpgcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Jpgcheck.ForeColor = System.Drawing.Color.Beige;
            this.Jpgcheck.Location = new System.Drawing.Point(90, 157);
            this.Jpgcheck.Name = "Jpgcheck";
            this.Jpgcheck.Size = new System.Drawing.Size(44, 18);
            this.Jpgcheck.TabIndex = 16;
            this.Jpgcheck.Text = "Jpg";
            this.Jpgcheck.UseVisualStyleBackColor = true;
            // 
            // Mp4check
            // 
            this.Mp4check.AutoSize = true;
            this.Mp4check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Mp4check.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mp4check.ForeColor = System.Drawing.Color.Beige;
            this.Mp4check.Location = new System.Drawing.Point(15, 157);
            this.Mp4check.Name = "Mp4check";
            this.Mp4check.Size = new System.Drawing.Size(44, 18);
            this.Mp4check.TabIndex = 15;
            this.Mp4check.Text = "Mp4";
            this.Mp4check.UseVisualStyleBackColor = true;
            // 
            // Mp3check
            // 
            this.Mp3check.AutoSize = true;
            this.Mp3check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Mp3check.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Mp3check.ForeColor = System.Drawing.Color.Beige;
            this.Mp3check.Location = new System.Drawing.Point(90, 134);
            this.Mp3check.Name = "Mp3check";
            this.Mp3check.Size = new System.Drawing.Size(44, 18);
            this.Mp3check.TabIndex = 14;
            this.Mp3check.Text = "Mp3";
            this.Mp3check.UseVisualStyleBackColor = true;
            // 
            // Javacheck
            // 
            this.Javacheck.AutoSize = true;
            this.Javacheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Javacheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Javacheck.ForeColor = System.Drawing.Color.Beige;
            this.Javacheck.Location = new System.Drawing.Point(15, 134);
            this.Javacheck.Name = "Javacheck";
            this.Javacheck.Size = new System.Drawing.Size(51, 18);
            this.Javacheck.TabIndex = 13;
            this.Javacheck.Text = "Java";
            this.Javacheck.UseVisualStyleBackColor = true;
            // 
            // Vcfcheck
            // 
            this.Vcfcheck.AutoSize = true;
            this.Vcfcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Vcfcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Vcfcheck.ForeColor = System.Drawing.Color.Beige;
            this.Vcfcheck.Location = new System.Drawing.Point(90, 111);
            this.Vcfcheck.Name = "Vcfcheck";
            this.Vcfcheck.Size = new System.Drawing.Size(44, 18);
            this.Vcfcheck.TabIndex = 12;
            this.Vcfcheck.Text = "Vcf";
            this.Vcfcheck.UseVisualStyleBackColor = true;
            // 
            // Pdfcheck
            // 
            this.Pdfcheck.AutoSize = true;
            this.Pdfcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Pdfcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pdfcheck.ForeColor = System.Drawing.Color.Beige;
            this.Pdfcheck.Location = new System.Drawing.Point(15, 111);
            this.Pdfcheck.Name = "Pdfcheck";
            this.Pdfcheck.Size = new System.Drawing.Size(44, 18);
            this.Pdfcheck.TabIndex = 11;
            this.Pdfcheck.Text = "Pdf";
            this.Pdfcheck.UseVisualStyleBackColor = true;
            // 
            // DocXcheck
            // 
            this.DocXcheck.AutoSize = true;
            this.DocXcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DocXcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocXcheck.ForeColor = System.Drawing.Color.Beige;
            this.DocXcheck.Location = new System.Drawing.Point(90, 88);
            this.DocXcheck.Name = "DocXcheck";
            this.DocXcheck.Size = new System.Drawing.Size(51, 18);
            this.DocXcheck.TabIndex = 10;
            this.DocXcheck.Text = "Docx";
            this.DocXcheck.UseVisualStyleBackColor = true;
            // 
            // Cscheck
            // 
            this.Cscheck.AutoSize = true;
            this.Cscheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Cscheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cscheck.ForeColor = System.Drawing.Color.Beige;
            this.Cscheck.Location = new System.Drawing.Point(90, 65);
            this.Cscheck.Name = "Cscheck";
            this.Cscheck.Size = new System.Drawing.Size(37, 18);
            this.Cscheck.TabIndex = 9;
            this.Cscheck.Text = "Cs";
            this.Cscheck.UseVisualStyleBackColor = true;
            // 
            // Xmlcheck
            // 
            this.Xmlcheck.AutoSize = true;
            this.Xmlcheck.Checked = true;
            this.Xmlcheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Xmlcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Xmlcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Xmlcheck.ForeColor = System.Drawing.Color.Beige;
            this.Xmlcheck.Location = new System.Drawing.Point(90, 42);
            this.Xmlcheck.Name = "Xmlcheck";
            this.Xmlcheck.Size = new System.Drawing.Size(44, 18);
            this.Xmlcheck.TabIndex = 8;
            this.Xmlcheck.Text = "Xml";
            this.Xmlcheck.UseVisualStyleBackColor = true;
            // 
            // Pptxcheck
            // 
            this.Pptxcheck.AutoSize = true;
            this.Pptxcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Pptxcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Pptxcheck.ForeColor = System.Drawing.Color.Beige;
            this.Pptxcheck.Location = new System.Drawing.Point(90, 19);
            this.Pptxcheck.Name = "Pptxcheck";
            this.Pptxcheck.Size = new System.Drawing.Size(51, 18);
            this.Pptxcheck.TabIndex = 7;
            this.Pptxcheck.Text = "Pptx";
            this.Pptxcheck.UseVisualStyleBackColor = true;
            // 
            // Doccheck
            // 
            this.Doccheck.AutoSize = true;
            this.Doccheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Doccheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Doccheck.ForeColor = System.Drawing.Color.Beige;
            this.Doccheck.Location = new System.Drawing.Point(15, 88);
            this.Doccheck.Name = "Doccheck";
            this.Doccheck.Size = new System.Drawing.Size(44, 18);
            this.Doccheck.TabIndex = 6;
            this.Doccheck.Text = "Doc";
            this.Doccheck.UseVisualStyleBackColor = true;
            // 
            // Csvcheck
            // 
            this.Csvcheck.AutoSize = true;
            this.Csvcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Csvcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Csvcheck.ForeColor = System.Drawing.Color.Beige;
            this.Csvcheck.Location = new System.Drawing.Point(15, 65);
            this.Csvcheck.Name = "Csvcheck";
            this.Csvcheck.Size = new System.Drawing.Size(44, 18);
            this.Csvcheck.TabIndex = 5;
            this.Csvcheck.Text = "Csv";
            this.Csvcheck.UseVisualStyleBackColor = true;
            // 
            // Htmlcheck
            // 
            this.Htmlcheck.AutoSize = true;
            this.Htmlcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Htmlcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Htmlcheck.ForeColor = System.Drawing.Color.Beige;
            this.Htmlcheck.Location = new System.Drawing.Point(15, 42);
            this.Htmlcheck.Name = "Htmlcheck";
            this.Htmlcheck.Size = new System.Drawing.Size(51, 18);
            this.Htmlcheck.TabIndex = 4;
            this.Htmlcheck.Text = "Html";
            this.Htmlcheck.UseVisualStyleBackColor = true;
            // 
            // Txtcheck
            // 
            this.Txtcheck.AutoSize = true;
            this.Txtcheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Txtcheck.Checked = true;
            this.Txtcheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Txtcheck.Enabled = false;
            this.Txtcheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Txtcheck.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txtcheck.ForeColor = System.Drawing.Color.Beige;
            this.Txtcheck.Location = new System.Drawing.Point(15, 19);
            this.Txtcheck.Name = "Txtcheck";
            this.Txtcheck.Size = new System.Drawing.Size(44, 18);
            this.Txtcheck.TabIndex = 3;
            this.Txtcheck.Text = "Txt";
            this.Txtcheck.UseVisualStyleBackColor = false;
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
            this.btnIndex.BackColor = System.Drawing.Color.SeaGreen;
            this.btnIndex.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnIndex.Enabled = false;
            this.btnIndex.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIndex.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIndex.ForeColor = System.Drawing.Color.DarkRed;
            this.btnIndex.Location = new System.Drawing.Point(407, 373);
            this.btnIndex.Name = "btnIndex";
            this.btnIndex.Size = new System.Drawing.Size(137, 44);
            this.btnIndex.TabIndex = 2;
            this.btnIndex.Text = "Proceed";
            this.btnIndex.UseVisualStyleBackColor = false;
            this.btnIndex.Click += new System.EventHandler(this.btnIndex_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnBrowse.FlatAppearance.BorderColor = System.Drawing.Color.Honeydew;
            this.btnBrowse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CadetBlue;
            this.btnBrowse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSlateGray;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ForeColor = System.Drawing.Color.GhostWhite;
            this.btnBrowse.Location = new System.Drawing.Point(481, 11);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(63, 20);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "...";
            this.btnBrowse.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.txtFolder.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFolder.ForeColor = System.Drawing.Color.Beige;
            this.txtFolder.Location = new System.Drawing.Point(3, 11);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(472, 20);
            this.txtFolder.TabIndex = 0;
            this.txtFolder.TextChanged += new System.EventHandler(this.txtFolder_TextChanged);
            // 
            // fbDialog
            // 
            this.fbDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // indexWorker
            // 
            this.indexWorker.WorkerReportsProgress = true;
            this.indexWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.indexWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.indexWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // indexUpdateNote
            // 
            this.indexUpdateNote.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.indexUpdateNote.BalloonTipText = "Corvus Indexing..";
            this.indexUpdateNote.BalloonTipTitle = "Corvus Indexer.";
            this.indexUpdateNote.Text = "Indexing of files by Corvus is complete.\r\n";
            this.indexUpdateNote.Visible = true;
            this.indexUpdateNote.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.Gray;
            // 
            // updateWorker
            // 
            this.updateWorker.WorkerReportsProgress = true;
            this.updateWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.updateWorker_DoWork);
            this.updateWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.updateWorker_RunWorkerCompleted);
            // 
            // OpenConfigWorker
            // 
            this.OpenConfigWorker.WorkerReportsProgress = true;
            this.OpenConfigWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.OpenConfigWorker_DoWork);
            this.OpenConfigWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.OpenConfigWorker_ProgressChanged);
            this.OpenConfigWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.OpenConfigWorker_RunWorkerCompleted);
            // 
            // ConfigWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(579, 457);
            this.Controls.Add(this.panel1);
            this.ForeColor = System.Drawing.Color.MediumSeaGreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.HelpButton = true;
            this.Name = "ConfigWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ConfigWin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigWin_FormClosing);
            this.Load += new System.EventHandler(this.ConfigWin_Load);
            this.Shown += new System.EventHandler(this.ConfigWin_Shown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ConfigWin_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ConfigWin_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ConfigWin_MouseUp);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.extensionsBox.ResumeLayout(false);
            this.extensionsBox.PerformLayout();
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
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox extensionsBox;
        private System.Windows.Forms.CheckBox Txtcheck;
        private System.Windows.Forms.CheckBox Rtfcheck;
        private System.Windows.Forms.CheckBox Pptcheck;
        private System.Windows.Forms.CheckBox Emlcheck;
        private System.Windows.Forms.CheckBox Xlsxcheck;
        private System.Windows.Forms.CheckBox Xlscheck;
        private System.Windows.Forms.CheckBox Pngcheck;
        private System.Windows.Forms.CheckBox Jpgcheck;
        private System.Windows.Forms.CheckBox Mp4check;
        private System.Windows.Forms.CheckBox Mp3check;
        private System.Windows.Forms.CheckBox Javacheck;
        private System.Windows.Forms.CheckBox Vcfcheck;
        private System.Windows.Forms.CheckBox Pdfcheck;
        private System.Windows.Forms.CheckBox DocXcheck;
        private System.Windows.Forms.CheckBox Cscheck;
        private System.Windows.Forms.CheckBox Xmlcheck;
        private System.Windows.Forms.CheckBox Pptxcheck;
        private System.Windows.Forms.CheckBox Doccheck;
        private System.Windows.Forms.CheckBox Csvcheck;
        private System.Windows.Forms.CheckBox Htmlcheck;
        internal System.Windows.Forms.NotifyIcon indexUpdateNote;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DomainUpDown configSelector;
        private System.Windows.Forms.TextBox TxtconfigId;
        private System.Windows.Forms.Button BtnloadConfig;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCreate;
        internal System.ComponentModel.BackgroundWorker indexWorker;
        private System.ComponentModel.BackgroundWorker updateWorker;
        private System.Windows.Forms.RadioButton rbtnCreateChoice;
        private System.Windows.Forms.RadioButton rbtnLoadChoice;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Splitter loadSplit;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel5;
        private System.ComponentModel.BackgroundWorker OpenConfigWorker;
    }
}