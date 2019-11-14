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

        private void btWeiter_Click(object sender, EventArgs e)
        {
            if (txtPasswort.Text.Equals("Superpasswort"))
                berechtigt = true;
            else
                MessageBox.Show("Passwort falsch");

            this.Close();
            
        }
    }
}
