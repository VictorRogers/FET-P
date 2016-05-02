namespace FETP_GUI
{
    partial class FETP_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FETP_Form));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newScheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openConstraintsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.as_PDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.as_textToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oneDayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fullScheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.auth1 = new FETP_GUI.Auth();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(258, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newScheduleToolStripMenuItem,
            this.openConstraintsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newScheduleToolStripMenuItem
            // 
            this.newScheduleToolStripMenuItem.Name = "newScheduleToolStripMenuItem";
            this.newScheduleToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.newScheduleToolStripMenuItem.Text = "New Schedule";
            this.newScheduleToolStripMenuItem.Click += new System.EventHandler(this.newScheduleToolStripMenuItem_Click);
            // 
            // openConstraintsToolStripMenuItem
            // 
            this.openConstraintsToolStripMenuItem.Name = "openConstraintsToolStripMenuItem";
            this.openConstraintsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.openConstraintsToolStripMenuItem.Text = "Open Constraints";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.openToolStripMenuItem.Text = "Open Schedule";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.saveAsToolStripMenuItem.Text = "Save";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.as_PDFToolStripMenuItem,
            this.as_textToolStripMenuItem1});
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // as_PDFToolStripMenuItem
            // 
            this.as_PDFToolStripMenuItem.Name = "as_PDFToolStripMenuItem";
            this.as_PDFToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
            this.as_PDFToolStripMenuItem.Text = "As PDF";
            this.as_PDFToolStripMenuItem.Click += new System.EventHandler(this.as_PDFToolStripMenuItem_Click);
            // 
            // as_textToolStripMenuItem1
            // 
            this.as_textToolStripMenuItem1.Name = "as_textToolStripMenuItem1";
            this.as_textToolStripMenuItem1.Size = new System.Drawing.Size(111, 22);
            this.as_textToolStripMenuItem1.Text = "As Text";
            this.as_textToolStripMenuItem1.Click += new System.EventHandler(this.as_textToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oneDayToolStripMenuItem,
            this.fullScheduleToolStripMenuItem,
            this.textToolStripMenuItem});
            this.viewToolStripMenuItem.Enabled = false;
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // oneDayToolStripMenuItem
            // 
            this.oneDayToolStripMenuItem.Name = "oneDayToolStripMenuItem";
            this.oneDayToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.oneDayToolStripMenuItem.Text = "One Day";
            this.oneDayToolStripMenuItem.Click += new System.EventHandler(this.oneDayToolStripMenuItem_Click);
            // 
            // fullScheduleToolStripMenuItem
            // 
            this.fullScheduleToolStripMenuItem.Name = "fullScheduleToolStripMenuItem";
            this.fullScheduleToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.fullScheduleToolStripMenuItem.Text = "Full Schedule";
            this.fullScheduleToolStripMenuItem.Click += new System.EventHandler(this.fullScheduleToolStripMenuItem_Click);
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.textToolStripMenuItem.Text = "Text";
            this.textToolStripMenuItem.Click += new System.EventHandler(this.textToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
            this.panel1.Controls.Add(this.auth1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(258, 199);
            this.panel1.TabIndex = 6;
            // 
            // auth1
            // 
            this.auth1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(22)))), ((int)(((byte)(107)))));
            this.auth1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.auth1.Location = new System.Drawing.Point(0, 0);
            this.auth1.Name = "auth1";
            this.auth1.Size = new System.Drawing.Size(258, 199);
            this.auth1.TabIndex = 1;
            this.auth1.Login += new FETP_GUI.Auth.LoginClickHandler(this.Login);
            // 
            // FETP_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(258, 199);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FETP_Form";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FETP_Form";
            this.Load += new System.EventHandler(this.FETP_Form_Load);
            this.Resize += new System.EventHandler(this.FETP_Form_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem oneDayToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem fullScheduleToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem newScheduleToolStripMenuItem;
        private Auth auth1;
        private System.Windows.Forms.ToolStripMenuItem openConstraintsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem as_PDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem as_textToolStripMenuItem1;
        //private DataCollection dataCollection1;
    }
}