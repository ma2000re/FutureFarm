namespace FutureFarm
{
    partial class FrmArtikel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btArtikelNeu = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelAuswahl = new System.Windows.Forms.Panel();
            this.btEinstellungen = new System.Windows.Forms.Button();
            this.btHome = new System.Windows.Forms.Button();
            this.listViewArtikel = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtArtikelID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBezeichnung = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNettopreis = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUST = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBrutto = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtLagerstand = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtReserviert = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLieferant = new System.Windows.Forms.TextBox();
            this.btNeu = new System.Windows.Forms.Button();
            this.btSpeichern = new System.Windows.Forms.Button();
            this.btLöschen = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DodgerBlue;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(205, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(957, 55);
            this.panel2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.btArtikelNeu);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.panelAuswahl);
            this.panel1.Controls.Add(this.btEinstellungen);
            this.panel1.Controls.Add(this.btHome);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(205, 678);
            this.panel1.TabIndex = 2;
            // 
            // btArtikelNeu
            // 
            this.btArtikelNeu.FlatAppearance.BorderSize = 0;
            this.btArtikelNeu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btArtikelNeu.Font = new System.Drawing.Font("Century Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btArtikelNeu.Image = global::FutureFarm.Properties.Resources.new1;
            this.btArtikelNeu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btArtikelNeu.Location = new System.Drawing.Point(12, 201);
            this.btArtikelNeu.Name = "btArtikelNeu";
            this.btArtikelNeu.Size = new System.Drawing.Size(190, 87);
            this.btArtikelNeu.TabIndex = 8;
            this.btArtikelNeu.Text = "   Artikel        anlegen";
            this.btArtikelNeu.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btArtikelNeu.UseVisualStyleBackColor = true;
            this.btArtikelNeu.Click += new System.EventHandler(this.btArtikelNeu_Click);
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
            // listViewArtikel
            // 
            this.listViewArtikel.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listViewArtikel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listViewArtikel.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewArtikel.FullRowSelect = true;
            this.listViewArtikel.Location = new System.Drawing.Point(205, 336);
            this.listViewArtikel.Name = "listViewArtikel";
            this.listViewArtikel.Size = new System.Drawing.Size(957, 342);
            this.listViewArtikel.TabIndex = 9;
            this.listViewArtikel.UseCompatibleStateImageBehavior = false;
            this.listViewArtikel.View = System.Windows.Forms.View.Details;
            this.listViewArtikel.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewArtikel_ItemSelectionChanged);
            this.listViewArtikel.SelectedIndexChanged += new System.EventHandler(this.listViewArtikel_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Artikel ID";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Bezeichnung";
            this.columnHeader2.Width = 300;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Nettopreis";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "UST";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Lieferant";
            this.columnHeader5.Width = 200;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Lagerstand";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Reserviert";
            this.columnHeader7.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader7.Width = 80;
            // 
            // txtArtikelID
            // 
            this.txtArtikelID.Location = new System.Drawing.Point(301, 81);
            this.txtArtikelID.Name = "txtArtikelID";
            this.txtArtikelID.ReadOnly = true;
            this.txtArtikelID.Size = new System.Drawing.Size(91, 21);
            this.txtArtikelID.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(224, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "ArtikelID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(224, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Bezeichnung";
            // 
            // txtBezeichnung
            // 
            this.txtBezeichnung.Location = new System.Drawing.Point(227, 143);
            this.txtBezeichnung.Name = "txtBezeichnung";
            this.txtBezeichnung.Size = new System.Drawing.Size(254, 21);
            this.txtBezeichnung.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(310, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 17);
            this.label3.TabIndex = 15;
            this.label3.Text = "Nettopreis";
            // 
            // txtNettopreis
            // 
            this.txtNettopreis.Location = new System.Drawing.Point(390, 170);
            this.txtNettopreis.Name = "txtNettopreis";
            this.txtNettopreis.Size = new System.Drawing.Size(91, 21);
            this.txtNettopreis.TabIndex = 14;
            this.txtNettopreis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(328, 199);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "UST in %";
            // 
            // txtUST
            // 
            this.txtUST.Location = new System.Drawing.Point(390, 197);
            this.txtUST.Name = "txtUST";
            this.txtUST.Size = new System.Drawing.Size(91, 21);
            this.txtUST.TabIndex = 16;
            this.txtUST.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(309, 231);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 17);
            this.label5.TabIndex = 19;
            this.label5.Text = "Bruttopreis";
            // 
            // txtBrutto
            // 
            this.txtBrutto.Location = new System.Drawing.Point(390, 229);
            this.txtBrutto.Name = "txtBrutto";
            this.txtBrutto.Size = new System.Drawing.Size(91, 21);
            this.txtBrutto.TabIndex = 18;
            this.txtBrutto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(319, 208);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 17);
            this.label6.TabIndex = 20;
            this.label6.Text = "_______________________";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(511, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 17);
            this.label7.TabIndex = 22;
            this.label7.Text = "Lagerstand";
            // 
            // txtLagerstand
            // 
            this.txtLagerstand.Location = new System.Drawing.Point(514, 219);
            this.txtLagerstand.Name = "txtLagerstand";
            this.txtLagerstand.Size = new System.Drawing.Size(86, 21);
            this.txtLagerstand.TabIndex = 21;
            this.txtLagerstand.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(674, 199);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 17);
            this.label8.TabIndex = 24;
            this.label8.Text = "Reserviert";
            // 
            // txtReserviert
            // 
            this.txtReserviert.Location = new System.Drawing.Point(677, 219);
            this.txtReserviert.Name = "txtReserviert";
            this.txtReserviert.Size = new System.Drawing.Size(86, 21);
            this.txtReserviert.TabIndex = 23;
            this.txtReserviert.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(507, 123);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 17);
            this.label9.TabIndex = 26;
            this.label9.Text = "Lieferant";
            // 
            // txtLieferant
            // 
            this.txtLieferant.Location = new System.Drawing.Point(510, 143);
            this.txtLieferant.Name = "txtLieferant";
            this.txtLieferant.Size = new System.Drawing.Size(254, 21);
            this.txtLieferant.TabIndex = 25;
            // 
            // btNeu
            // 
            this.btNeu.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btNeu.Location = new System.Drawing.Point(857, 97);
            this.btNeu.Name = "btNeu";
            this.btNeu.Size = new System.Drawing.Size(161, 53);
            this.btNeu.TabIndex = 27;
            this.btNeu.Text = "Neu";
            this.btNeu.UseVisualStyleBackColor = true;
            // 
            // btSpeichern
            // 
            this.btSpeichern.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSpeichern.Location = new System.Drawing.Point(857, 156);
            this.btSpeichern.Name = "btSpeichern";
            this.btSpeichern.Size = new System.Drawing.Size(161, 53);
            this.btSpeichern.TabIndex = 28;
            this.btSpeichern.Text = "Speichern";
            this.btSpeichern.UseVisualStyleBackColor = true;
            this.btSpeichern.Click += new System.EventHandler(this.btSpeichern_Click);
            // 
            // btLöschen
            // 
            this.btLöschen.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLöschen.Location = new System.Drawing.Point(857, 215);
            this.btLöschen.Name = "btLöschen";
            this.btLöschen.Size = new System.Drawing.Size(161, 53);
            this.btLöschen.TabIndex = 29;
            this.btLöschen.Text = "Löschen";
            this.btLöschen.UseVisualStyleBackColor = true;
            this.btLöschen.Click += new System.EventHandler(this.btLöschen_Click);
            // 
            // FrmArtikel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 678);
            this.Controls.Add(this.btLöschen);
            this.Controls.Add(this.btSpeichern);
            this.Controls.Add(this.btNeu);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtLieferant);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtReserviert);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtLagerstand);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBrutto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUST);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNettopreis);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBezeichnung);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtArtikelID);
            this.Controls.Add(this.listViewArtikel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmArtikel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmArtikel";
            this.Load += new System.EventHandler(this.FrmArtikel_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelAuswahl;
        private System.Windows.Forms.Button btEinstellungen;
        private System.Windows.Forms.Button btHome;
        private System.Windows.Forms.Button btArtikelNeu;
        private System.Windows.Forms.ListView listViewArtikel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.TextBox txtArtikelID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBezeichnung;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNettopreis;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUST;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBrutto;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLagerstand;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtReserviert;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLieferant;
        private System.Windows.Forms.Button btNeu;
        private System.Windows.Forms.Button btSpeichern;
        private System.Windows.Forms.Button btLöschen;
    }
}