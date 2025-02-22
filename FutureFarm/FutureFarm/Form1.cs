﻿using System;
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
using FutureFarm.Properties;
using System.Drawing.Imaging;

namespace FutureFarm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            f1 = this;
            
            InitializeComponent();

            //Auswahl anpassen
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;


            client = new RestClient("http://futurefarm.projects.hakmistelbach.ac.at:20216")
            {
                Authenticator = new HttpBasicAuthenticator("demo", "demo")
            };


        }

        RestClient client;
        ListViewItem lvItem;
        Artikel aktArtikel;
        RC4 rc4;

        internal static Form1 f1;
        internal static Form fl = new FrmLogin();
        internal static FrmLadebildschirm flade;


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

        internal string[] artikelÜbernahme;

        internal int anzahlBenutzer;
        internal int menge;

        internal double weite = Screen.PrimaryScreen.WorkingArea.Width;
        internal double höhe = Screen.PrimaryScreen.WorkingArea.Height;

        internal Image imgLogin = Properties.Resources.login;
        internal Image imgLogout = Properties.Resources.logout;
        internal Image min = Properties.Resources.min;
        internal Image max = Properties.Resources.max;
        internal Image bestätigungImage = Properties.Resources.Unbenannt_1;


        private void btHome_Click(object sender, EventArgs e)
        {
            Home();
        }

        private void Home()
        {
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;
            panelsDeaktivieren();
            pbMinMax.BringToFront();
        }

        private void btEinstellungen_Click(object sender, EventArgs e)
        {
            Einstellungen();
        }

        private void Einstellungen()
        {
            panelAuswahl.Height = btEinstellungen.Height;
            panelAuswahl.Top = btEinstellungen.Top;
            panelsDeaktivieren();
        }

        private void btRechnungen_Click(object sender, EventArgs e)
        {
            Rechnungen();
        }

        private void Rechnungen()
        {
            panelAuswahl.Height = btRechnungen.Height;
            panelAuswahl.Top = btRechnungen.Top;
            panelsDeaktivieren();
            panelRechnungen.Visible = true;
            panelRechnungen.Dock = DockStyle.Fill;
            panelRechnungen.BringToFront();
            pbMinMax.BringToFront();

            RechnungenEinlesen();
            RechnungArtikelEinlesen();
            RechnungSucheEinlesen();

            listViewRechnungen.Sorting = SortOrder.Descending;
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
            Artikel();
        }

        private void Artikel()
        {
            panelAuswahl.Height = btArtikel.Height;
            panelAuswahl.Top = btArtikel.Top;

            panelsDeaktivieren();

            panelArtikel.Visible = true;
            panelArtikel.Dock = DockStyle.Fill;
            panelArtikel.BringToFront();
            pbMinMax.BringToFront();


            listViewArtikel.Height = Convert.ToInt16(panelArtikel.Height * 0.45);
            panelArtikelInfo.Location = new Point(Convert.ToInt16(panelArtikel.Width * 0.05), Convert.ToInt16(panelArtikel.Height * 0.05));
            lbArtikelFilter.Location = new Point(10, (panelArtikel.Height - listViewArtikel.Height - 40));
            cbArtikelFilter.Location = new Point((10 + lbArtikelFilter.Width), (panelArtikel.Height - listViewArtikel.Height - 43));
            lbArtikelSuchen.Location = new Point((20 + lbArtikelFilter.Width + cbArtikelFilter.Width), (panelArtikel.Height - listViewArtikel.Height - 40));
            txtArtikelSuchen.Location = new Point((20 + lbArtikelFilter.Width + cbArtikelFilter.Width + lbArtikelSuchen.Width), (panelArtikel.Height - listViewArtikel.Height - 43));
            btArtikelSuchen.Location = new Point((25 + lbArtikelFilter.Width + cbArtikelFilter.Width + lbArtikelSuchen.Width + txtArtikelSuchen.Width), (panelArtikel.Height - listViewArtikel.Height - 43));



            ArtikelEinlesen();
            ArtikelLieferantenEinlesen();

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

        private void ArtikelLieferantenEinlesen()
        {
            cbArtikelLieferanten.Items.Clear();

            var request = new RestRequest("lieferanten", Method.GET);
            request.AddHeader("Content-Type", "application/Json");
            var response = client.Execute<List<Lieferanten>>(request);

            foreach(Lieferanten l in response.Data)
            {
                if(l.Aktiv==true)
                cbArtikelLieferanten.Items.Add(l.LieferantenID);
            }
        }

        private void LieferantenEinlesen()
        {
            listViewLieferanten.Items.Clear();

            var request = new RestRequest("lieferanten", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Lieferanten>>(request);

            
            foreach (Lieferanten l in response.Data)
            {
                lvItem = new ListViewItem(l.LieferantenID.ToString());
                lvItem.SubItems.Add(l.Vorname.ToString());
                lvItem.SubItems.Add(l.Nachname.ToString());
                lvItem.SubItems.Add(l.Firma.ToString());
                lvItem.SubItems.Add(l.Telefonnummer.ToString());
                lvItem.SubItems.Add(l.Email.ToString());
                lvItem.SubItems.Add(l.Strasse.ToString());
                lvItem.SubItems.Add(l.Postleitzahl.PLZ.ToString());
                lvItem.SubItems.Add(l.Aktiv.ToString());
                lvItem.SubItems.Add(l.UID.ToString());

                if(l.Aktiv==true)
                listViewLieferanten.Items.Add(lvItem);
            }

        }

        private void btLieferanten_Click(object sender, EventArgs e)
        {
            Lieferanten();
        }

        private void Lieferanten()
        {
            panelAuswahl.Height = btLieferanten.Height;
            panelAuswahl.Top = btLieferanten.Top;
            panelsDeaktivieren();


            panelLieferanten.Visible = true;
            panelLieferanten.Dock = DockStyle.Fill;
            panelLieferanten.BringToFront();

            pbMinMax.BringToFront();

            LieferantenEinlesen();
        }

        private void btKunden_Click(object sender, EventArgs e)
        {
            Kunden();
            cbKundenOrt.Items.Clear();
        }

        private void Kunden()
        {
            panelAuswahl.Height = btKunden.Height;
            panelAuswahl.Top = btKunden.Top;
            panelsDeaktivieren();

            panelKunden.Visible = true;
            panelKunden.Dock = DockStyle.Fill;
            panelKunden.BringToFront();

            lbKundenSuchen.Location = new Point((15), (panelKunden.Height - listViewKunden.Height - 40));
            txtArtikelSuchen.Location = new Point((20 + lbKundenSuchen.Width), (panelArtikel.Height - listViewArtikel.Height - 43));
            btArtikelSuchen.Location = new Point((25 + txtArtikelSuchen.Width + lbKundenSuchen.Width), (panelArtikel.Height - listViewArtikel.Height - 43));
            pbMinMax.BringToFront();


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
                
                if(k.Aktiv==true)
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
                   // btLogin2.Image = Image.FromFile("D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\logout.png");
                    btLogin2.Image = new Bitmap(FutureFarm.Properties.Resources.logout);

                    btLogin2.Text = "Log In";
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

        internal void CheckEingeloggt()
        {
            pbPasswort.Enabled = LogIn;
            btArtikelLöschen.Enabled = LogIn;
            btArtikelSpeichern.Enabled = LogIn;
            btNewsLöschen.Enabled = LogIn;
            btNewsSpeichern.Enabled = LogIn;
            btLieferantenSpeichern.Enabled = LogIn;
            btLieferantenLöschen.Enabled = LogIn;
            btKundenSpeichern.Enabled = LogIn;
            btKundenLöschen.Enabled = LogIn;
            btArtikelSpeichern.Enabled = LogIn;
            btArtikelLöschen.Enabled = LogIn;
            btTerminLöschen.Enabled = LogIn;
            btTerminSpeichern.Enabled = LogIn;
            btBenutzerLöschen.Enabled = LogIn;
            btBenutzerÄndern.Enabled = LogIn;
            btFirmendatenSpeichern.Enabled = LogIn;
            btRechnungSpeichern.Enabled = LogIn;
            btRechnungNeuSpeichern.Enabled = LogIn;
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
                Bestätigung();
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
            menuStrip1.Visible = false;
            flade = new FrmLadebildschirm();
            flade.ShowDialog();
            this.Hide();    

            
            GoFullscreen();
            MenuErstellen();
            panelsDeaktivieren();
            CheckEingeloggt();

            pbMinMax.Image = min;

            panelAuswahl.Top = btHome.Top;
            panelAuswahl.Height = btHome.Height;
            BenutzerEinlesen();

            flade.Hide();
            flade.Close();
            cbArtikelFilter.SelectedIndex = 1;

            fl.ShowDialog();
            this.Show();

            CheckEingeloggt();

            this.KeyPreview = true;

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
            btFirmendaten.Location = new Point(Convert.ToInt16(panelLinks.Width), (pbLogoHome.Height + (btHome.Height * 10)));
            btBenutzer.Location = new Point(Convert.ToInt16(panelLinks.Width), Convert.ToInt16((pbLogoHome.Height + (btHome.Height * 10.5))));
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
            panelAuswahl.Height = btFirmendaten.Height;
            panelAuswahl.Top = btFirmendaten.Top;
            
        }

        private void btBenutzer_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = true;
            panelUnterMenu.Location = btBenutzer.Location;
        }

        private void MenuErstellen()
        {
            int anzahlTasks = 11;
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
            btWebsite.Height = btHöhe;
            btWebsite.Width = btWeite;
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
            btWebsite.Location = new Point(btPositionX, btHöhe * 9);
            btEinstellungen.Location = new Point(btPositionX, btHöhe * 10);
            //AuswahlPanel
            panelAuswahl.Location = new Point(0, 0);
            panelAuswahl.Height = btHöhe;
            //Unterauswahl Einstellungen
            btFirmendaten.Height = btHöhe / 2;
            btBenutzer.Height = btHöhe / 2;


            //MinMax
            pbMinMax.Location = new Point(Convert.ToInt32(panelLinks.Width), Convert.ToInt32(panelMenü.Height / 2));

        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btHome.Top;

            panelUnterMenu.Visible = false;
            umenu();
        }

        private void btBenutzer_Click(object sender, EventArgs e)
        {
            Benutzer();
        }

        private void Benutzer()
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

            pbMinMax.BringToFront();
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
            panelTermine.Visible = false;
        }

        private void btAnfragen_Click(object sender, EventArgs e)
        {
            Anfragen();
        }

        private void Anfragen()
        {
            panelsDeaktivieren();

            panelAnfragen.Visible = true;
            panelAnfragen.BringToFront();
            panelAnfragen.Dock = DockStyle.Fill;

            pbMinMax.BringToFront();

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

                if(f.Status!="Abgeschlossen")
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
            Bestellungen();
        }

        private void Bestellungen()
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

            pbMinMax.BringToFront();
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
            News();
        }

        private void News()
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

            pbMinMax.BringToFront();
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
            Termine();
        }

        private void Termine()
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

            pbMinMax.BringToFront();

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
                lvItem = new ListViewItem(t.TerminID.ToString());
                lvItem.SubItems.Add(t.Titel);
                lvItem.SubItems.Add(t.Beschreibung.ToString());
                DateTime datumVon = Convert.ToDateTime(t.DatumVon);
                DateTime datumBis = Convert.ToDateTime(t.DatumBis);
                DateTime uhrzeitVon = Convert.ToDateTime(t.UhrzeitVon);
                DateTime uhrzeitBis = Convert.ToDateTime(t.UhrzeitBis);
                lvItem.SubItems.Add(datumVon.ToShortDateString()+"-"+datumBis.ToShortDateString());
                lvItem.SubItems.Add(uhrzeitVon.ToShortTimeString()+"-"+uhrzeitBis.ToShortTimeString());
                lvItem.SubItems.Add(t.Login.BenutzernameID.ToString());
                lvItem.SubItems.Add(t.Aktiv.ToString());

                if(t.Aktiv==true)
                listViewTermine.Items.Add(lvItem);
            }

        }

        private void listViewPanelBenutzerLogin_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lvItem = listViewPanelBenutzerLogin.SelectedItems[0];

                txtBenutzerBenutzerID.Text = lvItem.SubItems[0].Text;

                verPasswort = lvItem.SubItems[2].Text;
                PasswortEntschlüsseln();
                txtBenutzerPasswort.Text = entPasswort;

                txtBenutzerBenutzername.Text = lvItem.SubItems[1].Text;
                // inlesenKunden();
            }
            catch(Exception ex)
            {
                return;
            }
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
                        panelBenutzerLoginEinlesen();

                        txtBenutzerBenutzername.Clear();
                        txtBenutzerBenutzerID.Clear();
                        txtBenutzerPasswort.Clear();
                        Bestätigung();
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
            FrmSuperpasswort fSuper = new FrmSuperpasswort();

            if (!txtBenutzerBenutzerID.Text.Equals("")) //Speichern Methode
            {
                MessageBox.Show("Speichern");
                //PUT Methode
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
                            Bestätigung();
                            panelBenutzerLoginEinlesen();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Fehler bei der Änderung: " + ex.Message);
                    }
                }
            }
            else//Neu Methode
            {
                MessageBox.Show("Neu");
                //POST Methode
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

                        //Passwort verschlüsseln
                        entPasswort = txtBenutzerPasswort.Text;
                        PasswortVerschlüsseln();

                        login.Passwort = verPasswort;
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
                            Bestätigung();
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
            Firmendaten();
        }

        private void Firmendaten()
        {
            panelsDeaktivieren();

            panelFirmendaten.Visible = true;
            panelFirmendaten.Dock = DockStyle.Fill;
            panelFirmendaten.BringToFront();

            FirmendatenEinlesen();

            CheckEingeloggt();

            panelUnterMenu.Visible = false;
            pbMinMax.BringToFront();

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
            panelAuswahl.Height = btEinstellungen.Height;
            panelAuswahl.Top = btEinstellungen.Top;
        }

        private void panelBenutzer_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btEinstellungen.Height;
            panelAuswahl.Top = btEinstellungen.Top;

        }

        private void btSpeichern_Click(object sender, EventArgs e)
        {

        //PUT Methode
            try
            {

                Firmendaten fi = new Firmendaten();
                fi.FirmendatenID = 1;
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
                    Bestätigung();
                    FirmendatenEinlesen();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Änderung: " + ex.Message);
            }


        


    }

        private void listViewArtikel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lvItem = listViewArtikel.SelectedItems[0];


                txtArtikelArtikelID.Text = lvItem.SubItems[0].Text;
                txtArtikelExterneID.Text = lvItem.SubItems[1].Text;
                txtArtikelBezeichnung.Text = lvItem.SubItems[2].Text;
                txtArtikelNettopreis.Text = (lvItem.SubItems[3].Text);
                txtArtikelUST.Text = lvItem.SubItems[4].Text;
                Double netto = Convert.ToDouble(txtArtikelNettopreis.Text);
                Double ust = Convert.ToDouble(txtArtikelUST.Text);
                txtArtikelBrutto.Text = (netto + (netto * (ust / 100))).ToString();
                txtArtikelBrutto.Text.Replace(',', '.');
                cbArtikelLieferanten.Text = lvItem.SubItems[5].Text;
                txtArtikelLagerstand.Text = lvItem.SubItems[6].Text;
                txtArtikelReserviert.Text = lvItem.SubItems[7].Text;
                checkboxArtikelAktiv.Checked = Convert.ToBoolean(lvItem.SubItems[8].Text);

                string bild = txtArtikelBezeichnung.Text.Trim();
                pbArtikel.Image = Image.FromFile(@"D:\\OneDrive - BHAK und BHAS Mistelbach 316448\\Schule\\AP_SWE\\GitHub\\FutureFarmProgramm\\FutureFarm\\FutureFarm\\Properties\\Artikel\\" + bild + ".jpg");



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
            catch(Exception ex)
            {
                return;
            }

        }

        private void dtpNews_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listViewNews_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lvItem = listViewNews.SelectedItems[0];

                txtNewsID.Text = lvItem.SubItems[0].Text;
                txtNewsTitel.Text = lvItem.SubItems[1].Text;
                txtNewsBeitrag.Text = lvItem.SubItems[2].Text;
                dtpNews.Value = Convert.ToDateTime(lvItem.SubItems[3].Text);
            }
            catch(Exception ex)
            {
                return;
            }
            
        }

        private void btNewsNeu_Click(object sender, EventArgs e)
        {
            

        }

        private void btNewsSpeichern_Click(object sender, EventArgs e)
        {
            if(!txtNewsID.Text.Equals(""))
            {
                try
                {

                    //Benutzer holen
                    int aktuelleID = 0;


                    angBenutzer = btLogin2.Text;

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
                        Bestätigung();
                        NewsEinlesen();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler bei der Änderung: " + ex.Message);
                }

            }
            else
            {
                try
                {

                    //Benutzer holen
                    int aktuelleID = 0;


                    //ACHTUNG ÄNDERN WENN LOGIN WIEDER GEHT!!!!!!!!!!!!!!!!!!!!!!!!!!
                    angBenutzer = btLogin2.Text;

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
                        NewsEinlesen();
                        txtNewsTitel.Clear();
                        txtNewsID.Clear();
                        txtNewsBeitrag.Clear();
                        dtpNews.Value = DateTime.Now;
                        Bestätigung();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler bei der Neuanlage: " + ex.Message);
                }
            }
        }
            
        private void btArtikelSpeichern_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtArtikelArtikelID.Text!="") //Bestehender Artikel
                {
                    //Lieferant holen
                    var requestLieferant = new RestRequest("lieferanten/{id}", Method.GET);
                    requestLieferant.AddUrlSegment("id", cbArtikelLieferanten.Text);
                    requestLieferant.AddHeader("Content-Type", "application/json");
                    var responseLieferant = client.Execute<Lieferanten>(requestLieferant);
                    Lieferanten lieferant = responseLieferant.Data;


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
                    var requestArtikelUp = new RestRequest("artikel", Method.PUT);
                    requestArtikelUp.AddHeader("Content-Type", "application/json");
                    requestArtikelUp.AddJsonBody(aktArtikel);
                    var responseArtikelUp = client.Execute(requestArtikelUp);
                    if (responseArtikelUp.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Bestätigung();
                        ArtikelEinlesen();
                    }
                }
                else //Neuer Artikel
                {
                    //Lieferant holen
                    Lieferanten lief = new Lieferanten();
                    var requestLieferantNeu = new RestRequest("lieferanten/{id}", Method.GET);
                    requestLieferantNeu.AddHeader("Content-Type", "application/json");
                    requestLieferantNeu.AddUrlSegment("id", cbArtikelLieferanten.SelectedItem.ToString());
                    var responseLieferantNeu = client.Execute<List<Lieferanten>>(requestLieferantNeu);
                    foreach(Lieferanten l in responseLieferantNeu.Data)
                    {
                        lief.LieferantenID = l.LieferantenID;
                        lief.Vorname = l.Vorname;
                        lief.Nachname = l.Nachname;
                        lief.Firma = l.Firma;
                        lief.Telefonnummer = l.Telefonnummer;
                        lief.Email = l.Email;
                        lief.UID = l.UID;
                        lief.Strasse = l.Strasse;
                        lief.Aktiv = l.Aktiv;
                        lief.PLZ = l.PLZ;
                    }

                    //Artikel erzeugen
                    Artikel artikelNeu = new Artikel();
                    artikelNeu.ExterneID = Convert.ToInt32(txtArtikelExterneID.Text);
                    artikelNeu.Bezeichnung = txtArtikelBezeichnung.Text;
                    artikelNeu.Lagerstand = Convert.ToInt32(txtArtikelLagerstand.Text);
                    artikelNeu.Reserviert = Convert.ToInt32(txtArtikelReserviert.Text);
                    artikelNeu.PreisNetto = Convert.ToDouble(txtArtikelNettopreis.Text);
                    artikelNeu.Ust = Convert.ToDouble(txtArtikelUST.Text);
                    artikelNeu.Lieferant= lief;
                    artikelNeu.Bild = "x";
                    artikelNeu.Aktiv = checkboxArtikelAktiv.Checked;


                    var requestArtikelNeu = new RestRequest("artikel", Method.POST);
                    requestArtikelNeu.AddHeader("Content-Type", "application/json");
                    requestArtikelNeu.AddJsonBody(artikelNeu);
                    var responseArtikelNeu = client.Execute(requestArtikelNeu);
                    if (responseArtikelNeu.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Bestätigung();
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
           
            ArtikelPreisBerechnen();
        }

        private void ArtikelPreisBerechnen()
        {
            try
            {
                txtArtikelBrutto.Text = (Convert.ToDouble(txtArtikelNettopreis.Text) + (Convert.ToDouble(txtArtikelNettopreis.Text) * (Convert.ToDouble(txtArtikelUST.Text) / 100))).ToString();
            }
            catch
            {

            }
        }

        private void btArtikelLöschen_Click(object sender, EventArgs e)
        {
            //Lieferant holen
            var requestLieferant = new RestRequest("lieferanten/{id}", Method.GET);
            requestLieferant.AddUrlSegment("id", cbArtikelLieferanten.Text);
            requestLieferant.AddHeader("Content-Type", "application/json");
            var responseLieferant = client.Execute<Lieferanten>(requestLieferant);
            Lieferanten lieferant = responseLieferant.Data;


            //Artikel schreiben
            Artikel aktArtikel = new Artikel();
            aktArtikel.Lieferant = lieferant;
            aktArtikel.ExterneID = Convert.ToInt16(txtArtikelExterneID.Text);
            aktArtikel.Bezeichnung = txtArtikelBezeichnung.Text;
            aktArtikel.Lagerstand = Convert.ToInt16(txtArtikelLagerstand.Text);
            aktArtikel.Reserviert = Convert.ToInt16(txtArtikelReserviert.Text);
            aktArtikel.PreisNetto = Convert.ToDouble(txtArtikelNettopreis.Text);
            aktArtikel.Ust = Convert.ToDouble(txtArtikelUST.Text);
            aktArtikel.Aktiv = false;
            aktArtikel.ArtikelID = Convert.ToInt32(txtArtikelArtikelID.Text);
            aktArtikel.Bild = "";


            //Artikel updaten
            var requestArtikelUp = new RestRequest("artikel", Method.PUT);
            requestArtikelUp.AddHeader("Content-Type", "application/json");
            requestArtikelUp.AddJsonBody(aktArtikel);
            var responseArtikelUp = client.Execute(requestArtikelUp);
            if (responseArtikelUp.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Bestätigung();
                ArtikelEinlesen();
            }
            ArtikelFelderLeeren();
        }

        private void cbArtikelLieferanten_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Lieferant holen
            string lieferantenID = cbArtikelLieferanten.Text;

            var request = new RestRequest("lieferanten/{id}", Method.GET);
            request.AddUrlSegment("id", lieferantenID);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Lieferanten>>(request);

            foreach (Lieferanten l in response.Data)
            {
                txtArtikelLieferantFirma.Text = l.Firma.ToString();
            }


        }

        private void btReset_Click(object sender, EventArgs e)
        {
            reset = true;
            ArtikelFelderLeeren();
        }

        private void ArtikelFelderLeeren()
        {
            txtArtikelArtikelID.Clear();
            txtArtikelExterneID.Clear();
            txtArtikelBezeichnung.Clear();
            txtArtikelNettopreis.Clear();
            txtArtikelBrutto.Clear();
            txtArtikelLagerstand.Clear();
            txtArtikelReserviert.Clear();
            txtArtikelLieferantFirma.Clear();
            cbArtikelLieferanten.SelectedValue = "";
        }

        private void btNewsReset_Click(object sender, EventArgs e)
        {
            txtNewsBeitrag.Clear();
            txtNewsID.Clear();
            txtNewsTitel.Clear();
            dtpNews.Value = DateTime.Now;
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
                    NewsEinlesen();
                    txtNewsTitel.Clear();
                    txtNewsID.Clear();
                    txtNewsBeitrag.Clear();
                    dtpNews.Value = DateTime.Now;
                    Bestätigung();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Löschen: " + ex.Message);
            }

        }

        private void btLogin2_Click(object sender, EventArgs e)
        {
            FrmWarnung fw = new FrmWarnung();

            if (LogIn == false)
                fl.ShowDialog();
            else
            {
                fw.lbText.Text = "Der Benutzer wird jetzt abgemeldet...";
                fw.Text = "Abmeldung";
                LogIn = false;
                btLogin2.Text = "   Log In";
                btLogin2.Image = imgLogout;
            }

            CheckEingeloggt();
        }

        public void EinloggenNeu()
        {
            panelBenutzerLoginEinlesen();

            //einzelne Benutzer in der Listview durchgehen
            for(int i=0; i < listViewPanelBenutzerLogin.Items.Count;i++)
            {
                //einzelnes Item prüfen
                lvItem = listViewPanelBenutzerLogin.Items[i];
                if (benutzerEingabe.Equals(lvItem.SubItems[1].Text)) //Wenn Benutzer existiert --> Passwort prüfen
                {
                    verPasswort = lvItem.SubItems[2].Text;
                    PasswortEntschlüsseln();

                    if (passwortEingabe.Equals(entPasswort)) //Wenn Passwort stimmmt --> einloggen
                    {
                        LogIn = true;
                        btLogin2.Text = benutzerEingabe;
                        btLogin2.Image = imgLogin;
                        Bestätigung();
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
                        break;
                    }
                }
                CheckEingeloggt();
            }
            if (LogIn == false)
                fl.ShowDialog();
                
        }

        private void btRechnungSuchen_Click(object sender, EventArgs e)
        {
            //Rechnung in lv Suchen
            if (txtRechnungSuche.Text != "")
                RechnungenSuchen();
            else
                RechnungSucheEinlesen();
        }

        private void RechnungenSuchen()
        {
            foreach (ListViewItem item in listViewRechnungSuche.Items)
            {
                if (item.SubItems[0].Text.Contains(txtRechnungSuche.Text) || item.SubItems[1].Text.Contains(txtRechnungSuche.Text) || item.SubItems[2].Text.Contains(txtRechnungSuche.Text) || item.SubItems[3].Text.Contains(txtRechnungSuche.Text) || item.SubItems[4].Text.Contains(txtRechnungSuche.Text))
                {

                }
                else
                {
                    item.Remove();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Rechnung in lv Suchen
            if (txtRechnungSuche.Text != "")
                RechnungenSuchen();
            else
                RechnungSucheEinlesen();
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

            dtpRechnungNeu.Value = DateTime.Now;
            
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

            string cbKundenText = cbRechnungenKundeNeu.SelectedItem.ToString();
            rechnungNeuKunde = cbKundenText.Substring(0, cbKundenText.IndexOf(' '));

            var request = new RestRequest("bestellungen", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Bestellung>>(request);

            foreach (Bestellung b in response.Data)
            {
                if (b.Kunde.KundenID.ToString().Equals(rechnungNeuKunde))
                cbRechnungBestellungNeu.Items.Add(b.BestellungID + " | Status: " + b.Status);
            }

        }

        private void cbKundeRechnungNeu_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewRechnungGewähltArtikel.Items.Clear();
            ComboRechnungNeuBestellungenEinlesen();

        }

        private void cbBestellungRechnungNeu_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewRechnungNeuArtikel.Items.Clear();

            if(cbRechnungBestellungNeu.Text.Equals("Bestellung erstellen..."))
            {
                FrmWarnung fw = new FrmWarnung();
                fw.lbText.Text="Wollen Sie eine neue Bestellung erstellen?";
                fw.ShowDialog();
                if (fw.weiter == true)
                {
                    panelsDeaktivieren();
                    panelBestellungen.Dock = DockStyle.Fill;
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

        }



        private void button2_Click(object sender, EventArgs e)
        {
            string kundenID = cbRechnungenKundeNeu.Text.Substring(0, cbRechnungenKundeNeu.Text.IndexOf(" "));
            var requestKunde = new RestRequest("kunden", Method.GET);
            requestKunde.AddHeader("Content-Type", "Application/Json");
            var responseKunde = client.Execute<List<Kunden>>(requestKunde);
            Kunden kunde = new Kunden();
            foreach(Kunden k in responseKunde.Data)
            {
                if(kundenID.Equals(k.KundenID.ToString()))
                {
                    kunde.KundenID = k.KundenID;
                    kunde.Anrede = k.Anrede;
                    kunde.Vorname = k.Vorname;
                    kunde.Nachname = k.Nachname;
                    kunde.Firma = k.Firma;
                    kunde.Telefonnummer = k.Telefonnummer;
                    kunde.Email = k.Email;
                    kunde.Strasse = k.Strasse;
                    kunde.Aktiv = k.Aktiv;
                    kunde.PLZ = k.PLZ;
                }
            }

            string bestellungID = cbRechnungBestellungNeu.Text.Substring(0, cbRechnungBestellungNeu.Text.IndexOf(" "));
            
            var requestBestellung = new RestRequest("bestellungen", Method.GET);
            requestBestellung.AddHeader("Content-Type", "Application/Json");
            var responseBestellung = client.Execute<List<Bestellung>>(requestBestellung);
            Bestellung bestellung = new Bestellung();
            foreach (Bestellung b in responseBestellung.Data)
            {
                if(bestellungID.Equals(b.BestellungID.ToString()))
                {
                    bestellung.BestellungID = b.BestellungID;
                    bestellung.Status = "Abgeschlossen";
                    bestellung.Lieferdatum = b.Lieferdatum;
                    bestellung.Kunde = kunde;
                }
            }

            var requestBestellungUp = new RestRequest("bestellungen", Method.PUT);
            requestBestellungUp.AddHeader("Content-Type", "application/json");
            requestBestellungUp.AddJsonBody(bestellung);
            var responseBestellungUp = client.Execute(requestBestellungUp);

            Rechnung rechnung = new Rechnung();
            rechnung.Datum = Convert.ToDateTime(dtpRechnungNeu.Value.ToShortDateString());
            rechnung.Kunde = kunde;
            rechnung.Bestellung = bestellung;
            rechnung.Bezahlt = cbRechnungNeuBezahlt.Checked;
            rechnung.BezahltAm = Convert.ToDateTime(dtpRechnungNeuBezahlt.Value.ToShortDateString());

            //Rechnung erstellen
            var request = new RestRequest("rechnungen", Method.POST);
            request.AddHeader("Content-Type", "application/Json");
            request.AddJsonBody(rechnung);
            var response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Bestätigung();
            }

            //Rechnung ID finden
            //string rechnungID = "";
            //var request2 = new RestRequest("rechnungen", Method.GET);
            //request2.AddHeader("Content-Type", "application/json");
            //var response2 = client.Execute<List<Rechnung>>(request2);
            //foreach(Rechnung r in response2.Data)
            //{
            //    if (rechnung.Datum == r.Datum && r.Kunde.Nachname.Equals(rechnung.Kunde.Nachname) && r.Bestellung.BestellungID.Equals(rechnung.Bestellung.BestellungID) && (r.Bezahlt == rechnung.Bezahlt))
            //    {
                    
            //        rechnungID = r.RechnungID.ToString();
            //    }
            //}
            


            //Rechnungartikel erstellen

            for (int i=0; i<listViewRechnungNeuArtikel.Items.Count;i++)
            {
                lvItem = listViewRechnungNeuArtikel.Items[i];
                RechnungArtikel ra = new RechnungArtikel();
                Artikel artikel = new Artikel();
                Rechnung rechnungS = new Rechnung();

                var requestRechnung = new RestRequest("rechnungen", Method.GET);
                requestRechnung.AddHeader("Content-Type", "application/json");
                var responseRechnung = client.Execute<List<Rechnung>>(requestRechnung);
                foreach(Rechnung r in responseRechnung.Data)
                {
                    rechnungS.RechnungID = r.RechnungID;
                    rechnungS.Datum = r.Datum;
                    rechnungS.Bezahlt = r.Bezahlt;
                    rechnungS.BezahltAm = r.BezahltAm;
                    rechnungS.Kunde = r.Kunde;
                    rechnungS.Bestellung = r.Bestellung;
                }

                MessageBox.Show(rechnungS.RechnungID.ToString());

                var requestArtikel = new RestRequest("artikel", Method.GET);
                requestArtikel.AddHeader("Content-Type", "application/json");
                var responseArtikel = client.Execute<List<Artikel>>(requestArtikel);
                foreach (Artikel a in responseArtikel.Data)
                {
                    if (lvItem.Text.Equals(a.ArtikelID.ToString()))
                    {
                        artikel.ArtikelID = a.ArtikelID;
                        artikel.Bezeichnung = a.Bezeichnung;
                        artikel.ExterneID = a.ExterneID;
                        artikel.PreisNetto = a.PreisNetto;
                        artikel.Ust = a.Ust;
                        artikel.Lagerstand = a.Lagerstand;
                        artikel.Reserviert = a.Reserviert;
                        artikel.Bild = a.Bild;
                        artikel.Aktiv = a.Aktiv;
                        artikel.Lieferant = a.Lieferant;
                    }
                }

                ra.Menge = Convert.ToInt32(lvItem.SubItems[2].Text);
                ra.NettoPreis = Convert.ToDouble(lvItem.SubItems[3].Text);
                ra.Ust= Convert.ToDouble(lvItem.SubItems[4].Text);
                ra.Aktiv= true;
                ra.Rechnung = rechnungS;
                ra.Artikel = artikel;

                var request1 = new RestRequest("rechnungartikel", Method.POST);
                request1.AddHeader("Content-Type", "application/Json");
                request1.AddJsonBody(ra);
                
                var response1 = client.Execute(request1);
                if (response1.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Bestätigung();
                }
            }
        }

        private void panelTermine_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btTermine.Height;
            panelAuswahl.Top = btTermine.Top;

        }

        private void listViewTermine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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
                string zeitBis = gesamteZeit.Substring(gesamteZeit.IndexOf('-') + 1, 5);
                dtpTermineZeitBis.Value = Convert.ToDateTime(zeitBis);
            }
            catch(Exception ex)
            {
                return;
            }

        }

        private void btTerminNeu_Click(object sender, EventArgs e)
        {

        }

        private void listViewBestellungArtikelAlle_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbBestellungStatus.SelectedIndex = 1;
        }

        private void btKundenSpeichern_Click(object sender, EventArgs e)
        {
            if(txtKundenID.Text!="")
            {
                //Kunde speichern

                Kunden kunde = new Kunden();
                kunde.KundenID = Convert.ToInt32(txtKundenID.Text);
                kunde.Anrede = cbKundenAnrede.SelectedItem.ToString();
                kunde.Vorname = txtKundenVorname.Text;
                kunde.Nachname = txtKundenNachname.Text;
                kunde.Firma = txtKundenFirma.Text;
                kunde.Email = txtKundenEmail.Text;
                kunde.Telefonnummer = txtKundenTelefonnummer.Text;
                kunde.Strasse = txtKundenStrasse.Text;

                Postleitzahl plz = new Postleitzahl();
                //PLZ holen
                var request1 = new RestRequest("postleitzahlen", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Postleitzahl>>(request1);
                foreach (Postleitzahl pl in response1.Data)
                {
                    if (txtKundenPLZ.Text.Equals(pl.PLZ.ToString())&&cbKundenOrt.Text.Equals(pl.Ortschaft.ToString()))
                    {
                        plz.PLZID = pl.PLZID;
                        plz.PLZ = pl.PLZ;
                        plz.Ortschaft = pl.Ortschaft;
                        
                        MessageBox.Show(txtKundenPLZ.Text + " " + cbKundenOrt.Text);
                        break;
                    }
                }

                kunde.Postleitzahl = plz;
                MessageBox.Show(plz.PLZID.ToString() + " " + plz.Ortschaft.ToString());
                kunde.Aktiv = true;

                //Kunden updaten
                var requestUpdate = new RestRequest("kunden", Method.PUT);
                requestUpdate.AddHeader("Content-Type", "application/json");
                requestUpdate.AddJsonBody(kunde);
                var responseUpdate = client.Execute(requestUpdate);
                if (responseUpdate.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Bestätigung();
                    KundenEinlesen();
                }




            }
            else
            {
                //Kunde neu anlegen
                Kunden kunde = new Kunden();
                kunde.Anrede = cbKundenAnrede.SelectedItem.ToString();
                kunde.Vorname = txtKundenVorname.Text;
                kunde.Nachname = txtKundenNachname.Text;
                kunde.Firma = txtKundenFirma.Text;
                kunde.Email = txtKundenEmail.Text;
                kunde.Telefonnummer = txtKundenTelefonnummer.Text;
                kunde.Strasse = txtKundenStrasse.Text;

                Postleitzahl plz = new Postleitzahl();
                //PLZ holen
                var request1 = new RestRequest("postleitzahlen", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Postleitzahl>>(request1);
                foreach (Postleitzahl pl in response1.Data)
                {
                    if (txtKundenPLZ.Text.Equals(pl.PLZ.ToString()) && cbKundenOrt.Text.Equals(pl.Ortschaft.ToString()))
                    {
                        plz.PLZID = pl.PLZID;
                        plz.PLZ = pl.PLZ;
                        plz.Ortschaft = pl.Ortschaft;
                    }
                }

                kunde.Postleitzahl = plz;
                kunde.Aktiv = true;

                //Kunden hinzufügen
                var requestNeu = new RestRequest("kunden", Method.POST);
                requestNeu.AddHeader("Content-Type", "application/json");
                requestNeu.AddJsonBody(kunde);
                var responseNeu = client.Execute(requestNeu);
                if (responseNeu.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Bestätigung();
                    KundenEinlesen();
                }


            }
        }


        private void btKundenLöschen_Click(object sender, EventArgs e)
        {
            if(txtKundenID.Text!="")
            {
                //Kunden inaktiv setzen
                if (txtKundenID.Text != "")
                {
                    Kunden kunde = new Kunden();
                    kunde.KundenID = Convert.ToInt32(txtKundenID.Text);
                    kunde.Anrede = cbKundenAnrede.SelectedItem.ToString();
                    kunde.Vorname = txtKundenVorname.Text;
                    kunde.Nachname = txtKundenNachname.Text;
                    kunde.Firma = txtKundenFirma.Text;
                    kunde.Email = txtKundenEmail.Text;
                    kunde.Telefonnummer = txtKundenTelefonnummer.Text;
                    kunde.Strasse = txtKundenStrasse.Text;

                    Postleitzahl plz = new Postleitzahl();
                    //PLZ holen
                    var request1 = new RestRequest("postleitzahlen", Method.GET);
                    request1.AddHeader("Content-Type", "application/json");
                    var response1 = client.Execute<List<Postleitzahl>>(request1);
                    foreach (Postleitzahl pl in response1.Data)
                    {
                        if (txtKundenPLZ.Text.Equals(pl.PLZ.ToString()) && cbKundenOrt.Text.Equals(pl.Ortschaft.ToString()))
                        {
                            plz.PLZID = pl.PLZID;
                            plz.PLZ = pl.PLZ;
                            plz.Ortschaft = pl.Ortschaft;
                        }
                    }

                    kunde.Postleitzahl = plz;
                    kunde.Aktiv = false;

                    //Kunden updaten
                    var requestUpdate = new RestRequest("kunden", Method.PUT);
                    requestUpdate.AddHeader("Content-Type", "application/json");
                    requestUpdate.AddJsonBody(kunde);
                    var responseUpdate = client.Execute(requestUpdate);
                    if (responseUpdate.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Bestätigung();
                        KundenEinlesen();
                    }



                }

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
                if (lvItem.SubItems[1].Text.Equals("Frau"))
                    cbKundenAnrede.SelectedItem = cbKundenAnrede.Items[0];
                else if (lvItem.SubItems[1].Text.Equals("Herr"))
                    cbKundenAnrede.SelectedItem = cbKundenAnrede.Items[1];
                else
                    cbKundenAnrede.SelectedItem = cbKundenAnrede.Items[2];
                txtKundenVorname.Text = lvItem.SubItems[2].Text.ToString();
                txtKundenNachname.Text = lvItem.SubItems[3].Text.ToString();
                txtKundenFirma.Text = lvItem.SubItems[4].Text.ToString();
                txtKundenTelefonnummer.Text = lvItem.SubItems[5].Text.ToString();
                txtKundenEmail.Text = lvItem.SubItems[6].Text.ToString();
                txtKundenStrasse.Text = lvItem.SubItems[7].Text.ToString();

                cbKundenOrt.Items.Clear();

                
                //PLZID holen
                Postleitzahl plzKunde = new Postleitzahl();
                string plzid = "";

                var request1 = new RestRequest("kunden", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Kunden>>(request1);
                foreach(Kunden k in response1.Data)
                {
                    if(txtKundenID.Text.Equals(k.KundenID.ToString()))
                    {
                        //txtKundenPLZ.Text = k.Postleitzahl.PLZ;
                        //if(txtKundenPLZ.Text.Equals(k.Postleitzahl.PLZ)&&)
                        //cbKundenOrt.Items.Add(k.Postleitzahl.Ortschaft);
                        //cbKundenOrt.SelectedItem = cbKundenOrt.Items[0];
                        plzid = k.Postleitzahl.PLZID.ToString();
                    }
                }

                var request2 = new RestRequest("postleitzahlen", Method.GET);
                request2.AddHeader("Content-Type", "application/json");
                var response2 = client.Execute<List<Postleitzahl>>(request2);

                foreach(Postleitzahl plz in response2.Data)
                {
                    if (plzid.Equals(plz.PLZID.ToString()))
                    {
                        cbKundenOrt.Items.Add(plz.Ortschaft);
                        txtKundenPLZ.Text = plz.PLZ;
                        cbKundenOrt.SelectedItem =cbKundenOrt.Items[0];
                    }
                }

            }
            catch (Exception ex)
            {
                return;
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
                lvItem = listViewAnfragen.SelectedItems[0];

                var request = new RestRequest("formulare/{id}", Method.GET);
                request.AddUrlSegment("id", lvItem.Text);
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute<List<Formular>>(request);

                foreach (Formular f in response.Data)
                {
                    txtAnfrageID.Text = f.FormularID.ToString();
                    txtAnfrageArt.Text = f.Art.Bezeichnung.ToString();
                    txtAnfrageName.Text = f.Nachname.ToString() + " " + f.Vorname.ToString();
                    txtAnfrageEmail.Text = f.Email.ToString();
                    txtAnfrageInhalt.Text = f.Inhalt.ToString();
                    txtAnfrageTelefonnummer.Text = f.Telefonnummer.ToString();
                    dtpAnfrageDatum.Value = f.Datum;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void listViewRechnungSuche_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewRechnungGewähltArtikel.Items.Clear();
            try
            {
                lvItem = listViewRechnungSuche.SelectedItems[0];

                txtRechnungID.Text = lvItem.SubItems[0].Text;
                txtRechnungenBestellungID.Text = lvItem.SubItems[3].Text;
                txtRechnungKunde.Text = lvItem.SubItems[2].Text;

                for (int i = 0; i < listViewRechnungen.Items.Count; i++)
                {
                    if (txtRechnungID.Text.Equals(listViewRechnungen.Items[i].SubItems[0].Text))
                    {
                        cbRechnungBezahlt.Checked = Convert.ToBoolean(listViewRechnungen.Items[i].SubItems[2].Text.ToLower());
                        DateTime rechnungBezahltAm = Convert.ToDateTime(listViewRechnungen.Items[i].SubItems[3].Text);
                        dtpRechnungBezahlt.Value = rechnungBezahltAm;

                        dtpRechnungDatum.Value = Convert.ToDateTime(listViewRechnungen.Items[i].SubItems[1].Text);
                    }
                }

                //Artikel anzeigen
                for (int i = 0; i < listViewRechnungArtikel.Items.Count; i++)
                {
                    if (listViewRechnungArtikel.Items[i].SubItems[4].Text.Equals(txtRechnungID.Text))
                    {
                        string artikelID = listViewRechnungArtikel.Items[i].SubItems[5].Text;

                        //Client für API

                        var request = new RestRequest("artikel", Method.GET);
                        request.AddHeader("Content-Type", "application/json");
                        var response = client.Execute<List<Artikel>>(request);

                        foreach (Artikel a in response.Data)
                        {
                            if (artikelID.Equals(a.ArtikelID.ToString()))
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
                }


                //Summen berechnen
                double summeNetto = 0;
                double summeUst = 0;
                for (int i = 0; i < listViewRechnungGewähltArtikel.Items.Count; i++)
                {
                    summeNetto += Convert.ToDouble(listViewRechnungGewähltArtikel.Items[i].SubItems[2].Text);
                    summeUst += Convert.ToDouble(listViewRechnungGewähltArtikel.Items[i].SubItems[2].Text) * (Convert.ToDouble(listViewRechnungGewähltArtikel.Items[i].SubItems[4].Text) / 100);
                }

                txtRechnungSummeNetto.Text = summeNetto.ToString("C2");
                txtRechnungSummeUST.Text = summeUst.ToString("C2");
                txtRechnungSummeBrutto.Text = (summeNetto + summeUst).ToString("C2");


            }
            catch (Exception ex)
            {
                return;
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
                return;
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
                        MessageBox.Show(b.Kunde.Nachname);

                        //Bestellung hinzufügen
                        var request2 = new RestRequest("bestellungen", Method.POST);
                        request2.AddHeader("Content-Type", "application/json");
                        request2.AddJsonBody(b);
                        var response2 = client.Execute(request2);

                        BestellungenEinlesen();
                        int anzahl = listViewBestellungen.Items.Count;
                        listViewBestellungen.Items[anzahl-1].Selected = true;                      
                    }
                }
                else
                {
                    
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
                        Bestätigung();
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
                Bestätigung();
                listViewBestellungenArtikelGewählt.Items.Remove(listViewBestellungenArtikelGewählt.SelectedItems[0]);
            }
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

                    //Bestellung hinzufügen
                    var request2 = new RestRequest("bestellungen", Method.POST);
                    request2.AddHeader("Content-Type", "application/json");
                    request2.AddJsonBody(bestellung);
                    var response2 = client.Execute(request2);


                    BestellungenEinlesen();

                }
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


                //Neue Bestellung anlegen
                Bestellung bestellung = new Bestellung();
                bestellung.BestellungID =Convert.ToInt32(txtBestellungID.Text);
                bestellung.Status = cbBestellungStatus.Text;
                bestellung.Lieferdatum = Convert.ToDateTime(dtpBestellungenLieferdatum.Value.ToShortDateString());
                bestellung.Kunde = k;

                var request3 = new RestRequest("bestellungen", Method.PUT);
                request3.AddHeader("Content-Type", "application/json");
                request3.AddJsonBody(bestellung);
                var response3 = client.Execute(request3);

                BestellungenEinlesen();
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

        private void btKundenOrtSuche_Click(object sender, EventArgs e)
        {
            PLZKundenSuchen();
        }

        private void PLZKundenSuchen()
        {
            try
            {
                cbKundenOrt.Items.Clear();
                Postleitzahl plz = new Postleitzahl();
                //PLZ holen
                var request1 = new RestRequest("postleitzahlen", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Postleitzahl>>(request1);
                foreach (Postleitzahl pl in response1.Data)
                {
                    if (txtKundenPLZ.Text.Equals(pl.PLZ.ToString()))
                    {
                        plz.PLZID = pl.PLZID;
                        plz.PLZ = pl.PLZ;
                        plz.Ortschaft = pl.Ortschaft;
                        cbKundenOrt.Items.Add(plz.Ortschaft);
                    }
                }
                cbKundenOrt.DroppedDown = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void btTerminSpeichern_Click(object sender, EventArgs e)
        {
            if(!txtTerminID.Text.Equals(""))
            {
                //Termin update
                Termine termin = new Termine();
                termin.TerminID = Convert.ToInt32(txtTerminID.Text);
                termin.Titel = txtTerminTitel.Text;
                termin.Beschreibung = txtTerminBeschreibung.Text;
                termin.DatumVon = dtpTerminDatumVon.Value;
                termin.DatumBis = dtpTerminDatumBis.Value; //HAUS Zeit wird immer -1 in Datenbank übernommen
                termin.UhrzeitVon = dtpTermineZeitVon.Value;
                termin.UhrzeitBis = dtpTermineZeitBis.Value;
                termin.Aktiv = true;

                Login benutzer = new Login();
                var request = new RestRequest("logins", Method.GET);
                request.AddHeader("Content-Type", "application-json");
                var response = client.Execute<List<Login>>(request);
                foreach (Login l in response.Data)
                {
                    if (l.Benutzername.Equals(btLogin2.Text))
                    {
                        benutzer.BenutzernameID = l.BenutzernameID;
                        benutzer.Benutzername = l.Benutzername;
                        benutzer.Passwort = l.Passwort;
                        benutzer.LetzteAnmeldung = l.LetzteAnmeldung;
                    }
                }

                termin.Login = benutzer;

                var request1 = new RestRequest("termine", Method.PUT);
                request1.AddHeader("Content-Type", "Application/Json");
                request1.AddJsonBody(termin);
                var response1 = client.Execute(request1);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Bestätigung();
                    TermineEinlesen();
                }
            }
            else
            {
                //Termin neu
                try
                {

                    //Artikel erzeugen
                    Termine termin = new Termine();
                    termin.Titel = txtTerminTitel.Text;
                    termin.Beschreibung = txtTerminBeschreibung.Text;
                    termin.DatumVon = dtpTerminDatumVon.Value;
                    termin.DatumBis = dtpTerminDatumBis.Value;
                    termin.UhrzeitVon = dtpTermineZeitVon.Value;
                    termin.UhrzeitBis = dtpTermineZeitBis.Value;
                    termin.Aktiv = true;

                    Login benutzer = new Login();
                    var request = new RestRequest("logins", Method.GET);
                    request.AddHeader("Content-Type", "application-json");
                    var response = client.Execute<List<Login>>(request);
                    foreach(Login l in response.Data)
                    {
                        if(l.Benutzername.Equals(btLogin2.Text))
                        {
                            benutzer.BenutzernameID = l.BenutzernameID;
                            benutzer.Benutzername = l.Benutzername;
                            benutzer.Passwort = l.Passwort;
                            benutzer.LetzteAnmeldung = l.LetzteAnmeldung;
                        }
                    }

                    termin.Login = benutzer;

                    //Termin hinzufügen
                    var request2 = new RestRequest("termine", Method.POST);
                    request2.AddHeader("Content-Type", "application/json");
                    request2.AddJsonBody(termin);
                    var response2 = client.Execute(request2);
                    if (response2.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Bestätigung();
                        TermineEinlesen();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler bei der Neuanlage: " + ex.Message);
                }

            }
        }

        private void btTerminLöschen_Click(object sender, EventArgs e)
        {
            //Termin löschen --> inaktiv
            Termine termin = new Termine();
            termin.TerminID = Convert.ToInt32(txtTerminID.Text);
            termin.Titel = txtTerminTitel.Text;
            termin.Beschreibung = txtTerminBeschreibung.Text;
            termin.DatumVon = dtpTerminDatumVon.Value;
            termin.DatumBis = dtpTerminDatumBis.Value;
            termin.UhrzeitVon = dtpTermineZeitVon.Value;
            termin.UhrzeitBis = dtpTermineZeitBis.Value;
            termin.Aktiv = false;

            Login benutzer = new Login();
            var request = new RestRequest("logins", Method.GET);
            request.AddHeader("Content-Type", "application-json");
            var response = client.Execute<List<Login>>(request);
            foreach (Login l in response.Data)
            {
                if (l.Benutzername.Equals(btLogin2.Text))
                {
                    benutzer.BenutzernameID = l.BenutzernameID;
                    benutzer.Benutzername = l.Benutzername;
                    benutzer.Passwort = l.Passwort;
                    benutzer.LetzteAnmeldung = l.LetzteAnmeldung;
                }
            }

            termin.Login = benutzer;

            var request1 = new RestRequest("termine", Method.PUT);
            request1.AddHeader("Content-Type", "Application/Json");
            request1.AddJsonBody(termin);
            var response1 = client.Execute(request1);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Bestätigung();
                TermineEinlesen();
            }
        }

        private void txtArtikelSuchen_TextChanged(object sender, EventArgs e)
        {
            if (txtArtikelSuchen.Text != "")
            {
                ArtikelEinlesen();
                ArtikelSuchen();
            }
            else
                ArtikelEinlesen();
        }

        private void btArtikelSuchen_Click(object sender, EventArgs e)
        {
            if (txtArtikelSuchen.Text != "")
            {
                ArtikelSuchen();
            }
            else
                ArtikelEinlesen();
        }

        private void ArtikelSuchen()
        {
            foreach (ListViewItem item in listViewArtikel.Items)
            {
                if (item.SubItems[0].Text.ToLower().Contains(txtArtikelSuchen.Text.ToLower()) || item.SubItems[1].Text.ToLower().Contains(txtArtikelSuchen.Text.ToLower()) || item.SubItems[2].Text.ToLower().Contains(txtArtikelSuchen.Text.ToLower()) || item.SubItems[3].Text.ToLower().Contains(txtArtikelSuchen.Text.ToLower()) || item.SubItems[4].Text.ToLower().Contains(txtArtikelSuchen.Text.ToLower()))
                {

                }
                else
                {
                    item.Remove();
                }
            }
        }

        private void btKundenSuchen_Click(object sender, EventArgs e)
        {
            if (txtKundeSuchen.Text != "")
            {
                KundenEinlesen();
                KundenSuchen();
            }
            else
                KundenEinlesen();
        }

        private void txtKundeSuchen_TextChanged(object sender, EventArgs e)
        {
            if (txtKundeSuchen.Text != "")
            {
                KundenEinlesen();
                KundenSuchen();
            }
            else
                KundenEinlesen();
        }

        private void KundenSuchen()
        {
            foreach (ListViewItem item in listViewKunden.Items)
            {
                if (item.SubItems[0].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[1].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[2].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[3].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[4].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[5].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[6].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[7].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[8].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()) || item.SubItems[9].Text.ToLower().Contains(txtKundeSuchen.Text.ToLower()))
                {
                    
                }
                else
                {
                    item.Remove();
                }
            }
        }

        private void btAnfragenBestellungÜbernehmen_Click(object sender, EventArgs e)
        {
            FrmWarnung fw = new FrmWarnung();
            if(txtAnfrageArt.Text.Equals("Bestellung"))
            {
                string kunde = txtAnfrageName.Text;
                string telefon = txtAnfrageTelefonnummer.Text;
                string email = txtAnfrageEmail.Text;
                string inhalt = txtAnfrageInhalt.Text;
                bool vorhanden=false;

                var request = new RestRequest("kunden", Method.GET);
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute<List<Kunden>>(request);

                foreach (Kunden k in response.Data)
                {
                   if((k.Nachname+" "+k.Vorname).Equals(kunde))//||k.Telefonnummer.Equals(telefon)||k.Email.Equals(email)
                    {
                        vorhanden = true;
                        
                        fw.cbWarnungKunde.Items.Add(k.KundenID.ToString()+" | "+k.Nachname+" "+k.Vorname);
                        fw.cbWarnungKunde.SelectedItem = fw.cbWarnungKunde.Items[0];
                    }
                }

                if(vorhanden==true)
                {
                    try
                    {
                        fw.cbWarnungKunde.Visible = true;
                        fw.lbText.Text = "Kunden auswählen...";
                        fw.ShowDialog();
                        if (fw.weiter == true)
                        {
                            string cbKunde = fw.cbWarnungKunde.SelectedItem.ToString();
                            string gewKunde = cbKunde.Substring(cbKunde.IndexOf("|") + 2, cbKunde.Length - (cbKunde.IndexOf("|") + 2));
                            fw.cbWarnungKunde.Visible = false;

                            BestellungÜbernehmen(gewKunde, inhalt);
                        }
                        else
                        {
                            fw.lbText.Text = "Kein Kunde ausgewählt,\nbitte wiederholen Sie den Vorgang";
                            fw.ShowDialog();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    fw.lbText.Text = "Kein Kunde gefunden,\nbitte legen Sie einen neuen Kunden an";
                    fw.ShowDialog();
                    if(fw.weiter==true)
                    {
                        panelsDeaktivieren();
                        panelKunden.Dock = DockStyle.Fill;
                    }
                }

            }
        }

        private void BestellungÜbernehmen(string gewKunde, string inhalt)
        {

            Kunden kunde = new Kunden();
            var request = new RestRequest("kunden", Method.GET);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute<List<Kunden>>(request);

            foreach (Kunden k in response.Data)
            {
                if ((k.Nachname + " " + k.Vorname).Equals(gewKunde))
                {
                    kunde.KundenID = k.KundenID;
                    kunde.Vorname = k.Vorname;
                    kunde.Nachname = k.Nachname;
                    kunde.Telefonnummer = k.Telefonnummer;
                    kunde.Email = k.Email;
                    kunde.Anrede = k.Anrede;
                    kunde.Firma = k.Firma;
                    kunde.Strasse = k.Strasse;
                    kunde.Aktiv = k.Aktiv;
                    kunde.PLZ = k.PLZ;
                }
            }

            //Bestellung anlegen
            Bestellung bestellung = new Bestellung();
            bestellung.Kunde = kunde;
            bestellung.Lieferdatum = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            bestellung.Status = "Neu";

            var request2 = new RestRequest("bestellungen", Method.POST);
            request2.AddHeader("Content-Type", "application/json");
            request2.AddJsonBody(bestellung);
            var response2 = client.Execute(request2);
            if (response2.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                BestellungenEinlesen();

                Formular formular = new Formular();
                formular.FormularID = Convert.ToInt32(txtAnfrageID.Text);
                formular.Vorname = kunde.Vorname;
                formular.Nachname = kunde.Nachname;
                formular.Telefonnummer = txtAnfrageTelefonnummer.Text;
                formular.Email = txtAnfrageEmail.Text;
                formular.Inhalt = txtAnfrageInhalt.Text;
                formular.Datum = dtpAnfrageDatum.Value;
                formular.Status = "Abgeschlossen";
                Art art = new Art();
                art.ArtID = 2;
                art.Bezeichnung = "Bestellung";
                formular.Art = art;


                //Anfrage abschließen
                var request3 = new RestRequest("formulare", Method.PUT);
                request3.AddHeader("Content-Type", "application/json");
                request3.AddJsonBody(formular);
                var response3 = client.Execute(request3);

                AnfragenEinlesen();
                AnfragenFelderLeeren();
                FrmWarnung fw = new FrmWarnung();
                fw.lbText.Text = "Bestellung angelegt!\nBitte fügen Sie die Artikel zur Bestellung hinzu";
                fw.ShowDialog();
                if (fw.weiter == true)
                {
                    Bestätigung();
                    panelsDeaktivieren();
                    panelBestellungen.Dock=DockStyle.Fill;
                }
            }
        }

        private void AnfragenFelderLeeren()
        {
            txtAnfrageID.Clear();
            txtAnfrageArt.Clear();
            txtAnfrageName.Clear();
            txtAnfrageTelefonnummer.Clear();
            txtAnfrageEmail.Clear();
            txtAnfrageInhalt.Clear();
            dtpAnfrageDatum.Value = DateTime.Now;
            btAnfragenBestellungÜbernehmen.Visible = false;
        }

        private void btWebsite_MouseEnter(object sender, EventArgs e)
        {
            panelAuswahl.Top = btWebsite.Top;
            umenu();

        }

        private void btWebsite_Click(object sender, EventArgs e)
        {
            Website();
        }

        private void Website()
        {
            panelAuswahl.Height = btWebsite.Height;
            panelAuswahl.Top = btWebsite.Top;

            panelsDeaktivieren();

            panelWebsite.Visible = true;
            panelWebsite.Dock = DockStyle.Fill;
            panelWebsite.BringToFront();
            pbMinMax.BringToFront();


            OpenExplorer(@"D:\OneDrive - BHAK und BHAS Mistelbach 316448\Schule\AP_SWE\GitHub\FutureFarmProgramm\FutureFarm\FutureFarm\Properties\Galerie");

        }

        private void btRechnungWord_Click(object sender, EventArgs e)
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string vorlage = path + @"\RechnungVorlage.docx";
            string speicherort = path + @"\"+txtRechnungID.Text+"_"+txtRechnungKunde.Text+"_Rechnung.docx";

            CreateWordDocument(vorlage, speicherort);

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



                myWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing);

                //Timer
                wordTimer.Start();



                myWordDoc.Activate();
                wordApp.Visible = false;

                //Infos holen
                string nachname = txtRechnungKunde.Text.Substring(0, txtRechnungKunde.Text.IndexOf(" "));
                string vorname = txtRechnungKunde.Text.Substring(txtRechnungKunde.Text.IndexOf(" ") + 1, txtRechnungKunde.Text.Length - nachname.Length-1);
                string anrede="";
                string strasse = "";
                string plz = "";
                string ort = "";
                string artikelIDs = "";
                string artikelBezeichnungen = "";
                string artikelMengen = "";
                string artikelNetto = "";
                string artikelUST = "";


                var requestAnrede = new RestRequest("kunden", Method.GET);
                requestAnrede.AddHeader("Content-Type", "application/Json");
                var responseAnrede = client.Execute<List<Kunden>>(requestAnrede);
                foreach(Kunden k in responseAnrede.Data)
                {
                    if(nachname.Equals(k.Nachname)&&vorname.Equals(k.Vorname))
                    {
                        anrede = k.Anrede;
                        strasse = k.Strasse;
                        plz = k.Postleitzahl.PLZ;
                        ort = k.Postleitzahl.Ortschaft;
                    }
                }

                //gesamte Bestellung in String
                for(int i=0; i<listViewRechnungGewähltArtikel.Items.Count;i++)
                {
                    lvItem = listViewRechnungGewähltArtikel.Items[i];

                    artikelIDs += lvItem.SubItems[0].Text + "\r";
                    artikelBezeichnungen += lvItem.SubItems[1].Text + "\r";
                    artikelMengen += lvItem.SubItems[2].Text + "\r";
                    artikelNetto += (Convert.ToDouble(lvItem.SubItems[3].Text) * Convert.ToDouble(lvItem.SubItems[2].Text)) + "\r";
                    artikelUST += lvItem.SubItems[4].Text + "\r";
                }

                //HIER

                //suche
                this.FindAndReplace(wordApp, "<KundeAnrede>", anrede);
                this.FindAndReplace(wordApp, "<KundeVorname>", vorname);
                this.FindAndReplace(wordApp, "<KundeNachname>", nachname);
                this.FindAndReplace(wordApp, "<KundeStrasse>", strasse);
                this.FindAndReplace(wordApp, "<KundePLZ>", plz);
                this.FindAndReplace(wordApp, "<KundeOrt>", ort);
                this.FindAndReplace(wordApp, "<RechnungsNummer>", txtRechnungID.Text);
                this.FindAndReplace(wordApp, "<Datum>", dtpRechnungDatum.Value.ToShortDateString());
                this.FindAndReplace(wordApp, "<ArtikelNr>", artikelIDs);
                this.FindAndReplace(wordApp, "<ArtikelBezeichnung>", artikelBezeichnungen);
                this.FindAndReplace(wordApp, "<ArtikelMenge>", artikelMengen);
                this.FindAndReplace(wordApp, "<ArtikelPreis>", artikelNetto);
                this.FindAndReplace(wordApp, "<ArtikelUst>", artikelUST);
                this.FindAndReplace(wordApp, "<SummeNetto>", txtRechnungSummeNetto.Text);
                this.FindAndReplace(wordApp, "<SummeUst>", txtRechnungSummeUST.Text);
                this.FindAndReplace(wordApp, "<SummeBrutto>", txtRechnungSummeBrutto.Text);
                this.FindAndReplace(wordApp, "<Lieferdatum>", dtpRechnungDatum.Value.ToShortDateString());

                if(cbRechnungBezahlt.Checked==true)
                    this.FindAndReplace(wordApp, "<Zahlung>", "bezahlt");
                else
                    this.FindAndReplace(wordApp, "<Zahlung>", "offen");

                this.FindAndReplace(wordApp, "<AusgestelltAm>", DateTime.Now.ToShortDateString());

            }
            else
            {
                MessageBox.Show("Dokument konnte nicht gefunden werden");
            }

            //Speichern
            myWordDoc.SaveAs2(ref saveAs, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing);





            DialogResult dialogResult = MessageBox.Show("Die Rechnung wurde angelegt,\nsoll diese direkt geöffnet werden?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                wordApp.Visible = true;
                wordApp.ShowMe();

                //myWordDoc.Close();
                //wordApp.Quit();

            }
            else if (dialogResult == DialogResult.No)
            {
                myWordDoc.Close();
                wordApp.Quit();
            }

        }

        internal void FindAndReplace(Word.Application wordApp, object ToFindText, object replaceWithText)
        {
            object matchCase = true;
            object matchWHoleWord = true;
            object matchWildCards = false;
            object matchSoundLike = false;
            object nmatchAllForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiactitics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object replace = 2;
            object wrap = 1;

            wordApp.Selection.Find.Execute(ref ToFindText,
                ref matchCase, ref matchWHoleWord,
                ref matchWildCards, ref matchSoundLike,
                ref nmatchAllForms, ref forward,
                ref wrap, ref format, ref replaceWithText,
                ref replace, ref matchKashida,
                ref matchDiactitics, ref matchAlefHamza,
                ref matchControl);
        }

        private void panelAnfragen_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btAnfragen.Height;
            panelAuswahl.Top = btAnfragen.Top;
        }

        private void panelArtikel_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btArtikel.Height;
            panelAuswahl.Top = btArtikel.Top;
        }

        private void panelBestellungen_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btBestellungen.Height;
            panelAuswahl.Top = btBestellungen.Top;

        }

        private void panelKunden_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btKunden.Height;
            panelAuswahl.Top = btKunden.Top;

        }

        private void panelNews_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btNews.Height;
            panelAuswahl.Top = btNews.Top;
        }

        private void panelRechnungen_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btRechnungen.Height;
            panelAuswahl.Top = btRechnungen.Top;

        }

        private void panelWebsite_MouseEnter(object sender, EventArgs e)
        {
            panelUnterMenu.Visible = false;
            btFirmendaten.Visible = false;
            btBenutzer.Visible = false;
            panelAuswahl.Height = btWebsite.Height;
            panelAuswahl.Top = btWebsite.Top;

        }

        private void pbMinMax_Click(object sender, EventArgs e)
        {
            if(pbMinMax.Image==min)
            {
                panelLinks.Width = 50;
                pbMinMax.Location = new Point(Convert.ToInt32(panelLinks.Width), Convert.ToInt32(panelMenü.Height) / 2);
                pbMinMax.Image = max;
            }
            else
            {
                panelLinks.Width = Convert.ToInt16(weite * 0.10);
                pbMinMax.Location = new Point(Convert.ToInt32(panelLinks.Width), Convert.ToInt32(panelMenü.Height) / 2);
                pbMinMax.Image = min;
            }
        }

        private void btRechnungSpeichern_Click(object sender, EventArgs e)
        {
            Rechnung rechnung = new Rechnung();
            rechnung.RechnungID = Convert.ToInt32(txtRechnungID.Text);
            rechnung.Datum = dtpRechnungDatum.Value;
            rechnung.Bezahlt = cbRechnungBezahlt.Checked;
            rechnung.BezahltAm = dtpRechnungBezahlt.Value;

            //Kunde suchen
            Kunden kunde = new Kunden();
            var requestKunde = new RestRequest("kunden", Method.GET);
            requestKunde.AddHeader("Content-Type", "application/json");
            var responseKunde = client.Execute<List<Kunden>>(requestKunde);
            foreach (Kunden k in responseKunde.Data)
            {
                if (txtRechnungKunde.Text.Equals(k.Nachname+" "+k.Vorname))
                {
                    kunde.KundenID = k.KundenID;
                    kunde.Anrede = k.Anrede;
                    kunde.Vorname = k.Vorname;
                    kunde.Nachname = k.Nachname;
                    kunde.Firma = k.Firma;
                    kunde.Telefonnummer = k.Telefonnummer;
                    kunde.Email = k.Email;
                    kunde.Strasse = k.Strasse;
                    kunde.Aktiv = k.Aktiv;
                    kunde.PLZ = k.PLZ;
                }
            }

            rechnung.Kunde = kunde;

            var requestUpdate = new RestRequest("rechnungen", Method.PUT);
            requestUpdate.AddHeader("Content-Type", "application/json");
            requestUpdate.AddJsonBody(rechnung);
            var responseUpdate = client.Execute(requestUpdate);
            if (responseUpdate.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Bestätigung();
                RechnungenEinlesen();
            }
        }

        private void btWebsiteBilder_Click(object sender, EventArgs e)
        {
            OpenExplorer(@"D:\OneDrive - BHAK und BHAS Mistelbach 316448\Schule\AP_SWE\GitHub\FutureFarmProgramm\FutureFarm\FutureFarm\Properties\Galerie");
        }

        private static void OpenExplorer(string path)
        {
	        if (Directory.Exists(path))
		        Process.Start("explorer.exe", path);
        }

        private void listViewLieferanten_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lvItem = listViewLieferanten.SelectedItems[0];

                txtLieferantenID.Text = lvItem.SubItems[0].Text;
                txtLieferantenVorname.Text = lvItem.SubItems[1].Text;
                txtLieferantenNachname.Text = lvItem.SubItems[2].Text;
                txtLieferantenFirma.Text = lvItem.SubItems[3].Text;
                txtLieferantenTelefonnummer.Text = lvItem.SubItems[4].Text;
                txtLieferantenEmail.Text = lvItem.SubItems[5].Text;
                txtLieferantenStrasse.Text = lvItem.SubItems[6].Text;
                txtLieferantenPLZ.Text = lvItem.SubItems[7].Text;
                txtLieferantenUID.Text = lvItem.SubItems[9].Text;


                cbLieferantenOrtschaft.Items.Clear();
                //PLZID holen
                Postleitzahl plzLieferant = new Postleitzahl();
                var request1 = new RestRequest("lieferanten", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Lieferanten>>(request1);
                foreach (Lieferanten l in response1.Data)
                {
                    if (txtLieferantenID.Text.Equals(l.LieferantenID.ToString()))
                    {
                        plzLieferant.PLZID = l.Postleitzahl.PLZID;
                        plzLieferant.PLZ = l.Postleitzahl.PLZ;
                        plzLieferant.Ortschaft = l.Postleitzahl.Ortschaft;

                        txtLieferantenPLZ.Text = plzLieferant.PLZ;
                        cbLieferantenOrtschaft.Items.Add(plzLieferant.Ortschaft);
                        cbLieferantenOrtschaft.SelectedItem = cbLieferantenOrtschaft.Items[0];
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        private void btLieferantPLZ_Click(object sender, EventArgs e)
        {
            PLZLieferantSuchen();
        }

        private void PLZLieferantSuchen()
        {
            try
            {
                cbLieferantenOrtschaft.Items.Clear();
                Postleitzahl plz = new Postleitzahl();
                //PLZ holen
                var request1 = new RestRequest("postleitzahlen", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Postleitzahl>>(request1);
                foreach (Postleitzahl pl in response1.Data)
                {
                    if (txtLieferantenPLZ.Text.Equals(pl.PLZ.ToString()))
                    {
                        plz.PLZID = pl.PLZID;
                        plz.PLZ = pl.PLZ;
                        plz.Ortschaft = pl.Ortschaft;
                        cbLieferantenOrtschaft.Items.Add(plz.Ortschaft);
                    }
                }
                cbLieferantenOrtschaft.DroppedDown = true;
            }
            catch (Exception ex)
            {

            }

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (txtLieferantenSuchen.Text != "")
            {
                LieferantenEinlesen();
                LieferantenSuchen();
            }
            else
                LieferantenEinlesen();
        }

        private void LieferantenSuchen()
        {
            foreach (ListViewItem item in listViewLieferanten.Items)
            {
                if (item.SubItems[0].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[1].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[2].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[3].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[4].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[5].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[6].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[7].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[8].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[9].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()))
                {

                }
                else
                {
                    item.Remove();
                }
            }

        }

        private void btLieferantenSuchen_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewLieferanten.Items)
            {
                if (item.SubItems[0].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[1].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[2].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[3].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[4].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[5].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[6].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[7].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[8].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()) || item.SubItems[9].Text.ToLower().Contains(txtLieferantenSuchen.Text.ToLower()))
                {

                }
                else
                {
                    item.Remove();
                }
            }

        }

        private void btLieferantenSpeichern_Click(object sender, EventArgs e)
        {
            if (txtLieferantenID.Text != "")
            {
                //Lieferant speichern

                Lieferanten lieferant = new Lieferanten();
                lieferant.LieferantenID = Convert.ToInt32(txtLieferantenID.Text);
                lieferant.UID = txtLieferantenUID.Text;
                lieferant.Vorname = txtLieferantenVorname.Text;
                lieferant.Nachname = txtLieferantenNachname.Text;
                lieferant.Firma = txtLieferantenFirma.Text;
                lieferant.Email = txtLieferantenEmail.Text;
                lieferant.Telefonnummer = txtLieferantenTelefonnummer.Text;
                lieferant.Strasse = txtLieferantenStrasse.Text;

                Postleitzahl plz = new Postleitzahl();
                //PLZ holen
                var request1 = new RestRequest("postleitzahlen", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Postleitzahl>>(request1);
                foreach (Postleitzahl pl in response1.Data)
                {
                    if (txtLieferantenPLZ.Text.Equals(pl.PLZ.ToString()) && cbLieferantenOrtschaft.Text.Equals(pl.Ortschaft.ToString()))
                    {
                        plz.PLZID = pl.PLZID;
                        plz.PLZ = pl.PLZ;
                        plz.Ortschaft = pl.Ortschaft;
                    }
                }

                lieferant.Postleitzahl = plz;
                lieferant.Aktiv = true;

                //Lieferanten updaten
                var requestUpdate = new RestRequest("lieferanten", Method.PUT);
                requestUpdate.AddHeader("Content-Type", "application/json");
                requestUpdate.AddJsonBody(lieferant);
                var responseUpdate = client.Execute(requestUpdate);
                if (responseUpdate.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Bestätigung();
                    LieferantenEinlesen();
                }




            }
            else
            {
                //LIeferanten neu anlegen
                Lieferanten lieferant = new Lieferanten();
                //lieferant.LieferantenID = Convert.ToInt32(txtLieferantenID.Text);
                lieferant.UID = txtLieferantenUID.Text;
                lieferant.Vorname = txtLieferantenVorname.Text;
                lieferant.Nachname = txtLieferantenNachname.Text;
                lieferant.Firma = txtLieferantenFirma.Text;
                lieferant.Email = txtLieferantenEmail.Text;
                lieferant.Telefonnummer = txtLieferantenTelefonnummer.Text;
                lieferant.Strasse = txtLieferantenStrasse.Text;

                Postleitzahl plz = new Postleitzahl();
                //PLZ holen
                var request1 = new RestRequest("postleitzahlen", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Postleitzahl>>(request1);
                foreach (Postleitzahl pl in response1.Data)
                {
                    if (txtLieferantenPLZ.Text.Equals(pl.PLZ.ToString()) && cbLieferantenOrtschaft.Text.Equals(pl.Ortschaft.ToString()))
                    {
                        plz.PLZID = pl.PLZID;
                        plz.PLZ = pl.PLZ;
                        plz.Ortschaft = pl.Ortschaft;
                    }
                }

                lieferant.Postleitzahl = plz;
                lieferant.Aktiv = true;

                //Lieferanten hinzufügen
                var requestNeu = new RestRequest("lieferanten", Method.POST);
                requestNeu.AddHeader("Content-Type", "application/json");
                requestNeu.AddJsonBody(lieferant);
                var responseNeu = client.Execute(requestNeu);
                if (responseNeu.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Bestätigung();

                    LieferantenEinlesen();
                }


            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtLieferantenID.Clear();
            txtLieferantenVorname.Clear();
            txtLieferantenNachname.Clear();
            txtLieferantenFirma.Clear();
            txtLieferantenTelefonnummer.Clear();
            txtLieferantenEmail.Clear();
            txtLieferantenStrasse.Clear();
            txtLieferantenPLZ.Clear();
            txtLieferantenUID.Clear();
            cbLieferantenOrtschaft.Items.Clear();
        }

        private void txtLieferantenPLZ_TextChanged(object sender, EventArgs e)
        {
            if(cbLieferantenOrtschaft.Items.Count==0)
            {
                if (txtLieferantenPLZ.TextLength > 3)
                    PLZLieferantSuchen();

            }

        }

        private void btLieferantenLöschen_Click(object sender, EventArgs e)
        {
            if (txtLieferantenID.Text != "")
            {
                //Lieferant speichern

                Lieferanten lieferant = new Lieferanten();
                lieferant.LieferantenID = Convert.ToInt32(txtLieferantenID.Text);
                lieferant.UID = txtLieferantenUID.Text;
                lieferant.Vorname = txtLieferantenVorname.Text;
                lieferant.Nachname = txtLieferantenNachname.Text;
                lieferant.Firma = txtLieferantenFirma.Text;
                lieferant.Email = txtLieferantenEmail.Text;
                lieferant.Telefonnummer = txtLieferantenTelefonnummer.Text;
                lieferant.Strasse = txtLieferantenStrasse.Text;

                Postleitzahl plz = new Postleitzahl();
                //PLZ holen
                var request1 = new RestRequest("postleitzahlen", Method.GET);
                request1.AddHeader("Content-Type", "application/json");
                var response1 = client.Execute<List<Postleitzahl>>(request1);
                foreach (Postleitzahl pl in response1.Data)
                {
                    if (txtLieferantenPLZ.Text.Equals(pl.PLZ.ToString()) && cbLieferantenOrtschaft.Text.Equals(pl.Ortschaft.ToString()))
                    {
                        plz.PLZID = pl.PLZID;
                        plz.PLZ = pl.PLZ;
                        plz.Ortschaft = pl.Ortschaft;
                    }
                }

                lieferant.Postleitzahl = plz;
                lieferant.Aktiv = false;

                //Lieferanten updaten
                var requestUpdate = new RestRequest("lieferanten", Method.PUT);
                requestUpdate.AddHeader("Content-Type", "application/json");
                requestUpdate.AddJsonBody(lieferant);
                var responseUpdate = client.Execute(requestUpdate);
                if (responseUpdate.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("An Error occured", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Bestätigung();
                    LieferantenEinlesen();
                }

            }
        }

        private void txtKundenPLZ_TextChanged(object sender, EventArgs e)
        {
            if(cbKundenOrt.Items.Count==0)
            {
                if (txtKundenPLZ.TextLength > 3)
                    PLZKundenSuchen();
            }
        }

        private void btKundenReset_Click(object sender, EventArgs e)
        {
            txtKundenID.Clear();
            cbKundenAnrede.SelectedText = "";
            txtKundenVorname.Clear();
            txtKundenNachname.Clear();
            txtKundenFirma.Clear();
            txtKundenTelefonnummer.Clear();
            txtKundenEmail.Clear();
            txtKundenStrasse.Clear();
            txtKundenPLZ.Clear();
            cbKundenOrt.Items.Clear();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                MessageBox.Show("Enter");
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Home();
        }

        private void rechnungenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rechnungen();
        }

        private void artikelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Artikel();
        }

        private void kundenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kunden();
        }

        private void lieferantenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lieferanten();
        }

        private void bestellungeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bestellungen();
        }

        private void fAnfragenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Anfragen();
        }

        private void newsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            News();
        }

        private void termineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Termine();
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Website();
        }

        private void iFirmendatenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Firmendaten();
        }

        private void uBenutzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Benutzer();
        }

        private void Bestätigung()
        {
            timerBestätigung.Enabled = true;
            timerBestätigung.Start();

            int x = this.Width / 2 - pbBestätigung.Width / 2;
            int y = this.Height / 2 - pbBestätigung.Height / 2;
            pbBestätigung.Location = new Point(x, y);
            pbBestätigung.Visible = true;
            pbBestätigung.BringToFront();
        }


        private void timerBestätigung_Tick(object sender, EventArgs e)
        {
            pbBestätigung.Visible = false;
            timerBestätigung.Enabled = false;
        }

        private void txtArtikelUST_TextChanged(object sender, EventArgs e)
        {
            ArtikelPreisBerechnen();
        }

        private void txtArtikelBrutto_TextChanged(object sender, EventArgs e)
        {

        }

        private void listViewRechnungSuche_Click(object sender, EventArgs e)
        {
            

        }
    }
}


