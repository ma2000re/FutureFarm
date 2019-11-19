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
    public partial class FrmSuperpasswort : Form
    {
        public FrmSuperpasswort()
        {
            InitializeComponent();
        }

        internal bool berechtigt;
        Form1 f1 = new Form1();


        private void btWeiter_Click(object sender, EventArgs e)
        {
            if (txtPasswort.Text.Equals("Superpasswort"))
                berechtigt = true;
            else
                MessageBox.Show("Passwort falsch");

            f1.Enabled = true;
            this.Close();
            
        }

        private void FrmSuperpasswort_Load(object sender, EventArgs e)
        {
            f1.Enabled = false;
            txtPasswort.Focus();
        }
    }
}
