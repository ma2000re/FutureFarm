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
    public partial class FrmArtikel : Form
    {
        public FrmArtikel()
        {
            InitializeComponent();
            db = new Datenbank();
        }

        Datenbank db;
        String sql;
        StreamReader sr;
        StreamWriter sw;
        ListViewItem lvItem;
        OleDbDataReader dr;
        FrmWarnung fWarnung = new FrmWarnung();


        private void btHome_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;

            FrmWarnung fWarnung = new FrmWarnung();
            fWarnung.lbText.Text = "Nicht gesicherte Daten gehen verloren! \nWollen Sie fortfahren?";
            fWarnung.ShowDialog();
            if (fWarnung.weiter == true)
            this.Close();
            
        }

        private void btArtikelNeu_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btArtikelNeu.Height;
            panelAuswahl.Top = btArtikelNeu.Top;
        }

        private void FrmArtikel_Load(object sender, EventArgs e)
        {
            btNeu.Enabled = false;
            btLöschen.Enabled = false;
            btSpeichern.Enabled = false;

            ArtikelEinlesen();
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;
        }

        private void ArtikelEinlesen()
        {
            listViewArtikel.Items.Clear();
            sql = "SELECT tblArtikel.ArtikelID, tblArtikel.Bezeichnung, tblArtikel.PreisNetto, tblUmsatzsteuer.UstSatz, tblLieferanten.Firma, tblArtikel.Lagerstand, tblArtikel.Reserviert, tblArtikel.Aktiv FROM tblUmsatzsteuer INNER JOIN(tblLieferanten INNER JOIN tblArtikel ON tblLieferanten.LieferantenID = tblArtikel.LieferantenID) ON tblUmsatzsteuer.UstID = tblArtikel.UstID;";

            dr = db.Einlesen(sql);
            while (dr.Read())
            {
                lvItem = new ListViewItem(dr[0].ToString());
                lvItem.SubItems.Add(dr[1].ToString());
                lvItem.SubItems.Add(dr[2].ToString());
                lvItem.SubItems.Add(dr[3].ToString());
                lvItem.SubItems.Add(dr[4].ToString());
                lvItem.SubItems.Add(dr[5].ToString());
                lvItem.SubItems.Add(dr[6].ToString());

                if(dr[7].ToString()=="True")
                {
                    listViewArtikel.Items.Add(lvItem);
                }

            }
        }

        private void listViewArtikel_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }

        private void listViewArtikel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewArtikel.SelectedItems.Count == 0)
            {
                MessageBox.Show("Wählen Sie bitte einen Artikel aus!");
                return;
            }
            lvItem = listViewArtikel.SelectedItems[0];

            txtArtikelID.Text = lvItem.SubItems[0].Text;
            txtBezeichnung.Text = lvItem.SubItems[1].Text;
            txtNettopreis.Text = (lvItem.SubItems[2].Text);
            txtUST.Text = lvItem.SubItems[3].Text;
            Double netto = Convert.ToDouble(txtNettopreis.Text);
            Double ust = Convert.ToDouble(txtUST.Text);
            txtBrutto.Text = (netto + (1 + ust / 100)).ToString();
            txtLagerstand.Text = lvItem.SubItems[5].Text;
            txtReserviert.Text = lvItem.SubItems[6].Text;
            txtLieferant.Text = lvItem.SubItems[4].Text;
            //fKunde.id = Convert.ToInt64(lvItem.SubItems[0].Text);
            //fKunde.txtVorname.Text = lvItem.SubItems[1].Text;
            //fKunde.txtZuname.Text = lvItem.SubItems[2].Text;
            //fKunde.dtpGeburt.Value = Convert.ToDateTime(lvItem.SubItems[3].Text);
            //fKunde.txtEmail.Text = lvItem.SubItems[4].Text;
            //fKunde.txtTelefon.Text = lvItem.SubItems[5].Text;
            //fKunde.txtStraße.Text = lvItem.SubItems[7].Text;
            //fKunde.plz = lvItem.SubItems[6].Text;
            
           // inlesenKunden();

        }

        private void btSpeichern_Click(object sender, EventArgs e)
        {
            //sql = "SELECT tblArtikel.ArtikelID, tblArtikel.Bezeichnung, tblArtikel.PreisNetto, tblUmsatzsteuer.UstSatz, tblLieferanten.Firma, tblArtikel.Lagerstand, tblArtikel.Reserviert FROM tblUmsatzsteuer INNER JOIN(tblLieferanten INNER JOIN tblArtikel ON tblLieferanten.LieferantenID = tblArtikel.LieferantenID) ON tblUmsatzsteuer.UstID = tblArtikel.UstID;";
            // sql = "Update Artikel set Bezeichnung='" + txtBezeichnung.Text + "', Nettopreis='" + preis + "', UmsatzsteuerID='" + (cbUstSatz.SelectedIndex + 1) + "' where ArtikelID=" + id + ";";
            if (txtArtikelID.Text != "")
            {
                sql = "UPDATE tblArtikel, tblUmsatzsteuer, tblLieferanten SET tblArtikel.Bezeichnung='" + txtBezeichnung.Text + "', tblArtikel.PreisNetto='" + txtNettopreis.Text + "', tblUmsatzsteuer.UstSatz=";
            }
            else if (txtArtikelID.Text == "")
            {
                FrmWarnung fWarnung = new FrmWarnung();
                fWarnung.lbText.Text = "Bitte wählen Sie einen Artikel aus, bevor Sie Änderungen\nspeichern wollen!";
                fWarnung.ShowDialog();
            }
        }

        private void btLöschen_Click(object sender, EventArgs e)
        {
            if (txtArtikelID.Text != "")
            {
                fWarnung.Text = "Wollen Sie den Artikel wirklich löschen?";
                fWarnung.ShowDialog();
                if(fWarnung.weiter==true)
                {
                    sql = "UPDATE tblArtikel SET Aktiv=false WHERE ArtikelID="+Convert.ToInt64(txtArtikelID.Text)+";";
                    db.Ausfuehren(sql);
                    txtArtikelID.Clear();
                    txtBezeichnung.Clear();
                    txtBrutto.Clear();
                    txtNettopreis.Clear();
                    txtLagerstand.Clear();
                    txtReserviert.Clear();
                    txtUST.Clear();
                    txtLieferant.Clear();
                    ArtikelEinlesen();
                }
            }
            else if (txtArtikelID.Text == "")
            {
                fWarnung.lbText.Text = "Bitte wählen Sie einen Artikel aus, bevor Sie Änderungen\nspeichern wollen!";
                fWarnung.ShowDialog();
            }

        }
    }
}
