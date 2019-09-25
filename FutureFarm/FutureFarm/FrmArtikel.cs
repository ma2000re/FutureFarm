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
            sql = "Select * from tblArtikel";
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
    }
}
