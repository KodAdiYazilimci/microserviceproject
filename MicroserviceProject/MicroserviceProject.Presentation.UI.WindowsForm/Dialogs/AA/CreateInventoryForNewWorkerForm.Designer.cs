
namespace MicroserviceProject.Presentation.UI.WindowsForm.Dialogs.AA
{
    partial class CreateInventoryForNewWorkerForm
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
            this.btnKaydet = new System.Windows.Forms.Button();
            this.btnVazgec = new System.Windows.Forms.Button();
            this.cmbEnvanter = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Envanter:";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(88, 73);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(107, 23);
            this.btnKaydet.TabIndex = 2;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // btnVazgec
            // 
            this.btnVazgec.Location = new System.Drawing.Point(201, 73);
            this.btnVazgec.Name = "btnVazgec";
            this.btnVazgec.Size = new System.Drawing.Size(75, 23);
            this.btnVazgec.TabIndex = 3;
            this.btnVazgec.Text = "Vazgeç";
            this.btnVazgec.UseVisualStyleBackColor = true;
            this.btnVazgec.Click += new System.EventHandler(this.btnVazgec_Click);
            // 
            // cmbEnvanter
            // 
            this.cmbEnvanter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEnvanter.FormattingEnabled = true;
            this.cmbEnvanter.Location = new System.Drawing.Point(81, 35);
            this.cmbEnvanter.Name = "cmbEnvanter";
            this.cmbEnvanter.Size = new System.Drawing.Size(244, 21);
            this.cmbEnvanter.TabIndex = 4;
            // 
            // CreateInventoryForNewWorkerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 123);
            this.Controls.Add(this.cmbEnvanter);
            this.Controls.Add(this.btnVazgec);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.label1);
            this.Name = "CreateInventoryForNewWorkerForm";
            this.Text = "CreateInventoryForm";
            this.Load += new System.EventHandler(this.CreateInventoryForNewWorkerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnKaydet;
        private System.Windows.Forms.Button btnVazgec;
        private System.Windows.Forms.ComboBox cmbEnvanter;
    }
}