using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Kunden
    {
        public virtual int KundenID { get; set; }
        public virtual string Anrede { get; set; }
        public virtual string Vorname { get; set; }
        public virtual string Nachname { get; set; }
        public virtual string Firma { get; set; }
        public virtual string Telefonnummer { get; set; }
        public virtual string Email { get; set; }
        public virtual string Strasse { get; set; }
        public virtual Postleitzahl Postleitzahl { get; set; }
        public virtual bool Aktiv { get; set; }

        //[JsonIgnore]
        public virtual ICollection<Postleitzahl> PLZ { get; set; }


        public Kunden()
        {
            this.PLZ = new HashSet<Postleitzahl>();
        }
    }
}
