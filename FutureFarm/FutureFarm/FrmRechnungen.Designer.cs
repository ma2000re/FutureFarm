namespace FutureFarm
{
    partial class FrmRechnungen
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
            this.btRechnungErstellen = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelAuswahl = new System.Windows.Forms.Panel();
            this.btEinstellungen = new System.Windows.Forms.Button();
            this.btHome = new System.Windows.Forms.Button();
            this.listViewRechnungen = new System.Windows.Forms.ListView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btBestellungErstellen = new System.Windows.Forms.Button();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btRechnungErstellen
            // 
            this.btRechnungErstellen.FlatAppearance.BorderSize = 0;
            this.btRechnungErstellen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRechnungErstellen.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRechnungErstellen.Image = global::FutureFarm.Properties.Resources.new1;
            this.btRechnungErstellen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btRechnungErstellen.Location = new System.Drawing.Point(12, 201);
            this.btRechnungErstellen.Name = "btRechnungErstellen";
            this.btRechnungErstellen.Size = new System.Drawing.Size(190, 87);
            this.btRechnungErstellen.TabIndex = 8;
            this.btRechnungErstellen.Text = "    Rechnung erstellen";
            this.btRechnungErstellen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btRechnungErstellen.UseVisualStyleBackColor = true;
            this.btRechnungErstellen.Click += new System.EventHandler(this.btRechnungErstellen_Click);
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
            // listViewRechnungen
            // 
            this.listViewRechnungen.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
            this.listViewRechnungen.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listViewRechnungen.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewRechnungen.FullRowSelect = true;
            this.listViewRechnungen.Location = new System.Drawing.Point(205, 338);
            this.listViewRechnungen.Name = "listViewRechnungen";
            this.listViewRechnungen.Size = new System.Drawing.Size(700, 328);
            this.listViewRechnungen.TabIndex = 32;
            this.listViewRechnungen.UseCompatibleStateImageBehavior = false;
            this.listViewRechnungen.View = System.Windows.Forms.View.Details;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(205, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 55);
            this.panel2.TabIndex = 31;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.btBestellungErstellen);
            this.panel1.Controls.Add(this.btRechnungErstellen);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.panelAuswahl);
            this.panel1.Controls.Add(this.btEinstellungen);
            this.panel1.Controls.Add(this.btHome);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(205, 666);
            this.panel1.TabIndex = 30;
            // 
            // btBestellungErstellen
            // 
            this.btBestellungErstellen.FlatAppearance.BorderSize = 0;
            this.btBestellungErstellen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btBestellungErstellen.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btBestellungErstellen.Image = global::FutureFarm.Properties.Resources.new1;
            this.btBestellungErstellen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btBestellungErstellen.Location = new System.Drawing.Point(12, 294);
            this.btBestellungErstellen.Name = "btBestellungErstellen";
            this.btBestellungErstellen.Size = new System.Drawing.Size(190, 87);
            this.btBestellungErstellen.TabIndex = 9;
            this.btBestellungErstellen.Text = "    Bestellung erstellen";
            this.btBestellungErstellen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btBestellungErstellen.UseVisualStyleBackColor = true;
            this.btBestellungErstellen.Click += new System.EventHandler(this.btBestellungErstellen_Click);
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Bestellnr.";
            this.columnHeader8.Width = 80;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Empfänger";
            this.columnHeader9.Width = 400;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Datum";
            this.columnHeader10.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader10.Width = 100;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Betrag";
            this.columnHeader11.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader11.Width = 100;
            // 
            // FrmRechnungen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 666);
            this.Controls.Add(this.listViewRechnungen);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmRechnungen";
            this.Text = "FrmRechnungen";
            this.Load += new System.EventHandler(this.FrmRechnungen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btRechnungErstellen;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelAuswahl;
        private System.Windows.Forms.Button btEinstellungen;
        private System.Windows.Forms.Button btHome;
        private System.Windows.Forms.ListView listViewRechnungen;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btBestellungErstellen;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
    }
}