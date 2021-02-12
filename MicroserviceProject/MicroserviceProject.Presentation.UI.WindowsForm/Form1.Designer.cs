
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnDepartmanOlustur = new System.Windows.Forms.Button();
            this.lstDepartmanlar = new System.Windows.Forms.ListBox();
            this.btnDepartmanlariGetir = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnCalisanOlustur = new System.Windows.Forms.Button();
            this.lstCalisanlar = new System.Windows.Forms.ListBox();
            this.btnCalisanlariGetir = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.bnKisiOlustur = new System.Windows.Forms.Button();
            this.lstKisiler = new System.Windows.Forms.ListBox();
            this.btnKisileriGetir = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnEnvanterOlustur = new System.Windows.Forms.Button();
            this.lstEnvanterler = new System.Windows.Forms.ListBox();
            this.btnEnvanterleriGetir = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btnIdariIslerEnventerOlustur = new System.Windows.Forms.Button();
            this.lstIdariIslerEnvanterler = new System.Windows.Forms.ListBox();
            this.btnIdariIslerEnvanterleriGetir = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(13, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(775, 425);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
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
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnDepartmanOlustur);
            this.groupBox3.Controls.Add(this.lstDepartmanlar);
            this.groupBox3.Controls.Add(this.btnDepartmanlariGetir);
            this.groupBox3.Location = new System.Drawing.Point(245, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(235, 386);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Departmanlar";
            // 
            // btnDepartmanOlustur
            // 
            this.btnDepartmanOlustur.Location = new System.Drawing.Point(146, 19);
            this.btnDepartmanOlustur.Name = "btnDepartmanOlustur";
            this.btnDepartmanOlustur.Size = new System.Drawing.Size(75, 23);
            this.btnDepartmanOlustur.TabIndex = 5;
            this.btnDepartmanOlustur.Text = "Dpt Oluştur";
            this.btnDepartmanOlustur.UseVisualStyleBackColor = true;
            this.btnDepartmanOlustur.Click += new System.EventHandler(this.btnDepartmanOlustur_Click);
            // 
            // lstDepartmanlar
            // 
            this.lstDepartmanlar.FormattingEnabled = true;
            this.lstDepartmanlar.Location = new System.Drawing.Point(10, 49);
            this.lstDepartmanlar.Name = "lstDepartmanlar";
            this.lstDepartmanlar.Size = new System.Drawing.Size(216, 329);
            this.lstDepartmanlar.TabIndex = 4;
            // 
            // btnDepartmanlariGetir
            // 
            this.btnDepartmanlariGetir.Location = new System.Drawing.Point(9, 19);
            this.btnDepartmanlariGetir.Name = "btnDepartmanlariGetir";
            this.btnDepartmanlariGetir.Size = new System.Drawing.Size(131, 23);
            this.btnDepartmanlariGetir.TabIndex = 3;
            this.btnDepartmanlariGetir.Text = "Departmanları Getir";
            this.btnDepartmanlariGetir.UseVisualStyleBackColor = true;
            this.btnDepartmanlariGetir.Click += new System.EventHandler(this.btnDepartmanlariGetir_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCalisanOlustur);
            this.groupBox2.Controls.Add(this.lstCalisanlar);
            this.groupBox2.Controls.Add(this.btnCalisanlariGetir);
            this.groupBox2.Location = new System.Drawing.Point(486, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(275, 386);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Çalışanlar";
            // 
            // btnCalisanOlustur
            // 
            this.btnCalisanOlustur.Location = new System.Drawing.Point(143, 19);
            this.btnCalisanOlustur.Name = "btnCalisanOlustur";
            this.btnCalisanOlustur.Size = new System.Drawing.Size(126, 23);
            this.btnCalisanOlustur.TabIndex = 8;
            this.btnCalisanOlustur.Text = "Çalışan Oluştur";
            this.btnCalisanOlustur.UseVisualStyleBackColor = true;
            this.btnCalisanOlustur.Click += new System.EventHandler(this.btnCalisanOlustur_Click);
            // 
            // lstCalisanlar
            // 
            this.lstCalisanlar.FormattingEnabled = true;
            this.lstCalisanlar.Location = new System.Drawing.Point(7, 49);
            this.lstCalisanlar.Name = "lstCalisanlar";
            this.lstCalisanlar.Size = new System.Drawing.Size(262, 329);
            this.lstCalisanlar.TabIndex = 7;
            // 
            // btnCalisanlariGetir
            // 
            this.btnCalisanlariGetir.Location = new System.Drawing.Point(6, 19);
            this.btnCalisanlariGetir.Name = "btnCalisanlariGetir";
            this.btnCalisanlariGetir.Size = new System.Drawing.Size(131, 23);
            this.btnCalisanlariGetir.TabIndex = 6;
            this.btnCalisanlariGetir.Text = "Çalışanları Getir";
            this.btnCalisanlariGetir.UseVisualStyleBackColor = true;
            this.btnCalisanlariGetir.Click += new System.EventHandler(this.btnCalisanlariGetir_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bnKisiOlustur);
            this.groupBox1.Controls.Add(this.lstKisiler);
            this.groupBox1.Controls.Add(this.btnKisileriGetir);
            this.groupBox1.Location = new System.Drawing.Point(7, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 386);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Kişiler";
            // 
            // bnKisiOlustur
            // 
            this.bnKisiOlustur.Location = new System.Drawing.Point(143, 19);
            this.bnKisiOlustur.Name = "bnKisiOlustur";
            this.bnKisiOlustur.Size = new System.Drawing.Size(75, 23);
            this.bnKisiOlustur.TabIndex = 2;
            this.bnKisiOlustur.Text = "Kişi Oluştur";
            this.bnKisiOlustur.UseVisualStyleBackColor = true;
            this.bnKisiOlustur.Click += new System.EventHandler(this.bnKisiOlustur_Click);
            // 
            // lstKisiler
            // 
            this.lstKisiler.FormattingEnabled = true;
            this.lstKisiler.Location = new System.Drawing.Point(7, 49);
            this.lstKisiler.Name = "lstKisiler";
            this.lstKisiler.Size = new System.Drawing.Size(216, 329);
            this.lstKisiler.TabIndex = 1;
            // 
            // btnKisileriGetir
            // 
            this.btnKisileriGetir.Location = new System.Drawing.Point(6, 19);
            this.btnKisileriGetir.Name = "btnKisileriGetir";
            this.btnKisileriGetir.Size = new System.Drawing.Size(131, 23);
            this.btnKisileriGetir.TabIndex = 0;
            this.btnKisileriGetir.Text = "Kişileri Getir";
            this.btnKisileriGetir.UseVisualStyleBackColor = true;
            this.btnKisileriGetir.Click += new System.EventHandler(this.btnKisileriGetir_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(767, 399);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IT";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnEnvanterOlustur);
            this.groupBox4.Controls.Add(this.lstEnvanterler);
            this.groupBox4.Controls.Add(this.btnEnvanterleriGetir);
            this.groupBox4.Location = new System.Drawing.Point(6, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(232, 386);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Envanterler";
            // 
            // btnEnvanterOlustur
            // 
            this.btnEnvanterOlustur.Location = new System.Drawing.Point(143, 19);
            this.btnEnvanterOlustur.Name = "btnEnvanterOlustur";
            this.btnEnvanterOlustur.Size = new System.Drawing.Size(75, 23);
            this.btnEnvanterOlustur.TabIndex = 2;
            this.btnEnvanterOlustur.Text = "Env. Oluştur";
            this.btnEnvanterOlustur.UseVisualStyleBackColor = true;
            this.btnEnvanterOlustur.Click += new System.EventHandler(this.btnEnvanterOlustur_Click);
            // 
            // lstEnvanterler
            // 
            this.lstEnvanterler.FormattingEnabled = true;
            this.lstEnvanterler.Location = new System.Drawing.Point(7, 49);
            this.lstEnvanterler.Name = "lstEnvanterler";
            this.lstEnvanterler.Size = new System.Drawing.Size(216, 329);
            this.lstEnvanterler.TabIndex = 1;
            // 
            // btnEnvanterleriGetir
            // 
            this.btnEnvanterleriGetir.Location = new System.Drawing.Point(6, 19);
            this.btnEnvanterleriGetir.Name = "btnEnvanterleriGetir";
            this.btnEnvanterleriGetir.Size = new System.Drawing.Size(131, 23);
            this.btnEnvanterleriGetir.TabIndex = 0;
            this.btnEnvanterleriGetir.Text = "Envanterleri Getir";
            this.btnEnvanterleriGetir.UseVisualStyleBackColor = true;
            this.btnEnvanterleriGetir.Click += new System.EventHandler(this.btnEnvanterleriGetir_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox5);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(767, 399);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "İdari İşler";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btnIdariIslerEnventerOlustur
            // 
            this.btnIdariIslerEnventerOlustur.Location = new System.Drawing.Point(143, 19);
            this.btnIdariIslerEnventerOlustur.Name = "btnIdariIslerEnventerOlustur";
            this.btnIdariIslerEnventerOlustur.Size = new System.Drawing.Size(75, 23);
            this.btnIdariIslerEnventerOlustur.TabIndex = 2;
            this.btnIdariIslerEnventerOlustur.Text = "Env. Oluştur";
            this.btnIdariIslerEnventerOlustur.UseVisualStyleBackColor = true;
            this.btnIdariIslerEnventerOlustur.Click += new System.EventHandler(this.btnIdariIslerEnventerOlustur_Click);
            // 
            // lstIdariIslerEnvanterler
            // 
            this.lstIdariIslerEnvanterler.FormattingEnabled = true;
            this.lstIdariIslerEnvanterler.Location = new System.Drawing.Point(7, 49);
            this.lstIdariIslerEnvanterler.Name = "lstIdariIslerEnvanterler";
            this.lstIdariIslerEnvanterler.Size = new System.Drawing.Size(216, 329);
            this.lstIdariIslerEnvanterler.TabIndex = 1;
            // 
            // btnIdariIslerEnvanterleriGetir
            // 
            this.btnIdariIslerEnvanterleriGetir.Location = new System.Drawing.Point(6, 19);
            this.btnIdariIslerEnvanterleriGetir.Name = "btnIdariIslerEnvanterleriGetir";
            this.btnIdariIslerEnvanterleriGetir.Size = new System.Drawing.Size(131, 23);
            this.btnIdariIslerEnvanterleriGetir.TabIndex = 0;
            this.btnIdariIslerEnvanterleriGetir.Text = "Envanterleri Getir";
            this.btnIdariIslerEnvanterleriGetir.UseVisualStyleBackColor = true;
            this.btnIdariIslerEnvanterleriGetir.Click += new System.EventHandler(this.btnIdariIslerEnvanterleriGetir_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btnIdariIslerEnventerOlustur);
            this.groupBox5.Controls.Add(this.lstIdariIslerEnvanterler);
            this.groupBox5.Controls.Add(this.btnIdariIslerEnvanterleriGetir);
            this.groupBox5.Location = new System.Drawing.Point(6, 7);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(232, 386);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Envanterler";
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
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnDepartmanOlustur;
        private System.Windows.Forms.ListBox lstDepartmanlar;
        private System.Windows.Forms.Button btnDepartmanlariGetir;
        private System.Windows.Forms.Button btnCalisanOlustur;
        private System.Windows.Forms.ListBox lstCalisanlar;
        private System.Windows.Forms.Button btnCalisanlariGetir;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnEnvanterOlustur;
        private System.Windows.Forms.ListBox lstEnvanterler;
        private System.Windows.Forms.Button btnEnvanterleriGetir;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnIdariIslerEnventerOlustur;
        private System.Windows.Forms.ListBox lstIdariIslerEnvanterler;
        private System.Windows.Forms.Button btnIdariIslerEnvanterleriGetir;
    }
}

