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
using RestSharp.Authenticators;

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

        internal Boolean LogIn;
        internal String benutzerEingabe;
        internal String passwortEingabe;
        Datenbank db;
        ListViewItem lvItem;
        internal int anzahlBenutzer;
        internal double weite = Screen.PrimaryScreen.WorkingArea.Width;
        internal double höhe = Screen.PrimaryScreen.WorkingArea.Height;
        string pbBildName;
        bool alle;
        Artikel aktArtikel;
        public bool reset=false;
        internal string angBenutzer;
        

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
            //panelAuswahl.Height = btRechnungen.Height;
            //panelAuswahl.Top = btRechnungen.Top;
            //panelsDeaktivieren();
            //panelRechnung.Visible = true;
            //panelArtikel.Dock = DockStyle.Fill;
            //listViewArtikel.Height = Convert.ToInt16(panelArtikel.Height * 0.45);
            //panelArtikelInfo.Location = new Point(Convert.ToInt16(panelArtikel.Width * 0.05), Convert.ToInt16(panelArtikel.Height * 0.05));
            //lbArtikelFilter.Location = new Point(10, (panelArtikel.Height - listViewArtikel.Height - 40));
            //cbArtikelFilter.Location = new Point((10 + lbArtikelFilter.Width), (panelArtikel.Height - listViewArtikel.Height - 43));
            //btArtikelSpeichern.Enabled = false;

            //ArtikelEinlesen();
            //LieferantenEinlesen();

            //CheckEingeloggt();

            //panelUnterMenu.Visible = false;


            //>>
            panelAuswahl.Height = btRechnungen.Height;
            panelAuswahl.Top = btRechnungen.Top;
            panelsDeaktivieren();

            FrmRechnungen fRechnungen = new FrmRechnungen();
            //fRechnungen.ShowDialog();
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
            btArtikelSpeichern.Enabled = false;

            ArtikelEinlesen();
            LieferantenEinlesen();

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
                lvItem.SubItems.Add(a.Lieferant.LieferantenID.ToString());
                lvItem.SubItems.Add(a.Lagerstand.ToString());
                lvItem.SubItems.Add(a.Reserviert.ToString());
                lvItem.SubItems.Add(a.Aktiv.ToString());

                if(alle==false)
                {
                    if (a.Aktiv == true)
                        listViewArtikel.Items.Add(lvItem);
                }
                else
                    listViewArtikel.Items.Add(lvItem);
            }

        }

        private void LieferantenEinlesen()
        {
            cbArtikelLieferanten.Items.Clear();
            var client = new RestClient("http://localhost:8888");
            var request = new RestRequest("lieferanten", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Lieferanten>>(request);

            foreach (Lieferanten l in response.Data)
            {
                cbArtikelLieferanten.Items.Add(l.LieferantenID.ToString());
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
            CheckEinAusloggen();
            CheckEingeloggt();
        }

        internal void CheckEinAusloggen()
        {
            //Prüfung ob jemand eingeloggt ist
            FrmLogin fLogin = new FrmLogin();

            //Eingeloggt, dann Abmelden
            if (LogIn == true)
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
            //nicht eingeloggt, Anmelden
            else
            {
                fLogin.ShowDialog();
            }

        }

        public void Einloggen()
        {
            //Benutzer die eingelesen sind löschen
            listViewPanelBenutzerLogin.Items.Clear();

            //Benutzer neu aus DB holen
            BenutzerEinlesen();

            FrmLogin fLogin = new FrmLogin();

            //Daten aus TextBox aus FormLogin holen
            benutzerEingabe = fLogin.txtBenutzername.Text;
            passwortEingabe = fLogin.txtPasswort.Text;

            //MessageBox.Show(anzahlBenutzer.ToString());
            for (int i = 0; i < anzahlBenutzer; i++)
            {
                if (listViewLoginDaten.Items[i].SubItems[1].Text.Equals(benutzerEingabe))
                {
                    if (listViewLoginDaten.Items[i].SubItems[2].Text.Equals(passwortEingabe))
                    {
                        LogIn = true;
                        btLogin.Text = benutzerEingabe;
                        angBenutzer = benutzerEingabe;

                        btLogin.Image = Image.FromFile("D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\login.png");

                        LetzteAnmeldungAktualisieren();
                        MessageBox.Show("Login abgeschlossen, break eingeleitet!\n"+btLogin.Text+"§§"+LogIn.ToString());
                        
                    }
                    else
                    {
                        MessageBox.Show("Passwort falsch!");
                        LogIn = false;
                    }
                    CheckEingeloggt();
                }
                break;
            }

            if (LogIn == false)
            {
                MessageBox.Show("Login fehlgeschlagen!");
            }
        }

        internal void CheckEingeloggt()
        {
            if (LogIn == false)
            {
                //btLöschen.Enabled = false;
                //btNeu.Enabled = false;
                //btÄndern.Enabled = false;
                //pbPasswort.Enabled = false;
                //btSpeichern.Enabled = false;
                ////btArtikelLöschen.Enabled = false;
                ////btArtikelNeu.Enabled = false;
                ////btArtikelSpeichern.Enabled = false;
                //btNewsLöschen.Enabled = false;
                //btNewsSpeichern.Enabled = false;
            }
            else if (LogIn == true)
            {

                btBenutzerLöschen.Enabled = true;
                btBenutzerNeu.Enabled = true;
                btBenutzerÄndern.Enabled = true;
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
            panelBenutzerLoginEinlesen();

            //API Client
            var client = new RestClient("http://localhost:8888")
            {
                Authenticator = new HttpBasicAuthenticator("demo", "demo")
            };

            int aktuelleID = 0;
            string aktuellesPW="";
            
            for(int i=0; i < listViewPanelBenutzerLogin.Items.Count;i++)
            {
                lvItem = listViewPanelBenutzerLogin.Items[i];
                if (angBenutzer.Equals(lvItem.SubItems[1].Text))
                {
                    aktuelleID = Convert.ToInt16(lvItem.SubItems[0].Text);
                    aktuellesPW = lvItem.SubItems[2].Text;
                    break;
                }
            }

            //Login schreiben
            Login aktLogin = new Login();
            aktLogin.BenutzernameID = aktuelleID;
            aktLogin.Benutzername = angBenutzer;
            aktLogin.Passwort = aktuellesPW;
            aktLogin.LetzteAnmeldung = DateTime.Now;
            //Login updaten
            var request2 = new RestRequest("logins", Method.PUT);
            request2.AddHeader("Content-Type", "application/json");
            request2.AddJsonBody(aktLogin);
            var response2 = client.Execute(request2);
            if (response2.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("LETZTE Anmeldung: An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Erfolgreich Anmeldezeit geändert!");
                BenutzerEinlesen();
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
                //MessageBox.Show(l.Benutzername.ToString());
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
            //APIStart();
            GoFullscreen();
            MenuErstellen();

            panelAuswahl.Top = btHome.Top;
            panelAuswahl.Height = btHome.Height;
            BenutzerEinlesen();
            

            cbArtikelFilter.SelectedIndex = 1;
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
                btBenutzerNeu.Enabled = true;
                btBenutzerÄndern.Enabled = true;
                btBenutzerLöschen.Enabled = true;
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

                txtBenutzerBenutzerID.Text = lvItem.SubItems[0].Text;
                txtBenutzerBenutzername.Text = lvItem.SubItems[1].Text;
                txtBenutzerPasswort.Text = lvItem.SubItems[2].Text;
                // inlesenKunden();

            }

            private void pictureBox2_Click(object sender, EventArgs e)
            {
                if (pbBildName.Equals("eye-closed.png"))
                {
                    pbBildName = "eye-open.png";
                    pbPasswort.Image = Image.FromFile(@"D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\" + pbBildName);
                    txtBenutzerPasswort.PasswordChar = '\0';
                }
                else
                {
                    pbBildName = "eye-closed.png";
                    pbPasswort.Image = Image.FromFile(@"D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\" + pbBildName);
                    txtBenutzerPasswort.PasswordChar = '*';
                }

            }


            private void pbPasswort_MouseHover(object sender, EventArgs e)
            {
            }

            private void btNeu_Click(object sender, EventArgs e)
            {
            //POST Methode
            FrmSuperpasswort fSuper = new FrmSuperpasswort();
            fSuper.lbAktion.Text = "Benutzer anlegen...";
            fSuper.lbText.Text = "Bitte Passwort eingeben, um Ihre Berechtigung zum Hinzufügen von Benutzern zu prüfen:";
            fSuper.ShowDialog();
            if (fSuper.berechtigt == true)
            {
                //Benutzer hinzufügen
                try
                {
                    var client = new RestClient("http://localhost:8888")
                    {
                        Authenticator = new HttpBasicAuthenticator("demo", "demo")
                    };


                    //Benutzer erzeugen
                    Login login = new Login();
                    login.Benutzername = txtBenutzerBenutzername.Text;
                    login.Passwort = txtBenutzerPasswort.Text;
                    login.LetzteAnmeldung = DateTime.Now;
                    

                    //Benutezr hinzufügen
                    var request2 = new RestRequest("logins", Method.POST);
                    request2.AddHeader("Content-Type", "application/json");
                    request2.AddJsonBody(login);
                    //MessageBox.Show(login.Benutzername.ToString() + " ... " + login.Passwort.ToString());
                    var response2 = client.Execute(request2);
                    if (response2.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Erfolgreich Benutzer hinzugefügt!");
                        panelBenutzerLoginEinlesen();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler bei der Neuanlage: " + ex.Message);
                }



            }
            
            panelBenutzerLoginEinlesen();
            }

        private void btLöschen_Click(object sender, EventArgs e)
        {            
            //DELETE Methode
            FrmSuperpasswort fSuper = new FrmSuperpasswort();
            fSuper.lbAktion.Text = "Benutzer löschen...";
            fSuper.lbText.Text = "Bitte Passwort eingeben, um Ihre Berechtigung zum Löschen von Benutzern zu prüfen:";
            fSuper.ShowDialog();
            if (fSuper.berechtigt == true)
            {
                try
                {
                    //DELETE Methode
                    var client = new RestClient("http://localhost:8888")
                    {
                        Authenticator = new HttpBasicAuthenticator("demo", "demo")
                    };

                    if (listViewPanelBenutzerLogin.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Wählen Sie einen Benutzer aus!");
                        return;
                    }
                    lvItem = listViewPanelBenutzerLogin.SelectedItems[0];
                    Login login = new Login();
                    login.BenutzernameID = Convert.ToInt32(lvItem.SubItems[0].Text);
                    var request = new RestRequest("logins/{id}", Method.DELETE);
                    request.AddUrlSegment("id", login.BenutzernameID.ToString());
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(login);
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Erfolgreich gelöscht");
                        panelBenutzerLoginEinlesen();

                        txtBenutzerBenutzername.Clear();
                        txtBenutzerBenutzerID.Clear();
                        txtBenutzerPasswort.Clear();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler bei der Neuanlage: " + ex.Message);
                }


            }
        }

        private void btÄndern_Click(object sender, EventArgs e)
        {
            //PUT Methode
            FrmSuperpasswort fSuper = new FrmSuperpasswort();
            fSuper.lbAktion.Text = "Benutzer ändern...";
            fSuper.lbText.Text = "Bitte Passwort eingeben, \num Ihre Berechtigung zum Ändern von Benutzern zu prüfen:";
            fSuper.ShowDialog();
            if (fSuper.berechtigt == true)
            {
                try
                {
                    var client = new RestClient("http://localhost:8888")
                    {
                        Authenticator = new HttpBasicAuthenticator("demo", "demo")
                    };

                    if (listViewPanelBenutzerLogin.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Wählen Sie einen Benutzer aus!");
                        return;
                    }
                    lvItem = listViewPanelBenutzerLogin.SelectedItems[0];

                    Login login = new Login();
                    login.BenutzernameID = Convert.ToInt32(lvItem.SubItems[0].Text);
                    login.Benutzername = txtBenutzerBenutzername.Text;
                    login.Passwort = txtBenutzerPasswort.Text;
                    login.LetzteAnmeldung = Convert.ToDateTime(lvItem.SubItems[3].Text);

                    var request = new RestRequest("logins", Method.PUT);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(login);
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Erfolgreich geändert!");
                        panelBenutzerLoginEinlesen();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler bei der Änderung: " + ex.Message);
                }


            }
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

            //PUT Methode
            FrmSuperpasswort fSuper = new FrmSuperpasswort();
            fSuper.lbAktion.Text = "Firmendaten ändern...";
            fSuper.lbText.Text = "Bitte Passwort eingeben, \num Ihre Berechtigung zum Ändern von Firmendaten zu prüfen:";
            fSuper.ShowDialog();
            if (fSuper.berechtigt == true)
            {
                try
                {
                    var client = new RestClient("http://localhost:8888")
                    {
                        Authenticator = new HttpBasicAuthenticator("demo", "demo")
                    };

                    Firmendaten fi = new Firmendaten();
                    fi.FirmendatenID = 11;
                    fi.Name = txtFirmendatenName.Text;
                    fi.Anschrift = txtFirmendatenAnschrift.Text;
                    fi.Email = txtFirmendatenEmail.Text;
                    fi.Telefon = txtFirmendatenTelefonnummer.Text;
                    fi.Rechtsform = txtFirmendatenRechtsform.Text;
                    fi.Sitz = txtFirmendatenSitz.Text;
                    fi.Firmenbuchnummer = txtFirmendatenFirmenbuchnummer.Text;
                    fi.UIDNummer = txtFirmendatenUIDNummer.Text;
                    fi.MitgliedWKO = txtFirmendatenWKOMitglied.Text;
                    fi.Aufsichtsbehörde = txtFirmendatenAufsichtsbehörde.Text;
                    fi.Berufsbezeichnung = txtFirmendatenBerufsbezeichnung.Text;
                    fi.Datum = Convert.ToDateTime(DateTime.Now.ToShortDateString());

                    var request = new RestRequest("firmendaten", Method.PUT);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(fi);
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Firmendaten erfolgreich geändert!");
                        FirmendatenEinlesen();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler bei der Änderung: " + ex.Message);
                }


            }


        }

        private void listViewArtikel_SelectedIndexChanged(object sender, EventArgs e)
            {
                btArtikelSpeichern.Enabled = true;
                if (listViewArtikel.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Wählen Sie bitte einen Artikel aus!");
                    return;
                }
                lvItem = listViewArtikel.SelectedItems[0];

                for(int i=0; i<lvItem.SubItems.Count;i++)
            {
                MessageBox.Show(lvItem.SubItems[i].Text);
            }

                txtArtikelArtikelID.Text = lvItem.SubItems[0].Text;
                txtArtikelExterneID.Text = lvItem.SubItems[1].Text;
                txtArtikelBezeichnung.Text = lvItem.SubItems[2].Text;
                txtArtikelNettopreis.Text = (lvItem.SubItems[3].Text);
                txtArtikelUST.Text = lvItem.SubItems[4].Text;
                Double netto = Convert.ToDouble(txtArtikelNettopreis.Text);
                Double ust = Convert.ToDouble(txtArtikelUST.Text);
                txtArtikelBrutto.Text = (netto + (netto*(ust / 100))).ToString();
                txtArtikelBrutto.Text.Replace(',', '.');
                cbArtikelLieferanten.Text = lvItem.SubItems[5].Text;
                txtArtikelLagerstand.Text = lvItem.SubItems[6].Text;
                txtArtikelReserviert.Text = lvItem.SubItems[7].Text;
                checkboxArtikelAktiv.Checked = Convert.ToBoolean(lvItem.SubItems[8].Text);

                var client = new RestClient("http://localhost:8888")
                {
                    Authenticator = new HttpBasicAuthenticator("demo", "demo")
                };

                //Lieferant auswählen per ID
                var request = new RestRequest("lieferanten/{id}", Method.GET);
                request.AddUrlSegment("id", lvItem.SubItems[5].Text.ToString());
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute<Lieferanten>(request);
                txtArtikelLieferantFirma.Text = response.Data.Firma.ToString();
          
                //Artikel holen
                var request1 = new RestRequest("artikel/{id}", Method.GET);
                request1.AddUrlSegment("id", txtArtikelArtikelID.Text.ToString());
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<Artikel>(request1);
                aktArtikel = response1.Data;
        }

        private void dtpNews_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listViewNews_SelectedIndexChanged(object sender, EventArgs e)
        {
            btNewsNeu.Enabled = false;
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
            try
            {
                var client = new RestClient("http://localhost:8888")
                {
                    Authenticator = new HttpBasicAuthenticator("demo", "demo")
                };

                //Benutzer holen
                int aktuelleID = 0;
                
                
                //ACHTUNG ÄNDERN WENN LOGIN WIEDER GEHT!!!!!!!!!!!!!!!!!!!!!!!!!!
                angBenutzer = "Manuel.Reisinger";

                panelBenutzerLoginEinlesen();
                for (int i = 0; i < listViewPanelBenutzerLogin.Items.Count; i++)
                {
                    lvItem = listViewPanelBenutzerLogin.Items[i];
                    if (angBenutzer.Equals(lvItem.SubItems[1].Text))
                    {
                        aktuelleID = Convert.ToInt16(lvItem.SubItems[0].Text);
                        break;
                    }
                }

                var request1 = new RestRequest("logins/{id}", Method.GET);
                request1.AddUrlSegment("id", aktuelleID.ToString());
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<Login>(request1);
                Login l = response1.Data;
                MessageBox.Show(l.Benutzername.ToString());

                //News erzeugen
                News news = new News();
                news.Titel = txtNewsTitel.Text;
                news.Beitrag = txtNewsBeitrag.Text;
                news.Datum = Convert.ToDateTime(dtpNews.Value);
                news.Login = l;
                

                //Benutzer hinzufügen
                var request2 = new RestRequest("news", Method.POST);
                request2.AddHeader("Content-Type", "application/json");
                request2.AddJsonBody(news);
                var response2 = client.Execute(request2);
                if (response2.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //MessageBox.Show("Erfolgreich News hinzugefügt!");
                    NewsEinlesen();
                    txtNewsTitel.Clear();
                    txtNewsID.Clear();
                    txtNewsBeitrag.Clear();
                    dtpNews.Value = DateTime.Now;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Neuanlage: " + ex.Message);
            }

        }

        private void btNewsSpeichern_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient("http://localhost:8888")
                {
                    Authenticator = new HttpBasicAuthenticator("demo", "demo")
                };

                //Benutzer holen
                int aktuelleID = 0;


                //ACHTUNG ÄNDERN WENN LOGIN WIEDER GEHT!!!!!!!!!!!!!!!!!!!!!!!!!!
                angBenutzer = "Manuel.Reisinger";

                panelBenutzerLoginEinlesen();
                for (int i = 0; i < listViewPanelBenutzerLogin.Items.Count; i++)
                {
                    lvItem = listViewPanelBenutzerLogin.Items[i];
                    if (angBenutzer.Equals(lvItem.SubItems[1].Text))
                    {
                        aktuelleID = Convert.ToInt16(lvItem.SubItems[0].Text);
                        break;
                    }
                }

                var request1 = new RestRequest("logins/{id}", Method.GET);
                request1.AddUrlSegment("id", aktuelleID.ToString());
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<Login>(request1);
                Login l = response1.Data;
                //MessageBox.Show(l.Benutzername.ToString());


                News news = new News();
                news.NewsID = Convert.ToInt16(txtNewsID.Text);
                news.Beitrag = txtNewsBeitrag.Text;
                news.Titel = txtNewsTitel.Text;
                news.Login = l;
                news.Datum = dtpNews.Value;

                var request = new RestRequest("news", Method.PUT);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(news);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //MessageBox.Show("News erfolgreich geändert!");
                    NewsEinlesen();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Änderung: " + ex.Message);
            }
        }
            

            private void btArtikelNeu_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient("http://localhost:8888")
                {
                    Authenticator = new HttpBasicAuthenticator("demo", "demo")
                };


                //Lieferant holen
                var request1 = new RestRequest("lieferanten/{id}", Method.GET);
                string gewLieferant = cbArtikelLieferanten.SelectedItem.ToString();
                
                request1.AddUrlSegment("id", gewLieferant);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<Lieferanten>(request1);
                //MessageBox.Show(response1.Data.Firma.ToString());
                Lieferanten l = response1.Data;

                //Artikel erzeugen
                Artikel artikel = new Artikel();
                artikel.Lieferant = l;
                artikel.ExterneID = Convert.ToInt16(txtArtikelExterneID.Text);
                artikel.Bezeichnung = txtArtikelBezeichnung.Text;
                artikel.Aktiv = true;
                artikel.Lagerstand=Convert.ToInt16(txtArtikelLagerstand.Text);
                artikel.Reserviert = Convert.ToInt16(txtArtikelReserviert.Text);
                artikel.PreisNetto = Convert.ToDouble(txtArtikelNettopreis.Text);
                artikel.Ust = Convert.ToDouble(txtArtikelUST.Text);
                artikel.Aktiv = Convert.ToBoolean(checkboxArtikelAktiv.Text);


                //Artikel hinzufügen
                var request2 = new RestRequest("artikel", Method.POST);
                request2.AddHeader("Content-Type", "application/json");
                request2.AddJsonBody(artikel);
                var response2 = client.Execute(request2);
                if(response2.StatusCode==System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Erfolgreich Artikel hinzugefügt!");
                    ArtikelEinlesen();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Neuanlage: " + ex.Message);
            }

        }

        private void btArtikelSpeichern_Click(object sender, EventArgs e)
        {
            try
            {
                var client = new RestClient("http://localhost:8888")
                {
                    Authenticator = new HttpBasicAuthenticator("demo", "demo")
                };


                //Lieferant holen
                //string gewLieferant = cbArtikelLieferanten.SelectedItem.ToString();
                //int lid = Convert.ToInt16(gewLieferant.Substring(0, 2).Trim());

                Lieferanten lieferant = new Lieferanten();
                var request1 = new RestRequest("lieferanten", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Lieferanten>>(request1);
                foreach(Lieferanten l in response1.Data)
                {
                    //MessageBox.Show("Firma: "+l.Firma.ToString());
                    if(l.LieferantenID==Convert.ToInt32(lvItem.SubItems[5].Text))
                    {
                        lieferant.LieferantenID = l.LieferantenID;
                        lieferant.Vorname = l.Vorname;
                        lieferant.Nachname = l.Nachname;
                        lieferant.PLZ = l.PLZ;
                        lieferant.Strasse = l.Strasse;
                        lieferant.Telefonnummer = l.Telefonnummer;
                        lieferant.UID = l.UID;
                        lieferant.Firma = l.Firma;
                        lieferant.Email = l.Email;
                        lieferant.Aktiv = l.Aktiv;
                    }
                }

                //Artikel schreiben
                Artikel aktArtikel = new Artikel();
                aktArtikel.Lieferant = lieferant;
                aktArtikel.ExterneID = Convert.ToInt16(txtArtikelExterneID.Text);
                aktArtikel.Bezeichnung = txtArtikelBezeichnung.Text;
                aktArtikel.Lagerstand = Convert.ToInt16(txtArtikelLagerstand.Text);
                aktArtikel.Reserviert = Convert.ToInt16(txtArtikelReserviert.Text);
                aktArtikel.PreisNetto = Convert.ToDouble(txtArtikelNettopreis.Text);
                aktArtikel.Ust = Convert.ToDouble(txtArtikelUST.Text);
                aktArtikel.Aktiv = Convert.ToBoolean(checkboxArtikelAktiv.Text);
                aktArtikel.ArtikelID = Convert.ToInt32(txtArtikelArtikelID.Text);
                aktArtikel.Bild = "";


                //Artikel updaten
                var request2 = new RestRequest("artikel", Method.PUT);
                request2.AddHeader("Content-Type", "application/json");
                request2.AddJsonBody(aktArtikel);
                var response2 = client.Execute(request2);
                if (response2.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Erfolgreich Artikel geändert!");
                    ArtikelEinlesen();
                    btArtikelSpeichern.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Änderung: " + ex.Message);
            }
        }

        private void cbArtikelFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbArtikelFilter.SelectedIndex==1)
                alle = false;
            else
                alle = true;
            ArtikelEinlesen();
        }

        private void btLogin_TextChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Text wird geändert!");
        }

        private void txtArtikelNettopreis_TextChanged(object sender, EventArgs e)
        {
            PreisBerechnen();
        }

        private void PreisBerechnen()
        {
            try
            {
                if (reset == false)
                {
                    double netto = Convert.ToDouble(txtArtikelNettopreis.Text);
                    double ust = Convert.ToDouble(txtArtikelUST.Text);
                    double brutto = netto + (netto * (ust / 100));
                    txtArtikelBrutto.Text = brutto.ToString();
                }
                else
                    reset = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btArtikelLöschen_Click(object sender, EventArgs e)
        {
            var client = new RestClient("http://localhost:8888")
            {
                Authenticator = new HttpBasicAuthenticator("demo", "demo")
            };

            if (listViewArtikel.SelectedItems.Count==0)
            {
                MessageBox.Show("Wählen Sie einen Artikel aus!");
                return;
            }
            lvItem = listViewArtikel.SelectedItems[0];
            Artikel artikel = new Artikel();
            artikel.ArtikelID = Convert.ToInt32(lvItem.SubItems[0].Text);
            var request = new RestRequest("artikel/{id}", Method.DELETE);
            request.AddUrlSegment("id", artikel.ArtikelID.ToString());
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(artikel);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Erfolgreich gelöscht");
                ArtikelEinlesen();
            }

        }

        private void cbArtikelLieferanten_SelectedIndexChanged(object sender, EventArgs e)
        {
            var client = new RestClient("http://localhost:8888")
            {
                Authenticator = new HttpBasicAuthenticator("demo", "demo")
            };


            //Lieferant holen
            Lieferanten lieferant = new Lieferanten();
            var request1 = new RestRequest("lieferanten", Method.GET);
            request1.AddHeader("Content-Type", "application/json");
            var response1 = client.Execute<List<Lieferanten>>(request1);
            foreach (Lieferanten l in response1.Data)
            {
                if (l.LieferantenID == Convert.ToInt32(lvItem.SubItems[5].Text))
                {
                    txtArtikelLieferantFirma.Text = l.Firma;                    
                }
            }

        }

        private void btReset_Click(object sender, EventArgs e)
        {
            reset = true;
            txtArtikelArtikelID.Clear();
            txtArtikelExterneID.Clear();
            txtArtikelBezeichnung.Clear();
            txtArtikelNettopreis.Clear();
            txtArtikelBrutto.Clear();
            txtArtikelLagerstand.Clear();
            txtArtikelReserviert.Clear();
            txtArtikelLieferantFirma.Clear();
            cbArtikelLieferanten.SelectedValue="";
        }

        private void btNewsReset_Click(object sender, EventArgs e)
        {
            txtNewsBeitrag.Clear();
            txtNewsID.Clear();
            txtNewsTitel.Clear();
            dtpNews.Value = DateTime.Now;
            btNewsNeu.Enabled = true;
        }

        private void btNewsLöschen_Click(object sender, EventArgs e)
        {
            try
            {
                //DELETE Methode
                var client = new RestClient("http://localhost:8888")
                {
                    Authenticator = new HttpBasicAuthenticator("demo", "demo")
                };

                lvItem = listViewNews.SelectedItems[0];

                News news = new News();
                news.NewsID = Convert.ToInt32(lvItem.SubItems[0].Text);
                var request = new RestRequest("news/{id}", Method.DELETE);
                request.AddUrlSegment("id", news.NewsID.ToString());
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(news);
                var response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //MessageBox.Show("Erfolgreich gelöscht");
                    NewsEinlesen();
                    txtNewsTitel.Clear();
                    txtNewsID.Clear();
                    txtNewsBeitrag.Clear();
                    dtpNews.Value = DateTime.Now;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Löschen: " + ex.Message);
            }

        }

        private void btLogin2_Click(object sender, EventArgs e)
        {
            FrmLogin fl = new FrmLogin();

            if(LogIn==false)
            fl.ShowDialog();
            else
            {
                MessageBox.Show("Benutzer wird abgemeldet!");
                LogIn = false;
                btLogin2.Text = "   Log In";
                btLogin2.Image = Image.FromFile("D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\logout.png");
            }
        }

        public void EinloggenNeu()
        {
            //Benutzereingaben aus FrmLogin
            FrmLogin fl = new FrmLogin();
            string eingabeBenutzername = fl.txtBenutzername.Text;
            string eingabePasswort = fl.txtPasswort.Text;

            panelBenutzerLoginEinlesen();

            //einzelne Benutzer in der Listview durchgehen
            for(int i=0; i<listViewPanelBenutzerLogin.Items.Count;i++)
            {
                //Item prüfen
                lvItem = listViewPanelBenutzerLogin.Items[i];
                if (eingabeBenutzername.Equals(lvItem.SubItems[1].Text) && eingabePasswort.Equals(lvItem.SubItems[2].Text))
                {
                    LogIn = true;
                    btLogin2.Text = eingabeBenutzername;
                    btLogin2.Image = Image.FromFile("D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\login.png");

                    MessageBox.Show("Anmeldung erfolgreich!");
                    break;
                }
                else
                    MessageBox.Show("Anmelden fehlgeschlagen!");

                CheckEingeloggt();
            }
        }
    }
}

