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
    public partial class FrmLadebildschirm : Form
    {
        public FrmLadebildschirm()
        {
            InitializeComponent();
        }

        private void FrmLadebildschirm_Load(object sender, EventArgs e)
        {
            timer1.Start(); // Timer starten
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
