namespace PresentationLayer.Licenses.InternationalLicenses
{
    partial class frmShowInternationaLicenseInfo
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
            this.button1 = new System.Windows.Forms.Button();
            this.ctrlInternationalLicenseInfo2 = new PresentationLayer.Licenses.InternationalLicenses.ctrlInternationalLicenseInfo();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(235, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(428, 39);
            this.label1.TabIndex = 1;
            this.label1.Text = "International License Info";
            // 
            // button1
            // 
            this.button1.Image = global::PresentationLayer.Properties.Resources.Close_32;
            this.button1.Location = new System.Drawing.Point(665, 329);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 58);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ctrlInternationalLicenseInfo2
            // 
            this.ctrlInternationalLicenseInfo2.Location = new System.Drawing.Point(12, 77);
            this.ctrlInternationalLicenseInfo2.Name = "ctrlInternationalLicenseInfo2";
            this.ctrlInternationalLicenseInfo2.Size = new System.Drawing.Size(863, 267);
            this.ctrlInternationalLicenseInfo2.TabIndex = 0;
            // 
            // frmShowInternationaLicenseInfo
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(878, 395);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ctrlInternationalLicenseInfo2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmShowInternationaLicenseInfo";
            this.Load += new System.EventHandler(this.frmShowInternationaLicenseInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbTestTypeImage;
        private ctrlInternationalLicenseInfo ctrlInternationalLicenseInfo1;
        private ctrlInternationalLicenseInfo ctrlInternationalLicenseInfo2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}