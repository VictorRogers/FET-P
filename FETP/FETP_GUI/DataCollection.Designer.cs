namespace FETP_GUI
{
    partial class DataCollection
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.generate = new System.Windows.Forms.Button();
            this.clear = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label9 = new System.Windows.Forms.Label();
            this.scheduleBrowse_textBox = new System.Windows.Forms.TextBox();
            this.lunchLength_textBox = new System.Windows.Forms.TextBox();
            this.breakLength_textBox = new System.Windows.Forms.TextBox();
            this.examLength_textBox = new System.Windows.Forms.TextBox();
            this.days_textBox = new System.Windows.Forms.TextBox();
            this.scheduleBrowse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.startTime_textBox = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.enrollmentBrowse = new System.Windows.Forms.Button();
            this.enrollmentBrowse_textBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // generate
            // 
            this.generate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.generate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(105)))), ((int)(((byte)(18)))));
            this.generate.FlatAppearance.BorderSize = 2;
            this.generate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.generate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.generate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
            this.generate.Location = new System.Drawing.Point(176, 303);
            this.generate.Name = "generate";
            this.generate.Size = new System.Drawing.Size(87, 25);
            this.generate.TabIndex = 5;
            this.generate.Text = "Generate";
            this.generate.UseVisualStyleBackColor = false;
            this.generate.Click += new System.EventHandler(this.generate_Click);
            // 
            // clear
            // 
            this.clear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.clear.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(105)))), ((int)(((byte)(18)))));
            this.clear.FlatAppearance.BorderSize = 2;
            this.clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
            this.clear.Location = new System.Drawing.Point(68, 303);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(87, 25);
            this.clear.TabIndex = 4;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = false;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(22)))), ((int)(((byte)(107)))));
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.scheduleBrowse_textBox);
            this.splitContainer1.Panel1.Controls.Add(this.lunchLength_textBox);
            this.splitContainer1.Panel1.Controls.Add(this.breakLength_textBox);
            this.splitContainer1.Panel1.Controls.Add(this.examLength_textBox);
            this.splitContainer1.Panel1.Controls.Add(this.days_textBox);
            this.splitContainer1.Panel1.Controls.Add(this.scheduleBrowse);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.startTime_textBox);
            this.splitContainer1.Panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(22)))), ((int)(((byte)(107)))));
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.enrollmentBrowse);
            this.splitContainer1.Panel2.Controls.Add(this.enrollmentBrowse_textBox);
            this.splitContainer1.Size = new System.Drawing.Size(331, 297);
            this.splitContainer1.SplitterDistance = 189;
            this.splitContainer1.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(15, 164);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "File path:";
            // 
            // scheduleBrowse_textBox
            // 
            this.scheduleBrowse_textBox.Location = new System.Drawing.Point(74, 161);
            this.scheduleBrowse_textBox.Name = "scheduleBrowse_textBox";
            this.scheduleBrowse_textBox.Size = new System.Drawing.Size(243, 20);
            this.scheduleBrowse_textBox.TabIndex = 17;
            this.scheduleBrowse_textBox.TextChanged += new System.EventHandler(this.scheduleBrowse_textBox_TextChanged);
            // 
            // lunchLength_textBox
            // 
            this.lunchLength_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lunchLength_textBox.Location = new System.Drawing.Point(107, 135);
            this.lunchLength_textBox.Name = "lunchLength_textBox";
            this.lunchLength_textBox.Size = new System.Drawing.Size(116, 20);
            this.lunchLength_textBox.TabIndex = 16;
            // 
            // breakLength_textBox
            // 
            this.breakLength_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.breakLength_textBox.Location = new System.Drawing.Point(107, 109);
            this.breakLength_textBox.Name = "breakLength_textBox";
            this.breakLength_textBox.Size = new System.Drawing.Size(116, 20);
            this.breakLength_textBox.TabIndex = 15;
            // 
            // examLength_textBox
            // 
            this.examLength_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.examLength_textBox.Location = new System.Drawing.Point(107, 83);
            this.examLength_textBox.Name = "examLength_textBox";
            this.examLength_textBox.Size = new System.Drawing.Size(116, 20);
            this.examLength_textBox.TabIndex = 14;
            // 
            // days_textBox
            // 
            this.days_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.days_textBox.Location = new System.Drawing.Point(107, 31);
            this.days_textBox.Name = "days_textBox";
            this.days_textBox.Size = new System.Drawing.Size(116, 20);
            this.days_textBox.TabIndex = 12;
            // 
            // scheduleBrowse
            // 
            this.scheduleBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.scheduleBrowse.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(105)))), ((int)(((byte)(18)))));
            this.scheduleBrowse.FlatAppearance.BorderSize = 2;
            this.scheduleBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scheduleBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scheduleBrowse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
            this.scheduleBrowse.Location = new System.Drawing.Point(231, 80);
            this.scheduleBrowse.Name = "scheduleBrowse";
            this.scheduleBrowse.Size = new System.Drawing.Size(87, 25);
            this.scheduleBrowse.TabIndex = 11;
            this.scheduleBrowse.Text = "Browse...";
            this.scheduleBrowse.UseVisualStyleBackColor = false;
            this.scheduleBrowse.Click += new System.EventHandler(this.scheduleBrowse_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.label6.Location = new System.Drawing.Point(13, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Lunch Length";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.label5.Location = new System.Drawing.Point(13, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Break Length";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.label4.Location = new System.Drawing.Point(13, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Exam Length";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.label3.Location = new System.Drawing.Point(13, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Begin Time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.label2.Location = new System.Drawing.Point(14, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Days";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.label1.Location = new System.Drawing.Point(104, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Schedule Information";
            // 
            // startTime_textBox
            // 
            this.startTime_textBox.Location = new System.Drawing.Point(107, 57);
            this.startTime_textBox.Mask = "00:00";
            this.startTime_textBox.Name = "startTime_textBox";
            this.startTime_textBox.Size = new System.Drawing.Size(116, 20);
            this.startTime_textBox.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.label8.Location = new System.Drawing.Point(15, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "File path:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.label7.Location = new System.Drawing.Point(118, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Enrollment Data";
            // 
            // enrollmentBrowse
            // 
            this.enrollmentBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(159)))), ((int)(((byte)(17)))));
            this.enrollmentBrowse.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(105)))), ((int)(((byte)(18)))));
            this.enrollmentBrowse.FlatAppearance.BorderSize = 2;
            this.enrollmentBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.enrollmentBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enrollmentBrowse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(12)))), ((int)(((byte)(73)))));
            this.enrollmentBrowse.Location = new System.Drawing.Point(232, 69);
            this.enrollmentBrowse.Name = "enrollmentBrowse";
            this.enrollmentBrowse.Size = new System.Drawing.Size(87, 25);
            this.enrollmentBrowse.TabIndex = 33;
            this.enrollmentBrowse.Text = "Browse...";
            this.enrollmentBrowse.UseVisualStyleBackColor = false;
            this.enrollmentBrowse.Click += new System.EventHandler(this.enrollmentBrowse_Click);
            // 
            // enrollmentBrowse_textBox
            // 
            this.enrollmentBrowse_textBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enrollmentBrowse_textBox.Location = new System.Drawing.Point(74, 33);
            this.enrollmentBrowse_textBox.Name = "enrollmentBrowse_textBox";
            this.enrollmentBrowse_textBox.Size = new System.Drawing.Size(243, 20);
            this.enrollmentBrowse_textBox.TabIndex = 32;
            // 
            // DataCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.generate);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.splitContainer1);
            this.Name = "DataCollection";
            this.Size = new System.Drawing.Size(331, 334);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button generate;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.TextBox lunchLength_textBox;
        public System.Windows.Forms.TextBox breakLength_textBox;
        public System.Windows.Forms.TextBox examLength_textBox;
        public System.Windows.Forms.MaskedTextBox startTime_textBox;
        public System.Windows.Forms.TextBox days_textBox;
        private System.Windows.Forms.Button scheduleBrowse;
        public System.Windows.Forms.TextBox scheduleBrowse_textBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button enrollmentBrowse;
        public System.Windows.Forms.TextBox enrollmentBrowse_textBox;
        private System.Windows.Forms.Label label9;
    }
}
