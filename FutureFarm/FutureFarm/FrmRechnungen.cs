using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FutureFarm
{
    public partial class FrmRechnungen : Form
    {
        public FrmRechnungen()
        {
            InitializeComponent();
        }

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

        private void btRechnungErstellen_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btRechnungErstellen.Height;
            panelAuswahl.Top = btRechnungErstellen.Top;

        }

        private void btBestellungErstellen_Click(object sender, EventArgs e)
        {
            panelAuswahl.Height = btBestellungErstellen.Height;
            panelAuswahl.Top = btBestellungErstellen.Top;
        }

        private void FrmRechnungen_Load(object sender, EventArgs e)
        {
            panelAuswahl.Height = btHome.Height;
            panelAuswahl.Top = btHome.Top;

        }
    }
}
