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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
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
            panelAuswahl.Height = btBenutzer.Height;
            panelAuswahl.Top = btBenutzer.Top;
            txtBenutzername.Focus();
        }

        private void btPasswort_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btPasswort.Height;
            panelAuswahl.Top = btPasswort.Top;
            txtPasswort.Focus();
        }

        private void btAbbrechen_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btBestätigen_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.EinloggenNeu();
            this.Close();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            panelAuswahl.Height = btBenutzer.Height;
            panelAuswahl.Top = btBenutzer.Top;
            txtBenutzername.Focus();
        }

        
    }
}
