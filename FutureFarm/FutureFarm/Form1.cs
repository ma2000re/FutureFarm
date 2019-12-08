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
using Word = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.Security.Cryptography;


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


            client = new RestClient("http://localhost:8888")
            {
                Authenticator = new HttpBasicAuthenticator("demo", "demo")
            };


        }

        RestClient client;
        ListViewItem lvItem;
        Artikel aktArtikel;
        RC4 rc4;


        internal bool LogIn;
        internal bool mengeOK;
        internal bool alle;
        internal bool reset = false;

        internal string benutzerEingabe;
        internal string passwortEingabe;
        internal string pbBildName;
        internal string angBenutzer;
        internal string rechnungNeuKunde;
        internal string hash = "FutureFarm";
        internal string verPasswort;
        internal string entPasswort;

        internal int anzahlBenutzer;
        internal int menge;

        internal double weite = Screen.PrimaryScreen.WorkingArea.Width;
        internal double höhe = Screen.PrimaryScreen.WorkingArea.Height;



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
            panelRechnungen.Visible = true;
            panelRechnungen.Dock = DockStyle.Fill;
            panelRechnungen.BringToFront();

            RechnungenEinlesen();
            RechnungArtikelEinlesen();
            RechnungSucheEinlesen();




        }

        private void RechnungenEinlesen()
        {
            listViewRechnungen.Items.Clear();

            var request = new RestRequest("rechnungen", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Rechnung>>(request);

            foreach (Rechnung r in response.Data)
            {
                lvItem = new ListViewItem(r.RechnungID.ToString());
                lvItem.SubItems.Add(r.Datum.ToString("dd.MM.yyyy"));
                lvItem.SubItems.Add(r.Bezahlt.ToString());
                lvItem.SubItems.Add(r.BezahltAm.ToString("dd.MM.yyyy"));
                lvItem.SubItems.Add(r.Kunde.KundenID.ToString());
                lvItem.SubItems.Add(r.Bestellung.BestellungID.ToString());
                
                listViewRechnungen.Items.Add(lvItem);
            }
        }

        private void RechnungArtikelEinlesen()
        {
            listViewRechnungArtikel.Items.Clear();

            var request = new RestRequest("rechnungartikel", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<RechnungArtikel>>(request);

            foreach(RechnungArtikel ra in response.Data)
            {
                lvItem = new ListViewItem(ra.RechnungArtikelID.ToString());
                lvItem.SubItems.Add(ra.Menge.ToString());
                lvItem.SubItems.Add(ra.NettoPreis.ToString());
                lvItem.SubItems.Add(ra.Ust.ToString());
                lvItem.SubItems.Add(ra.Rechnung.RechnungID.ToString());
                lvItem.SubItems.Add(ra.Artikel.ArtikelID.ToString());

                listViewRechnungArtikel.Items.Add(lvItem);
            }
        }

        private void RechnungSucheEinlesen()
        {
            listViewRechnungSuche.Items.Clear();

            var request = new RestRequest("rechnungen", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Rechnung>>(request);

            foreach (Rechnung r in response.Data)
            {
                lvItem = new ListViewItem(r.RechnungID.ToString());
                lvItem.SubItems.Add(r.Datum.ToString());
                lvItem.SubItems.Add(r.Kunde.Nachname.ToString()+" "+r.Kunde.Vorname.ToString());
                lvItem.SubItems.Add(r.Bestellung.BestellungID.ToString());
                lvItem.SubItems.Add(r.Bestellung.BestellungID.ToString());

                listViewRechnungSuche.Items.Add(lvItem);
            }

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
            panelArtikel.BringToFront();

            listViewArtikel.Height = Convert.ToInt16(panelArtikel.Height * 0.45);
            panelArtikelInfo.Location = new Point(Convert.ToInt16(panelArtikel.Width * 0.05), Convert.ToInt16(panelArtikel.Height * 0.05));
            lbArtikelFilter.Location = new Point(10, (panelArtikel.Height-listViewArtikel.Height-40));
            cbArtikelFilter.Location = new Point((10 + lbArtikelFilter.Width), (panelArtikel.Height - listViewArtikel.Height - 43));

            ArtikelEinlesen();
            LieferantenEinlesen();

            CheckEingeloggt();

            panelUnterMenu.Visible = false;
        }

        private void ArtikelEinlesen()
        {
            listViewArtikel.Items.Clear();

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



                if (alle==false)
                {
                    if (a.Aktiv == true)
                    {
                        listViewArtikel.Items.Add(lvItem);
                    }
                }
                else
                {
                    listViewArtikel.Items.Add(lvItem);
                }                    
            }

            for(int i=0; i<listViewArtikel.Items.Count;i++)
            {
                //Lagerstand darstellen
                if (Convert.ToInt32(listViewArtikel.Items[i].SubItems[6].Text) < 5)
                    listViewArtikel.Items[i].BackColor = Color.Yellow;
                if (Convert.ToInt32(listViewArtikel.Items[i].SubItems[6].Text) < 3)
                    listViewArtikel.Items[i].BackColor = Color.Red;

                //Status prüfen (wenn false - kein Lagerstand notwendig - somit überschreibbar)
                if (listViewArtikel.Items[i].SubItems[8].Text.Equals("False"))
                    listViewArtikel.Items[i].BackColor = Color.LightGray;


            }

        }

        private void LieferantenEinlesen()
        {
            cbArtikelLieferanten.Items.Clear();

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

            //panelLieferanten.Visible = true;
            //panelLieferanten.Dock = DockStyle.Fill;
            //panelLieferanten.BringToFront();
        }

        private void btKunden_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btKunden.Height;
            panelAuswahl.Top = btKunden.Top;
            panelsDeaktivieren();

            panelKunden.Visible = true;
            panelKunden.Dock = DockStyle.Fill;
            panelKunden.BringToFront();

            KundenEinlesen();
        }

        private void KundenEinlesen()
        {
            listViewKunden.Items.Clear();
            //API Client

            var request = new RestRequest("kunden", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Kunden>>(request);

            foreach (Kunden k in response.Data)
            {
                //MessageBox.Show(l.Benutzername.ToString());
                lvItem = new ListViewItem(k.KundenID.ToString());
                lvItem.SubItems.Add(k.Anrede.ToString());
                lvItem.SubItems.Add(k.Vorname.ToString());
                lvItem.SubItems.Add(k.Nachname.ToString());
                lvItem.SubItems.Add(k.Firma.ToString());
                lvItem.SubItems.Add(k.Telefonnummer.ToString());
                lvItem.SubItems.Add(k.Email.ToString());
                lvItem.SubItems.Add(k.Strasse.ToString());
                lvItem.SubItems.Add(k.Postleitzahl.PLZ.ToString());
                lvItem.SubItems.Add(k.Aktiv.ToString());
                
                listViewKunden.Items.Add(lvItem);
            }

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

        //public void Einloggen()
        //{
        //    //Benutzer die eingelesen sind löschen
        //    listViewLoginDaten.Items.Clear();

        //    //Benutzer neu aus DB holen
        //    BenutzerEinlesen();

        //    FrmLogin fLogin = new FrmLogin();

        //    //Daten aus TextBox aus FormLogin holen
        //    benutzerEingabe = fLogin.txtBenutzername.Text;
        //    passwortEingabe = fLogin.txtPasswort.Text;

        //    //MessageBox.Show(anzahlBenutzer.ToString());
        //    for (int i = 0; i < listViewLoginDaten.Items.Count; i++)
        //    {
        //        if (listViewLoginDaten.Items[i].SubItems[1].Text.Equals(benutzerEingabe))
        //        {
        //            if (listViewLoginDaten.Items[i].SubItems[2].Text.Equals(passwortEingabe))
        //            {
        //                LogIn = true;
        //                btLogin.Text = benutzerEingabe;
        //                angBenutzer = benutzerEingabe;

        //                btLogin.Image = Image.FromFile("D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\login.png");

        //                LetzteAnmeldungAktualisieren();
        //                MessageBox.Show("Login abgeschlossen, break eingeleitet!\n"+btLogin.Text+"§§"+LogIn.ToString());
                        
        //            }
        //            else
        //            {
        //                MessageBox.Show("Passwort falsch!");
        //                LogIn = false;
        //            }
        //            CheckEingeloggt();
        //        }
        //        break;
        //    }

        //    if (LogIn == false)
        //    {
        //        MessageBox.Show("Login fehlgeschlagen!");
        //    }
        //}

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

            GoFullscreen();
            MenuErstellen();
            panelsDeaktivieren();

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


            //Unterauswahl anzeigen
            btRechnungNeu.Location = new Point(Convert.ToInt16(panelLinks.Width), (pbLogoHome.Height + (btHome.Height * 1)));
            btRechnungNeu.Visible = true;
            btRechnungNeu.Height = btRechnungen.Height;
            btRechnungNeu.BringToFront();
            panelUnterMenu.BringToFront();
            panelUnterMenu.Height = btRechnungNeu.Height;
            panelUnterMenu.Visible = false;


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
            btFirmendaten.Location = new Point(Convert.ToInt16(panelLinks.Width), (pbLogoHome.Height + (btHome.Height * 9)));
            btBenutzer.Location = new Point(Convert.ToInt16(panelLinks.Width), Convert.ToInt16((pbLogoHome.Height + (btHome.Height * 9.5))));
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
            btRechnungNeu.Visible = false;
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
            int menuHöhe = Convert.ToInt16(höhe - pbLogoHome.Height);
            int menuWeite = Convert.ToInt16(weite * 0.10);

            panelLinks.Width = menuWeite;
            panelLinks.Height = Convert.ToInt16(höhe);

            panelMenü.Width = menuWeite;
            panelMenü.Height = menuHöhe;

            int btHöhe = Convert.ToInt16(menuHöhe / anzahlTasks);
            int btWeite = Convert.ToInt16(menuWeite - 10);
            int btPositionX = 10;
            int btPositionY = Convert.ToInt16(pbLogoHome.Height);

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


            //MinMax
            btMenüMinMax.Location = new Point(Convert.ToInt32(panelLinks.Width), Convert.ToInt32(panelMenü.Height / 2));

        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btHome.Top;

            panelUnterMenu.Visible = false;
        }

        private void btBenutzer_Click(object sender, EventArgs e)
        {
            panelsDeaktivieren();

            panelBenutzer.Visible = true;
            panelBenutzer.Dock = DockStyle.Fill;
            panelBenutzer.BringToFront();


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
            panelRechnungen.Visible = false;
            panelRechnungNeu.Visible = false;
            panelBestellungen.Visible = false;
            panelKunden.Visible = false;
            panelAnfragen.Visible = false;

            //if (LogIn == true)
            //{
            //    btBenutzerNeu.Enabled = true;
            //    btBenutzerÄndern.Enabled = true;
            //    btBenutzerLöschen.Enabled = true;
            //    pbPasswort.Enabled = true;
            //}
        }

        private void btAnfragen_Click(object sender, EventArgs e)
        {
            panelsDeaktivieren();

            panelAnfragen.Visible = true;
            panelAnfragen.BringToFront();
            panelAnfragen.Dock = DockStyle.Fill;

            AnfragenEinlesen();
        }

        private void AnfragenEinlesen()
        {
            listViewAnfragen.Items.Clear();
            //Client für API

            var request = new RestRequest("formulare", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Formular>>(request);

            foreach (Formular f in response.Data)
            {
                lvItem = new ListViewItem(f.FormularID.ToString());
                lvItem.SubItems.Add(f.Datum.ToShortDateString());
                lvItem.SubItems.Add(f.Art.Bezeichnung.ToString());
                lvItem.SubItems.Add(f.Nachname + " " + f.Vorname).ToString();

                listViewAnfragen.Items.Add(lvItem);
            }

            for (int i = 0; i < listViewAnfragen.Items.Count; i++)
            {
                if (listViewAnfragen.Items[i].SubItems[2].Text.Equals("Kontakt"))
                    listViewAnfragen.Items[i].BackColor = Color.LightBlue;
                else
                    listViewAnfragen.Items[i].BackColor = Color.White;
            }

        }

        private void btBestellungen_Click(object sender, EventArgs e)
        {
            //Daten löschen
            txtBestellungID.Clear();
            listViewBestellungenArtikelGewählt.Items.Clear();
            cbBestellungStatus.SelectedIndex = 0;
            dtpBestellungenLieferdatum.Value = DateTime.Now;
            cbBestellungenKunde.SelectedIndex = 0;

            panelsDeaktivieren();

            panelBestellungen.Visible = true;
            panelBestellungen.Dock = DockStyle.Fill;
            panelBestellungen.BringToFront();

            BestellungenEinlesen();
            BestellungenArtikelEinlesen();
            ComboKundenEinlesen();
        }

        private void ComboKundenEinlesen()
        {
            cbBestellungenKunde.Items.Clear();
            //Client für API

            var request = new RestRequest("kunden", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Kunden>>(request);

            foreach (Kunden k in response.Data)
            {
                cbBestellungenKunde.Items.Add(k.KundenID.ToString() + " | " + k.Nachname.ToString() + " " + k.Vorname.ToString());
            }
        }

        private void BestellungenArtikelEinlesen()
        {
            listViewBestellungArtikelAlle.Items.Clear();
            //Client für API

            var request = new RestRequest("artikel", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Artikel>>(request);

            foreach (Artikel a in response.Data)
            {
                lvItem = new ListViewItem(a.ArtikelID.ToString());
                lvItem.SubItems.Add(a.Bezeichnung.ToString());
                lvItem.SubItems.Add(a.PreisNetto.ToString());
                lvItem.SubItems.Add(a.Lagerstand.ToString());
                lvItem.SubItems.Add(a.Reserviert.ToString());
                lvItem.SubItems.Add(a.ExterneID.ToString());
                lvItem.SubItems.Add(a.Aktiv.ToString());

                listViewBestellungArtikelAlle.Items.Add(lvItem);
            }

            for (int i = 0; i < listViewBestellungArtikelAlle.Items.Count; i++)
            {
                if (listViewBestellungArtikelAlle.Items[i].SubItems[6].Text.Equals("True"))
                    listViewBestellungArtikelAlle.Items[i].BackColor = Color.White;
                else
                    listViewBestellungArtikelAlle.Items[i].BackColor = Color.LightGray;
            }

        }

        private void BestellungenEinlesen()
        {
            listViewBestellungen.Items.Clear();
            //API Client

            var request = new RestRequest("bestellungen", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Bestellung>>(request);

            foreach (Bestellung b in response.Data)
            {
                lvItem = new ListViewItem(b.BestellungID.ToString());
                lvItem.SubItems.Add(b.Status.ToString());
                lvItem.SubItems.Add(b.Lieferdatum.ToString("dd.MM.yyyy"));
                lvItem.SubItems.Add(b.Kunde.KundenID.ToString());
                listViewBestellungen.Items.Add(lvItem);
            }

        }

        private void btNews_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btNews.Height;
            panelAuswahl.Top = btNews.Top;
            panelsDeaktivieren();

            panelNews.Visible = true;
            panelNews.Dock = DockStyle.Fill;
            panelNews.BringToFront();

            listViewNews.Width = Convert.ToInt16(panelNews.Width * 0.5);

            NewsEinlesen();

            CheckEingeloggt();

            panelUnterMenu.Visible = false;

        }

        private void NewsEinlesen()
        {
            listViewNews.Items.Clear();
            //API Client

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

            panelTermine.Visible = true;
            panelTermine.BringToFront();
            panelTermine.Dock = DockStyle.Fill;

            dtpTermineZeitVon.CustomFormat = "HH:mm";
            dtpTermineZeitVon.Format = DateTimePickerFormat.Custom;
            dtpTermineZeitVon.ShowUpDown = true;
            dtpTermineZeitBis.CustomFormat = "HH:mm";
            dtpTermineZeitBis.Format = DateTimePickerFormat.Custom;
            dtpTermineZeitBis.ShowUpDown = true;

            TermineEinlesen();
        }

        private void TermineEinlesen()
        {
            listViewTermine.Items.Clear();
            //API Client

            var request = new RestRequest("termine", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Termine>>(request);

            foreach (Termine t in response.Data)
            {
                //MessageBox.Show(a.Bezeichnung.ToString());
                lvItem = new ListViewItem(t.TerminID.ToString());
                lvItem.SubItems.Add(t.Titel.ToString());
                lvItem.SubItems.Add(t.Beschreibung.ToString());
                DateTime datumVon = Convert.ToDateTime(t.DatumVon);
                DateTime datumBis = Convert.ToDateTime(t.DatumBis);
                DateTime uhrzeitVon = Convert.ToDateTime(t.UhrzeitVon);
                DateTime uhrzeitBis = Convert.ToDateTime(t.UhrzeitBis);
                lvItem.SubItems.Add(datumVon.ToShortDateString()+"-"+datumBis.ToShortDateString());
                lvItem.SubItems.Add(uhrzeitVon.ToShortTimeString()+"-"+uhrzeitBis.ToShortTimeString());
                lvItem.SubItems.Add(t.Login.BenutzernameID.ToString());
                listViewTermine.Items.Add(lvItem);
            }

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

            verPasswort = lvItem.SubItems[2].Text;
            PasswortEntschlüsseln();
            txtBenutzerPasswort.Text = entPasswort;

            txtBenutzerBenutzername.Text = lvItem.SubItems[1].Text;
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

                    if (listViewPanelBenutzerLogin.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Wählen Sie einen Benutzer aus!");
                        return;
                    }
                    lvItem = listViewPanelBenutzerLogin.SelectedItems[0];

                    Login login = new Login();
                    login.BenutzernameID = Convert.ToInt32(lvItem.SubItems[0].Text);
                    login.Benutzername = txtBenutzerBenutzername.Text;

                    //Passwort verschlüsseln
                    PasswortVerschlüsseln();


                    login.Passwort = verPasswort;


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

        private void PasswortVerschlüsseln()
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(txtBenutzerPasswort.Text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    verPasswort = Convert.ToBase64String(results, 0, results.Length);
                }

            }
        }

        private void btFirmendaten_Click(object sender, EventArgs e)
        {
            panelsDeaktivieren();

            panelFirmendaten.Visible = true;
            panelFirmendaten.Dock = DockStyle.Fill;
            panelFirmendaten.BringToFront();

            FirmendatenEinlesen();

            CheckEingeloggt();

            panelUnterMenu.Visible = false;

        }

        private void FirmendatenEinlesen()
        {
            listViewPanelBenutzerLogin.Items.Clear();
            //API Client

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
                if (listViewArtikel.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Wählen Sie bitte einen Artikel aus!");
                    return;
                }
                lvItem = listViewArtikel.SelectedItems[0];


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
            //try
            //{

               
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Fehler bei der Neuanlage: " + ex.Message);
            //}

        }

        private void btArtikelSpeichern_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtArtikelArtikelID.Text!="")
                {
                    MessageBox.Show("Artikel nicht neu");
                    //ID da - Speichern

                    //Lieferant holen
                    Lieferanten lieferant = new Lieferanten();
                    var request1 = new RestRequest("lieferanten", Method.GET);
                    request1.AddHeader("Content-Type", "application/json");
                    var response1 = client.Execute<List<Lieferanten>>(request1);
                    foreach (Lieferanten l in response1.Data)
                    {
                        //MessageBox.Show("Firma: "+l.Firma.ToString());
                        if (l.LieferantenID == Convert.ToInt32(lvItem.SubItems[5].Text))
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
                    aktArtikel.Aktiv = Convert.ToBoolean(checkboxArtikelAktiv.Checked);
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
                        //MessageBox.Show("Erfolgreich Artikel geändert!");
                        ArtikelEinlesen();
                    }
                }
                else
                {
                    MessageBox.Show("Artikel neu");
                    //Keine ID - NEU
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
                    artikel.Lagerstand = Convert.ToInt16(txtArtikelLagerstand.Text);
                    artikel.Reserviert = Convert.ToInt16(txtArtikelReserviert.Text);
                    artikel.PreisNetto = Convert.ToDouble(txtArtikelNettopreis.Text);
                    artikel.Ust = Convert.ToDouble(txtArtikelUST.Text);
                    artikel.Aktiv = Convert.ToBoolean(checkboxArtikelAktiv.Text);


                    //Artikel hinzufügen
                    var request2 = new RestRequest("artikel", Method.POST);
                    request2.AddHeader("Content-Type", "application/json");
                    request2.AddJsonBody(artikel);
                    var response2 = client.Execute(request2);
                    if (response2.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Erfolgreich Artikel hinzugefügt!");
                        ArtikelEinlesen();
                    }
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
            MessageBox.Show("Löschen: Aktiv ist inaktiv");

            //if (listViewArtikel.SelectedItems.Count==0)
            //{
            //    MessageBox.Show("Wählen Sie einen Artikel aus!");
            //    return;
            //}
            //lvItem = listViewArtikel.SelectedItems[0];
            //Artikel artikel = new Artikel();
            //artikel.ArtikelID = Convert.ToInt32(lvItem.SubItems[0].Text);
            //var request = new RestRequest("artikel/{id}", Method.DELETE);
            //request.AddUrlSegment("id", artikel.ArtikelID.ToString());
            //request.AddHeader("Content-Type", "application/json");
            //request.AddJsonBody(artikel);
            //var response = client.Execute(request);
            //if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //{
            //    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //else
            //{
            //    MessageBox.Show("Erfolgreich gelöscht");
            //    ArtikelEinlesen();
            //}

        }

        private void cbArtikelLieferanten_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Lieferant holen
            string lieferantenID = cbArtikelLieferanten.Text;

            var request1 = new RestRequest("lieferanten", Method.GET);
            request1.AddHeader("Content-Type", "application/json");
            var response1 = client.Execute<List<Lieferanten>>(request1);
            foreach (Lieferanten l in response1.Data)
            {
                if(l.LieferantenID.Equals(lieferantenID))
                txtArtikelLieferantFirma.Text = l.Firma.ToString();
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
            //FrmLogin fl = new FrmLogin();
            //string eingabeBenutzername = fl.txtBenutzername.Text;
            //string eingabePasswort = fl.txtPasswort.Text;


            //MessageBox.Show(eingabeBenutzername + " " + eingabePasswort);

            panelBenutzerLoginEinlesen();

            //einzelne Benutzer in der Listview durchgehen
            for(int i=0; i < listViewPanelBenutzerLogin.Items.Count;i++)
            {
                //Item prüfen
                lvItem = listViewPanelBenutzerLogin.Items[i];
                if (benutzerEingabe.Equals(lvItem.SubItems[1].Text)) //Wenn Benutzer existiert --> Passwort prüfen
                {
                    verPasswort = lvItem.SubItems[2].Text;
                    PasswortEntschlüsseln();

                    if (passwortEingabe.Equals(entPasswort)) //Wenn Passwort stimmmt --> einloggen
                    {
                        LogIn = true;
                        btLogin2.Text = benutzerEingabe;
                        btLogin2.Image = Image.FromFile("D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\login.png");
                        lbBenutzername.Text = benutzerEingabe;
                        MessageBox.Show("Anmeldung erfolgreich!");
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Passwort falsch"); //Wenn Benutzer existiert, aber Passwort falsch
                        break;
                    }
                }
                else
                {
                    if(i==Convert.ToInt32(listViewPanelBenutzerLogin.Items.Count)-1)
                    {
                        MessageBox.Show("Benutzer wurde nicht gefunden!");
                    }
                }

                CheckEingeloggt();
            }
        }

        private void btRechnungSuchen_Click(object sender, EventArgs e)
        {
            //Rechnung in lv Suchen
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            RechnungSuchen(txtRechnungSuche.Text);
        }

        private void RechnungSuchen(string suchtext)
        {
            try
            {
                listViewRechnungSuche.Items.Clear();
                if (suchtext.Equals(""))
                {
                    RechnungSucheEinlesen();
                }
                else
                {
                    //if
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btRechnungNeu_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = true;
            panelUnterMenu.Location = btRechnungNeu.Location;
        }

        private void btRechnungNeu_Click(object sender, EventArgs e)
        {
            panelsDeaktivieren();
            btRechnungNeu.Visible = false;
            panelUnterMenu.Visible = false;

            panelRechnungNeu.Visible = true;
            panelRechnungNeu.Dock = DockStyle.Fill;
            panelRechnungNeu.BringToFront();

            ComboRechnungNeuKundenEinlesen();
            
        }

        private void ComboRechnungNeuKundenEinlesen()
        {
            cbRechnungenKundeNeu.Items.Clear();
            //Client für API

            var request = new RestRequest("kunden", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Kunden>>(request);

            foreach (Kunden k in response.Data)
            {
                cbRechnungenKundeNeu.Items.Add(k.KundenID+" "+k.Nachname+" "+k.Vorname+", "+k.Postleitzahl.PLZ);
            }

        }

        private void ComboRechnungNeuBestellungenEinlesen()
        {
            cbRechnungBestellungNeu.Items.Clear();
            //Client für API

            var request = new RestRequest("bestellungen", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Bestellung>>(request);

            //cbBestellungRechnungNeu.Items.Add("Bestellung erstellen...");
            foreach (Bestellung b in response.Data)
            {
                
                if (b.Kunde.KundenID.ToString().Equals(rechnungNeuKunde))
                cbRechnungBestellungNeu.Items.Add(b.BestellungID+" | Status: "+b.Status);
            }

        }

        private void cbKundeRechnungNeu_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cbKundenText = cbRechnungenKundeNeu.Items[0].ToString();
            rechnungNeuKunde = cbKundenText.Substring(0, cbKundenText.IndexOf(' '));
            ComboRechnungNeuBestellungenEinlesen();

        }

        private void cbBestellungRechnungNeu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbRechnungBestellungNeu.Text.Equals("Bestellung erstellen..."))
            {
                //panelBestellung.Visible = true;
                //panelBestellung.BringToFront();
                MessageBox.Show("Neue Bestellung erstellen");

                FrmWarnung fw = new FrmWarnung();
                fw.lbText.Text="Wollen Sie eine neue Bestellung erstellen?";
                fw.ShowDialog();
                if (fw.weiter == true)
                {
                    //panelBestellung.Visible = 0;
                    //panelBestellung.BringToFront();
                }

            }
            else
            {
                string BestellungID = cbRechnungBestellungNeu.Text.Substring(0, cbRechnungBestellungNeu.Text.IndexOf(" "));
                double ustSumme = 0;
                double nettoSumme = 0;
                int anzahlArtikel = 0;

                //Client für API

                var request = new RestRequest("bestellungartikel", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute<List<BestellungArtikel>>(request);

                foreach (BestellungArtikel b in response.Data)
                {
                    if (b.Bestellung.BestellungID.ToString().Equals(BestellungID)&&b.Aktiv==true)
                    {
                        lvItem = new ListViewItem(b.Artikel.ArtikelID.ToString());
                        lvItem.SubItems.Add(b.Artikel.Bezeichnung);
                        lvItem.SubItems.Add(b.Menge.ToString());
                        lvItem.SubItems.Add(b.Nettopreis.ToString());
                        lvItem.SubItems.Add(b.Ust.ToString());

                        listViewRechnungNeuArtikel.Items.Add(lvItem);

                        ustSumme += Math.Round(Convert.ToDouble(b.Ust / 100 * b.Nettopreis), 2);
                        nettoSumme += Math.Round(Convert.ToDouble(b.Nettopreis), 2);
                        anzahlArtikel++;
                    }
                }

                txtRechnungNeuNetto.Text = Math.Round(nettoSumme, 2).ToString("0.00");
                txtRechnungNeuUst.Text = Math.Round(ustSumme, 2).ToString("0.00");
                txtRechnungNeuBrutto.Text = (nettoSumme + ustSumme).ToString("0.00");

            }

        }

        private void panelRechnungNeu_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btRechnungNeu.Visible = false;
        }

        private void btWordErstellen_Click(object sender, EventArgs e)
        {

            ////Word Dokument erstellen
            //string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            //string vorlage = path + @"\RechnungVorlage.docx";
            //string speicherort = path + @"\Speiseplan.docx";

            //CreateWordDocument(vorlage, speicherort);

        }


        internal void CreateWordDocument(object filename, object saveAs)
        {
            Word.Application wordApp = new Word.Application();

            object missing = Missing.Value;
            Word.Document myWordDoc = null;

            if (File.Exists((string)filename))
            {

                object readOnly = false;
                object isVisible = false;
                wordApp.Visible = false;


                myWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing);


                myWordDoc.Activate();
                wordApp.Visible = false;

                //suche
                //this.FindAndReplace(wordApp, "<vorMontag>", cbVorMontag.Text);
                //this.FindAndReplace(wordApp, "<hauptMontag>", cbHauptMontag.Text);
                //this.FindAndReplace(wordApp, "<nachMontag>", cbNachMontag.Text);

                //this.FindAndReplace(wordApp, "<vorDienstag>", cbVorDienstag.Text);
                //this.FindAndReplace(wordApp, "<hauptDienstag>", cbHauptDienstag.Text);
                //this.FindAndReplace(wordApp, "<nachDienstag>", cbNachDienstag.Text);

                //this.FindAndReplace(wordApp, "<vorMittwoch>", cbVorMittwoch.Text);
                //this.FindAndReplace(wordApp, "<hauptMittwoch>", cbHauptMittwoch.Text);
                //this.FindAndReplace(wordApp, "<nachMittwoch>", cbNachMittwoch.Text);

                //this.FindAndReplace(wordApp, "<vorDonnerstag>", cbVorDonnerstag.Text);
                //this.FindAndReplace(wordApp, "<hauptDonnerstag>", cbHauptDonnerstag.Text);
                //this.FindAndReplace(wordApp, "<nachDonnerstag>", cbNachDonnerstag.Text);

                //this.FindAndReplace(wordApp, "<vorFreitag>", cbVorFreitag.Text);
                //this.FindAndReplace(wordApp, "<hauptFreitag>", cbHauptFreitag.Text);
                //this.FindAndReplace(wordApp, "<nachFreitag>", cbNachFreitag.Text);

                //this.FindAndReplace(wordApp, "<datum>", DateTime.Now.ToShortDateString());
            }
            else
            {
                MessageBox.Show("Dokument konnte nicht gefunden werden");
            }

            ////Speichern
            myWordDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing);

            DialogResult dialogResult = MessageBox.Show("Rechnung wurde angelegt,\nsoll diese direkt geöffnet werden?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                wordApp.Visible = true;
            }
            else if (dialogResult == DialogResult.No)
            {
                myWordDoc.Close();
                wordApp.Quit();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panelTermine_MouseEnter(object sender, EventArgs e)
        {

        }

        private void listViewTermine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewTermine.SelectedItems.Count == 0)
            {
                MessageBox.Show("Wählen Sie bitte einen Termin aus!");
                return;
            }

            lvItem = listViewTermine.SelectedItems[0];

            txtTerminID.Text = lvItem.SubItems[0].Text;
            txtTerminTitel.Text = lvItem.SubItems[1].Text;
            txtTerminBeschreibung.Text = lvItem.SubItems[2].Text;

            //Datum
            string gesamtesDatum = lvItem.SubItems[3].Text;
            string datumVor = gesamtesDatum.Substring(0, gesamtesDatum.IndexOf('-'));
            dtpTerminDatumVon.Value = Convert.ToDateTime(datumVor);
            string datumNach = gesamtesDatum.Substring(gesamtesDatum.IndexOf('-') + 1, 10);
            dtpTerminDatumBis.Value = Convert.ToDateTime(datumNach);

            //Uhrzeit
            string gesamteZeit = lvItem.SubItems[4].Text;
            string zeitVon = gesamteZeit.Substring(0, gesamteZeit.IndexOf('-'));
            dtpTermineZeitVon.Value = Convert.ToDateTime(zeitVon);
            string zeitBis=gesamteZeit.Substring(gesamteZeit.IndexOf('-')+1, 5);
            dtpTermineZeitBis.Value = Convert.ToDateTime(zeitBis);


        }

        private void btTerminNeu_Click(object sender, EventArgs e)
        {
            try
            {

                //Artikel erzeugen
                Termine termin = new Termine();
                termin.Titel = txtTerminTitel.Text;
                termin.Beschreibung = txtTerminBeschreibung.Text;
                //termin.


                //    //Artikel hinzufügen
                //    var request2 = new RestRequest("artikel", Method.POST);
                //    request2.AddHeader("Content-Type", "application/json");
                //    request2.AddJsonBody(artikel);
                //    var response2 = client.Execute(request2);
                //    if (response2.StatusCode == System.Net.HttpStatusCode.BadRequest)
                //    {
                //        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //    else
                //    {
                //        MessageBox.Show("Erfolgreich Artikel hinzugefügt!");
                //        ArtikelEinlesen();
                //    }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Neuanlage: " + ex.Message);
            }

        }

        private void listViewBestellungArtikelAlle_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btKundenSpeichern_Click(object sender, EventArgs e)
        {
            if(txtKundenID.Text!="")
            {
                //Kunde speichern
            }
            else
            {
                //Kunde neu anlegen
            }
        }

        private void btKundenLöschen_Click(object sender, EventArgs e)
        {
            if(txtKundenID.Text!="")
            {
                //Kunden inaktiv setzen

            }
            else
            {
                MessageBox.Show("Kein Kunde ausgewählt!", "Fehler");
            }
        }

        private void listViewKunden_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lvItem = listViewKunden.SelectedItems[0];

                txtKundenID.Text = lvItem.SubItems[0].Text.ToString();
                //...

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bestellungÜbernehmenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listViewAnfragen.SelectedItems[0].SubItems[2].Text.Equals("Bestellung"))
            {
                //Bestellung direkt übernehmen
            }
            else
                MessageBox.Show("Kontaktanfragen können nicht übernommen werden.");
        }

        private void listViewAnfragen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listViewAnfragen.SelectedItems[0].SubItems[2].Text.Equals("Bestellung"))
                {
                    btAnfragenBestellungÜbernehmen.Visible = true;
                    btAnfrageErledigt.Visible = false;
                }
                else
                {
                    btAnfrageErledigt.Visible = true;
                    btAnfragenBestellungÜbernehmen.Visible = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void listViewRechnungSuche_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lvItem = listViewRechnungSuche.SelectedItems[0];

                txtRechnungID.Text = lvItem.SubItems[0].Text;
                txtRechnungenBestellungID.Text = lvItem.SubItems[3].Text;
                txtRechnungKunde.Text = lvItem.SubItems[2].Text;
                
                for (int i=0; i<listViewRechnungen.Items.Count;i++)
                {
                    if(txtRechnungID.Text.Equals(listViewRechnungen.Items[i].SubItems[0].Text))
                    {
                        cbRechnungBezahlt.Checked = Convert.ToBoolean(listViewRechnungen.Items[i].SubItems[2].Text.ToLower());
                        DateTime rechnungBezahltAm = Convert.ToDateTime(listViewRechnungen.Items[i].SubItems[3].Text);
                        dtpRechnungBezahlt.Value = rechnungBezahltAm;
                    }
                }

                //Artikel anzeigen
                for(int i=0; i<listViewRechnungArtikel.Items.Count;i++)
                {
                    if(listViewRechnungArtikel.Items[i].SubItems[4].Text.Equals(txtRechnungID.Text))
                    {
                        string artikelID = listViewRechnungArtikel.Items[i].SubItems[5].Text;

                        //Client für API

                        var request = new RestRequest("artikel/{id}", Method.GET);
                        request.AddUrlSegment("id", artikelID);
                        request.AddHeader("Content-Type", "application/json");
                        var response = client.Execute<List<Artikel>>(request);

                        foreach (Artikel a in response.Data)
                        {
                            lvItem = new ListViewItem(a.ArtikelID.ToString());
                            lvItem.SubItems.Add(a.Bezeichnung.ToString());
                            
                            lvItem.SubItems.Add(listViewRechnungArtikel.Items[i].SubItems[1].Text);
                            lvItem.SubItems.Add(a.PreisNetto.ToString());
                            lvItem.SubItems.Add(a.Ust.ToString());

                            listViewRechnungGewähltArtikel.Items.Add(lvItem);
                        }
                    }
                }

                //Summen berechnen
                double summeNetto = 0;
                double summeUst=0;
                for (int i = 0; i < listViewRechnungGewähltArtikel.Items.Count; i++)
                {
                    summeNetto += Convert.ToDouble(listViewRechnungGewähltArtikel.Items[i].SubItems[2].Text);
                    summeUst += Convert.ToDouble(listViewRechnungGewähltArtikel.Items[i].SubItems[2].Text) * (Convert.ToDouble(listViewRechnungGewähltArtikel.Items[i].SubItems[4].Text) / 100);
                }

                txtRechnungSummeNetto.Text = summeNetto.ToString("C2");
                txtRechnungSummeUST.Text = summeUst.ToString("C2");
                txtRechnungSummeBrutto.Text = (summeNetto + summeUst).ToString("C2");


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btMenüMinMax_Click(object sender, EventArgs e)
        {
            if(btMenüMinMax.Text.Equals("<"))
            {
                btMenüMinMax.Text = ">";
                panelLinks.Width = 50;
                btMenüMinMax.Location = new Point(Convert.ToInt32(panelLinks.Width), Convert.ToInt32(panelMenü.Height) / 2);
            }
            else if(btMenüMinMax.Text.Equals(">"))
            {
                btMenüMinMax.Text = "<";
                panelLinks.Width = Convert.ToInt16(weite * 0.10);
                btMenüMinMax.Location = new Point(Convert.ToInt32(panelLinks.Width), Convert.ToInt32(panelMenü.Height) / 2);
            }
        }

        private void listViewBestellungen_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                listViewBestellungenArtikelGewählt.Items.Clear();
                lvItem = listViewBestellungen.SelectedItems[0];

                //ID
                txtBestellungID.Text = lvItem.SubItems[0].Text;

                //Status
                if (lvItem.SubItems[1].Text.Equals("Neu"))
                {
                    cbBestellungStatus.SelectedItem = cbBestellungStatus.Items[0];
                }
                else if (lvItem.SubItems[1].Text.Equals("Bearbeitet"))
                {
                    cbBestellungStatus.SelectedItem = cbBestellungStatus.Items[1];
                }
                else
                {
                    cbBestellungStatus.SelectedItem = cbBestellungStatus.Items[2];
                }

                //Datum
                dtpBestellungenLieferdatum.Value = Convert.ToDateTime(lvItem.SubItems[2].Text);

                //Kunde einlesen
                cbBestellungenKunde.Items.Clear();
                var request = new RestRequest("kunden", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute<List<Kunden>>(request);

                foreach (Kunden k in response.Data)
                {
                    cbBestellungenKunde.Items.Add(k.KundenID.ToString() + " | " + k.Nachname.ToString() + " " + k.Vorname.ToString());
                }

                //Kunden wählen
                string kundenID = lvItem.SubItems[3].Text;

                for (int i = 0; i < cbBestellungenKunde.Items.Count; i++)
                {
                    string aktItem = cbBestellungenKunde.Items[i].ToString();

                    if (kundenID.Equals(aktItem.Substring(0, aktItem.IndexOf("|") - 1)))
                    {
                        cbBestellungenKunde.SelectedItem = cbBestellungenKunde.Items[i];
                    }
                }

                //Artikel einlesen laut BestellungID
                string aktBestellungID = txtBestellungID.Text;

                ArtikelEinlesen();
                BestellungenArtikelEinlesen();

                BestellungArtikelNachIDEinlesen();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BestellungArtikelNachIDEinlesen()
        {
            listViewRechnungGewähltArtikel.Items.Clear();

            var request1 = new RestRequest("bestellungartikel", Method.GET);
            request1.AddHeader("Content-Type", "application/json");
            var response1 = client.Execute<List<BestellungArtikel>>(request1);



            foreach (BestellungArtikel ba in response1.Data)
            {
                if (ba.Bestellung.BestellungID.ToString().Equals(txtBestellungID.Text))
                {
                    lvItem = new ListViewItem(ba.Artikel.ArtikelID.ToString());
                    lvItem.SubItems.Add(ba.Artikel.Bezeichnung.ToString());
                    lvItem.SubItems.Add(ba.Menge.ToString());
                    lvItem.SubItems.Add(ba.Nettopreis.ToString());
                    lvItem.SubItems.Add(ba.Ust.ToString());
                    lvItem.SubItems.Add(ba.BestellungArtikelID.ToString());

                    if(ba.Aktiv==true)
                    listViewBestellungenArtikelGewählt.Items.Add(lvItem);
                }
            }
        }

        private void listViewBestellungArtikelAlle_DoubleClick(object sender, EventArgs e)
        {
            FrmWarnung fw = new FrmWarnung();

            //Leere ID --> Status neu
            if (txtBestellungID.Text.Equals(""))
            {
                fw.lbText.Text = "Keine Bestellung ausgewählt!\nWollen Sie eine neue Bestellung anlegen?";
                fw.ShowDialog();
                if (fw.weiter == true)
                {
                    if (cbBestellungStatus.Text.Equals("") || cbBestellungenKunde.Text.Equals(""))
                    {
                        fw.lbText.Text = "Bitte geben Sie alle Daten ein";
                        fw.ShowDialog();
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (txtBestellungID.Text != "")
                    {
                        //Neue Bestellung anlegen
                        Bestellung b = new Bestellung();
                        b.Status = cbBestellungStatus.Text;
                        b.Lieferdatum = dtpBestellungenLieferdatum.Value;
                        //Kunden holen
                        string kundenID = cbBestellungenKunde.Text.Substring(0, cbBestellungenKunde.Text.IndexOf('|') - 1);
                        var request1 = new RestRequest("kunden/{id}", Method.GET);
                        request1.AddUrlSegment("id", kundenID);
                        request1.AddHeader("Content-Type", "application/json");
                        var response1 = client.Execute<Kunden>(request1);
                        Kunden k = response1.Data;
                        b.Kunde = k;

                        //Bestellung hinzufügen
                        var request2 = new RestRequest("bestellungen", Method.POST);
                        request2.AddHeader("Content-Type", "application/json");
                        request2.AddJsonBody(b);
                        var response2 = client.Execute(request2);

                        BestellungenEinlesen();
                        lvItem = listViewBestellungen.Items[listViewBestellungen.Items.Count - 1];
                        txtBestellungID.Text = lvItem.Text;

                    }
                    else
                        return;
                }

            }


            if (listViewBestellungArtikelAlle.SelectedItems[0].SubItems[6].Text.Equals("False"))
            {
                fw.lbText.Text = "Gewählter Artikel ist inaktiv,\ntrotzdem hinzufügen?";
                fw.ShowDialog();
                if (fw.weiter == true)
                {
                    BestellungArtikelHinzufügen();


                }
                else
                {
                    //Artikel nicht hinzufügen
                }
            }
            else
            {
                BestellungArtikelHinzufügen();

            }
        }

        private void BestellungArtikelHinzufügen()
        {
            //Menge
            MengenPrüfung();

            //Artikel hinzufügen
            if (mengeOK == true)
            {
                var request2 = new RestRequest("artikel/{id}", Method.GET);
                request2.AddUrlSegment("id", listViewBestellungArtikelAlle.SelectedItems[0].Text);
                request2.AddHeader("Content-Type", "application/json");
                var response2 = client.Execute<Artikel>(request2);
                Artikel a = response2.Data;

                var request1 = new RestRequest("bestellungen/{id}", Method.GET);
                request1.AddUrlSegment("id", txtBestellungID.Text);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<Bestellung>(request1);
                Bestellung b = response1.Data;

                BestellungArtikel bestellungArtikel = new BestellungArtikel();
                bestellungArtikel.Menge = menge;
                bestellungArtikel.Nettopreis = Convert.ToDouble(listViewBestellungArtikelAlle.SelectedItems[0].SubItems[2].Text);
                bestellungArtikel.Bestellung = b;
                bestellungArtikel.Artikel = a;
                bestellungArtikel.Ust = a.Ust;
                bestellungArtikel.Aktiv = true;

                //BestellungArtikel hinzufügen
                var request3 = new RestRequest("bestellungartikel", Method.POST);
                request3.AddHeader("Content-Type", "application/json");
                request3.AddJsonBody(bestellungArtikel);
                var response3 = client.Execute(request3);
                if (response3.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Alle passenden BestellungenArtikel einlesen
                    listViewBestellungenArtikelGewählt.Items.Clear();
                    BestellungArtikelNachIDEinlesen();

                    //Menge reservieren
                    Artikel artikel = new Artikel();
                    artikel.ArtikelID = a.ArtikelID;
                    artikel.ExterneID = a.ExterneID;
                    artikel.Bezeichnung = a.Bezeichnung;
                    artikel.PreisNetto = a.PreisNetto;
                    artikel.Ust = a.Ust;
                    artikel.Lagerstand = a.Lagerstand;
                    artikel.Reserviert = (a.Reserviert + menge);
                    artikel.Bild = a.Bild;
                    artikel.Aktiv = a.Aktiv;
                    artikel.Lieferant = a.Lieferant;

                    var request = new RestRequest("artikel", Method.PUT);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(artikel);
                    var response = client.Execute(request);
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        BestellungenArtikelEinlesen();
                    }
                }
            }
            else
            {
                MengenPrüfung();
            }
        }

        private void MengenPrüfung()
        {
            FrmMenge fm = new FrmMenge();
            FrmWarnung fw = new FrmWarnung();


            fm.ShowDialog();
            try
            {
                menge = Convert.ToInt32(fm.txtMenge.Text);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //Menge mit Lagerstand vergleichen
            int lagerstand = Convert.ToInt32(listViewBestellungArtikelAlle.SelectedItems[0].SubItems[3].Text);
            int reserviert = Convert.ToInt32(listViewBestellungArtikelAlle.SelectedItems[0].SubItems[4].Text);

            if ((lagerstand-reserviert) < menge)
            {
                fw.lbText.Text = "Menge höher als der Lagerbestand!\nBitte geringere Menge eingeben";
                fw.ShowDialog();
                if (fw.weiter == true)
                    MengenPrüfung();
                else
                {
                    
                }
            }
            else if(menge<=0)
            {
                return;
            }
            else if((lagerstand-reserviert)>=menge)
            {
                mengeOK = true;
            }
            else
            {
                mengeOK = false;
            }

        }

        private void listViewBestellungenArtikelGewählt_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem gewlvItem = listViewBestellungenArtikelGewählt.SelectedItems[0];


            menge = Convert.ToInt32(gewlvItem.SubItems[2].Text);

            //Artikel nach ID holen
            var request2 = new RestRequest("artikel/{id}", Method.GET);
            request2.AddUrlSegment("id", gewlvItem.Text);
            request2.AddHeader("Content-Type", "application/json");
            var response2 = client.Execute<Artikel>(request2);
            Artikel a = response2.Data;
            //MessageBox.Show(a.Bezeichnung);

            //Artikel ändern - Menge
            Artikel artikel = new Artikel();
            artikel.ArtikelID = a.ArtikelID;
            artikel.ExterneID = a.ExterneID;
            artikel.Bezeichnung = a.Bezeichnung;
            artikel.PreisNetto = a.PreisNetto;
            artikel.Ust = a.Ust;
            artikel.Lagerstand = a.Lagerstand;
            artikel.Reserviert = (a.Reserviert - Convert.ToInt32(gewlvItem.SubItems[2].Text));
            artikel.Bild = a.Bild;
            artikel.Aktiv = a.Aktiv;
            artikel.Lieferant = a.Lieferant;

            var request = new RestRequest("artikel", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(artikel);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                BestellungenArtikelEinlesen();
            }

            //Bestellung holen
            var request3 = new RestRequest("bestellungen/{id}", Method.GET);
            request3.AddUrlSegment("id", txtBestellungID.Text);
            request3.AddHeader("Content-Type", "application/json");
            var response3 = client.Execute<Bestellung>(request3);
            Bestellung b = response3.Data;
            //MessageBox.Show(b.BestellungID.ToString() + " " + b.Kunde.Nachname);


            //Artikel holen
            var request4 = new RestRequest("artikel/{id}", Method.GET);
            string artikelID = gewlvItem.SubItems[0].Text;
            request4.AddUrlSegment("id", artikelID);
            request4.AddHeader("Content-Type", "application/json");
            var response4 = client.Execute<Artikel>(request4);
            Artikel a1 = response4.Data;
            //MessageBox.Show("Artikel holen: "+a1.ArtikelID.ToString() + " " + a1.Bezeichnung);

            //BestellungArtikel inaktiv
            BestellungArtikel bestellungArtikel = new BestellungArtikel();
            MessageBox.Show(gewlvItem.SubItems[5].Text);
            bestellungArtikel.BestellungArtikelID = Convert.ToInt32(gewlvItem.SubItems[5].Text);

            bestellungArtikel.Menge = Convert.ToInt32(gewlvItem.SubItems[2].Text);
            bestellungArtikel.Nettopreis = Convert.ToDouble(gewlvItem.SubItems[3].Text);
            bestellungArtikel.Ust= Convert.ToDouble(gewlvItem.SubItems[4].Text);
            bestellungArtikel.Aktiv = false;
            bestellungArtikel.Bestellung = b;
            bestellungArtikel.Artikel=a1;

            //MessageBox.Show(b.BestellungID.ToString() + " " + a1.Bezeichnung);

            var request1 = new RestRequest("bestellungartikel", Method.PUT);
            request1.AddHeader("Content-Type", "application/json");
            request1.AddJsonBody(bestellungArtikel);
            var response1 = client.Execute(request1);
            if (response1.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                listViewBestellungenArtikelGewählt.Items.Remove(listViewBestellungenArtikelGewählt.SelectedItems[0]);
                MessageBox.Show("Erfolgreich entfernt!");
            }

            //HIER
            //Falscher Artike gewählt




        }

        private void btBestellungSpeichern_Click(object sender, EventArgs e)
        {
            if (txtBestellungID.Text.Equals(""))
            {
                if(cbBestellungStatus.Text.Equals("") || cbBestellungenKunde.Text.Equals(""))
                {
                    FrmWarnung fm = new FrmWarnung();
                    fm.lbText.Text = "Bitte Eingabe prüfen!\n\nDaten nicht korrekt";
                    fm.ShowDialog();
                }
                else
                {
                    //Kunden holen
                    var request1 = new RestRequest("kunden/{id}", Method.GET);
                    string kundenID = cbBestellungenKunde.Text.Substring(0, cbBestellungenKunde.Text.IndexOf('|') - 1);
                    request1.AddUrlSegment("id", kundenID);
                    request1.AddHeader("Content-Type", "application/json");
                    var response1 = client.Execute<Kunden>(request1);
                    Kunden k = response1.Data;
                    MessageBox.Show(k.Nachname);


                    //Neue Bestellung anlegen
                    Bestellung bestellung = new Bestellung();
                    bestellung.Status = cbBestellungStatus.Text;
                    bestellung.Lieferdatum = Convert.ToDateTime(dtpBestellungenLieferdatum.Value.ToShortDateString());
                    bestellung.Kunde = k;

                    //Benutezr hinzufügen
                    var request2 = new RestRequest("bestellungen", Method.POST);
                    request2.AddHeader("Content-Type", "application/json");
                    request2.AddJsonBody(bestellung);
                    var response2 = client.Execute(request2);


                    BestellungenEinlesen();

                }
            }
            else
            {
                //Prüfen ob BestellungArtikel ID schon da ist
                for(int i=0; i<listViewBestellungenArtikelGewählt.Items.Count;i++)
                {
                    //if(listViewBestellungenArtikelGewählt.Items[i].Text.Equals())
                }
            }
       
        }

        private void listViewBestellungenArtikelGewählt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lvItem = listViewBestellungenArtikelGewählt.SelectedItems[0];
                //MessageBox.Show(lvItem.SubItems[0].Text);

            }
            catch(Exception ex)
            {

            }
        }

        private void PasswortEntschlüsseln()
        {
            byte[] data = Convert.FromBase64String(verPasswort.ToString());
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    entPasswort = UTF8Encoding.UTF8.GetString(results);
                }

            }
        }
    }
}

