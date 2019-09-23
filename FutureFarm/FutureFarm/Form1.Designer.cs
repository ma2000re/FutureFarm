namespace FutureFarm
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelAuswahl = new System.Windows.Forms.Panel();
            this.btEinstellungen = new System.Windows.Forms.Button();
            this.btLieferanten = new System.Windows.Forms.Button();
            this.btKunden = new System.Windows.Forms.Button();
            this.btArtikel = new System.Windows.Forms.Button();
            this.btRechnungen = new System.Windows.Forms.Button();
            this.btHome = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btBeenden = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.panelAuswahl);
            this.panel1.Controls.Add(this.btEinstellungen);
            this.panel1.Controls.Add(this.btLieferanten);
            this.panel1.Controls.Add(this.btKunden);
            this.panel1.Controls.Add(this.btArtikel);
            this.panel1.Controls.Add(this.btRechnungen);
            this.panel1.Controls.Add(this.btHome);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(205, 670);
            this.panel1.TabIndex = 0;
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
            // panelAuswahl
            // 
            this.panelAuswahl.BackColor = System.Drawing.Color.DodgerBlue;
            this.panelAuswahl.Location = new System.Drawing.Point(1, 97);
            this.panelAuswahl.Name = "panelAuswahl";
            this.panelAuswahl.Size = new System.Drawing.Size(10, 79);
            this.panelAuswahl.TabIndex = 2;
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
            this.btEinstellungen.Click += new System.EventHandler(this.btEinstellungen_Click);
            // 
            // btLieferanten
            // 
            this.btLieferanten.FlatAppearance.BorderSize = 0;
            this.btLieferanten.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLieferanten.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLieferanten.Image = global::FutureFarm.Properties.Resources.delivery_truck1;
            this.btLieferanten.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btLieferanten.Location = new System.Drawing.Point(12, 480);
            this.btLieferanten.Name = "btLieferanten";
            this.btLieferanten.Size = new System.Drawing.Size(190, 87);
            this.btLieferanten.TabIndex = 6;
            this.btLieferanten.Text = "   Lieferanten";
            this.btLieferanten.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btLieferanten.UseVisualStyleBackColor = true;
            this.btLieferanten.Click += new System.EventHandler(this.btLieferanten_Click);
            // 
            // btKunden
            // 
            this.btKunden.FlatAppearance.BorderSize = 0;
            this.btKunden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btKunden.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btKunden.Image = global::FutureFarm.Properties.Resources.multiple_users_silhouette1;
            this.btKunden.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btKunden.Location = new System.Drawing.Point(12, 387);
            this.btKunden.Name = "btKunden";
            this.btKunden.Size = new System.Drawing.Size(190, 87);
            this.btKunden.TabIndex = 5;
            this.btKunden.Text = "   Kunden";
            this.btKunden.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btKunden.UseVisualStyleBackColor = true;
            this.btKunden.Click += new System.EventHandler(this.btKunden_Click);
            // 
            // btArtikel
            // 
            this.btArtikel.FlatAppearance.BorderSize = 0;
            this.btArtikel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btArtikel.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btArtikel.Image = global::FutureFarm.Properties.Resources.artikel1;
            this.btArtikel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btArtikel.Location = new System.Drawing.Point(12, 294);
            this.btArtikel.Name = "btArtikel";
            this.btArtikel.Size = new System.Drawing.Size(190, 87);
            this.btArtikel.TabIndex = 4;
            this.btArtikel.Text = "   Artikel";
            this.btArtikel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btArtikel.UseVisualStyleBackColor = true;
            this.btArtikel.Click += new System.EventHandler(this.btArtikel_Click);
            // 
            // btRechnungen
            // 
            this.btRechnungen.FlatAppearance.BorderSize = 0;
            this.btRechnungen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRechnungen.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRechnungen.Image = global::FutureFarm.Properties.Resources.invoice1;
            this.btRechnungen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btRechnungen.Location = new System.Drawing.Point(12, 201);
            this.btRechnungen.Name = "btRechnungen";
            this.btRechnungen.Size = new System.Drawing.Size(190, 87);
            this.btRechnungen.TabIndex = 3;
            this.btRechnungen.Text = "   Rechnungen";
            this.btRechnungen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btRechnungen.UseVisualStyleBackColor = true;
            this.btRechnungen.Click += new System.EventHandler(this.btRechnungen_Click);
            // 
            // btHome
            // 
            this.btHome.FlatAppearance.BorderSize = 0;
            this.btHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btHome.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btHome.Image = global::FutureFarm.Properties.Resources.home1;
            this.btHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btHome.Location = new System.Drawing.Point(12, 108);
            this.btHome.Name = "btHome";
            this.btHome.Size = new System.Drawing.Size(190, 87);
            this.btHome.TabIndex = 2;
            this.btHome.Text = "   Home";
            this.btHome.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btHome.UseVisualStyleBackColor = true;
            this.btHome.Click += new System.EventHandler(this.btHome_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Controls.Add(this.btBeenden);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(205, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(945, 55);
            this.panel2.TabIndex = 1;
            // 
            // btBeenden
            // 
            this.btBeenden.FlatAppearance.BorderSize = 0;
            this.btBeenden.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBeenden.Image = global::FutureFarm.Properties.Resources.turn_off;
            this.btBeenden.Location = new System.Drawing.Point(903, 12);
            this.btBeenden.Name = "btBeenden";
            this.btBeenden.Size = new System.Drawing.Size(30, 30);
            this.btBeenden.TabIndex = 0;
            this.btBeenden.UseVisualStyleBackColor = true;
            this.btBeenden.Click += new System.EventHandler(this.btBeenden_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 670);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btHome;
        private System.Windows.Forms.Button btEinstellungen;
        private System.Windows.Forms.Button btLieferanten;
        private System.Windows.Forms.Button btKunden;
        private System.Windows.Forms.Button btArtikel;
        private System.Windows.Forms.Button btRechnungen;
        private System.Windows.Forms.Panel panelAuswahl;
        private System.Windows.Forms.Button btBeenden;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

