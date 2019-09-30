namespace FutureFarm
{
    partial class FrmLogin
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
            this.btEinstellungen = new System.Windows.Forms.Button();
            this.panelAuswahl = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btPasswort = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btBenutzer = new System.Windows.Forms.Button();
            this.txtBenutzername = new System.Windows.Forms.TextBox();
            this.txtPasswort = new System.Windows.Forms.TextBox();
            this.btAbbrechen = new System.Windows.Forms.Button();
            this.btBestätigen = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(205, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(353, 55);
            this.panel2.TabIndex = 11;
            // 
            // btEinstellungen
            // 
            this.btEinstellungen.FlatAppearance.BorderSize = 0;
            this.btEinstellungen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btEinstellungen.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btEinstellungen.Image = global::FutureFarm.Properties.Resources.customer_support1;
            this.btEinstellungen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btEinstellungen.Location = new System.Drawing.Point(12, 573);
            this.btEinstellungen.Name = "btEinstellungen";
            this.btEinstellungen.Size = new System.Drawing.Size(190, 87);
            this.btEinstellungen.TabIndex = 3;
            this.btEinstellungen.Text = "   Einstellungen";
            this.btEinstellungen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btEinstellungen.UseVisualStyleBackColor = true;
            // 
            // panelAuswahl
            // 
            this.panelAuswahl.BackColor = System.Drawing.Color.DodgerBlue;
            this.panelAuswahl.Location = new System.Drawing.Point(1, 97);
            this.panelAuswahl.Name = "panelAuswahl";
            this.panelAuswahl.Size = new System.Drawing.Size(10, 79);
            this.panelAuswahl.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FutureFarm.Properties.Resources.logoTransp;
            this.pictureBox1.Location = new System.Drawing.Point(31, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 91);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // btPasswort
            // 
            this.btPasswort.FlatAppearance.BorderSize = 0;
            this.btPasswort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btPasswort.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btPasswort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btPasswort.Location = new System.Drawing.Point(12, 201);
            this.btPasswort.Name = "btPasswort";
            this.btPasswort.Size = new System.Drawing.Size(190, 87);
            this.btPasswort.TabIndex = 8;
            this.btPasswort.Text = "   Passwort";
            this.btPasswort.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btPasswort.UseVisualStyleBackColor = true;
            this.btPasswort.Click += new System.EventHandler(this.btPasswort_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.btPasswort);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.panelAuswahl);
            this.panel1.Controls.Add(this.btEinstellungen);
            this.panel1.Controls.Add(this.btBenutzer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(205, 416);
            this.panel1.TabIndex = 10;
            // 
            // btBenutzer
            // 
            this.btBenutzer.FlatAppearance.BorderSize = 0;
            this.btBenutzer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBenutzer.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBenutzer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btBenutzer.Location = new System.Drawing.Point(12, 108);
            this.btBenutzer.Name = "btBenutzer";
            this.btBenutzer.Size = new System.Drawing.Size(190, 87);
            this.btBenutzer.TabIndex = 2;
            this.btBenutzer.Text = "   Benutzername";
            this.btBenutzer.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btBenutzer.UseVisualStyleBackColor = true;
            this.btBenutzer.Click += new System.EventHandler(this.btHome_Click);
            // 
            // txtBenutzername
            // 
            this.txtBenutzername.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBenutzername.Location = new System.Drawing.Point(211, 135);
            this.txtBenutzername.Name = "txtBenutzername";
            this.txtBenutzername.Size = new System.Drawing.Size(300, 33);
            this.txtBenutzername.TabIndex = 12;
            // 
            // txtPasswort
            // 
            this.txtPasswort.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPasswort.Location = new System.Drawing.Point(211, 228);
            this.txtPasswort.Name = "txtPasswort";
            this.txtPasswort.PasswordChar = '*';
            this.txtPasswort.Size = new System.Drawing.Size(300, 33);
            this.txtPasswort.TabIndex = 13;
            // 
            // btAbbrechen
            // 
            this.btAbbrechen.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btAbbrechen.Location = new System.Drawing.Point(211, 280);
            this.btAbbrechen.Name = "btAbbrechen";
            this.btAbbrechen.Size = new System.Drawing.Size(148, 50);
            this.btAbbrechen.TabIndex = 14;
            this.btAbbrechen.Text = "Abbrechen";
            this.btAbbrechen.UseVisualStyleBackColor = true;
            this.btAbbrechen.Click += new System.EventHandler(this.btAbbrechen_Click);
            // 
            // btBestätigen
            // 
            this.btBestätigen.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBestätigen.Location = new System.Drawing.Point(363, 280);
            this.btBestätigen.Name = "btBestätigen";
            this.btBestätigen.Size = new System.Drawing.Size(148, 50);
            this.btBestätigen.TabIndex = 15;
            this.btBestätigen.Text = "Bestätigen";
            this.btBestätigen.UseVisualStyleBackColor = true;
            this.btBestätigen.Click += new System.EventHandler(this.btBestätigen_Click);
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 416);
            this.Controls.Add(this.btBestätigen);
            this.Controls.Add(this.btAbbrechen);
            this.Controls.Add(this.txtPasswort);
            this.Controls.Add(this.txtBenutzername);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmLogin";
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btEinstellungen;
        private System.Windows.Forms.Panel panelAuswahl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btPasswort;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btBenutzer;
        private System.Windows.Forms.Button btAbbrechen;
        private System.Windows.Forms.Button btBestätigen;
        protected internal System.Windows.Forms.TextBox txtBenutzername;
        protected internal System.Windows.Forms.TextBox txtPasswort;
    }
}