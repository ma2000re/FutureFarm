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
using RestSharp;
using System.Diagnostics;
using Common.Models;



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

        internal Boolean LogIn = false;
        internal String benutzerEingabe;
        internal String passwortEingabe;
        Datenbank db;
        String sql;
        StreamReader sr;
        StreamWriter sw;
        ListViewItem lvItem;
        OleDbDataReader dr;
        internal int anzahlBenutzer;
        internal double weite = Screen.PrimaryScreen.WorkingArea.Width;
        internal double höhe = Screen.PrimaryScreen.WorkingArea.Height;
        string pbBildName;

        private void btHome_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;
            panelsDeaktivieren();

        }

        private void btEinstellungen_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btEinstellungen.Height;
            panelAuswahl.Top = btEinstellungen.Top;
            panelsDeaktivieren();
        }

        private void btRechnungen_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btRechnungen.Height;
            panelAuswahl.Top = btRechnungen.Top;
            panelsDeaktivieren();

            FrmRechnungen fRechnungen = new FrmRechnungen();
            fRechnungen.ShowDialog();
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;

        }

        private void btBeenden_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btArtikel_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btArtikel.Height;
            panelAuswahl.Top = btArtikel.Top;
            panelsDeaktivieren();


            FrmArtikel fArtikel = new FrmArtikel();
            fArtikel.ShowDialog();
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;


        }

        private void btLieferanten_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btLieferanten.Height;
            panelAuswahl.Top = btLieferanten.Top;
            panelsDeaktivieren();
        }

        private void btKunden_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btKunden.Height;
            panelAuswahl.Top = btKunden.Top;
            panelsDeaktivieren();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            if(LogIn==false)
            Einloggen();
            else
            {
                DialogResult dialogResult = MessageBox.Show("Wollen Sie den Benutzer wirklich abmelden?", "Log Out", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    LogIn = false;
                    btLogin.Image = Image.FromFile("D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\logout.png");
                    btLogin.Text = "Log In";
                }
                else if (dialogResult == DialogResult.No)
                {
                    this.Close(); 
                }
            }
        }

        internal void Einloggen()
        {
            FrmLogin fLogin = new FrmLogin();
            fLogin.ShowDialog();
            FrmArtikel fArtikel = new FrmArtikel();


            benutzerEingabe = fLogin.txtBenutzername.Text;
            passwortEingabe = fLogin.txtPasswort.Text;

            for (int i = 0; i < anzahlBenutzer; i++)
            {
                //MessageBox.Show(listViewLoginDaten.Items[i].SubItems[1].ToString());
                if (listViewLoginDaten.Items[i].SubItems[1].Text == benutzerEingabe)
                {
                    //MessageBox.Show(listViewLoginDaten.Items[i].SubItems[1].Text + "***" + benutzerEingabe);
                    if (listViewLoginDaten.Items[i].SubItems[2].Text == passwortEingabe)
                    {
                        LogIn = true;
                        btLogin.Text = benutzerEingabe;

                        btLogin.Image = Image.FromFile("D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\login.png");

                        fArtikel.btLöschen.Enabled = true;
                        fArtikel.btNeu.Enabled = true;
                        fArtikel.btSpeichern.Enabled = true;

                        //LetzteAnmeldungAktualisieren();
                    }
                    else
                        LogIn = false;
                }
            }

            if (LogIn == false)
            {
                MessageBox.Show("Login fehlgeschlagen!");
            }
        }

        private void LetzteAnmeldungAktualisieren()
        {
            //API Client
            var client = new RestClient("http://localhost:8888");
            var request = new RestRequest("logins", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Login>>(request);

            foreach (Login l in response.Data)
            {
                //MessageBox.Show(a.Bezeichnung.ToString());
                lvItem = new ListViewItem(l.BenutzernameID.ToString());
                lvItem.SubItems.Add(l.Benutzername.ToString());
                lvItem.SubItems.Add(l.Passwort.ToString());
                lvItem.SubItems.Add(l.LetzteAnmeldung.ToString());
                listViewLoginDaten.Items.Add(lvItem);
                anzahlBenutzer++;
            }

        }

        private void BenutzerEinlesen()
        {
            listViewPanelBenutzerLogin.Items.Clear();
            //API Client
            var client = new RestClient("http://localhost:8888");
            var request = new RestRequest("logins", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Login>>(request);

            foreach (Login l in response.Data)
            {
                //MessageBox.Show(a.Bezeichnung.ToString());
                lvItem = new ListViewItem(l.BenutzernameID.ToString());
                lvItem.SubItems.Add(l.Benutzername.ToString());
                lvItem.SubItems.Add(l.Passwort.ToString());
                lvItem.SubItems.Add(l.LetzteAnmeldung.ToString());
                listViewLoginDaten.Items.Add(lvItem);
                anzahlBenutzer++;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            APIStart();
            GoFullscreen();
            MenuErstellen();
            
            panelAuswahl.Top = btHome.Top;
            panelAuswahl.Height = btHome.Height;
            BenutzerEinlesen();
        }

        internal void GoFullscreen()
        {
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Left = Top = 0;
            Width = Screen.PrimaryScreen.WorkingArea.Width;
            Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void APIStart()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\\Users\\Manuel\\Desktop\\Api.exe.lnk");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(startInfo);

        }

        private void btRechnungen_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btRechnungen.Top;
            umenu();
        }

        private void btHome_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btHome.Top;
            umenu();
        }

        private void btArtikel_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btArtikel.Top;
            umenu();
        }

        private void btKunden_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btKunden.Top;
            umenu();
        }

        private void btLieferanten_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btLieferanten.Top;
            umenu();
        }

        private void btAnfragen_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btAnfragen.Top;
            umenu();
        }

        private void btBestellungen_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btBestellungen.Top;
            umenu();
        }

        private void btNews_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btNews.Top;
            umenu();
        }

        private void btTermine_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top=btTermine.Top;
            umenu();
        }

        private void btEinstellungen_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btEinstellungen.Top;

            //Unterauswahl anzeigen
            btFirmendaten.Location = new Point(Convert.ToInt16(weite * 0.1), (pictureBox1.Height + (btHome.Height * 9)));
            btBenutzer.Location = new Point(Convert.ToInt16(weite * 0.1), Convert.ToInt16((pictureBox1.Height + (btHome.Height * 9.5))));
            btFirmendaten.Visible = true;
            btBenutzer.Visible = true;
            panelUnterMenu.Height = btFirmendaten.Height;
            panelUnterMenu.Visible = false;

        }

        private void umenu()
        {
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelUnterMenu.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btFirmendaten_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = true;
            panelUnterMenu.Location = btFirmendaten.Location;
        }

        private void btBenutzer_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = true;
            panelUnterMenu.Location = btBenutzer.Location;
        }

        private void MenuErstellen()
        {
            int anzahlTasks = 10;
            int menuHöhe = Convert.ToInt16(höhe - pictureBox1.Height);
            int menuWeite = Convert.ToInt16(weite * 0.10);

            panel1.Width = menuWeite;
            panel1.Height = Convert.ToInt16(höhe);

            panel3.Width = menuWeite;
            panel3.Height = menuHöhe;

            int btHöhe = Convert.ToInt16(menuHöhe / anzahlTasks);
            int btWeite = Convert.ToInt16(menuWeite - 10);
            int btPositionX = 10;
            int btPositionY = Convert.ToInt16(pictureBox1.Height);

            //Button Höhe und Weite
            btHome.Height = btHöhe;
            btHome.Width = btWeite;
            btRechnungen.Height = btHöhe;
            btRechnungen.Width = btWeite;
            btArtikel.Height = btHöhe;
            btArtikel.Width = btWeite;
            btKunden.Height = btHöhe;
            btKunden.Width = btWeite;
            btLieferanten.Height = btHöhe;
            btLieferanten.Width = btWeite;
            btAnfragen.Height = btHöhe;
            btAnfragen.Width = btWeite;
            btBestellungen.Height = btHöhe;
            btBestellungen.Width = btWeite;
            btNews.Height = btHöhe;
            btNews.Width = btWeite;
            btTermine.Height = btHöhe;
            btTermine.Width = btWeite;
            btEinstellungen.Height = btHöhe;
            btEinstellungen.Width = btWeite;
            //Button Location Positionen
            btHome.Location = new Point(btPositionX, 0);
            btRechnungen.Location = new Point(btPositionX, btHöhe);
            btArtikel.Location = new Point(btPositionX, btHöhe * 2);
            btKunden.Location = new Point(btPositionX, btHöhe * 3);
            btLieferanten.Location = new Point(btPositionX, btHöhe * 4);
            btAnfragen.Location = new Point(btPositionX, btHöhe * 5);
            btBestellungen.Location = new Point(btPositionX, btHöhe * 6);
            btNews.Location = new Point(btPositionX, btHöhe * 7);
            btTermine.Location = new Point(btPositionX, btHöhe * 8);
            btEinstellungen.Location = new Point(btPositionX, btHöhe * 9);
            //AuswahlPanel
            panelAuswahl.Location = new Point(0, 0);
            panelAuswahl.Height = btHöhe;
            //Unterauswahl Einstellungen
            btFirmendaten.Height = btHöhe / 2;
            btBenutzer.Height = btHöhe / 2;

        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btHome.Top;
        }

        private void btBenutzer_Click(object sender, EventArgs e)
        {
            panelBenutzer.Dock = DockStyle.Fill;
            panelBenutzer.Visible = true;
            panelBenutzerLoginEinlesen();
            pbBildName = "eye-closed.png";
            pbPasswort.Image=Image.FromFile(@"D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\" + pbBildName);
            if(LogIn==false)
            {
                btLöschen.Enabled = false;
                btNeu.Enabled = false;
                btÄndern.Enabled=false;
                pbPasswort.Enabled = false;
            }
            panelUnterMenu.Visible = false;
        }

        private void panelBenutzerLoginEinlesen()
        {
            listViewPanelBenutzerLogin.Items.Clear();
            //API Client
            var client = new RestClient("http://localhost:8888");
            var request = new RestRequest("logins", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Login>>(request);

            foreach (Login l in response.Data)
            {
                //MessageBox.Show(a.Bezeichnung.ToString());
                lvItem = new ListViewItem(l.BenutzernameID.ToString());
                lvItem.SubItems.Add(l.Benutzername.ToString());
                lvItem.SubItems.Add(l.Passwort.ToString());
                lvItem.SubItems.Add(l.LetzteAnmeldung.ToString());
                listViewPanelBenutzerLogin.Items.Add(lvItem);
            }

        }

        private void panelsDeaktivieren()
        {
            //Alle Panels invisible
            panelBenutzer.Visible = false;
            if (LogIn ==true)
            {
                btNeu.Enabled = true;
                btÄndern.Enabled = true;
                btLöschen.Enabled = true;
                pbPasswort.Enabled = true;
            }
        }

        private void btAnfragen_Click(object sender, EventArgs e)
        {
            panelsDeaktivieren();
        }

        private void btBestellungen_Click(object sender, EventArgs e)
        {
            panelsDeaktivieren();
        }

        private void btNews_Click(object sender, EventArgs e)
        {
            panelsDeaktivieren();
        }

        private void btTermine_Click(object sender, EventArgs e)
        {
            panelsDeaktivieren();
        }

        private void listViewPanelBenutzerLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewPanelBenutzerLogin.SelectedItems.Count == 0)
            {
                MessageBox.Show("Wählen Sie bitte einen Benutzer aus!");
                return;
            }
            lvItem = listViewPanelBenutzerLogin.SelectedItems[0];

            txtID.Text = lvItem.SubItems[0].Text;
            txtBenutzername.Text = lvItem.SubItems[1].Text;
            txtPasswort.Text = lvItem.SubItems[2].Text;
            // inlesenKunden();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(pbBildName.Equals("eye-closed.png"))
            {
                pbBildName = "eye-open.png";
                pbPasswort.Image = Image.FromFile(@"D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\"+pbBildName);
                txtPasswort.PasswordChar = '\0';
            }
            else
            {
                pbBildName = "eye-closed.png";
                pbPasswort.Image = Image.FromFile(@"D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\" + pbBildName);
                txtPasswort.PasswordChar = '*';
            }

        }


        private void pbPasswort_MouseHover(object sender, EventArgs e)
        {
        }

        private void btNeu_Click(object sender, EventArgs e)
        {
            //POST Methode

            panelBenutzerLoginEinlesen();
        }

        private void btLöschen_Click(object sender, EventArgs e)
        {
            //DELETE Methode

            panelBenutzerLoginEinlesen();

        }

        private void btÄndern_Click(object sender, EventArgs e)
        {
            //PUT Methode

            panelBenutzerLoginEinlesen();
        }
    }


}
