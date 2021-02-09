
namespace MicroserviceProject.Presentation.UI.WindowsForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstKisiler = new System.Windows.Forms.ListBox();
            this.btnKisileriGetir = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.bnKisiOlustur = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 425);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(767, 399);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "İK";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(284, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(271, 386);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Çalışanlar";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bnKisiOlustur);
            this.groupBox1.Controls.Add(this.lstKisiler);
            this.groupBox1.Controls.Add(this.btnKisileriGetir);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(271, 386);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kişiler";
            // 
            // lstKisiler
            // 
            this.lstKisiler.FormattingEnabled = true;
            this.lstKisiler.Location = new System.Drawing.Point(7, 49);
            this.lstKisiler.Name = "lstKisiler";
            this.lstKisiler.Size = new System.Drawing.Size(258, 329);
            this.lstKisiler.TabIndex = 1;
            // 
            // btnKisileriGetir
            // 
            this.btnKisileriGetir.Location = new System.Drawing.Point(6, 19);
            this.btnKisileriGetir.Name = "btnKisileriGetir";
            this.btnKisileriGetir.Size = new System.Drawing.Size(178, 23);
            this.btnKisileriGetir.TabIndex = 0;
            this.btnKisileriGetir.Text = "Kişileri Getir";
            this.btnKisileriGetir.UseVisualStyleBackColor = true;
            this.btnKisileriGetir.Click += new System.EventHandler(this.btnKisileriGetir_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 399);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // bnKisiOlustur
            // 
            this.bnKisiOlustur.Location = new System.Drawing.Point(190, 19);
            this.bnKisiOlustur.Name = "bnKisiOlustur";
            this.bnKisiOlustur.Size = new System.Drawing.Size(75, 23);
            this.bnKisiOlustur.TabIndex = 2;
            this.bnKisiOlustur.Text = "Kişi Oluştur";
            this.bnKisiOlustur.UseVisualStyleBackColor = true;
            this.bnKisiOlustur.Click += new System.EventHandler(this.bnKisiOlustur_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnKisileriGetir;
        private System.Windows.Forms.ListBox lstKisiler;
        private System.Windows.Forms.Button bnKisiOlustur;
    }
}

