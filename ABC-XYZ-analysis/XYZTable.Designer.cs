namespace ABC_XYZ_analysis
{
    partial class XYZTable
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
            this.listBoxGroupX = new System.Windows.Forms.ListBox();
            this.listBoxGroupY = new System.Windows.Forms.ListBox();
            this.listBoxGroupZ = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxGroupX
            // 
            this.listBoxGroupX.FormattingEnabled = true;
            this.listBoxGroupX.Location = new System.Drawing.Point(25, 44);
            this.listBoxGroupX.Name = "listBoxGroupX";
            this.listBoxGroupX.Size = new System.Drawing.Size(202, 238);
            this.listBoxGroupX.TabIndex = 0;
            // 
            // listBoxGroupY
            // 
            this.listBoxGroupY.FormattingEnabled = true;
            this.listBoxGroupY.Location = new System.Drawing.Point(252, 44);
            this.listBoxGroupY.Name = "listBoxGroupY";
            this.listBoxGroupY.Size = new System.Drawing.Size(202, 238);
            this.listBoxGroupY.TabIndex = 1;
            // 
            // listBoxGroupZ
            // 
            this.listBoxGroupZ.FormattingEnabled = true;
            this.listBoxGroupZ.Location = new System.Drawing.Point(478, 44);
            this.listBoxGroupZ.Name = "listBoxGroupZ";
            this.listBoxGroupZ.Size = new System.Drawing.Size(202, 238);
            this.listBoxGroupZ.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Группа X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Группа Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(552, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Группа Z";
            // 
            // XYZTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 294);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxGroupZ);
            this.Controls.Add(this.listBoxGroupY);
            this.Controls.Add(this.listBoxGroupX);
            this.Name = "XYZTable";
            this.Text = "Таблица XYZ-анализа";
            this.Load += new System.EventHandler(this.XYZTable_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxGroupX;
        private System.Windows.Forms.ListBox listBoxGroupY;
        private System.Windows.Forms.ListBox listBoxGroupZ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}