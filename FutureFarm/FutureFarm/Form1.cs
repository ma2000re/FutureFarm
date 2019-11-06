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
            panelArtikel.Visible = true;
            panelArtikel.Dock = DockStyle.Fill;
            listViewArtikel.Height = Convert.ToInt16(panelArtikel.Height * 0.45);
            panelArtikelInfo.Location = new Point(Convert.ToInt16(panelArtikel.Width * 0.05), Convert.ToInt16(panelArtikel.Height * 0.05));
            lbArtikelFilter.Location = new Point(10, (panelArtikel.Height-listViewArtikel.Height-40));
            cbArtikelFilter.Location = new Point((10 + lbArtikelFilter.Width), (panelArtikel.Height - listViewArtikel.Height - 43));
            cbArtikelFilter.SelectedIndex = 1;

            //ArtikelEinlesen(true);

            CheckEingeloggt();

            panelUnterMenu.Visible = false;
        }

        private void ArtikelEinlesen()
        {
            listViewArtikel.Items.Clear();
            //Client für API
            var client = new RestClient("http://localhost:8888");
            var request = new RestRequest("artikel", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Artikel>>(request);

            foreach (Artikel a in response.Data)
            {
                lvItem = new ListViewItem(a.ArtikelID.ToString());
                lvItem.SubItems.Add(a.ExterneID.ToString());
                lvItem.SubItems.Add(a.Bezeichnung.ToString());
                lvItem.SubItems.Add(a.PreisNetto.ToString());
                lvItem.SubItems.Add(a.Ust.ToString());
                lvItem.SubItems.Add(a.Lieferant.Firma.ToString());
                lvItem.SubItems.Add(a.Lagerstand.ToString());
                lvItem.SubItems.Add(a.Reserviert.ToString());
                lvItem.SubItems.Add(a.Aktiv.ToString());
                listViewArtikel.Items.Add(lvItem);


            }

        }

        private void AlleArtikelEinlesen()
        {
            //Client für API
            var client = new RestClient("http://localhost:8888");
            var request = new RestRequest("artikel", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Artikel>>(request);

            foreach (Artikel a in response.Data)
            {
                //MessageBox.Show(a.Bezeichnung.ToString());
                lvItem = new ListViewItem(a.ArtikelID.ToString());
                lvItem.SubItems.Add(a.ExterneID.ToString());
                lvItem.SubItems.Add(a.Bezeichnung.ToString());
                lvItem.SubItems.Add(a.PreisNetto.ToString());
                lvItem.SubItems.Add(a.Ust.ToString());
                lvItem.SubItems.Add(a.Lieferant.Firma.ToString() + ", " + a.Lieferant.Nachname.ToString());
                lvItem.SubItems.Add(a.Lagerstand.ToString());
                lvItem.SubItems.Add(a.Reserviert.ToString());
                lvItem.SubItems.Add(a.Aktiv.ToString());

            }

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
            //panelsDeaktivieren();
            if (LogIn == false)
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
            CheckEingeloggt();
        }

        private void Einloggen()
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
                    {
                        LogIn = false;

                    }
                    CheckEingeloggt();
                }
            }

            if (LogIn == false)
            {
                MessageBox.Show("Login fehlgeschlagen!");
            }

        }

        private void CheckEingeloggt()
        {
            if (LogIn == false)
            {
                btLöschen.Enabled = false;
                btNeu.Enabled = false;
                btÄndern.Enabled = false;
                pbPasswort.Enabled = false;
                btSpeichern.Enabled = false;
                btArtikelLöschen.Enabled = false;
                btArtikelNeu.Enabled = false;
                btArtikelSpeichern.Enabled = false;
                btNewsLöschen.Enabled = false;
                btNewsSpeichern.Enabled = false;
            }
            else if (LogIn == true)
            {
                btLöschen.Enabled = true;
                btNeu.Enabled = true;
                btÄndern.Enabled = true;
                pbPasswort.Enabled = true;
                btSpeichern.Enabled = true;
                btArtikelLöschen.Enabled = true;
                btArtikelNeu.Enabled = true;
                btArtikelSpeichern.Enabled = true;
                btNewsLöschen.Enabled = true;
                btNewsSpeichern.Enabled = true;

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
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\\Users\Manuel\\Desktop\\Api.exe.lnk");
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
            panelAuswahl.Top = btTermine.Top;
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
            btFirmendaten.BringToFront();
            btBenutzer.BringToFront();
            panelUnterMenu.BringToFront();
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

            panelUnterMenu.Visible = false;
        }

        private void btBenutzer_Click(object sender, EventArgs e)
        {
            panelBenutzer.Dock = DockStyle.Fill;
            panelsDeaktivieren();
            panelBenutzer.Visible = true;

            panelBenutzerLoginEinlesen();
            pbBildName = "eye-closed.png";
            pbPasswort.Image = Image.FromFile(@"D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\" + pbBildName);

            CheckEingeloggt();

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
            panelFirmendaten.Visible = false;
            panelArtikel.Visible = false;

            if (LogIn == true)
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
            panelAuswahl.Height = btNews.Height;
            panelAuswahl.Top = btNews.Top;
            panelsDeaktivieren();
            panelNews.Visible = true;
            panelNews.Dock = DockStyle.Fill;
            listViewNews.Width = Convert.ToInt16(panelNews.Width * 0.5);

            NewsEinlesen();

            CheckEingeloggt();

            panelUnterMenu.Visible = false;

        }

        private void NewsEinlesen()
        {
            listViewNews.Items.Clear();
            //API Client
            var client = new RestClient("http://localhost:8888");
            var request = new RestRequest("news", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<News>>(request);

            foreach (News n in response.Data)
            {
                //MessageBox.Show(a.Bezeichnung.ToString());
                lvItem = new ListViewItem(n.NewsID.ToString());
                lvItem.SubItems.Add(n.Titel.ToString());
                lvItem.SubItems.Add(n.Beitrag.ToString());
                DateTime datum = Convert.ToDateTime(n.Datum);
                lvItem.SubItems.Add(datum.ToShortDateString());
                lvItem.SubItems.Add(n.Login.Benutzername.ToString());
                listViewNews.Items.Add(lvItem);
            }
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
                if (pbBildName.Equals("eye-closed.png"))
                {
                    pbBildName = "eye-open.png";
                    pbPasswort.Image = Image.FromFile(@"D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\" + pbBildName);
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

            private void btFirmendaten_Click(object sender, EventArgs e)
            {
                panelFirmendaten.Dock = DockStyle.Fill;
                panelFirmendaten.Visible = true;
                FirmendatenEinlesen();

                CheckEingeloggt();
                panelUnterMenu.Visible = false;

            }

            private void FirmendatenEinlesen()
            {
                listViewPanelBenutzerLogin.Items.Clear();
                //API Client
                var client = new RestClient("http://localhost:8888");
                var request = new RestRequest("firmendaten", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute<List<Firmendaten>>(request);

                foreach (Firmendaten f in response.Data)
                {
                    //MessageBox.Show(a.Bezeichnung.ToString());
                    //lvItem = new ListViewItem(f.FirmendatenID.ToString());
                    //lvItem.SubItems.Add(f.Name.ToString());
                    //lvItem.SubItems.Add(f.Rechtsform.ToString());
                    //lvItem.SubItems.Add(l.LetzteAnmeldung.ToString());
                    //listViewFirmendaten.Items.Add(lvItem);
                    txtFirmendatenName.Text = f.Name;
                    txtFirmendatenAnschrift.Text = f.Anschrift;
                    txtFirmendatenEmail.Text = f.Email;
                    txtFirmendatenTelefonnummer.Text = f.Telefon;
                    txtFirmendatenRechtsform.Text = f.Rechtsform;
                    txtFirmendatenSitz.Text = f.Sitz;
                    txtFirmendatenFirmenbuchnummer.Text = f.Firmenbuchnummer;
                    txtFirmendatenUIDNummer.Text = f.UIDNummer;
                    txtFirmendatenWKOMitglied.Text = f.MitgliedWKO;
                    txtFirmendatenAufsichtsbehörde.Text = f.Aufsichtsbehörde;
                    txtFirmendatenBerufsbezeichnung.Text = f.Berufsbezeichnung;
                }

            }

            private void pictureBox1_Click(object sender, EventArgs e)
            {
                panelsDeaktivieren();
            }

            private void panelFirmendaten_MouseEnter(object sender, EventArgs e)
            {
                panelUnterMenu.Visible = false;
                btFirmendaten.Visible = false;
                btBenutzer.Visible = false;
            }

            private void panelBenutzer_MouseEnter(object sender, EventArgs e)
            {
                panelUnterMenu.Visible = false;
                btFirmendaten.Visible = false;
                btBenutzer.Visible = false;

            }

            private void btSpeichern_Click(object sender, EventArgs e)
            {
                //UPDATE
            }

            private void listViewArtikel_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (listViewArtikel.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Wählen Sie bitte einen Artikel aus!");
                    return;
                }
                lvItem = listViewArtikel.SelectedItems[0];

                txtArtikelArtikelID.Text = lvItem.SubItems[0].Text;
                txtArtikelBezeichnung.Text = lvItem.SubItems[2].Text;
                txtArtikelNettopreis.Text = (lvItem.SubItems[3].Text);
                txtArtikelUST.Text = lvItem.SubItems[4].Text;
                Double netto = Convert.ToDouble(txtArtikelNettopreis.Text);
                Double ust = Convert.ToDouble(txtArtikelUST.Text);
                txtArtikelBrutto.Text = (netto + (1 + ust / 100)).ToString();
                txtArtikelLagerstand.Text = lvItem.SubItems[6].Text;
                txtArtikelReserviert.Text = lvItem.SubItems[7].Text;
                txtArtikelLieferant.Text = lvItem.SubItems[5].Text;
            

            }

        private void dtpNews_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listViewNews_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewNews.SelectedItems.Count == 0)
            {
                MessageBox.Show("Wählen Sie bitte einen Beitrag aus!");
                return;
            }
            lvItem = listViewNews.SelectedItems[0];

            txtNewsID.Text = lvItem.SubItems[0].Text;
            txtNewsTitel.Text = lvItem.SubItems[1].Text;
            txtNewsBeitrag.Text = lvItem.SubItems[2].Text;
            dtpNews.Value = Convert.ToDateTime(lvItem.SubItems[3].Text);
        }

        private void btNewsNeu_Click(object sender, EventArgs e)
        {
            txtNewsID.Clear();
            txtNewsTitel.Clear();
            txtNewsBeitrag.Clear();
            dtpNews.Value = DateTime.Now;

            
        }

        private void btNewsSpeichern_Click(object sender, EventArgs e)
        {
            var client = new RestClient("http://localhost:8888");
            //var request = new RestRequest("firmendaten", Method.GET);
            //request.AddHeader("Content-Type", "application/json");
            //var response = client.Execute<List<Firmendaten>>(request);

            var request = new RestRequest("news", Method.POST);
            request.AddParameter("Titel", txtNewsTitel.Text);
            request.AddParameter("Beitrag", txtNewsBeitrag.Text);
            request.AddParameter("Datum", dtpNews.Value);
            request.AddParameter("Benutzer", btLogin.Text);
            request.AddHeader("content-type", "application/json");

            var response = client.Execute(request);

            NewsEinlesen();
        }

        private void btArtikelNeu_Click(object sender, EventArgs e)
        {
            //var client = new RestClient("http://localhost:8888");
            //var request = new RestRequest("artikel", Method.PUT);
            //request.AddParameter("ExterneID", txtArtikelExterneID.Text);
            //request.AddParameter("Bezeichnung", txtArtikelBezeichnung.Text);
            //request.AddParameter("PreisNetto", Convert.ToDouble(txtArtikelNettopreis.Text));
            //request.AddParameter("Ust", Convert.ToDouble(txtArtikelUST.Text));
            //request.AddParameter("Reserviert", Convert.ToInt16(txtArtikelReserviert.Text));
            //request.AddParameter("Lagerstand", Convert.ToInt16(txtArtikelLagerstand.Text));
            //request.AddParameter("LieferantenID", Convert.ToInt16(txtArtikelLieferant.Text));
            //request.AddParameter("Aktiv", true);

            //var response = client.Execute(request);
            //ArtikelEinlesen();
        }

        private void btArtikelSpeichern_Click(object sender, EventArgs e)
        {

        }

        private void cbArtikelFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}

