namespace guimain
{
    partial class Parameter
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
            this.TxtMaxSize = new System.Windows.Forms.TextBox();
            this.TxtOrgWinRow = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TxtOmaxiter = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtOrgWinCol = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TxtPropotion = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.TxtTabu = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Txtlossweight = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "MaxSize:";
            // 
            // TxtMaxSize
            // 
            this.TxtMaxSize.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtMaxSize.Location = new System.Drawing.Point(154, 9);
            this.TxtMaxSize.Name = "TxtMaxSize";
            this.TxtMaxSize.Size = new System.Drawing.Size(100, 31);
            this.TxtMaxSize.TabIndex = 1;
            this.TxtMaxSize.TextChanged += new System.EventHandler(this.TxtMaxSize_TextChanged);
            // 
            // TxtOrgWinRow
            // 
            this.TxtOrgWinRow.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtOrgWinRow.Location = new System.Drawing.Point(154, 52);
            this.TxtOrgWinRow.Name = "TxtOrgWinRow";
            this.TxtOrgWinRow.Size = new System.Drawing.Size(100, 31);
            this.TxtOrgWinRow.TabIndex = 3;
            this.TxtOrgWinRow.TextChanged += new System.EventHandler(this.TxtOrgWinRow_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "WindowRow:";
            // 
            // TxtOmaxiter
            // 
            this.TxtOmaxiter.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtOmaxiter.Location = new System.Drawing.Point(154, 150);
            this.TxtOmaxiter.Name = "TxtOmaxiter";
            this.TxtOmaxiter.Size = new System.Drawing.Size(100, 31);
            this.TxtOmaxiter.TabIndex = 7;
            this.TxtOmaxiter.TextChanged += new System.EventHandler(this.TxtOmaxiter_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(35, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "maxiteration:";
            // 
            // TxtOrgWinCol
            // 
            this.TxtOrgWinCol.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtOrgWinCol.Location = new System.Drawing.Point(154, 101);
            this.TxtOrgWinCol.Name = "TxtOrgWinCol";
            this.TxtOrgWinCol.Size = new System.Drawing.Size(100, 31);
            this.TxtOrgWinCol.TabIndex = 5;
            this.TxtOrgWinCol.TextChanged += new System.EventHandler(this.TxtWinCol_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(44, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "WindowCol:";
            // 
            // TxtPropotion
            // 
            this.TxtPropotion.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtPropotion.Location = new System.Drawing.Point(154, 200);
            this.TxtPropotion.Name = "TxtPropotion";
            this.TxtPropotion.Size = new System.Drawing.Size(100, 31);
            this.TxtPropotion.TabIndex = 9;
            this.TxtPropotion.TextChanged += new System.EventHandler(this.TxtPropotion_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(48, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 8;
            this.label5.Text = "Proportion:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(260, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 23);
            this.label6.TabIndex = 10;
            this.label6.Text = "(GB)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(260, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 23);
            this.label7.TabIndex = 11;
            this.label7.Text = "(0~1)";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(154, 243);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 31);
            this.textBox1.TabIndex = 13;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(-3, 246);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(151, 23);
            this.label8.TabIndex = 12;
            this.label8.Text = "Constantiteration:";
            // 
            // TxtTabu
            // 
            this.TxtTabu.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtTabu.Location = new System.Drawing.Point(154, 289);
            this.TxtTabu.Name = "TxtTabu";
            this.TxtTabu.Size = new System.Drawing.Size(100, 31);
            this.TxtTabu.TabIndex = 14;
            this.TxtTabu.TextChanged += new System.EventHandler(this.TxtTabu_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 292);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(137, 23);
            this.label9.TabIndex = 15;
            this.label9.Text = "TabuProportion:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(40, 341);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 23);
            this.label10.TabIndex = 17;
            this.label10.Text = "LossWeight:";
            // 
            // Txtlossweight
            // 
            this.Txtlossweight.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Txtlossweight.Location = new System.Drawing.Point(154, 333);
            this.Txtlossweight.Name = "Txtlossweight";
            this.Txtlossweight.Size = new System.Drawing.Size(100, 31);
            this.Txtlossweight.TabIndex = 16;
            this.Txtlossweight.TextChanged += new System.EventHandler(this.Txtlossweight_TextChanged);
            // 
            // Parameter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 386);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.Txtlossweight);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.TxtTabu);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TxtPropotion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TxtOmaxiter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TxtOrgWinCol);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TxtOrgWinRow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TxtMaxSize);
            this.Controls.Add(this.label1);
            this.Name = "Parameter";
            this.Text = "Parameter";
            this.Load += new System.EventHandler(this.Parameter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtMaxSize;
        private System.Windows.Forms.TextBox TxtOrgWinRow;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TxtOmaxiter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtOrgWinCol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TxtPropotion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TxtTabu;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Txtlossweight;
    }
}