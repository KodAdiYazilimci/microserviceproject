
namespace Presentation.UI.WindowsForm.Dialogs.HR
{
    partial class CreateWorkerForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbKisiler = new System.Windows.Forms.ComboBox();
            this.cmbUnvanlar = new System.Windows.Forms.ComboBox();
            this.cmbDepartmanlar = new System.Windows.Forms.ComboBox();
            this.dtBaslamaTarihi = new System.Windows.Forms.DateTimePicker();
            this.txtIBAN = new System.Windows.Forms.TextBox();
            this.clYoneticiler = new System.Windows.Forms.CheckedListBox();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnVazgec = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ünvan:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Departman:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Kişi:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Başlama Tarihi:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Maaş IBAN:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Yöneticileri:";
            // 
            // cmbKisiler
            // 
            this.cmbKisiler.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKisiler.FormattingEnabled = true;
            this.cmbKisiler.Location = new System.Drawing.Point(126, 17);
            this.cmbKisiler.Name = "cmbKisiler";
            this.cmbKisiler.Size = new System.Drawing.Size(220, 21);
            this.cmbKisiler.TabIndex = 6;
            // 
            // cmbUnvanlar
            // 
            this.cmbUnvanlar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnvanlar.FormattingEnabled = true;
            this.cmbUnvanlar.Location = new System.Drawing.Point(126, 47);
            this.cmbUnvanlar.Name = "cmbUnvanlar";
            this.cmbUnvanlar.Size = new System.Drawing.Size(220, 21);
            this.cmbUnvanlar.TabIndex = 7;
            // 
            // cmbDepartmanlar
            // 
            this.cmbDepartmanlar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartmanlar.FormattingEnabled = true;
            this.cmbDepartmanlar.Location = new System.Drawing.Point(126, 76);
            this.cmbDepartmanlar.Name = "cmbDepartmanlar";
            this.cmbDepartmanlar.Size = new System.Drawing.Size(220, 21);
            this.cmbDepartmanlar.TabIndex = 8;
            // 
            // dtBaslamaTarihi
            // 
            this.dtBaslamaTarihi.Location = new System.Drawing.Point(126, 105);
            this.dtBaslamaTarihi.Name = "dtBaslamaTarihi";
            this.dtBaslamaTarihi.Size = new System.Drawing.Size(220, 20);
            this.dtBaslamaTarihi.TabIndex = 9;
            // 
            // txtIBAN
            // 
            this.txtIBAN.Location = new System.Drawing.Point(126, 139);
            this.txtIBAN.Name = "txtIBAN";
            this.txtIBAN.Size = new System.Drawing.Size(220, 20);
            this.txtIBAN.TabIndex = 10;
            // 
            // clYoneticiler
            // 
            this.clYoneticiler.FormattingEnabled = true;
            this.clYoneticiler.Location = new System.Drawing.Point(126, 173);
            this.clYoneticiler.Name = "clYoneticiler";
            this.clYoneticiler.Size = new System.Drawing.Size(220, 154);
            this.clYoneticiler.TabIndex = 11;
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(126, 333);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(112, 23);
            this.btnKaydet.TabIndex = 12;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnVazgec
            // 
            this.btnVazgec.Location = new System.Drawing.Point(244, 333);
            this.btnVazgec.Name = "btnVazgec";
            this.btnVazgec.Size = new System.Drawing.Size(102, 23);
            this.btnVazgec.TabIndex = 13;
            this.btnVazgec.Text = "Vazgeç";
            this.btnVazgec.UseVisualStyleBackColor = true;
            this.btnVazgec.Click += new System.EventHandler(this.btnVazgec_Click);
            // 
            // CreateWorkerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 362);
            this.Controls.Add(this.btnVazgec);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.clYoneticiler);
            this.Controls.Add(this.txtIBAN);
            this.Controls.Add(this.dtBaslamaTarihi);
            this.Controls.Add(this.cmbDepartmanlar);
            this.Controls.Add(this.cmbUnvanlar);
            this.Controls.Add(this.cmbKisiler);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(390, 401);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(390, 401);
            this.Name = "CreateWorkerForm";
            this.Text = "Çalışan Oluştur";
            this.Load += new System.EventHandler(this.CreateWorkerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbKisiler;
        private System.Windows.Forms.ComboBox cmbUnvanlar;
        private System.Windows.Forms.ComboBox cmbDepartmanlar;
        private System.Windows.Forms.DateTimePicker dtBaslamaTarihi;
        private System.Windows.Forms.TextBox txtIBAN;
        private System.Windows.Forms.CheckedListBox clYoneticiler;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnVazgec;
    }
}