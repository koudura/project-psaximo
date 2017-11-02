namespace Corvus._1._0
{
    partial class PerHome
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Controls.Add(this.panel8);
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(849, 601);
            this.panel5.TabIndex = 2;
            this.panel5.Paint += new System.Windows.Forms.PaintEventHandler(this.panel5_Paint);
            // 
            // panel8
            // 
            this.panel8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel8.Controls.Add(this.btnSearch);
            this.panel8.Controls.Add(this.searchBox);
            this.panel8.Location = new System.Drawing.Point(152, 269);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(532, 202);
            this.panel8.TabIndex = 8;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSearch.AutoSize = true;
            this.btnSearch.BackColor = System.Drawing.Color.SlateGray;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearch.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.GreenYellow;
            this.btnSearch.Location = new System.Drawing.Point(438, 3);
            this.btnSearch.MinimumSize = new System.Drawing.Size(77, 41);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 41);
            this.btnSearch.TabIndex = 7;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // searchBox
            // 
            this.searchBox.AcceptsReturn = true;
            this.searchBox.AcceptsTab = true;
            this.searchBox.AllowDrop = true;
            this.searchBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.searchBox.AutoCompleteCustomSource.AddRange(new string[] {
            "Jedi",
            "One",
            "Money"});
            this.searchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.searchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.searchBox.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.searchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchBox.ForeColor = System.Drawing.Color.White;
            this.searchBox.Location = new System.Drawing.Point(24, 11);
            this.searchBox.MaximumSize = new System.Drawing.Size(408, 28);
            this.searchBox.Multiline = true;
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(408, 28);
            this.searchBox.TabIndex = 2;
            this.searchBox.WordWrap = false;
            this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
            // 
            // panel7
            // 
            this.panel7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel7.AutoSize = true;
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Location = new System.Drawing.Point(243, 128);
            this.panel7.MinimumSize = new System.Drawing.Size(312, 115);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(313, 138);
            this.panel7.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Khaki;
            this.label3.Location = new System.Drawing.Point(177, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "on Fornax.Net v1.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe Script", 48F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Linen;
            this.label4.Location = new System.Drawing.Point(14, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(278, 106);
            this.label4.TabIndex = 0;
            this.label4.Text = "Corvus";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.Gray;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 561);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(845, 36);
            this.panel6.TabIndex = 9;
            // 
            // PerHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel5);
            this.Name = "PerHome";
            this.Size = new System.Drawing.Size(849, 601);
            this.Load += new System.EventHandler(this.PerHome_Load);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel panel6;
    }
}
