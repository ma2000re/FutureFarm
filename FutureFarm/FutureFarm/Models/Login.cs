using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Login
    {
        public virtual int BenutzernameID { get; set; }
        public virtual string Benutzername { get; set; }
        public virtual string Passwort { get; set; }
        public virtual DateTime LetzteAnmeldung { get; set; }


        public Login()
        {
        }
    }
}
