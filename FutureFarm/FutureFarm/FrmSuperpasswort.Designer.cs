namespace FutureFarm
{
    partial class FrmSuperpasswort
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbAktion = new System.Windows.Forms.Label();
            this.lbText = new System.Windows.Forms.Label();
            this.btWeiter = new System.Windows.Forms.Button();
            this.btAbbrechen = new System.Windows.Forms.Button();
            this.txtPasswort = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Controls.Add(this.lbAktion);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(532, 60);
            this.panel2.TabIndex = 23;
            // 
            // lbAktion
            // 
            this.lbAktion.AutoSize = true;
            this.lbAktion.Font = new System.Drawing.Font("Century Gothic", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAktion.ForeColor = System.Drawing.Color.White;
            this.lbAktion.Location = new System.Drawing.Point(12, 9);
            this.lbAktion.Name = "lbAktion";
            this.lbAktion.Size = new System.Drawing.Size(124, 42);
            this.lbAktion.TabIndex = 0;
            this.lbAktion.Text = "Aktion";
            // 
            // lbText
            // 
            this.lbText.AutoSize = true;
            this.lbText.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbText.Location = new System.Drawing.Point(15, 110);
            this.lbText.Name = "lbText";
            this.lbText.Size = new System.Drawing.Size(55, 21);
            this.lbText.TabIndex = 26;
            this.lbText.Text = "Text...";
            // 
            // btWeiter
            // 
            this.btWeiter.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btWeiter.Location = new System.Drawing.Point(268, 321);
            this.btWeiter.Name = "btWeiter";
            this.btWeiter.Size = new System.Drawing.Size(250, 53);
            this.btWeiter.TabIndex = 25;
            this.btWeiter.Text = "&Weiter";
            this.btWeiter.UseVisualStyleBackColor = true;
            this.btWeiter.Click += new System.EventHandler(this.btWeiter_Click);
            // 
            // btAbbrechen
            // 
            this.btAbbrechen.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAbbrechen.Location = new System.Drawing.Point(12, 321);
            this.btAbbrechen.Name = "btAbbrechen";
            this.btAbbrechen.Size = new System.Drawing.Size(250, 53);
            this.btAbbrechen.TabIndex = 24;
            this.btAbbrechen.Text = "Abbrechen";
            this.btAbbrechen.UseVisualStyleBackColor = true;
            // 
            // txtPasswort
            // 
            this.txtPasswort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasswort.Location = new System.Drawing.Point(19, 143);
            this.txtPasswort.Name = "txtPasswort";
            this.txtPasswort.PasswordChar = '+';
            this.txtPasswort.Size = new System.Drawing.Size(100, 26);
            this.txtPasswort.TabIndex = 27;
            // 
            // FrmSuperpasswort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(532, 383);
            this.Controls.Add(this.txtPasswort);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lbText);
            this.Controls.Add(this.btWeiter);
            this.Controls.Add(this.btAbbrechen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmSuperpasswort";
            this.Text = "FrmSuperpasswort";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Label lbText;
        private System.Windows.Forms.Button btWeiter;
        private System.Windows.Forms.Button btAbbrechen;
        internal System.Windows.Forms.Label lbAktion;
        internal System.Windows.Forms.TextBox txtPasswort;
    }
}