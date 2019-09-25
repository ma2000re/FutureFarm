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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelAuswahl = new System.Windows.Forms.Panel();
            this.btEinstellungen = new System.Windows.Forms.Button();
            this.btHome = new System.Windows.Forms.Button();
            this.btArtikelNeu = new System.Windows.Forms.Button();
            this.listViewArtikel = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.listViewArtikel.Location = new System.Drawing.Point(205, 385);
            this.listViewArtikel.Name = "listViewArtikel";
            this.listViewArtikel.Size = new System.Drawing.Size(957, 293);
            this.listViewArtikel.TabIndex = 9;
            this.listViewArtikel.UseCompatibleStateImageBehavior = false;
            this.listViewArtikel.View = System.Windows.Forms.View.Details;
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
            // FrmArtikel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 678);
            this.Controls.Add(this.listViewArtikel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
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
    }
}