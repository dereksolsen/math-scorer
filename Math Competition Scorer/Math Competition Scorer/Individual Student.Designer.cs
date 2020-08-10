namespace Math_Competition_Scorer
{
    partial class Individual_Student
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
            this.label1 = new System.Windows.Forms.Label();
            this.FNTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LNTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CLTB = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SCTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ANTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cnclBtn = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Name";
            // 
            // FNTB
            // 
            this.FNTB.Location = new System.Drawing.Point(194, 76);
            this.FNTB.Name = "FNTB";
            this.FNTB.Size = new System.Drawing.Size(100, 20);
            this.FNTB.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Last Name";
            // 
            // LNTB
            // 
            this.LNTB.Location = new System.Drawing.Point(194, 109);
            this.LNTB.Name = "LNTB";
            this.LNTB.Size = new System.Drawing.Size(100, 20);
            this.LNTB.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Class";
            // 
            // CLTB
            // 
            this.CLTB.Location = new System.Drawing.Point(194, 142);
            this.CLTB.Name = "CLTB";
            this.CLTB.Size = new System.Drawing.Size(100, 20);
            this.CLTB.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "School";
            // 
            // SCTB
            // 
            this.SCTB.Location = new System.Drawing.Point(194, 175);
            this.SCTB.Name = "SCTB";
            this.SCTB.Size = new System.Drawing.Size(100, 20);
            this.SCTB.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Answers";
            // 
            // ANTB
            // 
            this.ANTB.Location = new System.Drawing.Point(67, 208);
            this.ANTB.MaxLength = 40;
            this.ANTB.Name = "ANTB";
            this.ANTB.Size = new System.Drawing.Size(227, 20);
            this.ANTB.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(46, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(216, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Add an Individual Student";
            // 
            // cnclBtn
            // 
            this.cnclBtn.Location = new System.Drawing.Point(12, 279);
            this.cnclBtn.Name = "cnclBtn";
            this.cnclBtn.Size = new System.Drawing.Size(75, 23);
            this.cnclBtn.TabIndex = 11;
            this.cnclBtn.Text = "Cancel";
            this.cnclBtn.UseVisualStyleBackColor = true;
            this.cnclBtn.Click += new System.EventHandler(this.cnclBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(208, 279);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(86, 23);
            this.addBtn.TabIndex = 12;
            this.addBtn.Text = "Add Student";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // Individual_Student
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 314);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.cnclBtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ANTB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SCTB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CLTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LNTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FNTB);
            this.Controls.Add(this.label1);
            this.Name = "Individual_Student";
            this.Text = "Individual_Student";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FNTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox LNTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CLTB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SCTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ANTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cnclBtn;
        private System.Windows.Forms.Button addBtn;
    }
}