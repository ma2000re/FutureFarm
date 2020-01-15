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
            Form1.flade = this;
        }

        

        private void FrmLadebildschirm_Load(object sender, EventArgs e)
        {
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = pictureBox1.Height;
            this.Location = new Point(0, this.Height / 3);

            pictureBox1.Location = new Point((this.Width / 2 - pictureBox1.Width / 2), 0);

            timer1.Start(); // Timer starten
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            timer1.Stop();
            this.Close();
            Form1.f1.Show();
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
