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

        private void btHome_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;
            this.Close();
        }

        private void btArtikelNeu_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btArtikelNeu.Height;
            panelAuswahl.Top = btArtikelNeu.Top;
        }

        private void FrmArtikel_Load(object sender, EventArgs e)
        {
            ArtikelEinlesen();
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;
        }

        private void ArtikelEinlesen()
        {
            listViewArtikel.Items.Clear();
            sql = "SELECT tblArtikel.ArtikelID, tblArtikel.Bezeichnung, tblArtikel.PreisNetto, tblUmsatzsteuer.UstSatz, tblLieferanten.Firma, tblArtikel.Lagerstand, tblArtikel.Reserviert FROM tblUmsatzsteuer INNER JOIN(tblLieferanten INNER JOIN tblArtikel ON tblLieferanten.LieferantenID = tblArtikel.LieferantenID) ON tblUmsatzsteuer.UstID = tblArtikel.UstID;";

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



                listViewArtikel.Items.Add(lvItem);

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
    }
}
