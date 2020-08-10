namespace Math_Competition_Scorer
{
    partial class Remove_Individual
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
            this.cancelBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListView();
            this.H1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.H2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.H3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(713, 422);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(626, 393);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(162, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Delete Selected Student";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.H1,
            this.H2,
            this.H3});
            this.listBox1.FullRowSelect = true;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(608, 426);
            this.listBox1.TabIndex = 3;
            this.listBox1.UseCompatibleStateImageBehavior = false;
            this.listBox1.View = System.Windows.Forms.View.Details;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged_1);
            // 
            // H1
            // 
            this.H1.Tag = "";
            this.H1.Text = "First Name";
            this.H1.Width = 25;
            // 
            // H2
            // 
            this.H2.Tag = "";
            this.H2.Text = "Last Name";
            this.H2.Width = 25;
            // 
            // H3
            // 
            this.H3.Text = "School";
            this.H3.Width = 25;
            // 
            // Remove_Individual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cancelBtn);
            this.Name = "Remove_Individual";
            this.Text = "Remove Student";
            this.Load += new System.EventHandler(this.Remove_Individual_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listBox1;
        private System.Windows.Forms.ColumnHeader H1;
        private System.Windows.Forms.ColumnHeader H2;
        private System.Windows.Forms.ColumnHeader H3;
    }
}