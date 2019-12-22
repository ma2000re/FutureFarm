using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Formular
    {
        public virtual int FormularID { get; set; }
        public virtual Art Art { get; set; }
        public virtual string Vorname { get; set; }
        public virtual string Nachname { get; set; }
        public virtual string Telefonnummer { get; set; }
        public virtual string Email { get; set; }
        public virtual string Inhalt { get; set; }
        public virtual DateTime Datum { get; set; }
        public virtual Bestellung Bestellung { get; set; }
        public virtual string Status { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Art> Arten { get; set; }

        public Formular()
        {
            this.Arten = new HashSet<Art>();
        }
    }
}
