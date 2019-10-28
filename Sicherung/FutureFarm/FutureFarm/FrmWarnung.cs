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
    public partial class FrmWarnung : Form
    {
        public FrmWarnung()
        {
            InitializeComponent();
        }


        internal bool weiter;

        private void FrmWarnung_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            weiter = true;
            this.Close();
        }

        private void btAbbrechen_Click(object sender, EventArgs e)
        {
            weiter = false;
            this.Close();
        }
    }
}
