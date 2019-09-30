using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace FutureFarm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Auswahl anpassen
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;

            db = new Datenbank();
        }

        Boolean passwortKorrekt = false;
        internal String benutzerEingabe;
        internal String passwortEingabe;
        Datenbank db;
        String sql;
        StreamReader sr;
        StreamWriter sw;
        ListViewItem lvItem;
        OleDbDataReader dr;
        int anzahlBenutzer;

        private void btHome_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;

        }

        private void btEinstellungen_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btEinstellungen.Height;
            panelAuswahl.Top = btEinstellungen.Top;
        }

        private void btRechnungen_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btRechnungen.Height;
            panelAuswahl.Top = btRechnungen.Top;
        }

        private void btBeenden_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btArtikel_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btArtikel.Height;
            panelAuswahl.Top = btArtikel.Top;

            FrmArtikel fArtikel = new FrmArtikel();
            fArtikel.ShowDialog();

        }

        private void btLieferanten_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btLieferanten.Height;
            panelAuswahl.Top = btLieferanten.Top;
        }

        private void btKunden_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btKunden.Height;
            panelAuswahl.Top = btKunden.Top;
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            FrmLogin fLogin = new FrmLogin();
            fLogin.ShowDialog();

            benutzerEingabe = fLogin.txtBenutzername.Text;
            passwortEingabe = fLogin.txtPasswort.Text;

            

            //if (benutzerEingabe.Equals("Manuel") && passwortEingabe.Equals("Passw0rd"))
            //{
            //    passwortKorrekt = true;
            //    MessageBox.Show("Login erfolgreich!");
            //}
            //else
            //    MessageBox.Show("Login fehlgeschlagen!");
        }

        private void BenutzerEinlesen()
        {
            listViewLoginDaten.Items.Clear();
            sql = "Select * from tblLogin";
            dr = db.Einlesen(sql);
            while (dr.Read())
            {
                lvItem = new ListViewItem(dr[0].ToString());
                lvItem.SubItems.Add(dr[1].ToString());
                lvItem.SubItems.Add(dr[2].ToString());

                listViewLoginDaten.Items.Add(lvItem);
                anzahlBenutzer++;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BenutzerEinlesen();
        }
    }
}
