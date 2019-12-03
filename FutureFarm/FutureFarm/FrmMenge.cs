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
    public partial class FrmMenge : Form
    {
        public FrmMenge()
        {
            InitializeComponent();
        }

        string zahl = "";

        private void btMengeEnter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btMengeEins_Click(object sender, EventArgs e)
        {
            zahl += "1";
            txtMenge.Text=zahl;
        }

        private void btMengeClear_Click(object sender, EventArgs e)
        {
            zahl = "";
            txtMenge.Text = zahl;
        }

        private void btMengeZwei_Click(object sender, EventArgs e)
        {
            zahl += "2";
            txtMenge.Text = zahl;
        }

        private void btMengeDrei_Click(object sender, EventArgs e)
        {
            zahl += "3";
            txtMenge.Text = zahl;

        }

        private void btMengeVier_Click(object sender, EventArgs e)
        {
            zahl += "4";
            txtMenge.Text = zahl;

        }

        private void btMengeFünf_Click(object sender, EventArgs e)
        {
            zahl += "5";
            txtMenge.Text = zahl;

        }

        private void btMengeSechs_Click(object sender, EventArgs e)
        {
            zahl += "6";
            txtMenge.Text = zahl;

        }

        private void btMengeSieben_Click(object sender, EventArgs e)
        {
            zahl += "7";
            txtMenge.Text = zahl;

        }

        private void btMengeAcht_Click(object sender, EventArgs e)
        {
            zahl += "8";
            txtMenge.Text = zahl;

        }

        private void btMengeNeun_Click(object sender, EventArgs e)
        {
            zahl += "9";
            txtMenge.Text = zahl;

        }

        private void btMengeNull_Click(object sender, EventArgs e)
        {
            zahl += "0";
            txtMenge.Text = zahl;

        }

        private void FrmMenge_Load(object sender, EventArgs e)
        {
            zahl = "";
            txtMenge.Text = zahl;
        }
    }
}
